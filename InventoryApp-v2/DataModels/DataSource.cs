using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Application = Android.App.Application;
using Exception = System.Exception;
using File = System.IO.File;

namespace InventoryApp_v2.DataModels
{
    public static class DataSource
    {
#if !DEBUG
        private const string _webApiUri = "http://mfg-fs:5482/InventoryService/";
#else
        private const string _webApiUri = "http://edingeldein:5482/InventoryService/";
#endif
        private const string _serializedFileName = "masterSaved.json";
        private const string _cachedMasterList = "masterCache.json";
        private static readonly Dictionary<string, string> _commands = new Dictionary<string, string>()
        {
            { "PingSource", "PingDevice" },
            { "GetMasterList", "/GetData/RackSummary"},
            { "GetParentList", "/GetData/ParentParts" },
            { "PostSummary", "/PostData/summary.json" }
        };

        public static async Task<bool> PingSource()
        {
            try
            {
                var request = WebRequest.Create(_webApiUri + _commands["PingSource"]) as HttpWebRequest;
                request.Method = "GET";
                request.KeepAlive = false;

                var pingSource = Task.Run(() =>
                {
                    using (var response = request.GetResponse() as HttpWebResponse)
                    using (var responseStream = new StreamReader(response.GetResponseStream()))
                    {
                        var result = responseStream.ReadToEnd();
                        Log.Debug("InventoryApp.DataSource", "Response from service: " + result);
                        return result.Equals("true");
                    }
                });

                return await pingSource;
            }
            catch (Exception e)
            {
                Log.Error("InventoryApp.DataSource", "Error pinging source:\t" + e.Message);
                return false;
            }
        }

        public static async Task<bool> CacheMasterList()
        {
            try
            {
                var masterList = await GetMasterListFromWeb();
                await WriteMasterListToDisk(masterList, true);
            }
            catch (Exception e)
            {
                Log.Error("InventoryApp.DataSource", "Error caching master list: " + e.Message);
                return false;
            }

            return true;
        }

        public static async Task<string> GetMasterListFromWeb()
        {
            var request = WebRequest.Create(_webApiUri + _commands["GetMasterList"]) as HttpWebRequest;
            request.Method = "GET";
            request.KeepAlive = false;
            request.Timeout = 12000;

            try
            {
                using (var response = await request.GetResponseAsync())
                using (var responseStream = new StreamReader(response.GetResponseStream()))
                {
                    var content = responseStream.ReadToEnd();
                    return content;
                }
            }
            catch (Exception e)
            {
                Log.Error("InventoryApp.DataSource", "Error getting master list:\t" + e.Message);
                return null;
            }
        }

        public static async Task<string> GetMasterListFromDisk(bool isCache)
        {
            var baseDir = Application.Context.FilesDir.AbsolutePath;
            var filePath = Path.Combine(baseDir, (isCache) ? _cachedMasterList : _serializedFileName);

            try
            {
                var readFile = Task.Run(() => File.ReadAllText(filePath));
                return await readFile;
            }
            catch (Exception e)
            {
                Log.Error("InventoryApp.DataSource", "Error reading master list from file: " + e.Message);
                return null;
            }

        }

        public static async Task WriteMasterListToDisk(string serializedMasterList, bool isCache)
        {
            var baseDir = Application.Context.FilesDir.AbsolutePath;
            var savePath = Path.Combine(baseDir, (isCache) ? _cachedMasterList : _serializedFileName);
            Log.Debug("InventoryApp.DataSource", "Master list file path: " + savePath);
            try
            {
                var writeFile = Task.Run(() => File.WriteAllText(savePath, serializedMasterList));
                await writeFile;
            }
            catch (Exception e)
            {
                Log.Error("InventoryApp.DataSource", "Error writing master list to disk: " + e.Message);
            }
        }

    }
}