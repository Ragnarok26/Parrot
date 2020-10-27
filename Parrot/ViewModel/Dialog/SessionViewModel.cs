using Parrot.ViewModel.Common;
using System;
using System.Windows.Input;
using System.Windows.Threading;

namespace Parrot.ViewModel.Dialog
{
    public class SessionViewModel : BaseViewModel
    {
        private DispatcherTimer _timer;

        private System.Windows.Window _view;

        private TimeSpan _elapsed;

        private string _labelText;

        public bool Continue;
        /// <summary>
        /// 
        /// </summary>
        public string LabelText
        {
            get => _labelText;
            set => SetValue(value, ref _labelText);
        }

        public ICommand LoadCommand { get; set; }

        public ICommand ContinueCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        public SessionViewModel()
        {
            LoadCommand = new RelayCommand<System.Windows.Window>(LoadEvent);
            ContinueCommand = new RelayCommand<object>(ContinueEvent);
            CloseCommand = new RelayCommand<object>(CloseEvent);
            _elapsed = new TimeSpan(0, 5, 0);
            LabelText = $"Su sessión Expirará en {_elapsed}";
            _timer = new DispatcherTimer(
                new TimeSpan(0, 0, 1),
                DispatcherPriority.Background,
                timer_Tick,
                Dispatcher.CurrentDispatcher
            );
            _timer.IsEnabled = true;
            _timer.Start();
        }

        private void LoadEvent(System.Windows.Window view)
        {
            _view = view;
        }

        private void ContinueEvent(object param)
        {
            _timer.Stop();
            Continue = true;
            _view.Close();
        }

        private void CloseEvent(object param)
        {
            _timer.Stop();
            Continue = false;
            _view.Close();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            _elapsed = _elapsed.Subtract(new TimeSpan(0, 0, 1));
            LabelText = $"Su sessión Expirará en {_elapsed}";
            if (_elapsed <= new TimeSpan(0, 0, 0))
            {
                _timer.Stop();
                CloseEvent(null);
            }
        }
    }
}
