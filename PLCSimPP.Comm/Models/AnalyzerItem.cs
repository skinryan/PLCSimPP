using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace BCI.PLCSimPP.Comm.Models
{
    /// <summary>
    /// Analyzer config item
    /// </summary>
    public class AnalyzerItem : BindableBase
    {
        private int mNum;

        /// <summary>
        /// instrument number
        /// </summary>
        public int Num 
        {
            get { return mNum; }
            set { SetProperty(ref mNum, value); }
        }


        private DcAnalyzerType mDcType;

        /// <summary>
        /// instrumetn type 
        /// </summary>
        public DcAnalyzerType DcType
        {
            get { return mDcType; }
            set { SetProperty(ref mDcType, value); }
        }

        private DxCAnalyzerType mDxCType;

        /// <summary>
        /// instrumetn type 
        /// </summary>
        public DxCAnalyzerType DxCType
        {
            get { return mDxCType; }
            set { SetProperty(ref mDxCType, value); }
        }
    }
}
