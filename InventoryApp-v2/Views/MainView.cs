using System;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using InventoryApp_v2.DataModels;
using InventoryApp_v2.ViewModels;
using ReactiveUI;

namespace InventoryApp_v2.Views
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class MainView : Activity, IViewFor<MainViewModel>
    {
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (MainViewModel)value;
        }
        public MainViewModel ViewModel { get; set; }

        public Task GetMasterCache;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.main_view);
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
        }

        protected override void OnPause()
        {
            base.OnPause();
            // TODO unsubscribe from view model data
        }


    }
}

