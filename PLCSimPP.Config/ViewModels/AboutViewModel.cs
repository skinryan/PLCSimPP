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
            var entry = Assembly.GetEntryAssembly();
            if (entry != null)
            {
                var versionInfo = FileVersionInfo.GetVersionInfo(entry.Location);
                mVersion = $"PLC Simulator {versionInfo.ProductVersion}X";
                mCopyRight = versionInfo.LegalCopyright;
            }
        }

    }
}
