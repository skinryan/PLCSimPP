using System;
using System.Collections.Generic;
using System.Text;
using CommonServiceLocator;
using BCI.PLCSimPP.Comm;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Events;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Service.Devicies.StandardResponds;
using Prism.Events;

namespace BCI.PLCSimPP.Service.Devicies
{
    [Serializable]
    public class HMOutlet : UnitBase
    {
        private IEventAggregator mEvent;
        private Dictionary<string, Shelf> mShelfList = new Dictionary<string, Shelf>();

        public int StoredCount
        {
            get
            {
                int result = 0;

                foreach (var shelf in mShelfList.Values)
                {
                    foreach (var rack in shelf.RackList.Values)
                    {
                        result += rack.SampleList.Count;
                    }
                }

                return result;
            }
        }

        public override void OnReceivedMsg(string cmd, string content)
        {
            mLogger.LogSys(cmd + "|" + content);

            base.OnReceivedMsg(cmd, content);

            if (cmd == LcCmds._0011 || cmd == LcCmds._0012)
            {
                base.MoveSample();
            }

            if (cmd == LcCmds._0017)
            {
                string floor = content.Substring(16, 1);
                string rack = content.Substring(17, 1);
                string position = content.Substring(18, 3);

                var msg = SendMsg.GetMsg_1015(this, content);
                mSendBehavior.PushMsg(msg);

                StoreSample(floor, rack, position, CurrentSample);

                CurrentSample = null;
            }
        }

        private void StoreSample(string shelf, string rack, string position, ISample sample)
        {
            if (!mShelfList.ContainsKey(shelf))
            {
                mShelfList[shelf] = new Shelf();
            }

            if (!mShelfList[shelf].RackList.ContainsKey(rack))
            {
                mShelfList[shelf].RackList[rack] = new Rack();
            }

            mShelfList[shelf].RackList[rack].SampleList[position] = sample;
            RaisePropertyChanged("StoredCount");
            RaisePropertyChanged("PendingCount");
            mEvent.GetEvent<NotifyOffLineEvent>().Publish(true);
        }

        public HMOutlet() : base()
        {
            if (ServiceLocator.IsLocationProviderSet)
            {
                mEvent = ServiceLocator.Current.GetInstance<IEventAggregator>();
            }            
        }
    }
}
