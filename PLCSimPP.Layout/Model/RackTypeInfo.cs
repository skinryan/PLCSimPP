using PLCSimPP.Comm;
using Prism.Mvvm;

namespace PLCSimPP.Layout.Model
{
    public class RackTypeInfo : BindableBase
    {
        private string mName;

        public string Name
        {
            get { return mName; }
            set { SetProperty(ref mName, value); }
        }

        private RackType mValue;

        public RackType Value
        {
            get { return mValue; }
            set { SetProperty(ref mValue, value); }
        }
    }
}