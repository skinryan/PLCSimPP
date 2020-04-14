using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PLCSimPP.Comm;
using Prism.Mvvm;

namespace PLCSimPP.Config.ViewDatas
{
    public class UnitTypeInfo : BindableBase
    {
        private string mName;

        public string Name
        {
            get { return mName; }
            set { SetProperty(ref mName, value); }
        }

        private UnitType mValue;

        public UnitType Value
        {
            get { return mValue; }
            set { SetProperty(ref mValue, value); }
        }
    }
}
