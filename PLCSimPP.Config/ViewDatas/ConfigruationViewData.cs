using BCI.PLCSimPP.PresentationControls.ViewData;
using BCI.PLCSimPP.PresentationControls.ValidationAttributes;
using System.Collections.ObjectModel;
using BCI.PLCSimPP.PresentationControls.Controls;
using BCI.PLCSimPP.Comm.Models;

namespace BCI.PLCSimPP.Config.ViewDatas
{
    public class ConfigurationViewData : EditViewDataBase
    {
        private string mSiteMapFilePath;

        /// <summary>
        /// Gets or sets site map file path
        /// </summary>
        public string SiteMapFilePath
        {
            get { return mSiteMapFilePath; }
            set
            {
                if (value != mSiteMapFilePath)
                {
                    var temp = mSiteMapFilePath;
                    mSiteMapFilePath = value;
                    NotifyPropertyChanged(this, "SiteMapFilePath", temp, value);
                }
            }
        }


        private int mSendInterval;
        /// <summary>
        /// Gets or sets Send Interval
        /// </summary>
        [NumberRange(50, 1000, ErrorMessageResourceName = "SendIntervalInputErrorInfo", ErrorMessageResourceType = typeof(Properties.Resources))]
        public int SendInterval
        {
            get
            {
                return mSendInterval;
            }
            set
            {
                if (mSendInterval != value)
                {
                    var temp = mSendInterval;
                    mSendInterval = value;
                    NotifyPropertyChanged(this, "SendInterval", temp, value);

                }
            }
        }

        private string mDcSimLocation;

        /// <summary>
        /// DCSim file path
        /// </summary>
        public string DcSimLocation
        {
            get
            {
                return mDcSimLocation;
            }
            set
            {
                if (mDcSimLocation != value)
                {
                    var temp = mDcSimLocation;
                    mDcSimLocation = value;
                    NotifyPropertyChanged(this, "DcSimLocation", temp, value);
                }
            }
        }

        private string mDxCSimLocation;

        /// <summary>
        /// DCSim file path
        /// </summary>
        public string DxCSimLocation
        {
            get
            {
                return mDxCSimLocation;
            }
            set
            {
                if (mDxCSimLocation != value)
                {
                    var temp = mDxCSimLocation;
                    mDxCSimLocation = value;
                    NotifyPropertyChanged(this, "DxCSimLocation", temp, value);
                }
            }
        }

        private ObservableCollection<AnalyzerItem> mAnalyzerItems;

        /// <summary>
        /// Analyzer items collection
        /// </summary>
        public ObservableCollection<AnalyzerItem> AnalyzerItems
        {
            get
            {
                return mAnalyzerItems;
            }
            set
            {
                if (mAnalyzerItems != value)
                {
                    var temp = mAnalyzerItems;
                    mAnalyzerItems = value;
                    NotifyPropertyChanged(this, "AnalyzerItems", temp, value);

                }
            }
        }

        private ObservableCollection<AnalyzerItem> mDxCAnalyzerItems;

        /// <summary>
        /// Analyzer items collection
        /// </summary>
        public ObservableCollection<AnalyzerItem> DxCAnalyzerItems
        {
            get
            {
                return mDxCAnalyzerItems;
            }
            set
            {
                if (mDxCAnalyzerItems != value)
                {
                    var temp = mDxCAnalyzerItems;
                    mDxCAnalyzerItems = value;
                    NotifyPropertyChanged(this, "DxCAnalyzerItems", temp, value);

                }
            }
        }

        public ConfigurationViewData()
        {
            AnalyzerItems = new ObservableCollection<AnalyzerItem>();
            DxCAnalyzerItems = new ObservableCollection<AnalyzerItem>();
        }
    }
}
