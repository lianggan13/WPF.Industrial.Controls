using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CustomDashboard.View
{
    /// <summary>
    /// Thermometer.xaml 的交互逻辑
    /// </summary>
    public partial class Thermometer : UserControl
    {
        public Thermometer()
        {
            InitializeComponent();
            this.Loaded += Thermometer_Loaded;
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(Thermometer), new FrameworkPropertyMetadata(5d,
        FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, valueChangedCallback));



        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(Thermometer), new FrameworkPropertyMetadata(100d,
        FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, rangeChangedCallback));

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minimum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(Thermometer), new FrameworkPropertyMetadata(0d,
        FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, rangeChangedCallback));

        private static void valueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Thermometer).Refresh();
        }

        private static void rangeChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Thermometer).Refresh();
        }

        private void Thermometer_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            double w = PART_TickCanvas.ActualWidth;// Math.Max(PART_TickCanvas.Width, PART_TickCanvas.ActualWidth);
            double h = PART_TickCanvas.ActualHeight;// Math.Max(PART_TickCanvas.Height, PART_TickCanvas.ActualHeight);
            if (w * h == 0)
                return;

            double max = Maximum;
            double min = Minimum;
            // 数值 <--> 高度   数量
            double hi = h / (max - min);        // 每个刻度对应的高度间隔
            double step = 1d;                   // 每个刻度对应的数值
            double count = (max - min) / step;  // 可视化的刻度数量

            PART_TickCanvas.Children.Clear();
            for (int i = 0; i <= count; i++)
            {
                double num = step * i;
                double x = 15;
                double y = num * hi;
                Line line = new Line();
                line.Stroke = Brushes.Black;
                line.StrokeThickness = 1;

                // 大刻度  10
                if (num % 10 == 0)
                {
                    x = 15;
                    {
                        TextBlock txt = new TextBlock();
                        txt.Text = $"{max - num}";
                        txt.Width = 20;
                        txt.FontSize = 9;
                        txt.VerticalAlignment = VerticalAlignment.Center;
                        txt.HorizontalAlignment = HorizontalAlignment.Center;
                        txt.TextAlignment = TextAlignment.Center;

                        PART_TickCanvas.Children.Add(txt);
                        txt.UpdateLayout();

                        Canvas.SetLeft(txt, 0 - txt.ActualWidth / 6);
                        Canvas.SetTop(txt, y - txt.ActualHeight / 2);
                    }
                    {
                        TextBlock txt2 = new TextBlock();
                        txt2.Text = $"{max - num}";
                        txt2.Width = 20;
                        txt2.FontSize = 9;
                        txt2.VerticalAlignment = VerticalAlignment.Center;
                        txt2.HorizontalAlignment = HorizontalAlignment.Center;
                        txt2.TextAlignment = TextAlignment.Center;

                        PART_TickCanvas.Children.Add(txt2);
                        txt2.UpdateLayout();

                        Canvas.SetLeft(txt2, w - txt2.ActualWidth + txt2.ActualWidth / 6);
                        Canvas.SetTop(txt2, y - txt2.ActualHeight / 2);
                    }

                }
                // 中刻度  5
                else if (num % 5 == 0)
                {
                    x = 20;
                }
                // 小刻度  1
                else
                {
                    x = 25;
                }

                line.X1 = x;
                line.Y1 = y;
                line.X2 = w - x;
                line.Y2 = y;

                PART_TickCanvas.Children.Add(line);
            }

            // 水银柱动画
            double hv = Value * hi;
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = PART_ValueRect.Height;
            animation.To = 20d + hv;
            animation.Duration = TimeSpan.FromMilliseconds(300);//new Duration(Duration 500;

            // method 1
            //PART_ValueRect.BeginAnimation(Rectangle.HeightProperty, animation);

            // method 2
            Storyboard board = new Storyboard();
            board.Children.Add(animation);
            Storyboard.SetTarget(animation, PART_ValueRect);
            //Storyboard.SetTargetName(animation, "PART_ValueRect");
            Storyboard.SetTargetProperty(animation, new PropertyPath(Rectangle.HeightProperty));
            board.Begin();

            //BeginStoryboard(board);
        }
    }
}

