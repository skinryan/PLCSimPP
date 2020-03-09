using System;
using System.Collections.Generic;
using System.Text;
using PLCSimPP.Comm.Interfaces;
using Prism.Mvvm;

namespace PLCSimPP.Comm.Models
{
    public class Sample : BindableBase, ISample
    {
        private string _sampleID;

        /// <inheritdoc/>
        public string SampleID
        {
            get { return _sampleID; }
            set { SetProperty(ref _sampleID, value); }
        }


        public override string ToString()
        {
            return base.ToString();
        }
    }
}
