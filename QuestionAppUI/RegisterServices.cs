using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.IdentityModel.Tokens;
using QuestionAppUI.Authentication;
using System.Text;

namespace QuestionAppUI
{
    public static class RegisterServices
    {
        public static void ConfigureServices(this WebApplicationBuilder builder) 
        {
            builder.Services.AddAuthenticationCore();

            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddMemoryCache();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => 
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => 
                {
                    //TODO
                    policy.RequireClaim("jobTitle", "Admin");
                });
            });

            builder.Services.AddSingleton<IDbConnection, DbConnection>();
            builder.Services.AddSingleton<ICategoryData, MongoCategoryData>();
            builder.Services.AddSingleton<IStatusData, MongoStatusData>();
            builder.Services.AddSingleton<IUserData, MongoUserData>();
            builder.Services.AddSingleton<IQuestionData, MongoQuestionData>();

            builder.Services.AddScoped<ProtectedSessionStorage>();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        }
    }

    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!123123412341234";   // ключ для шифрации
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
