using System;
using System.Collections.Generic;
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
using CommonServiceLocator;
using PLCSimPP.Comm.Interfaces;
using PLCSimPP.Config.ViewModels;

namespace PLCSimPP.Config.Views
{
    /// <summary>
    /// Interaction logic for SiteMapEditer.xaml
    /// </summary>
    public partial class SiteMapEditer : UserControl
    {
        public SiteMapEditer()
        {
            InitializeComponent();
        }



        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            TextChange[] change = new TextChange[e.Changes.Count];
            e.Changes.CopyTo(change, 0);
            int offset = change[0].Offset;
            if (change[0].AddedLength > 0)
            {
                double num = 0;
                if (!Double.TryParse(textBox.Text, out num))
                {
                    textBox.Text = textBox.Text.Remove(offset, change[0].AddedLength);
                    textBox.Select(offset, 0);
                }
            }
        }

        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }
            var unit = (IUnit)e.AddedItems[0];

            var viewModel = (SiteMapEditerViewModel)this.DataContext;
            viewModel.SelectedUnit = unit;
            viewModel.SelectUnitCommand.Execute(unit);

            ls_port2.SelectedItem = null;
            ls_port3.SelectedItem = null;
        }

        private void ListBox_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }
            var unit = (IUnit)e.AddedItems[0];
            var viewModel = (SiteMapEditerViewModel)this.DataContext;
            viewModel.SelectedUnit = unit;
            viewModel.SelectUnitCommand.Execute(unit);
            ls_port1.SelectedItem = null;
            ls_port3.SelectedItem = null;
        }

        private void ListBox_SelectionChanged_3(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }

            //mViewModel.SelectedUnit = (IUnit)e.AddedItems[0];
            var unit = (IUnit)e.AddedItems[0];
            ls_port2.SelectedItem = null;
            ls_port1.SelectedItem = null;

            var viewModel = (SiteMapEditerViewModel)this.DataContext;
            viewModel.SelectedUnit = unit;
            viewModel.SelectUnitCommand.Execute(unit);
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (SiteMapEditerViewModel)this.DataContext;
            viewModel.AddCommand.Execute(e);
            if (viewModel.SelectedUnit == null)
            {
                ls_port2.SelectedItem = null;
                ls_port1.SelectedItem = null;
                ls_port3.SelectedItem = null;
            }
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            ls_port2.SelectedItem = null;
            ls_port1.SelectedItem = null;
            ls_port3.SelectedItem = null;
        }
    }
}
