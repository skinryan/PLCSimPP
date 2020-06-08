using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Comm.Events;
using BCI.PLCSimPP.Service.DB;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace BCI.PLCSimPP.Launcher.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private const string APPTITLE = "PLCSim for PP";

        private readonly IEventAggregator mEventAggr;

        private string mTitleText;

        public string TitleText
        {
            get { return mTitleText; }
            set { SetProperty(ref mTitleText, value); }
        }

        public ICommand NavigateCommand { get; set; }

        public MainWindowViewModel(IEventAggregator eventAggr)
        {
            mEventAggr = eventAggr;
            TitleText = $"{APPTITLE} - [Layout]";

            mEventAggr.GetEvent<SetTitleEvent>().Subscribe(title =>
            {
                TitleText = $"{APPTITLE} - [{title}]";
            });

            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string viewName)
        {
            mEventAggr.GetEvent<NavigateEvent>().Publish(viewName);
        }
    }
}
