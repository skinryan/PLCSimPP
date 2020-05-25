using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
    public class Stocker : UnitBase
    {
        private readonly IEventAggregator mEvent;
        private Dictionary<string, Shelf> mShelfList = new Dictionary<string, Shelf>();

        protected ISample mRetrievingSample;
        public ISample RetrievingSample
        {
            get { return mRetrievingSample; }
            set { this.SetProperty(ref mRetrievingSample, value); }
        }

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
            base.OnReceivedMsg(cmd, content);

            if (cmd == LcCmds._0012)
            {
                //reply 1011 bcr2
                string bcr = content.Substring(0, 1);
                if (bcr == ParamConst.BCR_1)
                {
                    var msg = SendMsg.GetMsg_1011(this, ParamConst.BCR_2);
                    mSendBehavior.PushMsg(msg);
                }
            }

            if (cmd == LcCmds._0011)
            {
                string bcr = content.Substring(0, 1);
                string sid = content.Substring(1, 15);
                if (bcr == ParamConst.BCR_2)
                {
                    if (CurrentSample != null && CurrentSample.SampleID == sid.Trim())
                    {
                        base.MoveSample();
                    }
                    else
                    {
                        RetriveSample(sid.Trim());
                    }
                }
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

            if (cmd == LcCmds._0018)
            {
                string floor = content.Substring(0, 1);
                string rack = content.Substring(1, 1);
                var msg = SendMsg.GetMsg_1016(this, floor, rack);
                mSendBehavior.PushMsg(msg);

                EmptyTargetRack(floor, rack);
            }

            if (cmd == LcCmds._0019)
            {
                var msg1026 = SendMsg.GetMsg_1026(this, "1");//'0': Holder Shortage cleared, '1': Holder Shortage occurred
                mSendBehavior.PushMsg(msg1026);

                string sid = content.Substring(0, 15);
                string floor = content.Substring(15, 1);
                string rack = content.Substring(16, 1);
                string position = content.Substring(17, 3);

                MsgCmd msg = new MsgCmd();
                msg.Command = UnitCmds._1019;
                msg.Port = this.Port;
                msg.UnitAddr = this.Address;
                msg.Param = $"{sid.PadRight(15)}{floor}{rack}{position}0"; //last number '0': Normal, '1': Error
                mSendBehavior.PushMsg(msg);

                string param = ParamConst.BCR_2 + sid;
                var arrivalMsg = new MsgCmd()
                {
                    Command = UnitCmds._1011,
                    Param = param,
                    Port = this.Port,
                    UnitAddr = this.Address
                };
                this.mSendBehavior.PushMsg(arrivalMsg);
            }

            if (cmd == LcCmds._0026)
            {
                var msg1026 = SendMsg.GetMsg_1026(this, "0");//'0': Holder Shortage cleared, '1': Holder Shortage occurred
                mSendBehavior.PushMsg(msg1026);
            }
        }


        private void RetriveSample(string sid)//string floor, string rack, string position
        {
            bool isfind = false;
            //string pos = string.Empty;
            //Dictionary<string, ISample> dict;
            foreach (var shelf in mShelfList.Values)
            {
                if (isfind) break;
                foreach (var rack in shelf.RackList.Values)
                {
                    if (isfind) break;
                    foreach (var sample in rack.SampleList)
                    {
                        if (sample.Value.SampleID == sid)
                        {
                            isfind = true;
                            //dict = rack.SampleList;

                            var dest = mRouterService.FindNextDestination(this);
                            dest.EnqueueSample(sample.Value);
                            rack.SampleList.Remove(sample.Key);
                            break;
                        }
                    }
                }
            }


            //if (!mShelfList.ContainsKey(floor)) return;
            //if (!mShelfList[floor].RackList.ContainsKey(rack)) return;
            //if (!mShelfList[floor].RackList[rack].SampleList.ContainsKey(position)) return;

            //var sample = mShelfList[floor].RackList[rack].SampleList[position];
            //sample.Retrieving = true;
            //this.RetrievingSample = sample;
            //this.EnqueueSample(sample);

            //mShelfList[floor].RackList[rack].SampleList.Remove(position);
            RaisePropertyChanged("StoredCount");
        }

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

        protected override void OnSampleArrived()
        {
            base.OnSampleArrived();
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

        public Stocker() : base()
        {
            if (ServiceLocator.IsLocationProviderSet)
            {
                mEvent = ServiceLocator.Current.GetInstance<IEventAggregator>();
            }
        }
    }

    public class Shelf
    {
        public Dictionary<string, Rack> RackList { get; set; } = new Dictionary<string, Rack>();
    }

    public class Rack
    {
        public Dictionary<string, ISample> SampleList { get; set; } = new Dictionary<string, ISample>();
    }

    enum Flag
    {
        Normal = 0,
        Pass = 1,
        SamplePassedForErrorRecoveryPerformed = 9
    }
}
