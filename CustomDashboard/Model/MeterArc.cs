using CustomDashboard.Common;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;


namespace CustomDashboard.Model
{
    public class MeterArc : ArcShape
    {
        public MeterArc()
        {
            SetCurrentValue(Shape.FillProperty, Brushes.Chocolate);
            SetCurrentValue(Shape.StrokeThicknessProperty, 0d);
            SetCurrentValue(Shape.StrokeProperty, Brushes.Orange);
        }

        protected override Geometry GeometryGenerator()
        {
            StreamGeometry stream = new StreamGeometry();
            using (StreamGeometryContext context = stream.Open())
            {
                if (base.ActualHeight * base.ActualWidth == 0.0 && base.Height * base.Width == 0.0)
                    return stream;

                double aMin = Math.Min(AngleFrom, AngleTo);
                double aMax = Math.Max(AngleFrom, AngleTo);

                double b = Interpo.GetBaseLength(this);
                double rMin = b * Math.Min(RadiusFrom, RadiusTo);
                double rMax = b * Math.Max(RadiusFrom, RadiusTo);

                Point p1 = AxisTransfer.PolarToCartesian(aMin, rMin);
                Point p2 = AxisTransfer.PolarToCartesian(aMax, rMin);
                Point p3 = AxisTransfer.PolarToCartesian(aMax, rMax);
                Point p4 = AxisTransfer.PolarToCartesian(aMin, rMax);

                Size sMin = new Size(rMin, rMin);
                Size sMax = new Size(rMax, rMax);

                bool isLargeArc = Math.Abs(aMax - aMin) > 180;

                // 偏移至中心点
                var offset = Interpo.GetCenterPoint(this);

                p1.Offset(offset.X, offset.Y);
                p2.Offset(offset.X, offset.Y);
                p3.Offset(offset.X, offset.Y);
                p4.Offset(offset.X, offset.Y);

                context.BeginFigure(p1, isFilled: true, isClosed: true);
                context.ArcTo(p2, sMin, 0d, isLargeArc, SweepDirection.Clockwise, isStroked: true, isSmoothJoin: false);
                context.LineTo(p3, isStroked: true, isSmoothJoin: false);
                context.ArcTo(p4, sMax, 0d, isLargeArc, SweepDirection.Counterclockwise, isStroked: true, isSmoothJoin: false);
                context.LineTo(p1, isStroked: true, isSmoothJoin: false);
            }
            return stream;
        }
    }
}
