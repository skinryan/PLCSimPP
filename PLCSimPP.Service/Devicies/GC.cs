using System;
using System.Collections.Generic;
using System.Text;
using CommonServiceLocator;
using BCI.PLCSimPP.Comm;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Service.Analyzer;
using BCI.PLCSimPP.Service.Devicies.StandardResponds;

namespace BCI.PLCSimPP.Service.Devicies
{
    [Serializable]
    public class GC : UnitBase
    {
        private DCSimService mDCSimService;
        public int InstrumentUnitNum { get; set; }

        public override void OnReceivedMsg(string cmd, string content)
        {
            base.OnReceivedMsg(cmd, content);

            if (cmd == LcCmds._0011)
            {
                string bcr = content.Substring(0, 1);
                if (bcr == ParamConst.BCR_1)
                {
                    var msg = SendMsg.GetMsg_1011(this, ParamConst.BCR_3);
                    this.mSendBehavior.PushMsg(msg);
                }

                if (bcr == ParamConst.BCR_3)
                {
                    var msg = SendMsg.GetMsg_1011(this, ParamConst.BCR_2);
                    this.mSendBehavior.PushMsg(msg);
                }

                if (bcr == ParamConst.BCR_2)
                {
                    var msg = SendMsg.GetMsg_1015(this);
                    this.mSendBehavior.PushMsg(msg);

                    var tubeid = CurrentSample.SampleID;
                    if (CurrentSample.IsSubTube)
                    {
                        tubeid = tubeid.Substring(0, tubeid.Length - 1);
                    }
                    mDCSimService.SendMsg(InstrumentUnitNum, CurrentSample.DcToken, tubeid);

                    base.MoveSample();
                }
            }

            if (cmd == LcCmds._0012)
            {
                base.MoveSample();
            }
        }

        public GC() : base()
        {
            Init();
        }

        private void Init()
        {
            if (ServiceLocator.IsLocationProviderSet)
            {
                mDCSimService = ServiceLocator.Current.GetInstance<DCSimService>();
            }
        }
    }
}
