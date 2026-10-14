using Microsoft.AspNetCore.Identity;
using QuestionAppUI;
using QuestionAppUI.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var sp = scope.ServiceProvider;
    await DemoDataSeeder.EnsureDemoDataAsync(
        sp.GetRequiredService<IUserData>(),
        sp.GetRequiredService<IPasswordHasher<User>>(),
        sp.GetRequiredService<ICategoryData>(),
        sp.GetRequiredService<IStatusData>(),
        sp.GetRequiredService<IQuestionData>(),
        sp.GetRequiredService<IAnswerData>());
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

var uploadsRoot = Path.Combine(app.Environment.WebRootPath, "uploads");
Directory.CreateDirectory(uploadsRoot);

app.MapPost("/upload/image", async (HttpRequest request) =>
{
    if (request.Form.Files.Count == 0)
    {
        return Results.BadRequest(new { error = "No file uploaded." });
    }

    var file = request.Form.Files[0];
    var allowed = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "image/png", "image/jpeg", "image/gif", "image/webp"
    };

    if (!allowed.Contains(file.ContentType))
    {
        return Results.BadRequest(new { error = "Unsupported image type." });
    }

    const long maxBytes = 2 * 1024 * 1024;
    if (file.Length <= 0 || file.Length > maxBytes)
    {
        return Results.BadRequest(new { error = "File must be between 1 byte and 2 MB." });
    }

    var extension = Path.GetExtension(file.FileName);
    if (string.IsNullOrWhiteSpace(extension))
    {
        extension = file.ContentType switch
        {
            "image/png" => ".png",
            "image/jpeg" => ".jpg",
            "image/gif" => ".gif",
            "image/webp" => ".webp",
            _ => ".bin"
        };
    }

    var fileName = $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
    var savePath = Path.Combine(uploadsRoot, fileName);
    await using (var stream = File.Create(savePath))
    {
        await file.CopyToAsync(stream);
    }

    return Results.Json(new { url = $"/uploads/{fileName}" });
}).RequireAuthorization().DisableAntiforgery();

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
