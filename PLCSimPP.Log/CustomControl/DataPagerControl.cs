using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BCI.PLCSimPP.Log.CustomControl
{
    [TemplatePart(Name = TEMPLATE_PART_BTN_FIRST, Type = typeof(Button))]
    [TemplatePart(Name = TEMPLATE_PART_BTN_PREVIOUS, Type = typeof(Button))]
    [TemplatePart(Name = TEMPLATE_PART_BTN_NEXT, Type = typeof(Button))]
    [TemplatePart(Name = TEMPLATE_PART_BTN_LAST, Type = typeof(Button))]
    [TemplatePart(Name = TEMPLATE_PART_TEXT_INDEX, Type = typeof(TextBox))]
    [TemplatePart(Name = TEMPLATE_PART_BTN_GO, Type = typeof(Button))]
    public class DataPagerControl : Control
    {
        public const string TEMPLATE_PART_BTN_FIRST = "Btn_First";
        public const string TEMPLATE_PART_BTN_PREVIOUS = "Btn_Previous";
        public const string TEMPLATE_PART_BTN_NEXT = "Btn_Next";
        public const string TEMPLATE_PART_BTN_LAST = "Btn_Last";
        public const string TEMPLATE_PART_TEXT_INDEX = "Text_Index";
        public const string TEMPLATE_PART_BTN_GO = "Btn_Go";

        #region property
        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(DataPagerControl), new PropertyMetadata(0));


        public int TotalRowCount
        {
            get { return (int)GetValue(TotalRowCountProperty); }
            set { SetValue(TotalRowCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalRowCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalRowCountProperty =
            DependencyProperty.Register("TotalRowCount", typeof(int), typeof(DataPagerControl), new PropertyMetadata(0));
        
        public int SearchIndex
        {
            get { return (int)GetValue(SearchIndexProperty); }
            set { SetValue(SearchIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SearchIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SearchIndexProperty =
            DependencyProperty.Register("SearchIndex", typeof(int), typeof(DataPagerControl), new PropertyMetadata(0, OnSearchIndexChangedCallBack));
        private static void OnSearchIndexChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DataPagerControl)
            {
                DataPagerControl control = sender as DataPagerControl;
                control.CurrentIndex = control.PageCount > 0 ? 1 : 0;
            }
        }


        /// <summary>
        /// Total page count
        /// </summary>
        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            set { SetValue(PageCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PageCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageCountProperty =
            DependencyProperty.Register("PageCount", typeof(int), typeof(DataPagerControl), new PropertyMetadata(0));

     
        /// <summary>
        /// Current page index
        /// </summary>
        public int CurrentIndex
        {
            get { return (int)GetValue(CurrentIndexProperty); }
            set { SetValue(CurrentIndexProperty, value); }
        }

        public static readonly DependencyProperty CurrentIndexProperty =
            DependencyProperty.Register("CurrentIndex", typeof(int), typeof(DataPagerControl), new PropertyMetadata(0, OnCurrentIndexChangedCallback));

        private static void OnCurrentIndexChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is DataPagerControl)
            {
                DataPagerControl control = sender as DataPagerControl;
                if (control.mIsApplyTemplate == false)
                    return;
                control.TextIndex.Text = e.NewValue.ToString();

                DatePageRoutedEventArgs arg = new DatePageRoutedEventArgs(PageChangingRoutedEvent, control)
                {
                    PageCount = control.PageCount,
                    PageIndex = Convert.ToInt32(e.NewValue)
                };
                //CurrentIndex = pageIndex;
                control.RaiseEvent(arg);
            }
        }        

        #endregion

        public Button BtnFirst { get; set; }
        public Button BtnPreview { get; set; }
        public Button BtnNext { get; set; }
        public Button BtnLast { get; set; }
        public TextBox TextIndex { get; set; }
        public Button BtnGo { get; set; }

        private bool mIsApplyTemplate;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //First page
            BtnFirst = GetTemplateChild(TEMPLATE_PART_BTN_FIRST) as Button;
            if (BtnFirst != null)
            {
                BtnFirst.Click += BtnFirstClick;
            }

            //Previous page
            BtnPreview = GetTemplateChild(TEMPLATE_PART_BTN_PREVIOUS) as Button;
            if (BtnPreview != null)
            {
                BtnPreview.Click += BtnPreviewClick;
            }

            //Next page
            BtnNext = GetTemplateChild(TEMPLATE_PART_BTN_NEXT) as Button;
            if (BtnNext != null)
            {
                BtnNext.Click += BtnNextClick;
            }

            //Last page
            BtnLast = GetTemplateChild(TEMPLATE_PART_BTN_LAST) as Button;
            if (BtnLast != null)
            {
                BtnLast.Click += BtnLastClick;
            }

            //Current page
            TextIndex = GetTemplateChild(TEMPLATE_PART_TEXT_INDEX) as TextBox;
            if (TextIndex != null)
            {
                TextIndex.KeyUp += TextIndexKeyUp;
                TextIndex.TextInput += TextIndexPreviewTextInput;
            }

            //Go button
            BtnGo = GetTemplateChild(TEMPLATE_PART_BTN_GO) as Button;
            if (BtnGo != null)
            {
                BtnGo.Click += BtnGoClick;
            }
            mIsApplyTemplate = true;
        }

        #region event

        /// <summary>
        /// Go button event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BtnGoClick(object sender, RoutedEventArgs e)
        {
            if (TextIndex == null || string.IsNullOrEmpty(TextIndex.Text))
                return;
            try
            {
                TextIndex.Text = Regex.Replace(TextIndex.Text, @"[^\d]*", "");

                int index = int.Parse(TextIndex.Text);
                if (index < PageCount)
                {
                    CurrentIndex = index;
                }
                else
                {
                    CurrentIndex = PageCount;
                    TextIndex.Text = CurrentIndex.ToString();
                }
            }
            catch
            {
                TextIndex.Text = string.Empty;
                CurrentIndex = 1;
            }
        }

        /// <summary>
        /// First page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFirstClick(object sender, RoutedEventArgs e)
        {
            CurrentIndex = 1;
        }

        /// <summary>
        /// Previous page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPreviewClick(object sender, RoutedEventArgs e)
        {
            CurrentIndex = CurrentIndex - 1 < 1 ? 1 : CurrentIndex - 1;
        }

        /// <summary>
        /// Next page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNextClick(object sender, RoutedEventArgs e)
        {
            CurrentIndex = CurrentIndex + 1 > PageCount ? PageCount : CurrentIndex + 1;
        }

        /// <summary>
        /// Last page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLastClick(object sender, RoutedEventArgs e)
        {
            CurrentIndex = PageCount;
        }

        /// <summary>
        /// Text box input 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextIndexPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsNumber(e.Text);
        }

        /// <summary>
        /// Filter keyboard input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextIndexKeyUp(object sender, KeyEventArgs e)
        {
           if (e.Key == Key.Enter && !string.IsNullOrEmpty(TextIndex.Text))
            {
                try
                {
                    TextIndex.Text = Regex.Replace(TextIndex.Text, @"[^\d]*", "");
                    int index = int.Parse(TextIndex.Text);
                    if (index < PageCount)
                    {
                        CurrentIndex = index;
                    }
                    else
                    {
                        CurrentIndex = PageCount;
                        TextIndex.Text = CurrentIndex.ToString();
                    }
                }
                catch
                {
                    TextIndex.Text = string.Empty;
                    CurrentIndex = 1;
                }
            }
        }

        #endregion

        #region RoutedEvent

        public event EventHandler<DatePageRoutedEventArgs> PageChangingEvent
        {
            add { AddHandler(PageChangingRoutedEvent, value); }
            remove { RemoveHandler(PageChangingRoutedEvent, value); }
        }

        public static readonly RoutedEvent PageChangingRoutedEvent =
            EventManager.RegisterRoutedEvent("PageChanging", RoutingStrategy.Bubble, typeof(EventHandler<DatePageRoutedEventArgs>), typeof(DataPagerControl));

        #endregion

        #region public method

        /// <summary>
        /// Check number
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNumber(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            foreach (char c in str)
            {
                if (!char.IsDigit(c))
                    //if(c<'0' c="">'9')
                    return false;
            }
            return true;
        }

        #endregion

        static DataPagerControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataPagerControl), new FrameworkPropertyMetadata(typeof(DataPagerControl)));
        }
    }
}
