using CustomDashboard.Common;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;


namespace CustomDashboard.Model
{
    public class MeterTick : ArcShape
    {
        public MeterTick()
        {
            //SetCurrentValue(Shape.FillProperty, Brushes.White);
            SetCurrentValue(Shape.StrokeThicknessProperty, 2d);
            SetCurrentValue(Shape.StrokeProperty, Brushes.White);
        }

        protected override Geometry GeometryGenerator()
        {
            StreamGeometry stream = new StreamGeometry();
            using (StreamGeometryContext context = stream.Open())
            {
                if (base.ActualHeight * base.ActualWidth == 0.0 && base.Height * base.Width == 0.0)
                    return stream;

                double b = Interpo.GetBaseLength(this);
                double rMin = b * Math.Min(RadiusFrom, RadiusTo);
                double rMax = b * Math.Max(RadiusFrom, RadiusTo);

                Point offset = Interpo.GetCenterPoint(this);

                double valueMin = ValueRange.GetValueMin(this);
                double valueMax = ValueRange.GetValueMax(this);
                double step = ValueRange.GetValueStep(this);

                for (double v = valueMin; v <= valueMax; v += step)
                {
                    double angle = Interpo.Linear(x: v, x1: valueMin, y1: this.AngleFrom, x2: valueMax, y2: this.AngleTo);

                    Point p1 = AxisTransfer.PolarToCartesian(angle, rMin);
                    Point p2 = AxisTransfer.PolarToCartesian(angle, rMax);

                    // 偏移至中心点
                    p1.Offset(offset.X, offset.Y);
                    p2.Offset(offset.X, offset.Y);

                    context.BeginFigure(p1, isFilled: false, isClosed: false);
                    context.LineTo(p2, isStroked: true, isSmoothJoin: false);
                }
            }
            return stream;
        }
    }
}
