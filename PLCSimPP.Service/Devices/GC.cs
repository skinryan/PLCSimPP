using System;
using System.Collections.Generic;
using System.Text;
using CommonServiceLocator;
using BCI.PLCSimPP.Comm;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Service.Analyzer;
using BCI.PLCSimPP.Service.Devices.StandardResponds;

namespace BCI.PLCSimPP.Service.Devices
{
    [Serializable]
    public class GC : UnitBase
    {
        private IAnalyzerSimService mDCSimService;
        public int InstrumentUnitNum { get; set; }

        /// <summary>
        /// Add GC sequence to handle msg
        /// </summary>
        /// <param name="cmd">command</param>
        /// <param name="content">command parameter</param>
        public override void OnReceivedMsg(string cmd, string content)
        {
            base.OnReceivedMsg(cmd, content);

            if (cmd == LcCmds._0011)
            {
                string bcr = content.Substring(0, 1);
                if (bcr == ParamConst.BCR_1)
                {
                    var msg = SendMsg.GetMsg1011(this, ParamConst.BCR_3);
                    this.mSendBehavior.PushMsg(msg);
                }

                if (bcr == ParamConst.BCR_3)
                {
                    var msg = SendMsg.GetMsg1011(this, ParamConst.BCR_2);
                    this.mSendBehavior.PushMsg(msg);
                }

                if (bcr == ParamConst.BCR_2)
                {
                    var msg = SendMsg.GetMsg1015(this);
                    this.mSendBehavior.PushMsg(msg);

                    var tubeId = CurrentSample.SampleID;
                    if (CurrentSample.IsSubTube)
                    {
                        tubeId = tubeId.Substring(0, tubeId.Length - 1);
                    }
                    mDCSimService.SendMsg(InstrumentUnitNum, CurrentSample.DcToken, tubeId);

                    base.MoveSample();
                }
            }

            if (cmd == LcCmds._0012)
            {
                base.MoveSample();
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        public GC() : base()
        {
            Init();
        }

        /// <summary>
        /// init unit
        /// </summary>
        private void Init()
        {
            if (ServiceLocator.IsLocationProviderSet)
            {
                mDCSimService = ServiceLocator.Current.GetInstance<IAnalyzerSimService>("DCSimService");
            }
        }
    }
}

