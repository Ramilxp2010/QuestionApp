using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using QuestionApp.Application.Question.Filters;

namespace QuestionAppUI.Pages
{
    public partial class Index
    {
        [Inject] ICategoryData categoryData { get; set; }

        [Inject] IStatusData statusData { get; set; }

        [Inject] IQuestionData questionData { get; set; }

        [Inject] IUserData userData { get; set; }

        [Inject] IAnswerData answerData { get; set; }

        [Inject] NavigationManager navManager { get; set; }

        [Inject] ProtectedSessionStorage sessionStorage { get; set; }

        [Inject] AuthenticationStateProvider authProvider { get; set; }

        private User loggedUser;

        private ICollection<Question> questions;

        private List<Category> categories;

        private List<Status> statuses;

        private Question archivingQuestion;

        private List<Answer> userAnswers;

        private HashSet<string> selectedCategoriesId = new HashSet<string>();

        private string selectedStatusId = string.Empty;

        private string searchText = "";

        bool isSortedByNew = true;

        protected async override Task OnInitializedAsync()
        {
            categories = await categoryData.GetAllCategories();
            statuses = await statusData.GetAllStatuses();

            loggedUser = await authProvider.GetUserFromAuth(userData);
            userAnswers = loggedUser != null
                ? await answerData.GetUserAnswers(loggedUser.Id)
                : new List<Answer>();
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadFilterState();
                await FilterQuestions();
                StateHasChanged();
            }
        }

        private void AskArchive(Question question)
        {
            archivingQuestion = question;
        }

        private void CancelArchive()
        {
            archivingQuestion = null;
        }

        private async Task ArchiveQuestion()
        {
            if (archivingQuestion is null)
            {
                return;
            }

            archivingQuestion.IsArchived = true;
            await questionData.UpdateQuestion(archivingQuestion);
            questions.Remove(archivingQuestion);
            archivingQuestion = null;
        }

        private async Task LoadFilterState()
        {
            var categoriesResults = await sessionStorage.GetAsync<HashSet<string>>(nameof(selectedCategoriesId));
            selectedCategoriesId = categoriesResults.Success ? categoriesResults.Value : new HashSet<string>() {};

            var stringResults = await sessionStorage.GetAsync<string>(nameof(selectedStatusId));
            selectedStatusId = stringResults.Success ? stringResults.Value : null;

            stringResults = await sessionStorage.GetAsync<string>(nameof(searchText));
            searchText = stringResults.Success ? stringResults.Value : "";

            var boolResults = await sessionStorage.GetAsync<bool>(nameof(isSortedByNew));
            isSortedByNew = boolResults.Success ? boolResults.Value : true;
        }

        private async Task SaveFilterState()
        {
            await sessionStorage.SetAsync(nameof(selectedCategoriesId), selectedCategoriesId);
            await sessionStorage.SetAsync(nameof(selectedStatusId), selectedStatusId);
            await sessionStorage.SetAsync(nameof(searchText), searchText);
            await sessionStorage.SetAsync(nameof(isSortedByNew), isSortedByNew);
        }

        private async Task FilterQuestions()
        {
            var filter = new QuestionFilter(null, selectedCategoriesId, selectedStatusId);
            var output = await questionData.GetApprovedQuestions(filter);

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                output = output.Where(x => !string.IsNullOrWhiteSpace(x.QuestionText) && x.QuestionText.Contains(searchText, StringComparison.CurrentCultureIgnoreCase) ||
                !string.IsNullOrWhiteSpace(x.Description) && x.Description.Contains(searchText, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
            }

            if (isSortedByNew)
            {
                output = output.OrderByDescending(x => x.DateCreated).ToList();
            }
            else
            {
                output = output.OrderByDescending(x => x.UserVotes.Count).ThenByDescending(x => x.DateCreated).ToList();
            }

            questions = output;
            await SaveFilterState();
        }

        private async Task OrderByNew(bool isNew)
        {
            isSortedByNew = isNew;
            await FilterQuestions();
        }

        private void LoadCreatePage()
        {
            if (loggedUser != null)
            {
                navManager.NavigateTo("/Create");
            }
            else
            {
                navManager.NavigateTo("/identity/account/login", true);
            }
        }

        private async Task OnSearchInput(string searchInput)
        {
            searchText = searchInput;
            await FilterQuestions();
        }

        private async Task OnCategoryClick(string categoryId = null)
        {
            if (string.IsNullOrWhiteSpace(categoryId))
                selectedCategoriesId.Clear();
            else if (!selectedCategoriesId.Add(categoryId))
                selectedCategoriesId.Remove(categoryId);

            await FilterQuestions();
        }

        private async Task OnStatusClick(string statusId = null)
        {
            selectedStatusId = statusId;
            await FilterQuestions();
        }

        private async Task UpVote(Question question)
        {
            if (loggedUser is null)
            {
                navManager.NavigateTo("/identity/account/login", true);
                return;
            }

            if (question.Author.Id != loggedUser.Id)
            {
                if (!question.UserVotes.Add(loggedUser.Id))
                {
                    question.UserVotes.Remove(loggedUser.Id);
                }

                await questionData.UpVoted(question.Id, loggedUser.Id);

                if (!isSortedByNew)
                {
                    questions = questions.OrderByDescending(s => s.UserVotes.Count)
                        .ThenByDescending(x => x.DateCreated).ToList();
                }
            }
        }

        private string GetUpvoteTopText(Question question)
        {
            return question.UserVotes?.Count.ToString("00");
        }

        private string GetUpvoteBottomText(Question question)
        {
            return question.UserVotes?.Count > 1 ? "Upvotes" : "Upvote";
        }

        private void OpenDetails(Question question)
        {
            navManager.NavigateTo($"/Details/{question.Id}");
        }

        private string SortedByNewClass(bool isNew)
        {
            return isNew == isSortedByNew
            ? "sort-selected"
            : "";
        }

        private string GetVoteClass(Question question)
        {
            return question.UserVotes.Contains(loggedUser?.Id) ? "thumb-up-icon-clicked" : "thumb-up-icon";
        }

        private string GetQuestionStatusClass(Question question)
        {
            if (question == null || question.Status == null)
            {
                return "question-entry-status-none";
            }

            string output = question.Status.Name?.ToLowerInvariant() switch
            {
                "completed" => "question-entry-status-complated",
                "watching" => "question-entry-status-wathing",
                "upcoming" => "question-entry-status-upcoming",
                "dismissed" => "question-entry-status-dismissed",
                _ => "question-entry-status-none"
            };

            return output;
        }

        private string GetSelectedCategiryClass(string categoryId = null)
        {
            if (string.IsNullOrWhiteSpace(categoryId) && !selectedCategoriesId.Any())
                return "selected-category";

            return selectedCategoriesId.Any(x => x == categoryId) ? "selected-category" : "";
        }

        private string GetSelectedStatusClass(string statusId = null)
        {
            return (statusId == null && selectedStatusId == null) || statusId == selectedStatusId ? "selected-status" : "";
        }

        private void GoToEdit(string questionId)
        {
            navManager.NavigateTo($"/Edit/{questionId}");
        }

        private string GetAnswerMarkClass(Question questionModel)
        {
            return userAnswers?.Any(x => x.QuestionId == questionModel.Id) == true ? "answered-icon" : "";
        }
    }
}
