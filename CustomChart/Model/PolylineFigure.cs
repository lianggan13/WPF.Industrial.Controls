using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CustomChart.Model
{
    public class PolylineFigure : Shape
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
                                                                            typeof(PolylineFigure),
                                                                            new PropertyMetadata(
                                                                                default(DataSeries),
                                                                                OnDataSeriesChanged
                                                                                )
                                                                        );

        private static void OnDataSeriesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as PolylineFigure;
            if (self == null)
                return;
            self.InvalidateVisual();

        }

        //(s, e) => { (s as PolylineFigure)?.Refresh();
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
                                                                            typeof(PolylineFigure),
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
                                                                            typeof(PolylineFigure),
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
            => (d as PolylineFigure)?.ResetScale(null);



        private double _Height;
        public double _Kx { get; private set; }
        public double _Ky { get; private set; }
        public Canvas Root { get; private set; }


        public PolylineFigure()
        {
            SetCurrentValue(StrokeProperty, Brushes.Lime);
            SetCurrentValue(StrokeThicknessProperty, 2d);

            SetCurrentValue(HorizontalRangeProperty, new Range(0d, 200d));
            SetCurrentValue(VerticalRangeProperty, new Range(-100, 100));

            DataSeries serise = new DataSeries()
            {
                new Point(0, 0),
                new Point(50, -20),
                new Point(30, 50),
                new Point(123, 100),
            };
            SetCurrentValue(DataSeriesProperty, serise);

            //SizeChanged += (s, e) =>
            //{
            //    ResetScale(s as PolylineFigure);
            //};
        }


        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            ResetScale(this);
        }

        /// <summary>
        /// 重置缩放因子
        /// </summary>
        private void ResetScale(PolylineFigure self)
        {
            _Kx = 0;
            _Ky = 0;

            if (HorizontalRange != null && HorizontalRange.Distance != 0)
            {
                if (!double.IsNaN(Width))
                    _Kx = Width / HorizontalRange.Distance;
                else if (RenderSize.Width > 0)
                    _Kx = RenderSize.Width / HorizontalRange.Distance;
            }

            if (VerticalRange != null && VerticalRange.Distance != 0)
            {
                if (!double.IsNaN(Height))
                {
                    _Ky = Height / VerticalRange.Distance;
                    _Height = Height;
                }
                else if (RenderSize.Height > 0)
                {
                    _Ky = RenderSize.Height / VerticalRange.Distance;
                    _Height = RenderSize.Height;
                }
            }

            InvalidateVisual(); // --> GetGeomoetry()

            #region old code
            //if (HorizontalRange == null || VerticalRange == null) return;
            //if (HorizontalRange.Distance == 0 || VerticalRange.Distance == 0) return;

            //if (double.IsNaN(Width))
            //    _Kx = RenderSize.Width / HorizontalRange.Distance;
            //else
            //    _Kx = Width / HorizontalRange.Distance;

            //if (double.IsNaN(Height))
            //    _Height = RenderSize.Height;
            //else
            //    _Height = Height;

            //_Ky = _Height / VerticalRange.Distance; 
            #endregion
        }


        protected override Geometry DefiningGeometry => GetGeomoetry();

        private Geometry GetGeomoetry()
        {
            if (_Kx == 0 || _Ky == 0)
                return StreamGeometry.Empty;

            StreamGeometry stream = new StreamGeometry();
            using (StreamGeometryContext geom = stream.Open())
            {
                var points = DataSeries.Select(p => Normalize(p)).ToList();
                geom.BeginFigure(points[0], false, false);
                geom.PolyLineTo(points, true, true);
            }
            stream.Freeze();
            return stream;
        }

        /// <summary>
        /// 数据点到视图点的投影
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private Point Normalize(Point p)
          => new Point
            (
              (p.X - HorizontalRange.Min) * _Kx,
                _Height - (p.Y - VerticalRange.Min) * _Ky
            );

    }
}
