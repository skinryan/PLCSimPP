using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BCI.PLCSimPP.Comm.Events;
using BCI.PLCSimPP.Comm.Models;
using BCI.PLCSimPP.Log.ViewModels;
using CommonServiceLocator;
using Prism.Events;

namespace BCI.PLCSimPP.Log.Views
{
    /// <summary>
    /// Interaction logic for Monitor.xaml
    /// </summary>
    public partial class Monitor : UserControl
    {
        public Monitor()
        {
            InitializeComponent();

            var eventAggr = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggr.GetEvent<RollingEvent>().Subscribe(OnRolling);
        }

        private void OnRolling(MsgLog msgLog)
        {
            dg.ScrollIntoView(msgLog);
        }
    }
}
