using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace BCI.PLCSimPP.Service.Log
{
    public class PageData<T> : BindableBase where T : BindableBase
    {

        private int mTotalNum;

        /// <summary>
        /// Total row count
        /// </summary>
        public int TotalNum
        {
            get { return mTotalNum; }
            set
            {
                SetProperty(ref mTotalNum, value);
            }
        }


        private ObservableCollection<T> mItems;
        /// <summary>
        /// Current page content
        /// </summary>
        public ObservableCollection<T> Items
        {
            get { return mItems; }
            set
            {
                SetProperty(ref mItems, value);
            }
        }

        private int mCurrentPage;

        /// <summary>
        /// Current page number
        /// </summary>
        public int CurrentPage
        {
            get { return mCurrentPage; }
            set
            {
                SetProperty(ref mCurrentPage, value);
            }
        }

        private int mTotalPageCount;

        /// <summary>
        /// Total page count
        /// </summary>
        public int TotalPageCount
        {
            get { return mTotalPageCount; }
            set
            {
                SetProperty(ref mTotalPageCount, value);
            }
        }

        public PageData()
        {
            this.mItems = new ObservableCollection<T>();
        }

    }
}
