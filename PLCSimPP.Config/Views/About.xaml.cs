using BCI.PLCSimPP.Config.ViewModels;
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
using Unity;

namespace BCI.PLCSimPP.Config.Views
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();
        }
        
        [Dependency]
        public AboutViewModel ViewModel
        {
            get { return DataContext as AboutViewModel; }
            set { DataContext = value; }
        }
    }
}
