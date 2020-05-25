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
using BCI.PLCSimPP.Config.ViewModels;
using Prism.Unity;
using Unity;

namespace BCI.PLCSimPP.Config.Views
{
    /// <summary>
    /// Interaction logic for ConfigruationView.xaml
    /// </summary>
    public partial class Configuration : UserControl
    {
        [Dependency]
        public ConfigruationViewModel ViewModel
        {
            get { return DataContext as ConfigruationViewModel; }
            set { DataContext = value; }
        }
        public Configuration()
        {
            InitializeComponent();
        }
    }
}
