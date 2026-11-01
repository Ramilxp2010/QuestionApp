using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using QuestionApp.Application.DataGenerator.Options;
using QuestionAppLibrary.Models;

namespace QuestionApp.Application.DataGenerator;

/// <summary>
/// Заполняет пустую базу данными из <see cref="TestDataGenerator"/>.
/// Предназначен только для Development: всем пользователям ставится пароль
/// <see cref="DemoUserSeeder.DefaultPassword"/>.
/// </summary>
public static class TestDataSeeder
{
    /// <returns>true, если данные были записаны; false, если база уже не пустая.</returns>
    public static async Task<bool> EnsureTestDataAsync(
        ISeedDataStore store,
        IPasswordHasher<User> passwordHasher,
        SeedOptions options,
        ILogger logger,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(store);
        ArgumentNullException.ThrowIfNull(passwordHasher);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(logger);

        if (!await store.IsEmptyAsync(cancellationToken))
        {
            logger.LogInformation(
                "Test data seeding skipped: database is not empty. Drop the database to regenerate test data.");
            return false;
        }

        var data = new TestDataGenerator().Generate(options);

        // PBKDF2 в PasswordHasher намеренно медленный (~десятки мс на хеш).
        // Для тестовых пользователей с одинаковым паролем считаем хеш один раз:
        // общая соль допустима только для тестовых данных.
        if (data.Users.Count > 0)
        {
            var sharedHash = passwordHasher.HashPassword(data.Users[0], DemoUserSeeder.DefaultPassword);
            foreach (var user in data.Users)
            {
                user.PasswordHash = sharedHash;
            }
        }

        await store.SaveAsync(data, cancellationToken);

        logger.LogInformation(
            "Test data seeded: {Users} users, {Categories} categories, {Statuses} statuses, {Questions} questions, {Answers} answers (seed {Seed}).",
            data.Users.Count, data.Categories.Count, data.Statuses.Count,
            data.Questions.Count, data.Answers.Count, options.RandomSeed);

        return true;
    }
}
