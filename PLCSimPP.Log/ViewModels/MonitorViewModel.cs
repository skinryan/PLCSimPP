using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Events;
using BCI.PLCSimPP.Comm.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace BCI.PLCSimPP.Log.ViewModels
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

        private ObservableCollection<MsgLog> mLogCollection;

        public ObservableCollection<MsgLog> LogCollection
        {
            get { return mLogCollection; }
            set { SetProperty(ref mLogCollection, value); }
        }

        public ICommand CancelCommand { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="eventAggr">auto inject</param>
        public MonitorViewModel(IEventAggregator eventAggr)
        {
            mEventAggr = eventAggr;

            LogCollection = new ObservableCollection<MsgLog>();
            mEventAggr.GetEvent<MonitorEvent>().Subscribe(AddToMonitor, ThreadOption.UIThread);

            CancelCommand = new DelegateCommand<string>(viewName =>
            {
                mEventAggr.GetEvent<NavigateEvent>().Publish(ViewName.DEVICE_LAYOUT);
            });
        }

        private void AddToMonitor(MsgLog msgLog)
        {
            if (!FreezeScreen)
            {
                if (LogCollection.Count > 10000)
                {
                    LogCollection.RemoveAt(0);
                }

                LogCollection.Add(msgLog);
            }
        }
    }
}
