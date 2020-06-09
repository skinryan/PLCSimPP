using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using BCI.PLCSimPP.Comm.Enums;
using BCI.PLCSimPP.Comm.Models;
using Prism.Mvvm;

namespace BCI.PLCSimPP.PresentationControls.Controls
{
    [TemplatePart(Name = TEMPLATE_PART_COMBOBOX, Type = typeof(ComboBox))]
    [TemplatePart(Name = TEMPLATE_PART_LISTBOX, Type = typeof(ListBox))]

    public class AnalyzerConfigControl : Control
    {
        public const string TEMPLATE_PART_COMBOBOX = "Part_ComboBox";
        public const string TEMPLATE_PART_LISTBOX = "Part_ListBox";

       
        #region property

        /// <summary>
        /// Analyzer item collection
        /// </summary>
        public ObservableCollection<AnalyzerItem> AnalyzerItems
        {
            get { return (ObservableCollection<AnalyzerItem>)GetValue(AnalyzerItemsProperty); }
            set { SetValue(AnalyzerItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AnalyzerItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnalyzerItemsProperty =
            DependencyProperty.Register("AnalyzerItems", typeof(ObservableCollection<AnalyzerItem>), typeof(AnalyzerConfigControl), new PropertyMetadata(new ObservableCollection<AnalyzerItem>()));



        /// <summary>
        /// 
        /// </summary>
        public int MaxInstruments
        {
            get { return (int)GetValue(MaxInstrumentsProperty); }
            set { SetValue(MaxInstrumentsProperty, value); }
        }

        public static readonly DependencyProperty MaxInstrumentsProperty =
            DependencyProperty.Register("MaxInstruments", typeof(int), typeof(AnalyzerConfigControl), new PropertyMetadata(16));

        /// <summary>
        /// true dcsim, false dxcsim
        /// </summary>
        public bool IsDcSim
        {
            get { return (bool)GetValue(IsDcSimProperty); }
            set { SetValue(IsDcSimProperty, value); }
        }

        public static readonly DependencyProperty IsDcSimProperty =
            DependencyProperty.Register("IsDcSim", typeof(bool), typeof(AnalyzerConfigControl), new PropertyMetadata(true));

        #endregion

        public ComboBox BtnComboBox { get; set; }

        public ListBox BtnListBox { get; set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            BtnComboBox = GetTemplateChild(TEMPLATE_PART_COMBOBOX) as ComboBox;
            if (BtnComboBox != null)
            {
                BtnComboBox.ItemsSource = GetItemSource(MaxInstruments);

                if (AnalyzerItems.Count > 0)
                {
                    BtnComboBox.SelectedValue = AnalyzerItems.Count;
                }

                BtnComboBox.SelectionChanged += BtnComboBoxSelectionChanged;
            }

            BtnListBox = GetTemplateChild(TEMPLATE_PART_LISTBOX) as ListBox;
        }

        private IEnumerable<int> GetItemSource(int count)
        {
            for (int i = 1; i <= count; i++)
            {
                yield return i;
            }
        }

        public void Clear()
        {
            BtnComboBox.SelectedIndex = -1;
        }

        /// <summary>
        /// combobox selection changed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            int count = Convert.ToInt32(cb?.SelectedItem);

            AnalyzerItems.Clear();
            for (int i = 1; i <= count; i++)
            {
                AnalyzerItems.Add(new AnalyzerItem() { Num = AnalyzerItems.Count + 1, DcType = DcAnalyzerType.GC, DxCType = DxCAnalyzerType.DxC });
            }
        }

        static AnalyzerConfigControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnalyzerConfigControl), new FrameworkPropertyMetadata(typeof(AnalyzerConfigControl)));
        }
    }




}
