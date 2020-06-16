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
using BCI.PLCSimPP.Comm.Interfaces;
using BCI.PLCSimPP.Config.ViewModels;
using Unity;

namespace BCI.PLCSimPP.Config.Views
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

        [Dependency]
        public SiteMapEditerViewModel ViewModel
        {
            get { return DataContext as SiteMapEditerViewModel; }
            set { DataContext = value; }
        }

        /// <summary>
        /// input must be number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// selection changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// selection changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// selection changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBox_SelectionChanged_3(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }

            var unit = (IUnit)e.AddedItems[0];
            ls_port2.SelectedItem = null;
            ls_port1.SelectedItem = null;

            var viewModel = (SiteMapEditerViewModel)this.DataContext;
            viewModel.SelectedUnit = unit;
            viewModel.SelectUnitCommand.Execute(unit);
        }

        /// <summary>
        /// add button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddCommand.Execute(e);
            if (ViewModel.SelectedUnit == null)
            {
                ls_port2.SelectedItem = null;
                ls_port1.SelectedItem = null;
                ls_port3.SelectedItem = null;
            }
        }

        /// <summary>
        /// cancel button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            ls_port2.SelectedItem = null;
            ls_port1.SelectedItem = null;
            ls_port3.SelectedItem = null;
        }
    }
}
