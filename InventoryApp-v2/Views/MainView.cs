using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using InventoryApp_v2.DataModels;
using InventoryApp_v2.ViewModels;
using PsWebServiceLib;
using ReactiveUI;

namespace InventoryApp_v2.Views
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class MainView : Activity, IViewFor<MainViewModel>
    {

        public Button NewSession { get; set; }
        public Button LoadSession { get; set; }

        #region ViewModel
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (MainViewModel)value;
        }
        public MainViewModel ViewModel { get; set; }
        #endregion ViewModel

        #region Lifecycle Hooks
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.main_view);

            ViewModel = new MainViewModel { NextActivityIntent = StartNextActivity };
        }

        protected override void OnResume()
        {
            base.OnResume();
#if DEBUG
            Task.Run(async () =>
            {
                var result = await ApplicationData.GetMasterCache();
                Log.Debug("InventoryApp.MainView", result);
            });
#endif

            // TODO subscribe to view model data
            this.WireUpControls();
            this.BindCommand(ViewModel, vm => vm.NewSessionCommand, v => v.NewSession);
            this.BindCommand(ViewModel, vm => vm.LoadSessionCommand, v => v.LoadSession);

        }

        protected override void OnPause()
        {
            base.OnPause();
            // TODO unsubscribe from view model data
        }
        #endregion Lifecycle Hooks

        private void StartNextActivity(SessionType st, InventoryCountType ct)
        {
            var intent = new Intent(this, typeof(RackListView));
            intent.PutExtra("CountType", ct.ToString());
            StartActivity(intent);
        }
    }
}

