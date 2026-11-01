using QuestionApp.Application.DataGenerator.Model;

namespace QuestionApp.Application.DataGenerator;

/// <summary>
/// Хранилище для массовой записи тестовых данных.
/// Обычные IQuestionData и IAnswerData пишут по одной записи и на каждый вопрос
/// дополнительно обновляют автора, поэтому для тысяч записей не подходят.
/// </summary>
public interface ISeedDataStore
{
    /// <summary>true, если ни в одной коллекции приложения нет данных.</summary>
    Task<bool> IsEmptyAsync(CancellationToken cancellationToken = default);

    /// <summary>Записывает набор целиком, с уже назначенными Id.</summary>
    Task SaveAsync(SeedData data, CancellationToken cancellationToken = default);
}
