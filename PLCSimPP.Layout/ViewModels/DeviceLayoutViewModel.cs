using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Comm.Interfaces.Services;
using PLCSimPP.Comm.Models;
using PLCSimPP.Service.Devicies;
using Prism.Commands;
using Prism.Mvvm;

namespace PLCSimPP.Layout.ViewModels
{
    public class DeviceLayoutViewModel : BindableBase
    {
        private readonly IPipeLine mPipeLineServ;

        public DelegateCommand DrawLayoutCmd { get; set; }

        public DelegateCommand LoadSampleCmd
        {
            get; set;
        }

        public DelegateCommand RackExchangeCmd
        {
            get; set;
        }

        public DelegateCommand ConnectCmd
        {
            get; set;
        }

        public DelegateCommand DisConnectCmd
        {
            get; set;
        }

        public ObservableCollection<IUnit> UnitCollection { get; set; }

        public DeviceLayoutViewModel(IPipeLine pipeline)
        {
            UnitCollection = new ObservableCollection<IUnit>();
            mPipeLineServ = pipeline;

            //TODO: get layout config.
            GetLayout();
            DrawLayoutCmd = new DelegateCommand(DrawLayout);
        }


        private void DrawLayout()
        {

            //var s = XmlConverter.Serialize(UnitCollection);
            //mLayoutServ.Write(UnitCollection.ToList());
        }

        //for debug test
        private void GetLayout()
        {
            HMOutlet hmoutlet = new HMOutlet() { Address = "111112222", Port = 1, DisplayName = "HMOutlet" };
            UnitCollection.Add(hmoutlet);
            HLane hmoutlet2 = new HLane() { Address = "111112222", Port = 2, DisplayName = "HLane" };
            UnitCollection.Add(hmoutlet2);
            ILane hmoutlet3 = new ILane() { Address = "111112222", Port = 3, DisplayName = "ILane" };
            UnitCollection.Add(hmoutlet3);
        }

        private void LoadSample()
        {
            throw new System.NotImplementedException();
        }

        private void RackExchange()
        {
            throw new System.NotImplementedException();
        }

        private void Connect()
        {
            throw new System.NotImplementedException();
        }

        private void DisConnect()
        {
            throw new System.NotImplementedException();
        }
    }
}
