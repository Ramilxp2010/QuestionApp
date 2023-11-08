
using Microsoft.AspNetCore.Components;

namespace QuestionAppUI.Toaster
{
    public partial class Toaster : ComponentBase, IDisposable
    {
        [Inject] private ToasterService? _toasterService { get; set; }

        private ToasterService toasterService => _toasterService!;

        protected override void OnInitialized()
        {
            this.toasterService.ToasterChanged += ToastChanged;
            this.toasterService.ToasterTimerElapsed += ToastChanged;
        }

        private void ClearToast(Toast toast)
            => toasterService.RemoveToast(toast);

        private void ToastChanged(object? sender, EventArgs e)
            => this.InvokeAsync(this.StateHasChanged);

        public void Dispose()
        {
            this.toasterService.ToasterChanged -= ToastChanged;
            this.toasterService.ToasterTimerElapsed -= ToastChanged;
        }

        private string toastCss(Toast toast)
        {
            return "bg-Primary text-white";
        }
    }
}