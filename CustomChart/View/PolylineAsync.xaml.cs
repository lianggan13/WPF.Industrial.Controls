using CustomChart.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CustomChart.View
{
    /// <summary>
    /// PolylineAsync.xaml 的交互逻辑
    /// </summary>
    public partial class PolylineAsync : UserControl
    {
        /// <summary>
        /// 数据源
        /// </summary>
        [Category("Loong Egg"), Description("数据源")]
        public DataSeries DataSeries
        {
            get { return (DataSeries)GetValue(DataSeriesProperty); }
            set { SetValue(DataSeriesProperty, value); }
        }
        /// <summary>
        /// <see cref="DataSeries"/>
        /// </summary>
        public static readonly DependencyProperty DataSeriesProperty = DependencyProperty.Register
                                                                        (
                                                                            nameof(DataSeries),
                                                                            typeof(DataSeries),
                                                                            typeof(PolylineAsync),
                                                                            new PropertyMetadata(
                                                                                default(DataSeries),
                                                                                (s, e) => { (s as PolylineAsync)?.Refresh(); })
                                                                        );

        /// <summary>
        /// 横坐标数据范围
        /// </summary>
        [Category("Loong Egg"), Description("横坐标数据范围")]
        public Range HorizontalRange
        {
            get { return (Range)GetValue(HorizontalRangeProperty); }
            set { SetValue(HorizontalRangeProperty, value); }
        }
        /// <summary>
        /// <see cref="HorizontalRange"/>
        /// </summary>
        public static readonly DependencyProperty HorizontalRangeProperty = DependencyProperty.Register
                                                                        (
                                                                            nameof(HorizontalRange),
                                                                            typeof(Range),
                                                                            typeof(PolylineAsync),
                                                                            new PropertyMetadata(
                                                                                default(Range),
                                                                                OnParamterChanged)
                                                                        );

        /// <summary>
        /// 纵坐标数据范围
        /// </summary>
        [Category("Loong Egg"), Description("纵坐标数据范围")]
        public Range VerticalRange
        {
            get { return (Range)GetValue(VerticalRangeProperty); }
            set { SetValue(VerticalRangeProperty, value); }
        }
        /// <summary>
        /// <see cref="VerticalRange"/>
        /// </summary>
        public static readonly DependencyProperty VerticalRangeProperty = DependencyProperty.Register
                                                                        (
                                                                            nameof(VerticalRange),
                                                                            typeof(Range),
                                                                            typeof(PolylineAsync),
                                                                            new PropertyMetadata(
                                                                                default(Range),
                                                                                OnParamterChanged)
                                                                        );

        /// <summary>
        /// 当参数发生改变时
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnParamterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => (d as PolylineAsync)?.ResetScale();


        private double _Height;
        public double Kx { get; private set; }
        public double Ky { get; private set; }
        public Canvas Root { get; private set; }

        public PolylineAsync()
        {
            InitializeComponent();

            Root = new Canvas();
            Content = Root;

            DataSeries points = new DataSeries();
            Random random = new Random();
            for (int i = 0; i < 1000; i++) /* 慎重渲染10W点 */
            {
                points.Add(
                    new Point(
                        random.Next(0, 300),
                        random.Next(-100, 100)
                    ));
            }
            DataSeries = points;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            ResetScale(); Refresh();
        }

        /// <summary>
        /// 重置缩放因子
        /// </summary>
        private void ResetScale()
        {
            Kx = 0;
            Ky = 0;
            if (HorizontalRange == null || VerticalRange == null) return;
            if (HorizontalRange.Distance == 0 || VerticalRange.Distance == 0) return;

            if (double.IsNaN(Width))
                Kx = RenderSize.Width / HorizontalRange.Distance;
            else
                Kx = Width / HorizontalRange.Distance;

            if (double.IsNaN(Height))
                _Height = RenderSize.Height;
            else
                _Height = Height;

            Ky = _Height / VerticalRange.Distance;
        }

        /// <summary>
        /// 数据点到视图点的投影
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private System.Drawing.PointF Normalize(Point p)
          => new System.Drawing.PointF
            (
              (float)((p.X - HorizontalRange.Min) * Kx),
              (float)(_Height - (p.Y - VerticalRange.Min) * Ky)
            );

        /// <summary>
        /// 刷新
        /// </summary>
        private void Refresh()
        {
            Root.Children.Clear();
            if (Kx == 0 || Ky == 0) return;
            if (DataSeries.Any())
            {
                var points = new List<System.Drawing.PointF>();
                DataSeries.ToList().ForEach(
                    p =>
                    {
                        points.Add(Normalize(p));
                    }
                );

                if (points.Count() <= 1) return;

                // 创建一个新的图形
                WriteableBitmap bitmap = new WriteableBitmap
                (
                    (int)this.RenderSize.Width,
                    (int)this.RenderSize.Height,
                    96, 96,
                    PixelFormats.Bgra32,
                    null
                );
                Image figure = new Image { Source = bitmap };
                bitmap.Lock(); /* 别忘了锁定 */

                // 绘制图形
                using (System.Drawing.Bitmap buff = new System.Drawing.Bitmap(
                        (int)bitmap.Width,
                        (int)bitmap.Height,
                        bitmap.BackBufferStride,
                        System.Drawing.Imaging.PixelFormat.Format32bppArgb,
                        bitmap.BackBuffer))
                {
                    // 使用GDI+绘制
                    using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(buff))
                    {
                        Color color = (Foreground as SolidColorBrush).Color;
                        var brush = System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
                        var pen = new System.Drawing.Pen(brush, 1); /* 颜色和线条宽度 */
                        graphics.DrawLines(pen, points.ToArray());
                        graphics.Flush();
                    }
                }

                bitmap.AddDirtyRect(new Int32Rect(0, 0, (int)bitmap.Width, (int)bitmap.Height));
                bitmap.Unlock();

                Root.Children.Add(figure);
            }
        }

        /// <summary>
        /// 重载追加对前景色改变的响应
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == ForegroundProperty)
                Refresh();
        }


    }
}
