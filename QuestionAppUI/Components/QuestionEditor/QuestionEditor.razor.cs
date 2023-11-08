using Microsoft.AspNetCore.Components;
using QuestionAppUI.Models;

namespace QuestionAppUI.Components.QuestionEditor
{
    public partial class QuestionEditor : ComponentBase
    {
        private List<CategoryModel> categories = new List<CategoryModel>();

        [Inject] private ICategoryData? categoryData { get; set; }

        [Parameter]
        public QuestionVM EditableQuestion { get; set; }

        private QuestionVM questionVM => EditableQuestion;

        [Parameter]
        public EventCallback<QuestionVM> OnSubmitCallback { get; set; }
        
        [Parameter]
        public EventCallback OnCloseCallback { get; set; }

        protected override async Task OnInitializedAsync()
        {
            categories = await categoryData.GetAllCategoies();
        }

        private async Task SaveQuestion() 
        {
            await OnSubmitCallback.InvokeAsync(questionVM);
        }

        private async Task ClosePage() 
        {
            await OnCloseCallback.InvokeAsync();
        }

        private string GetTagClass(CategoryModel category)
        {
            return questionVM.CategoryIds.Contains(category.Id)
            ? "success-color"
            : "";
        }

        private void ClickTag(CategoryModel category)
        {
            if (!questionVM.CategoryIds.Add(category.Id))
                questionVM.CategoryIds.Remove(category.Id);
        }
    }
}