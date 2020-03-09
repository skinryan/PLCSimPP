using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using PLCSimPP.Comm.Models;
using Prism.Mvvm;

namespace PLCSimPP.Config.ViewModels
{
    public class ConfigruationViewModel : BindableBase
    {
        private ClientConfigInfo mConfigInfo;

        public ICommand SaveConfigCmd { get; set; }

        public ConfigruationViewModel()
        {

        }

        private void DoSaveConfig()
        {

        }

        private void LoadSysConfig()
        {

        }
    }
}
