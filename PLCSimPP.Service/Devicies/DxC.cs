using System;
using System.Collections.Generic;
using System.Text;
using CommonServiceLocator;
using PLCSimPP.Comm;
using PLCSimPP.Comm.Constants;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Models;
using PLCSimPP.Service.Analyzer;
using PLCSimPP.Service.Devicies.StandardResponds;

namespace PLCSimPP.Service.Devicies
{
    [Serializable]
    public class DxC : UnitBase
    {
        private DxCSimService mDxCSimService;
        public int InstrumentUnitNum { get; set; }

        public override void OnReceivedMsg(string cmd, string content)
        {
            base.OnReceivedMsg(cmd, content);

            if (cmd == LcCmds._0011)
            {
                string bcr = content.Substring(1, 1);
                if (bcr == ParamConst.BCR_1)
                {
                    var msg = SendMsg.GetMsg_1011(this, ParamConst.BCR_2);
                    this.mSendBehavior.PushMsg(msg);
                }
                if (bcr == ParamConst.BCR_2)
                {
                    var msg = SendMsg.GetMsg_1015(this);
                    this.mSendBehavior.PushMsg(msg);

                    mDxCSimService.SendMsg(InstrumentUnitNum, CurrentSample.DxCToken, CurrentSample.SampleID);

                    base.MoveSample();
                }
            }

            if (cmd == LcCmds._0012)
            {
                base.MoveSample();
            }
        }


        public DxC() : base()
        {
            Init();
        }

        private void Init()
        {
            mDxCSimService = ServiceLocator.Current.GetInstance<DxCSimService>();
        }
    }
}
