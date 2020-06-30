using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BCI.PLCSimPP.Comm.Constants;
using BCI.PLCSimPP.Config.ViewModels;
using BCI.PLCSimPP.Config.Views;
using CommonServiceLocator;
using Prism.Regions;

namespace BCI.PLCSimPP.Launcher.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            var result = MessageBox.Show("Confirm to close the program?", "Confirm", MessageBoxButton.YesNo);

            if (result != MessageBoxResult.Yes)
            {
                e.Cancel = true;
                return;
            }

            var regionManager = ServiceLocator.Current.GetInstance<IRegionManager>();
            var views = regionManager.Regions[RegionName.LAYOUT_REGION].Views;

            Configuration configuration = views.FirstOrDefault(t => t.GetType() == typeof(Configuration)) as Configuration;
            SiteMapEditer siteMap = views.FirstOrDefault(t => t.GetType() == typeof(SiteMapEditer)) as SiteMapEditer;


            if (configuration != null && siteMap != null)
            {
                if (configuration.ViewModel.ConfigurationController.Data.IsValueChanged || siteMap.ViewModel.IsChanged())
                {
                    var rst = MessageBox.Show("Do you need to save the changed Settings?", "Warning", MessageBoxButton.YesNoCancel);
                    if (rst == MessageBoxResult.Yes)
                    {
                        configuration.ViewModel.ConfigurationController.Save();
                        siteMap.ViewModel.DoSave();
                    }
                    if (rst == MessageBoxResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                }
            }

        }

    }
}
