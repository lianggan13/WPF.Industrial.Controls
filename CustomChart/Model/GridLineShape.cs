using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;


namespace CustomChart.Model
{
    public class GridLineShape : Shape
    {
        /// <summary>
        /// 横向分块数
        /// </summary>
        [Category("Custom")]
        [Description("横向分块数")]
        public double HorizontalCount
        {
            get { return (double)GetValue(HorizontalCountProperty); }
            set { SetValue(HorizontalCountProperty, value); }
        }
        /// <summary>
        /// <see cref="HorizontalCount"/>
        /// </summary>
        public static readonly DependencyProperty HorizontalCountProperty = DependencyProperty.Register
          (nameof(HorizontalCount), typeof(double), typeof(GridLineShape),
            new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 纵向分块数
        /// </summary>
        [Category("Custom")]
        [Description("纵向分块数")]
        public double VerticalCount
        {
            get { return (double)GetValue(VerticalCountProperty); }
            set { SetValue(VerticalCountProperty, value); }
        }
        /// <summary>
        /// <see cref="VerticalCount"/>
        /// </summary>
        public static readonly DependencyProperty VerticalCountProperty = DependencyProperty.Register
          (nameof(VerticalCount), typeof(double), typeof(GridLineShape),
            new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender));

        public GridLineShape()
        {
            SetCurrentValue(HorizontalCountProperty, 10d);
            SetCurrentValue(VerticalCountProperty, 5d);

            /* 颜色、宽度、分布默认值 */
            SetCurrentValue(StrokeProperty, Brushes.Lime);
            SetCurrentValue(StrokeThicknessProperty, 2d);
            SetCurrentValue(StrokeDashArrayProperty, new DoubleCollection() { 1, 1, 1 });

            // binding parent's width & height
            Loaded += (s, e) =>
            {
                var panel = this.Parent as Panel;
                if (panel == null)
                    return;

                SetBinding(WidthProperty, new Binding(nameof(ActualWidth)) { Source = panel });
                SetBinding(HeightProperty, new Binding(nameof(ActualHeight)) { Source = panel });
            };

        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                double width = double.IsNaN(Width) ? RenderSize.Width : Width;
                double height = double.IsNaN(Height) ? RenderSize.Height : Height;
                if (width <= 0 || height <= 0) return StreamGeometry.Empty;
                if (HorizontalCount <= 1 || VerticalCount <= 0) return StreamGeometry.Empty;

                StreamGeometry stream = new StreamGeometry();

                using (StreamGeometryContext geo = stream.Open())
                {
                    // 纵向线
                    int i = 1;
                    double offset = width / HorizontalCount;
                    Point p1, p2;
                    //p1 = new Point(0, 0);
                    //p2 = new Point(0, height);
                    //geo.BeginFigure(p1, false, false);
                    //geo.LineTo(p2, true, false);

                    do
                    {
                        p1 = new Point(i * offset, 0);
                        p2 = new Point(i * offset, height);
                        geo.BeginFigure(p1, false, false);
                        geo.LineTo(p2, true, false);
                        i += 1;
                    } while (i < HorizontalCount);

                    // 横向线
                    i = 1;
                    offset = height / VerticalCount;
                    do
                    {
                        p1 = new Point(0, i * offset);
                        p2 = new Point(width, i * offset);
                        geo.BeginFigure(p1, false, false);
                        geo.LineTo(p2, true, false);
                        i += 1;
                    } while (i < VerticalCount);

                }
                stream.Freeze();
                return stream;
            }
        }

    }
}
