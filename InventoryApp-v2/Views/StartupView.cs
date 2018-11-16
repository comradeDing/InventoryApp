using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using InventoryApp_v2.DataModels;

namespace InventoryApp_v2.Views
{
    [Activity(Label = "StartupView", MainLauncher = true)]
    public class StartupView : Activity
    {
        private bool _loadingComplete = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.startup_view);
            // Create your application here
        }

        protected override void OnResume()
        {
            base.OnResume();
            Task.Run(async() =>
            {
                await ApplicationData.FetchMasterList();
                _loadingComplete = true;
            });

            Task.Run(() =>
            {
                while (!_loadingComplete) ;
                LaunchApplication();
                Finish();
            });
        }

        private void LaunchApplication()
        {
            var launchApp = new Intent(this, typeof(MainView));
            StartActivity(launchApp);
        }
    }
}