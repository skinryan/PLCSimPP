using BCI.PLCSimPP.PresentationControls.ViewData;
using BCI.PLCSimPP.PresentationControls.ValidationAttributes;
using System.Collections.ObjectModel;
using BCI.PLCSimPP.PresentationControls.Controls;
using BCI.PLCSimPP.Comm.Models;

namespace BCI.PLCSimPP.Config.ViewDatas
{
    public class ConfigruationViewData : EditViewDataBase
    {
        private string mSitemapFilePath;
        /// <summary>
        /// Gets or sets sitemap file path
        /// </summary>
        public string SitemapFilePath
        {
            get { return mSitemapFilePath; }
            set
            {
                if (value != mSitemapFilePath)
                {
                    var temp = mSitemapFilePath;
                    mSitemapFilePath = value;
                    NotifyPropertyChanged(this, "SitemapFilePath", temp, value);
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

        public ConfigruationViewData()
        {
            AnalyzerItems = new ObservableCollection<AnalyzerItem>();
            DxCAnalyzerItems = new ObservableCollection<AnalyzerItem>();
        }
    }
}
