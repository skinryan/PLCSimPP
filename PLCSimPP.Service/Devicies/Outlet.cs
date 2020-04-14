﻿using System;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Comm;
using PLCSimPP.Comm.Constants;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Models;
using PLCSimPP.Service.Devicies.StandardResponds;

namespace PLCSimPP.Service.Devicies
{
    [Serializable]
    public class Outlet : UnitBase
    {
        private Dictionary<string, Shelf> mShelfList = new Dictionary<string, Shelf>();

        public override void OnReceivedMsg(string cmd, string content)
        {
            base.OnReceivedMsg(cmd, content);

            if (cmd == LcCmds._0011)
            {
                //todo replay 1015
            }

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
        }


        public Outlet() : base()
        {

        }
    }
}
