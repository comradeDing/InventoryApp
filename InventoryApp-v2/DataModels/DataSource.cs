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
    public class DataSource
    {
        private const string _webApiUri = "http://mfg-fs:5482/InventoryService/";
        private const string _serializedFileName = "lastMasterList.json";
        private readonly Dictionary<string, string> _commands = new Dictionary<string, string>()
        {
            { "PingSource", "PingDevice" },
            { "GetMasterList", "/GetData/RackSummary"},
            { "GetParentList", "/GetData/ParentParts" },
            { "PostSummary", "/PostData/summary.json" }
        };

        public async Task<bool> PingSource()
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

        public async Task<string> GetMasterListFromWeb()
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

        public async Task<string> GetMasterListFromDisk()
        {
            var baseDir = Application.Context.FilesDir.AbsolutePath;
            var filePath = Path.Combine(baseDir, _serializedFileName);

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

        public async Task WriteMasterListToDisk(string serializedMasterList)
        {
            var baseDir = Application.Context.FilesDir.AbsolutePath;
            var savePath = Path.Combine(baseDir, _serializedFileName);

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