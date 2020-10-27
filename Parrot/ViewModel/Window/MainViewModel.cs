using Entity.Response;
using Entity.Restaurant;
using Entity.Token;
using Parrot.Model;
using Parrot.View.Dialog;
using Parrot.View.Window;
using Parrot.ViewModel.Common;
using Parrot.ViewModel.Dialog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;

namespace Parrot.ViewModel.Window
{
    /// <summary>
    /// ViewModel for Main View <em>(Extends of <strong>BaseViewModel</strong>)</em>
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        #region Internal Properties
        /// <summary>
        /// Background worker to do something in background thread
        /// </summary>
        private BackgroundWorker _worker;

        /// <summary>
        /// 
        /// </summary>
        private DispatcherTimer _timer;

        /// <summary>
        /// 
        /// </summary>
        private System.Windows.Window _window;
        #endregion

        #region Bindable Properties
        private ObservableCollection<Store> _stores;
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Store> Stores
        {
            get => _stores;
            set => SetValue(value, ref _stores);
        }
        #endregion

        #region Commands
        /// <summary>
        /// Main Window Load Command
        /// </summary>
        public ICommand MainLoadCommand { get; set; }

        /// <summary>
        /// Toggle Checked Command
        /// </summary>
        public ICommand CheckedCommand { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public MainViewModel()
        {
            MainLoadCommand = new RelayCommand<System.Windows.Window>(LoadEvent);
            CheckedCommand = new RelayCommand<object[]>(CheckedEvent);
        }
        #endregion

        #region Command Methods
        /// <summary>
        /// Main window load command event
        /// </summary>
        /// <param name="param">Parameter for event</param>
        private void LoadEvent(System.Windows.Window param)
        {
            _window = param;
            SetTimer();
        }

        /// <summary>
        /// Toggle Checked command event
        /// </summary>
        /// <param name="param">Parameter for event</param>
        private void CheckedEvent(object[] param)
        {
            using (var logic = new Logic.Restaurant.Restaurant())
            {
                var response = logic.UpdateProduct(AppManager.TokenData.Access, (Guid)param.Last(), param.First().ToString());
                if (response.Success)
                {
                    var prod =
                    Stores
                    .First(
                        store =>
                        store.Categories.Any(
                            category =>
                            category.Uuid == response.Result.Category.Uuid
                        )
                    )
                    .Categories
                    .First(
                        category =>
                        category.Uuid == response.Result.Category.Uuid
                    )
                    .Products
                    .First(
                        product =>
                        product.Uuid == response.Result.Uuid
                    );
                    prod.AlcoholCount = response.Result.AlcoholCount;
                    prod.Availability = response.Result.Availability;
                    prod.Description = response.Result.Description;
                    prod.ImageUrl = response.Result.ImageUrl;
                    prod.LegacyId = response.Result.LegacyId;
                    prod.Name = response.Result.Name;
                    prod.Price = response.Result.Price;
                    prod.ProviderAvailability = response.Result.ProviderAvailability;
                    prod.SoldAlone = response.Result.SoldAlone;
                }
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Response<Store> ProcessStores()
        {
            Response<Store> response = null;
            List<Store> stores = new List<Store>();
            foreach (var store in AppManager.UserStoreInfo.Stores)
            {
                using (var logic = new Logic.Restaurant.Restaurant())
                {
                    response = logic.GetCategoriesByStore(AppManager.TokenData.Access, store);
                    if (response.Success)
                    {
                        stores.Add(response.Result);
                    }
                }
            }
            Stores =
                new ObservableCollection<Store>(stores);
            return response;
        }

        private void SetTimer()
        {
            TimeSpan elapsed = DateTime.Now - AppManager.ExpirationDate.Value;
            if (elapsed > new TimeSpan(0, 30, 0))
            {
                CloseSession();
            }
            else if (elapsed > new TimeSpan(0, 25, 0))
            {
                ShowDialog();
            }
            else
            {
                _timer = new DispatcherTimer(
                    new TimeSpan(0, 25, 0) - elapsed,
                    DispatcherPriority.Background,
                    timer_Tick,
                    Dispatcher.CurrentDispatcher
                );
                _timer.IsEnabled = true;
                _timer.Start();
                Response<Store> response = null;
                _worker = new BackgroundWorker();
                _worker.DoWork += (s, e) => {
                    response = ProcessStores();
                };
                _worker.RunWorkerAsync();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            ShowDialog();
        }

        private void ShowDialog()
        {
            var dialog = new Session
            {
                DataContext = new SessionViewModel()
            };
            dialog.Owner = _window;
            dialog.ShowDialog();
            if (((SessionViewModel)dialog.DataContext).Continue)
            {
                using (var logic = new Logic.Restaurant.Restaurant())
                {
                    Response<TokenData> response = logic.Refresh(AppManager.TokenData);
                    if (response.Success)
                    {
                        AppManager.TokenData = response.Result;
                        AppManager.ExpirationDate = DateTime.Now;
                        SetTimer();
                    }
                    else
                    {
                        CloseSession();
                    }
                }
            }
            else
            {
                CloseSession();
            }
        }

        private void CloseSession()
        {
            AppManager.ExpirationDate = null;
            AppManager.TokenData = null;
            AppManager.UserStoreInfo = null;
            var loginView = new Login();
            loginView.Show();
            _window.Close();
        }
        #endregion
    }
}
