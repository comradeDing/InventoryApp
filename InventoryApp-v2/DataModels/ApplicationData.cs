using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using PsWebServiceLib;

namespace InventoryApp_v2.DataModels
{
    public class ApplicationData : Application
    {
        
        public static SessionController Session { get; private set; }

        public static async Task FetchMasterList()
        {
            var success = await DataSource.CacheMasterList();
            Log.Debug("InventoryApp.ApplicationData", success ? "Master list cached" : "Failure to cache master list");
        }

        public static async void NewSession(SessionType session, InventoryCountType count)
        {
            Session = new SessionController(session, count);
        }

        public static async Task<string> GetMasterCache()
        {
            return await DataSource.GetMasterListFromDisk(true);
        }

    }
}