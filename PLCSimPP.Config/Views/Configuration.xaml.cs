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

        

        private void TextBox_KeyUp_dxc(object sender, KeyEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                tb.Text = string.Empty;
                dxcControl.Clear();
            }
        }

        private void TextBox_KeyUp_dc(object sender, KeyEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                tb.Text = string.Empty;
                dcControl.Clear();
            }
        }
    }
}
