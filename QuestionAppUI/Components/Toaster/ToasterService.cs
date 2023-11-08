using System.Timers;
using System.Xml.Serialization;

namespace QuestionAppUI.Toaster
{
    public class ToasterService : IDisposable
    {
        private List<Toast> _toasts = new List<Toast>();
        private System.Timers.Timer _timer = new System.Timers.Timer();
        public event EventHandler? ToasterChanged;
        public event EventHandler? ToasterTimerElapsed;
        public bool HasToasts => _toasts.Count > 0;

        public ToasterService() 
        {
            _timer.Interval = 1500;
            _timer.AutoReset = true;
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        public List<Toast> GetToasts()
        {
            ClearBurntToast();
            return _toasts;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            ClearBurntToast();
            ToasterTimerElapsed?.Invoke(this, EventArgs.Empty);
        }

        public void AddToast(Toast toast) 
        {
            _toasts.Add(toast);
            if (!ClearBurntToast())
                ToasterChanged?.Invoke(this, EventArgs.Empty);
        }

        public void RemoveToast(Toast toast)
        {
            if (_toasts.Contains(toast))
            {
                _toasts.Remove(toast);
                if (!ClearBurntToast())
                    ToasterChanged?.Invoke(this, EventArgs.Empty);
            }
        
        }

        public bool ClearBurntToast() 
        {
            var toastToDelete = _toasts.Where(x => x.IsBurnt).ToList();
            if (toastToDelete != null && toastToDelete.Any()) 
            {
                toastToDelete.ForEach(toast => _toasts.Remove(toast));
                ToasterChanged?.Invoke(this, EventArgs.Empty);
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Elapsed += TimerElapsed;
                _timer.Stop();
            }
        }
    }
}
