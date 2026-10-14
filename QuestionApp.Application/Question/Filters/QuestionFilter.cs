namespace QuestionApp.Application.Question.Filters
{
    public record QuestionFilter(string? Search, HashSet<string>? CategoriesId, string? StatusId, int Page = 1,  int PageSize = 10)
    {
    }
}
