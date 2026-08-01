using Microsoft.AspNetCore.Authentication.Cookies;
using QuestionAppUI.Toaster;

namespace QuestionAppUI
{
    public static class RegisterServices
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAuthenticationCore();

            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddMemoryCache();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(20);
                options.LoginPath = Urls.LoginPath;
                options.LogoutPath = Urls.LogoutPath;
            });

            builder.Services.AddAuthorization();

            builder.Services.AddSingleton<IDbConnection, DbConnection>();
            builder.Services.AddSingleton<ICategoryData, MongoCategoryData>();
            builder.Services.AddSingleton<IStatusData, MongoStatusData>();
            builder.Services.AddSingleton<IUserData, MongoUserData>();
            builder.Services.AddSingleton<IQuestionData, MongoQuestionData>();
            builder.Services.AddSingleton<IAnswerData, MongoAnswerData>();
            builder.Services.AddSingleton<ToasterService>();

        }
    }
}
