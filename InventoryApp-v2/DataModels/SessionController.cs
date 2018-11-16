using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DynamicData;
using Newtonsoft.Json;
using PsWebServiceLib;
using PsWebServiceLib.Models.InventoryAppV2;

namespace InventoryApp_v2.DataModels
{
    public class SessionController
    {

        private readonly SessionType _sessionType;
        private readonly InventoryCountType _countType;

        private SourceList<Part> _masterList;

        public SessionController(SessionType st, InventoryCountType ct)
        {
            _masterList = new SourceList<Part>();
            _sessionType = st;
            _countType = ct;
        }

    }
}