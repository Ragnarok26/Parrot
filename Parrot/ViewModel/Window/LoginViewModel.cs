using Entity.Response;
using Entity.Token;
using Entity.User;
using MaterialDesignThemes.Wpf;
using Parrot.Model;
using Parrot.View.Window;
using Parrot.ViewModel.Common;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Parrot.ViewModel.Window
{
    /// <summary>
    /// ViewModel for Login View <em>(Extends of <strong>BaseViewModel</strong>)</em>
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        #region Internal Properties
        /// <summary>
        /// Background worker to do something in background thread
        /// </summary>
        private BackgroundWorker _worker;
        #endregion

        #region Bindable Properties
        private string _userText;
        public string UserText
        {
            get => _userText;
            set => SetValue(value, ref _userText);
        }

        private string _passwordText;
        public string PasswordText
        {
            get => _passwordText;
            set => SetValue(value, ref _passwordText);
        }

        private string _errorText;
        public string ErrorText
        {
            get => _errorText;
            set => SetValue(value, ref _errorText);
        }

        //private ObservableCollection<SeqPackage> _seqPackageItems;
        ///// <summary>
        ///// 
        ///// </summary>
        //public ObservableCollection<SeqPackage> SeqPackagesItems
        //{
        //    get => _seqPackageItems;
        //    set
        //    {
        //        if (SetValue(value, "SeqPackagesItems", ref _seqPackageItems)) return;
        //    }
        //}
        #endregion

        #region Commands
        /// <summary>
        /// Login Command
        /// </summary>
        public ICommand LoginCommand { get; set; }

        /// <summary>
        /// Close Command
        /// </summary>
        public ICommand CloseCommand { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public LoginViewModel()
        {
            #region Bindable Properties Initialization
            //ToolboxVisibility = Visibility.Collapsed;
            UserText = string.Empty;
            PasswordText = string.Empty;
            #endregion

            #region Command Initialization
            LoginCommand = new RelayCommand<object[]>(LoginClick);
            CloseCommand = new RelayCommand<object>(CloseClick);
            #endregion
        }
        #endregion

        #region Command Methods
        /// <summary>
        /// Login button clicked command event
        /// </summary>
        /// <param name="loginView">Parameter for event</param>
        private async void LoginClick(object[] param)
        {
            ErrorText = string.Empty;
            Login loginView = (Login)param.First();
            Response<TokenData> response = null;
            _worker = new BackgroundWorker();
            _worker.DoWork += (s, e) => {
                using (var logic = new Logic.Restaurant.Restaurant())
                {
                    var user = new User
                    {
                        Username = UserText,
                        Password = PasswordText
                    };
                    response = logic.Login(user);
                    if (response.Success)
                    {
                        AppManager.TokenData = response.Result;
                        AppManager.ExpirationDate = DateTime.Now;
                        var resp = logic.GetStores(AppManager.TokenData.Access);
                        if (resp.Success)
                        {
                            AppManager.UserStoreInfo = resp.Result;
                        }
                    }
                }
            };
            _worker.RunWorkerCompleted += (s, e) => {
                DialogHost.CloseDialogCommand.Execute(response, null);
            };
            var result = await DialogHost.Show(
                param.Last(), 
                "LoaderDialog", 
                (s, e) => { _worker.RunWorkerAsync(); }, 
                (s, e) => { ClosingEventHandler(e, loginView); }
            );
        }

        /// <summary>
        /// Close button clicked command event
        /// </summary>
        /// <param name="obj">Parameter for event</param>
        private void CloseClick(object param)
        {
            System.Windows.Application.Current.Shutdown();
        }
        #endregion

        #region Dialog Closing Events
        /// <summary>
        ///
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <param name="loginView"></param>
        public void ClosingEventHandler(DialogClosingEventArgs eventArgs, System.Windows.Window loginView)
        {
            var parameter = (Response<TokenData>)eventArgs.Parameter;
            if (parameter.Success == false)
            {
                ErrorText = $"{parameter.Message}";
                return;
            }
            eventArgs.Cancel();
            var mainView = new MainWindow();
            mainView.Show();
            loginView.Close();
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
