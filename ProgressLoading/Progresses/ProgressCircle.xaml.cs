using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
            SetCurrentValue(MaximumProperty, 100d);
            InitializeComponent();
        }

        private Path? path;
        private Storyboard? _indeterminateStoryboard;

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

        public Brush CircleBrush
        {
            get => (Brush)GetValue(CircleBrushProperty);
            set => SetValue(CircleBrushProperty, value);
        }

        public static readonly DependencyProperty CircleBrushProperty =
            DependencyProperty.Register(
                nameof(CircleBrush),
                typeof(Brush),
                typeof(ProgressCircle),
                new PropertyMetadata(Brushes.Transparent));

        public Brush CircleBarStroke
        {
            get => (Brush)GetValue(CircleBarStrokeProperty);
            set => SetValue(CircleBarStrokeProperty, value);
        }

        public static readonly DependencyProperty CircleBarStrokeProperty =
            DependencyProperty.Register(
                nameof(CircleBarStroke),
                typeof(Brush),
                typeof(ProgressCircle),
                new PropertyMetadata(Brushes.Transparent));

        public bool IsIndeterminate
        {
            get => (bool)GetValue(IsIndeterminateProperty);
            set => SetValue(IsIndeterminateProperty, value);
        }

        public static readonly DependencyProperty IsIndeterminateProperty =
            DependencyProperty.Register(
                nameof(IsIndeterminate),
                typeof(bool),
                typeof(ProgressCircle),
                new PropertyMetadata(false, OnIsIndeterminateChanged));


        private static void OnIsIndeterminateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProgressCircle circle)
            {
                if ((bool)e.NewValue)
                {
                    circle.StartIndeterminateAnimation();
                }
                else
                {
                    circle.StopIndeterminateAnimation();
                    circle.SetArcPath(circle.Value);
                }
            }
        }

        public double AnimationValueDuration
        {
            get => (double)GetValue(AnimationValueDurationProperty);
            set => SetValue(AnimationValueDurationProperty, value);
        }

        public static readonly DependencyProperty AnimationValueDurationProperty =
            DependencyProperty.Register(
                nameof(AnimationValueDuration),
                typeof(double),
                typeof(ProgressCircle),
                new PropertyMetadata(400d));

        public double AnimationValue
        {
            get => (double)GetValue(AnimationValueProperty);
            set => SetValue(AnimationValueProperty, value);
        }

        public static readonly DependencyProperty AnimationValueProperty =
                DependencyProperty.Register(
                    nameof(AnimationValue),
                    typeof(double),
                    typeof(ProgressCircle),
                    new PropertyMetadata(0.0, OnAnimationValueChanged),
                    value => value is double d && d >= 0.0 // ValidateValueCallback
                );

        private static void OnAnimationValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProgressCircle circle)
            {
                if (circle.IsIndeterminate)
                    circle.DrawFlowArc((double)e.NewValue);
                else
                    circle.SetArcPath((double)e.NewValue);
            }
        }

        private void StartIndeterminateAnimation()
        {
            StopIndeterminateAnimation();

            var animation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(2)))
            {
                RepeatBehavior = RepeatBehavior.Forever
            };
            Storyboard.SetTarget(animation, this);
            Storyboard.SetTargetProperty(animation, new PropertyPath(nameof(AnimationValue)));

            _indeterminateStoryboard = new Storyboard();
            _indeterminateStoryboard.Children.Add(animation);
            _indeterminateStoryboard.Begin();
        }


        private void StopIndeterminateAnimation()
        {
            _indeterminateStoryboard?.Stop();
            _indeterminateStoryboard = null;
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            if (IsIndeterminate)
            { }
            else
            {
                if (AnimationValueDuration <= 0)
                    SetArcPath(newValue);
                else
                {
                    StopIndeterminateAnimation();

                    var animation = new DoubleAnimation(oldValue, newValue, new Duration(TimeSpan.FromMilliseconds(AnimationValueDuration)))
                    {
                        RepeatBehavior = new RepeatBehavior(1)
                    };
                    Storyboard.SetTarget(animation, this);
                    Storyboard.SetTargetProperty(animation, new PropertyPath(nameof(AnimationValue)));

                    _indeterminateStoryboard = new Storyboard();
                    _indeterminateStoryboard.Children.Add(animation);
                    _indeterminateStoryboard.Begin();
                }
            }
        }


        private void SetArcPath(double value)
        {
            const double _2pi = 2 * Math.PI;

            // 防止除零
            if (Maximum - Minimum == 0) return;

            var percent = (value - Minimum) / (Maximum - Minimum);
            var rad = percent * _2pi;

            double offset = CircleThickness / 2;
            double r = Radius - offset;

            // 起点
            double x0 = Radius;
            double y0 = offset;

            // 终点
            double x = Radius + r * Math.Sin(rad);
            double y = Radius - r * Math.Cos(rad);

            int isLargeArcFlag = rad < Math.PI ? 0 : 1;

            if (path != null)
            {
                if (percent >= 1.0)
                {
                    // 绘制完整圆环
                    path.Data = PathGeometry.Parse(
                        $"M {Radius} {offset} " +
                        $"A {r} {r} 0 1 1 {Radius - 0.01} {offset} "
                    );
                }
                else if (percent <= 0)
                {
                    path.Data = null;
                }
                else
                {
                    path.Data = PathGeometry.Parse(
                        $"M {x0} {y0} A {r} {r} 0 {isLargeArcFlag} 1 {x} {y}"
                    );
                }
            }
        }

        private void DrawFlowArc(double mCurAnimValue = 0.5)
        {
            if (path == null) return;

            const double _2pi = 2 * Math.PI;
            double offset = CircleThickness / 2;
            double r = Radius - offset;
            double length = _2pi * r;

            // 计算stop和start（以弧长为单位）
            double stop = length * mCurAnimValue;
            double segmentLen = (0.5 - Math.Abs(mCurAnimValue - 0.5)) * length;
            double start = stop - segmentLen;

            // 转换为弧度（0~2π）
            double startRad = (start / length) * _2pi;
            double stopRad = (stop / length) * _2pi;

            // 起点
            double x0 = Radius + r * Math.Sin(startRad);
            double y0 = Radius - r * Math.Cos(startRad);

            // 终点
            double x1 = Radius + r * Math.Sin(stopRad);
            double y1 = Radius - r * Math.Cos(stopRad);

            // 计算大弧标志
            double sweepRad = stopRad - startRad;
            if (sweepRad < 0) sweepRad += _2pi;
            int isLargeArcFlag = sweepRad > Math.PI ? 1 : 0;

            // 画弧
            if (segmentLen > 0)
            {
                path.Data = PathGeometry.Parse(
                    $"M {x0} {y0} A {r} {r} 0 {isLargeArcFlag} 1 {x1} {y1}"
                );
            }
            else
            {
                path.Data = null;
            }
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
            if (IsIndeterminate)
            { }
            else
                SetArcPath(Value);

            return base.ArrangeOverride(arrangeBounds);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }
    }
}
