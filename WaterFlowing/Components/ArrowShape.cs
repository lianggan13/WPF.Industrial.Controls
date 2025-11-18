using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WaterFlowing.Components
{
    public class ArrowShape : Shape
    {
        // 斜切程度依赖属性，范围0~1
        public static readonly DependencyProperty SkewRatioProperty =
            DependencyProperty.Register(
                nameof(SkewRatio),
                typeof(double),
                typeof(ArrowShape),
                new FrameworkPropertyMetadata(0.5, FrameworkPropertyMetadataOptions.AffectsRender, null, CoerceSkewRatio));

        public double SkewRatio
        {
            get => (double)GetValue(SkewRatioProperty);
            set => SetValue(SkewRatioProperty, value);
        }

        private static object CoerceSkewRatio(DependencyObject d, object baseValue)
        {
            double v = (double)baseValue;
            if (v < 0) return 0.0;
            if (v > 1) return 1.0;
            return v;
        }

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register(
                nameof(Direction),
                typeof(ArrowDirection),
                typeof(ArrowShape),
                new FrameworkPropertyMetadata(ArrowDirection.Right,
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public ArrowDirection Direction
        {
            get => (ArrowDirection)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                double w = Width > 0 ? Width : 18;
                double h = Height > 0 ? Height : 13;
                double skew = w * SkewRatio;

                var geometry = new StreamGeometry();
                using (var ctx = geometry.Open())
                {
                    if (Direction == ArrowDirection.Right)
                    {
                        // 朝右
                        ctx.BeginFigure(new Point(w - skew, 0), true, true);
                        ctx.LineTo(new Point(0, 0), false, false);
                        ctx.LineTo(new Point(skew, h / 2), false, false);
                        ctx.LineTo(new Point(w, h / 2), false, false);

                        ctx.BeginFigure(new Point(w, h / 2), true, true);
                        ctx.LineTo(new Point(skew, h / 2), false, false);
                        ctx.LineTo(new Point(0, h), false, false);
                        ctx.LineTo(new Point(w - skew, h), false, false);
                    }
                    else
                    {
                        // 朝左
                        ctx.BeginFigure(new Point(skew, 0), true, true);
                        ctx.LineTo(new Point(w, 0), false, false);
                        ctx.LineTo(new Point(w - skew, h / 2), false, false);
                        ctx.LineTo(new Point(0, h / 2), false, false);

                        ctx.BeginFigure(new Point(0, h / 2), true, true);
                        ctx.LineTo(new Point(w - skew, h / 2), false, false);
                        ctx.LineTo(new Point(w, h), false, false);
                        ctx.LineTo(new Point(skew, h), false, false);
                    }
                }
                geometry.Freeze();
                return geometry;
            }
        }
    }

    public enum ArrowDirection
    {
        Left,
        Right
    }
}