using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ProgressLoading.Progresses
{
    /// <summary>
    /// CircularProgressBar.xaml 的交互逻辑
    /// </summary>
    [TemplatePart(Name = "PART_Ellipse", Type = typeof(Ellipse)),
     TemplatePart(Name = "PART_Progress", Type = typeof(Path)),]
    public partial class ProgressCircle : RangeBase
    {
        static ProgressCircle()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressCircle), new FrameworkPropertyMetadata(typeof(ProgressCircle)));
        }

        public ProgressCircle()
        {
            SetCurrentValue(MinimumProperty, 0d);
            SetCurrentValue(MaximumProperty, 360d);
            InitializeComponent();
        }
        private Path? path;

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty RadiusProperty =
          DependencyProperty.Register(nameof(Radius), typeof(double), typeof(ProgressCircle), new FrameworkPropertyMetadata(75d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));


        public double CircleThickness
        {
            get { return (double)GetValue(CircleThicknessProperty); }
            set { SetValue(CircleThicknessProperty, value); }
        }

        public static readonly DependencyProperty CircleThicknessProperty =
            DependencyProperty.Register(nameof(CircleThickness), typeof(double), typeof(ProgressCircle), new FrameworkPropertyMetadata(6d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));


        public double CircleBarThickness
        {
            get { return (double)GetValue(CircleBarThicknessProperty); }
            set { SetValue(CircleBarThicknessProperty, value); }
        }

        public static readonly DependencyProperty CircleBarThicknessProperty =
            DependencyProperty.Register(nameof(CircleBarThickness), typeof(double), typeof(ProgressCircle), new FrameworkPropertyMetadata(4d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            SetCircularBarLength();
        }


        private void SetCircularBarLength()
        {
            const double _2pi = 2 * Math.PI;

            var rad = (Value / (Maximum - Minimum)) * _2pi;

            double offset = CircleThickness / 2;

            double r = Radius - offset;

            double x = Radius + r * Math.Sin(rad);
            double y = Radius - r * Math.Cos(rad);

            int isLargeArcFlag = rad < Math.PI ? 0 : 1;

            if (path != null)
                path.Data = PathGeometry.Parse($"M {Radius} {offset} A {r} {r} 0 {isLargeArcFlag} 1 {x} {y}");
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            path = GetTemplateChild("PART_Progress") as Path;

        }

        protected override Size MeasureOverride(Size constraint)
        {
            Ellipse? ellipse = base.GetTemplateChild("PART_Ellipse") as Ellipse;
            ellipse.Width = Radius * 2;
            ellipse.Height = Radius * 2;

            return base.MeasureOverride(constraint);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            SetCircularBarLength();
            return base.ArrangeOverride(arrangeBounds);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }

    }
}
