using CustomDashboard.Common;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;


namespace CustomDashboard.Model
{
    public class MeterNeedle : Shape
    {
        /// <summary>
        /// 针尖的高度，相对值 0~1
        /// </summary>
        [Category("Custom")]
        [Description("针尖的高度，相对值 0~1")]
        public double NeedleHeight
        {
            get { return (double)GetValue(NeedleHeightProperty); }
            set { SetValue(NeedleHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NeedleHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NeedleHeightProperty =
            DependencyProperty.Register("NeedleHeight", typeof(double), typeof(MeterNeedle), new FrameworkPropertyMetadata(0.0618d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 针尖头部的宽度，相对值 0~1
        /// </summary>
        [Category("Custom")]
        [Description("针尖头部的宽度，相对值 0~1")]
        public double NeedleHeadWidth
        {
            get { return (double)GetValue(NeedleHeadWidthProperty); }
            set { SetValue(NeedleHeadWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NeedleWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NeedleHeadWidthProperty =
            DependencyProperty.Register("NeedleWidth", typeof(double), typeof(MeterNeedle), new FrameworkPropertyMetadata(0.2d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 针尖尾部的宽度，相对值0~1
        /// </summary>
        [Category("Custom")]
        [Description("针尖尾部的宽度，相对值0~1")]
        public double NeedleTailLength
        {
            get { return (double)GetValue(NeedleTailLengthProperty); }
            set { SetValue(NeedleTailLengthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NeedleTailLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NeedleTailLengthProperty =
            DependencyProperty.Register("NeedleTailLength", typeof(double), typeof(MeterNeedle), new FrameworkPropertyMetadata(0.1d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 针尖的半径，相对值0~1
        /// </summary>
        [Category("Custom")]
        [Description("针尖的半径，相对值0~1")]

        public double NeedleRadius
        {
            get { return (double)GetValue(NeedleRadiusProperty); }
            set { SetValue(NeedleRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NeedleRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NeedleRadiusProperty =
            DependencyProperty.Register("NeedleRadius", typeof(double), typeof(MeterNeedle), new FrameworkPropertyMetadata(0.43d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));


        protected override Geometry DefiningGeometry => GeometryGenerator();

        public MeterNeedle()
        {
            SetCurrentValue(Shape.FillProperty, Brushes.SteelBlue);
            SetCurrentValue(Shape.StrokeThicknessProperty, 2d);
            SetCurrentValue(Shape.StrokeProperty, Brushes.Gray);
        }

        private Geometry GeometryGenerator()
        {
            StreamGeometry stream = new StreamGeometry();
            using (StreamGeometryContext context = stream.Open())
            {
                if (base.ActualHeight * base.ActualWidth == 0d)
                    return stream;

                double b = Interpo.GetBaseLength(this);
                double height = b * NeedleHeight;
                double tail = b * NeedleTailLength;
                double head = b * NeedleHeadWidth;
                double r = b * NeedleRadius - tail - head - base.StrokeThickness; // 可能为负数
                double hd = height / 2;
                Point sp = new Point(0, -hd);
                Point p1 = new Point(tail, -hd);
                Point p2 = new Point(tail + head, 0);
                Point p3 = new Point(tail, hd);
                Point p4 = new Point(0, hd);

                Point offset = Interpo.GetCenterPoint(this);

                sp.Offset(offset.X + r, offset.Y);
                p1.Offset(offset.X + r, offset.Y);
                p2.Offset(offset.X + r, offset.Y);
                p3.Offset(offset.X + r, offset.Y);
                p4.Offset(offset.X + r, offset.Y);

                context.BeginFigure(sp, isFilled: true, isClosed: true);
                context.LineTo(p1, isStroked: true, isSmoothJoin: true);
                context.LineTo(p2, isStroked: true, isSmoothJoin: true);
                context.LineTo(p3, isStroked: true, isSmoothJoin: true);
                context.LineTo(p4, isStroked: true, isSmoothJoin: true);
            }

            return stream;
        }
    }
}
