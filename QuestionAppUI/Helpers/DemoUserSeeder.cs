using Microsoft.AspNetCore.Identity;
using QuestionAppUI.Data;

namespace QuestionAppUI.Helpers
{
    public static class DemoUserSeeder
    {
        public const string DefaultPassword = "1234";

        public static async Task EnsureDemoUsersAsync(IUserData userData, IPasswordHasher<User> passwordHasher)
        {
            foreach (var template in SampleData.SampleUsers)
            {
                var existing = await userData.GetUserByIdentifier(template.ObjectIdentifier);
                if (existing == null)
                {
                    var user = new User
                    {
                        ObjectIdentifier = template.ObjectIdentifier,
                        UserName = template.UserName,
                        Name = template.Name,
                        DisplayName = template.DisplayName,
                        Role = template.Role,
                        Email = template.Email,
                        PasswordHash = passwordHasher.HashPassword(template, DefaultPassword)
                    };
                    await userData.CreateUser(user);
                    continue;
                }

                var needsUpdate = false;
                if (string.IsNullOrWhiteSpace(existing.UserName))
                {
                    existing.UserName = template.UserName;
                    needsUpdate = true;
                }

                if (string.IsNullOrWhiteSpace(existing.Role))
                {
                    existing.Role = template.Role;
                    needsUpdate = true;
                }

                if (string.IsNullOrWhiteSpace(existing.PasswordHash))
                {
                    existing.PasswordHash = passwordHasher.HashPassword(existing, DefaultPassword);
                    needsUpdate = true;
                }

                if (needsUpdate)
                {
                    await userData.UpdateUser(existing);
                }
            }
        }
    }
}
