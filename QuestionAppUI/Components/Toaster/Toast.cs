namespace QuestionAppUI.Toaster
{

    public enum ToastAction
    {
        Close,
        Yes, 
        No, 
        Cancel
    }
    
    public enum ToastType
    {
        Alert,
        Info, 
        Warning,
        Default
    }

    public class Toast
    {
        public Guid Id = Guid.NewGuid();

        public ToastType ToastType { get; set; } = ToastType.Info;

        public string Header { get; init; } = string.Empty;

        public string Body { get; init; } = string.Empty;

        public readonly DateTimeOffset PostedTime = DateTimeOffset.Now;

        public DateTimeOffset TimeToClose { get; init; } = DateTimeOffset.Now.AddSeconds(30);

        public bool IsBurnt => TimeToClose < DateTimeOffset.Now;

        public string ElapsedTimeText =>
            elapsedTime.Seconds > 60
            ? $"posted {-elapsedTime.Minutes} mins ago"
            : $"posted {-elapsedTime.Seconds} secs ago";

        public List<ToastAction> Actions { get; init; } = new() { ToastAction.Close };

        private TimeSpan elapsedTime => PostedTime - DateTimeOffset.Now;

        /// <summary>
        /// Will create toast that has header, body and Close btn
        /// </summary>
        /// <param name="header"></param>
        /// <param name="body"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static Toast CreateToastDefault(string header, string body, int timeout)
            => new Toast
            {
                Header = header,
                Body = body,
                TimeToClose = DateTimeOffset.Now.AddMilliseconds(timeout),
                Actions = new List<ToastAction>() { ToastAction.Close },
                ToastType = ToastType.Default,
            };

        public static Toast CreateToastAlert(string body, int timeout, bool closeBtn = false)
            => new Toast
            {
                Body = body,
                TimeToClose = DateTimeOffset.Now.AddMilliseconds(timeout),
                Actions = closeBtn ? new List<ToastAction>() { ToastAction.Close } : new List<ToastAction>(),
                ToastType = ToastType.Alert
            };
    }
}
