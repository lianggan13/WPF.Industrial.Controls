using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CustomDashboard.View
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private void UserControl1_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            double b = Math.Min(sizeInfo.NewSize.Width, sizeInfo.NewSize.Height);
            //b = 200d;
            this.ellipse.Width = ellipse.Height = b;
            Refresh();

        }

        [Category("Custom")]
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(UserControl1), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, propertyChangedCallback));


        [Category("Custom")]
        public double ValueMax
        {
            get { return (double)GetValue(ValueMaxProperty); }
            set { SetValue(ValueMaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValueMax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueMaxProperty =
            DependencyProperty.Register("ValueMax", typeof(double), typeof(UserControl1), new FrameworkPropertyMetadata(100d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        [Category("Custom")]
        public double ValueMin
        {
            get { return (double)GetValue(ValueMinProperty); }
            set { SetValue(ValueMinProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValueMin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueMinProperty =
            DependencyProperty.Register("ValueMin", typeof(double), typeof(UserControl1), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        private static void propertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as UserControl1).UpdateNeedleAngle();
        }

        double angleRange = 270d;
        double radius;// = Math.Min(ellipse.ActualWidth, ellipse.ActualHeight) / 2;
        private void Refresh()
        {
            radius = Math.Min(ellipse.Height, ellipse.Width) / 2;

            this.canvas.Children.Clear();
            // 画刻度
            double min = this.ValueMin;
            double max = this.ValueMax;
            double step = this.angleRange / (max - min); // 总度数 270°   double --> angle

            for (double i = min; i <= max; i += step)
            {

            }
            for (int i = 0; i < max - min; i++)
            {
                // 小刻度线
                double a = min + (i * step) + 135; // 角度偏移 135°
                Line line = new Line();
                line.Stroke = Brushes.White;
                line.StrokeThickness = 1d;


                double rf = radius - 13d; // radius from
                double rt = radius - 8d;  // radius to


                AddLineToCanvas(a, rf, rt, line);
            }

            double interval = 10d;       // 刻度间隔
            double n = (max - min) / interval;
            for (int i = 0; i <= n; i++)
            {
                double v = i * interval;
                double a = (v * step) + 135; // 角度偏移 135°

                Line line = new Line();
                line.Stroke = Brushes.White;
                line.StrokeThickness = 2d;

                double rf = radius - 23d; // radius from
                double rt = radius - 8d;  // radius to

                AddLineToCanvas(a, rf, rt, line);

                // 大刻度值
                TextBlock text = new TextBlock();
                text.Text = $"{i * interval}";
                text.FontSize = 24;
                text.VerticalAlignment = VerticalAlignment.Center;
                text.HorizontalAlignment = HorizontalAlignment.Center;
                text.TextAlignment = TextAlignment.Center;

                AddTextToCanvas(a, rf - text.FontSize, text);
            }

            // 过渡圆弧
            {
                //Data = "M20,20 A100,100 0 1 1 50 50"
                double ir = radius * 0.5; // 注: ir 并不是 Arc 的半径,只是为了方便计算 起止坐标
                step = angleRange / (max - min);
                Point sp = new Point()
                {
                    X = ir * Math.Cos((min * step + 135d) * Math.PI / 180),
                    Y = ir * Math.Sin((min * step + 135d) * Math.PI / 180)
                };
                Point ep = new Point()
                {
                    X = ir * Math.Cos((max * step + 135d) * Math.PI / 180),
                    Y = ir * Math.Sin((max * step + 135d) * Math.PI / 180)
                };

                sp.Offset(radius, radius);
                ep.Offset(radius, radius);

                string data = $"M{sp.X},{sp.Y} A{ir},{ir} 0 1 1 {ep.X},{ep.Y}";
                var convert = TypeDescriptor.GetConverter(typeof(Geometry));
                this.path.Data = (Geometry)convert.ConvertFrom(data);

            }

            // 指针
            {
                UpdateNeedleAngle();
            }
        }

        private void AddLineToCanvas(double a, double rf, double rt, Line line)
        {
            Point p1 = new Point();
            Point p2 = new Point();


            p1.X = rf * Math.Cos(a * Math.PI / 180);
            p1.Y = rf * Math.Sin(a * Math.PI / 180);
            p2.X = rt * Math.Cos(a * Math.PI / 180);
            p2.Y = rt * Math.Sin(a * Math.PI / 180);


            // 偏移至中心点
            p1.Offset(radius, radius);
            p2.Offset(radius, radius);

            // p1 --> p2 
            line.X1 = p1.X;
            line.Y1 = p1.Y;
            line.X2 = p2.X;
            line.Y2 = p2.Y;

            this.canvas.Children.Add(line);

        }

        private void AddTextToCanvas(double a, double rf, TextBlock text)
        {
            Point p1 = new Point();

            p1.X = rf * Math.Cos(a * Math.PI / 180);
            p1.Y = rf * Math.Sin(a * Math.PI / 180);

            // 偏移至中心点
            p1.Offset(radius, radius);

            this.canvas.Children.Add(text);

            text.UpdateLayout(); // 不然 text 宽高为 NaN

            Canvas.SetLeft(text, p1.X - text.ActualWidth / 2);
            Canvas.SetTop(text, p1.Y - text.ActualHeight / 2);
        }

        private void UpdateNeedleAngle()
        {
            double min = this.ValueMin;
            double max = this.ValueMax;
            double step = this.angleRange / (max - min);
            //Data = "M50,50 64,135"
            double ir = radius * 0.8;
            Point sp = new Point()
            {
                X = 0d,
                Y = 0d
            };
            Point ep = new Point()
            {
                X = ir * Math.Cos((min * step + 135d) * Math.PI / 180),
                Y = ir * Math.Sin((min * step + 135d) * Math.PI / 180)
            };

            sp.Offset(radius, radius);
            ep.Offset(radius, radius);

            string data = $"M{sp.X},{sp.Y} {ep.X},{ep.Y}";
            var convert = TypeDescriptor.GetConverter(typeof(Geometry));
            this.needlePath.Data = (Geometry)convert.ConvertFrom(data);


            // value ---> double
            double value = this.Value;
            step = angleRange / (max - min);
            double angle = value * step;

            this.needlePath.RenderTransform = new RotateTransform(angle);
        }
    }
}
