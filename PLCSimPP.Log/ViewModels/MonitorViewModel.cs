﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using PLCSimPP.Comm.Constant;
using PLCSimPP.Comm.Events;
using PLCSimPP.Comm.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace PLCSimPP.Log.ViewModels
{
    public class MonitorViewModel : BindableBase
    {
        private readonly IEventAggregator mEventAggr;

        private bool mFreezeScreen = false;

        public bool FreezeScreen
        {
            get { return mFreezeScreen; }
            set { SetProperty(ref mFreezeScreen, value); }
        }

        private ObservableCollection<MsgLog> LogCollection { get; set; }

        public ICommand CancelCommand { get; set; }

        public MonitorViewModel(IEventAggregator eventAggr)
        {
            mEventAggr = eventAggr;

            LogCollection = new ObservableCollection<MsgLog>();
            mEventAggr.GetEvent<MonitorEvent>().Subscribe(AddToMonitor);

            CancelCommand = new DelegateCommand<string>(viewName =>
            {
                mEventAggr.GetEvent<NavigateEvent>().Publish("DeviceLayout");
            });
        }

        private void AddToMonitor(MsgLog msgLog)
        {
            if (!FreezeScreen)
            {
                if (LogCollection.Count > 10000)
                {
                    LogCollection.RemoveAt(0);
                    LogCollection.Add(msgLog);
                }
            }
        }
    }
}
