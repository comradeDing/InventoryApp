using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ReactiveUI;

using PsWebServiceLib;

namespace InventoryApp_v2.ViewModels
{
    public class RackListViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment { get; }
        public IScreen HostScreen { get; }

        public SessionType SessionType { get; }
        public InventoryCountType CountType { get; }

        public RackListViewModel(SessionType sessionType, InventoryCountType countType)
        {
            SessionType = sessionType;
            CountType = countType;
        }
    }
}