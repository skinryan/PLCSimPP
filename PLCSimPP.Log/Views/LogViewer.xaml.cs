using System.ComponentModel;
using BCI.PLCSimPP.Log.ViewModels;
using System.Windows.Controls;
using System.Windows.Data;

namespace BCI.PLCSimPP.Log.Views
{
    /// <summary>
    /// Interaction logic for LogViewer.xaml
    /// </summary>
    public partial class LogViewer : UserControl
    {
        public LogViewer()
        {
            InitializeComponent();
        }

        private void dg_Sorting(object sender, DataGridSortingEventArgs e)
        {
            ListSortDirection lsd = ListSortDirection.Ascending;
            if (e.Column.SortDirection.HasValue)
            {
                if (e.Column.SortDirection.Value == ListSortDirection.Ascending)
                {
                    lsd = ListSortDirection.Descending;
                }
                else
                {
                    lsd = ListSortDirection.Ascending;
                }
            }
            ICollectionView cvs = CollectionViewSource.GetDefaultView(dg.ItemsSource);
            if (cvs != null && cvs.CanSort == true)
            {
                cvs.SortDescriptions.Clear();
                cvs.SortDescriptions.Add(new SortDescription(e.Column.SortMemberPath, lsd));
                cvs.SortDescriptions.Add(new SortDescription("Time", ListSortDirection.Ascending));
            }

            e.Column.SortDirection = lsd;
            e.Handled = true;
        }
    }
}