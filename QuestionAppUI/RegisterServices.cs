using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using QuestionAppUI.Toaster;
using QuestionsApp.Infrastructure.MongoDb;
using QuestionsApp.Infrastructure.MongoDb.DataAccess;
using Radzen;

namespace QuestionAppUI
{
    public static class RegisterServices
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            var mapsterConfig = new TypeAdapterConfig();

            // Сканируем сборки, в которых объявлены
            // классы с конфигурациями Mapster (IRegister).
            mapsterConfig.Scan(
                typeof(MongoAnswerData).Assembly,
                typeof(User).Assembly);

            builder.Services.AddSingleton(mapsterConfig);
            builder.Services.AddSingleton<IMapper, ServiceMapper>();


            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAuthenticationCore();

            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddMemoryCache();

            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromDays(20);
                    options.LoginPath = Urls.LoginPath;
                    options.LogoutPath = Urls.LogoutPath;
                });

            builder.Services.AddAuthorization();
            builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

            builder.Services.AddSingleton<DbConnection>();
            builder.Services.AddSingleton<ICategoryData, MongoCategoryData>();
            builder.Services.AddSingleton<IStatusData, MongoStatusData>();
            builder.Services.AddSingleton<IUserData, MongoUserData>();
            builder.Services.AddSingleton<IQuestionData, MongoQuestionData>();
            builder.Services.AddSingleton<IAnswerData, MongoAnswerData>();
            builder.Services.AddSingleton<ToasterService>();

            builder.Services.AddRadzenComponents();
        }
    }
}
