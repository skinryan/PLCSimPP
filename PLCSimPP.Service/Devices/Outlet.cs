using System;
using System.Collections.Generic;
using System.Text;
using CommonServiceLocator;
using BCI.PLCSimPP.Comm;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Events;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Service.Devices.StandardResponds;
using Prism.Events;

namespace BCI.PLCSimPP.Service.Devices
{
    [Serializable]
    public class Outlet : UnitBase
    {
        private readonly IEventAggregator mEvent;
        private Dictionary<string, Shelf> mShelfList = new Dictionary<string, Shelf>();
        /// <summary>
        /// Stored sample Count
        /// </summary>
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

        /// <inheritdoc />
        public override void OnReceivedMsg(string cmd, string content)
        {
            base.OnReceivedMsg(cmd, content);

            if (cmd == LcCmds._0012)
            {
                base.MoveSample();
            }

            if (cmd == LcCmds._0017)
            {
                string floor = content.Substring(16, 1);
                string rack = content.Substring(17, 1);
                string position = content.Substring(18, 3);

                var msg = SendMsg.GetMsg1015(this, content);
                mSendBehavior.PushMsg(msg);

                StoreSample(floor, rack, position, CurrentSample);
                CurrentSample = null;
            }

            if (cmd == LcCmds._0018)
            {
                string floor = content.Substring(0, 1);
                string rack = content.Substring(1, 1);
                var msg = SendMsg.GetMsg1016(this, floor, rack);
                mSendBehavior.PushMsg(msg);

                EmptyTargetRack(floor, rack);
            }
        }

        /// <summary>
        /// clear target rack
        /// </summary>
        /// <param name="floor">floor id</param>
        /// <param name="rack">rack id</param>
        private void EmptyTargetRack(string floor, string rack)
        {
            if (mShelfList.ContainsKey(floor))
            {
                if (mShelfList[floor].RackList.ContainsKey(rack))
                {
                    mShelfList[floor].RackList.Remove(rack);
                }
            }

            RaisePropertyChanged("StoredCount");
        }

        /// <summary>
        /// Store Sample to target position
        /// </summary>
        /// <param name="shelf"></param>
        /// <param name="rack"></param>
        /// <param name="position"></param>
        /// <param name="sample"></param>
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
            mEvent.GetEvent<NotifyOnlineSampleEvent>().Publish(-1);
        }

        /// <summary>
        /// on rack exchange 
        /// </summary>
        /// <param name="rep"></param>
        private void OnRackExchange(RackExchangeParam rep)
        {
            if (rep.Address == this.Address)
            {
                EmptyTargetRack(rep.Shelf, rep.Rack);
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public Outlet() : base()
        {
            if (ServiceLocator.IsLocationProviderSet)
            {
                mEvent = ServiceLocator.Current.GetInstance<IEventAggregator>();
                mEvent.GetEvent<NotifyRackExchangeEvent>().Subscribe(OnRackExchange);
            }
        }
    }
}
