using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PsWebServiceLib;
using ReactiveUI;

namespace InventoryApp_v2.ViewModels
{
    public class MainViewModel : ReactiveObject
    {

        public Action<SessionType, InventoryCountType> NextActivityIntent { get; set; }

        #region Commands
        public ReactiveCommand<InventoryCountType, Unit> NewSessionCommand { get; set; }
        public ReactiveCommand<InventoryCountType, Unit> LoadSessionCommand { get; set; }
        #endregion Commands

        public MainViewModel()
        {
            NewSessionCommand =
                ReactiveCommand.Create<InventoryCountType>(countType => NextActivityIntent(SessionType.NewSession, InventoryCountType.Daily));
            LoadSessionCommand =
                ReactiveCommand.Create<InventoryCountType>(countType => NextActivityIntent(SessionType.LastSession, InventoryCountType.Daily));
        }

    }
}