using System;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Comm;
using PLCSimPP.Comm.Constants;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Models;
using PLCSimPP.Service.Devicies.StandardResponds;

namespace PLCSimPP.Service.Devicies
{
    public class Stocker : UnitBase
    {

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

                var msg = SendMsg.GetMsg_1015(this, content);
                mSendBehavior.PushMsg(msg);

                StoreSample(floor, rack, position, CurrentSample);

                CurrentSample = null;
            }

            if (cmd == LcCmds._0019)
            {
                string sid = content.Substring(0, 15);
                string floor = content.Substring(16, 1);
                string rack = content.Substring(17, 1);
                string position = content.Substring(18, 3);
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
        }


        public Stocker() : base()
        {

        }
    }

    public class Shelf
    {
        public Dictionary<string, Rack> RackList
        {
            get; set;
        } = new Dictionary<string, Rack>();
    }

    public class Rack
    {
        public Dictionary<string, ISample> SampleList
        {
            get; set;
        } = new Dictionary<string, ISample>();
    }

    enum Flag
    {
        Normal = 0,
        Pass = 1,
        SamplePassedForErrorRecoveryPerformed = 9
    }
}
