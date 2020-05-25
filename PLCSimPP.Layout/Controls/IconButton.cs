using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BCI.PLCSimPP.Layout.Controls
{
    public class IconButton : Button
    {
        /// <summary>
        /// Geometry data
        /// </summary>
        public Geometry IconData
        {
            get { return (Geometry)GetValue(IconDataProperty); }
            set { SetValue(IconDataProperty, value); }
        }

        public static readonly DependencyProperty IconDataProperty =
            DependencyProperty.Register("IconData", typeof(Geometry), typeof(IconButton), new PropertyMetadata());


        /// <summary>
        /// Icon width
        /// </summary>
        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }

        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register("IconWidth", typeof(double), typeof(IconButton), new PropertyMetadata(15d));


        /// <summary>
        /// Icon height
        /// </summary>
        public double IconHeight
        {
            get { return (double)GetValue(IconHeightProperty); }
            set { SetValue(IconHeightProperty, value); }
        }

        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.Register("IconHeight", typeof(double), typeof(IconButton), new PropertyMetadata(15d));


        /// <summary>
        /// Show text
        /// </summary>
        public bool IsShowText
        {
            get { return (bool)GetValue(IsShowTextProperty); }
            set { SetValue(IsShowTextProperty, value); }
        }

        public static readonly DependencyProperty IsShowTextProperty =
            DependencyProperty.Register("IsShowText", typeof(bool), typeof(IconButton), new PropertyMetadata(true));


        /// <summary>
        /// Icon brush
        /// </summary>
        public Brush IconBrush
        {
            get { return (Brush)GetValue(IconBrushProperty); }
            set { SetValue(IconBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconBrushProperty =
            DependencyProperty.Register("IconBrush", typeof(Brush), typeof(IconButton), new PropertyMetadata(Brushes.Black));



        /// <summary>
        /// Icon brush when mouse over
        /// </summary>
        public Brush IconMouseOverBrush
        {
            get { return (Brush)GetValue(IconMouseOverBrushProperty); }
            set { SetValue(IconMouseOverBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconMouseOverBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconMouseOverBrushProperty =
            DependencyProperty.Register("IconMouseOverBrush", typeof(Brush), typeof(IconButton), new PropertyMetadata(Brushes.Gray));




        public Dock IconDock
        {
            get { return (Dock)GetValue(IconDockProperty); }
            set { SetValue(IconDockProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconDock.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconDockProperty =
            DependencyProperty.Register("IconDock", typeof(Dock), typeof(IconButton), new PropertyMetadata(Dock.Left));



    }

}
