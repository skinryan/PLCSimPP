using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Events;
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Log.CustomControl;
using BCI.PLCSimPP.Service.DB;
using BCI.PLCSimPP.Service.Log;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace BCI.PLCSimPP.Log.ViewModels
{
    public class LogViewerViewModel : BindableBase
    {

        #region property
        private readonly IAutomation mAutomation;
        private readonly IEventAggregator mEventAggr;
        public ICommand CancelCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        public int TotalPage { get; set; }

        private List<string> mAddresses = new List<string>();
        /// <summary>
        ///     Gets or sets search begin datetime
        /// </summary>
        public List<string> Addresses
        {
            get { return mAddresses; }
            set
            {
                mAddresses = value;
                RaisePropertyChanged();
            }
        }

        private int mPageSize;
        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize
        {
            get { return mPageSize; }
            set { SetProperty(ref mPageSize, value); }
        }


        private int mSearchIndex = 0;
        /// <summary>
        /// Search Index
        /// </summary>
        public int SearchIndex
        {
            get { return mSearchIndex; }
            set { SetProperty(ref mSearchIndex, value); }
        }
        
        private PageData<LogContent> mCurrentPage;

        /// <summary>
        /// Current page
        /// </summary>
        public PageData<LogContent> CurrentPage
        {
            get { return mCurrentPage; }
            set { SetProperty(ref mCurrentPage, value); }
        }

        private string mAddress;

        /// <summary>
        /// Address
        /// </summary>
        public string Address
        {
            get { return mAddress; }
            set { SetProperty(ref mAddress, value); }
        }

        private string mParam;

        /// <summary>
        /// Param
        /// </summary>
        public string Param
        {
            get { return mParam; }
            set { SetProperty(ref mParam, value); }
        }

        private DateTime mSearchFromDatetime = DateTime.Now.AddDays(-1);

        /// <summary>
        ///     Gets or sets search begin datetime
        /// </summary>
        public DateTime SearchFromDatetime
        {
            get { return mSearchFromDatetime; }
            set
            {
                mSearchFromDatetime = value;
                RaisePropertyChanged();
            }
        }

        private DateTime mSearchToDatetime = DateTime.Now.AddDays(1);
        /// <summary>
        ///     Gets or sets search end datetime
        /// </summary>
        public DateTime SearchToDatetime
        {
            get { return mSearchToDatetime; }
            set
            {
                mSearchToDatetime = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        private ICommand mSearchCmd;
        /// <summary>
        /// Search result command
        /// </summary>
        public ICommand SearchCmd
        {
            get
            {
                if (mSearchCmd == null)
                {
                    mSearchCmd = new DelegateCommand<object>((obj) =>
                    {
                        CurrentPage = GetPage(1, PageSize, mSearchFromDatetime, mSearchToDatetime);
                        SearchIndex++;
                    });
                }
                return mSearchCmd;
            }
        }

        private ICommand mPageChangingCommand;
        /// <summary>
        /// Page changing command
        /// </summary>
        public ICommand PageChangingCommand
        {
            get
            {
                if (mPageChangingCommand == null)
                {
                    mPageChangingCommand = new DelegateCommand<object>((e) =>
                    {
                        var args = (DatePageRoutedEventArgs)e;
                        CurrentPage = GetPage(args.PageIndex, PageSize, mSearchFromDatetime, mSearchToDatetime);
                    });
                }
                return mPageChangingCommand;
            }
        }

        private PageData<LogContent> GetPage(int page, int pageSize, DateTime tmStart, DateTime tmEnd)
        {
            PageCriteria criteria = new PageCriteria() { Condition = $"[Time] <= '{tmEnd}' and [Time] >= '{tmStart}'" };

            if (!string.IsNullOrEmpty(Address))
                criteria.Condition += $" and [Address] = '{Address}'";

            if (!string.IsNullOrEmpty(Param))
                criteria.Condition += $" and [Details] like '%{Param}%'";

            criteria.CurrentPage = page;
            criteria.PageSize = pageSize;
            criteria.TableName = DbConst.MSGLOG_TABLE_NAME;
            criteria.Sort = "[Time]";

            var result = DBService.Current.GetPageData<LogContent>(criteria);
            return result;
        }

        /// <summary>
        /// init address drop down list
        /// </summary>
        public void GetAddresses()
        {
            mAddresses.Clear();
            mAddresses.Add(string.Empty);
            if (mAutomation.UnitCollection.Count > 0)
            {
                foreach (var masterUnit in mAutomation.UnitCollection)
                {
                    mAddresses.Add(masterUnit.Address);

                    foreach (var unit in masterUnit.Children)
                    {
                        if (!mAddresses.Contains(unit.Address))
                        {
                            mAddresses.Add(unit.Address);
                        }
                    }
                }
            }
            mAddresses.Sort();
            RaisePropertyChanged("Addresses");
        }

        public LogViewerViewModel(IEventAggregator eventAggr, IAutomation automation)
        {
            mAutomation = automation;
            GetAddresses();
            PageSize = DbConst.PAGE_DEFAULT_VALUE_PAGESIZE;

            CurrentPage = GetPage(1, PageSize, mSearchFromDatetime, mSearchToDatetime);
            mEventAggr = eventAggr;

            SaveCommand = new DelegateCommand(DoSave);
            CancelCommand = new DelegateCommand(() =>
            {
                mEventAggr.GetEvent<NavigateEvent>().Publish(ViewName.DEVICE_LAYOUT);
            });
        }

        private void DoSave()
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Title = "Select the save path for the current query result",
                Filter = "Text File(*.txt)|*.txt",
                CheckPathExists = true,
                DefaultExt = "txt",
                RestoreDirectory = true
            };

           var diaResult= sfd.ShowDialog();

            try
            {
                if (diaResult.HasValue && diaResult.Value)
                {
                    using (FileStream fs = new FileStream(sfd.FileName, FileMode.OpenOrCreate))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            var result = DBService.Current.QueryLogContents(SearchFromDatetime, SearchToDatetime, Address, Param);

                            foreach (var log in result)
                            {
                                sw.WriteLine($"[{log.Time}] [{log.Direction}] [{log.Address}] [{log.Command}] [{log.Details}]");
                            }

                            sw.Flush();
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show("File Save Exception");
            }

        }
    }
}
