﻿using System;
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
    public class DxC : UnitBase
    {
        private IAnalyzerSimService mDxCSimService;
        /// <summary>
        /// Instrument number
        /// </summary>
        public int InstrumentUnitNum { get; set; }

        /// <inheritdoc />
        public override void OnReceivedMsg(string cmd, string content)
        {
            base.OnReceivedMsg(cmd, content);

            if (cmd == LcCmds._0011)
            {
                string bcr = content.Substring(0, 1);
                if (bcr == ParamConst.BCR_1)
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
                    mDxCSimService.SendMsg(InstrumentUnitNum, CurrentSample.DxCToken, CurrentSample.SampleID);//send token

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
        public DxC() : base()
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
                mDxCSimService = ServiceLocator.Current.GetInstance<IAnalyzerSimService>("DxCSimService");
            }
        }
    }
}
