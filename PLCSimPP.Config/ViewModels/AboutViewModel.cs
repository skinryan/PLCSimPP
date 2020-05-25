using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Prism.Mvvm;

namespace BCI.PLCSimPP.Config.ViewModels
{
    public class AboutViewModel : BindableBase
    {
        private string mVersion;
        /// <summary>
        /// Version
        /// </summary>
        public string Version
        {
            get { return mVersion; }
            set
            {
                mVersion = value;
                SetProperty(ref mVersion, value);
            }
        }


        private string mCopyRight;
        /// <summary>
        /// CopyRight
        /// </summary>
        public string CopyRight
        {
            get { return mCopyRight; }
            set
            {
                mCopyRight = value;
                SetProperty(ref mCopyRight, value);
            }
        }        

        public AboutViewModel()
        {
            var versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

            mVersion = string.Format("PLC Simulator {0}", versionInfo.ProductVersion);
            mCopyRight = versionInfo.LegalCopyright;
        }
        
    }
}
