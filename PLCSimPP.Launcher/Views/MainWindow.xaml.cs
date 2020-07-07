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

        /// <inheritdoc />
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
           
            //Check save before closing
            if (configuration != null)
            {
                if (configuration.ViewModel.ConfigurationController.Data.IsValueChanged )
                {
                    var rst = MessageBox.Show("Do you need to save the changed Settings?", "Warning", MessageBoxButton.YesNoCancel);
                    if (rst == MessageBoxResult.Yes)
                    {
                        configuration.ViewModel.ConfigurationController.Save();
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
