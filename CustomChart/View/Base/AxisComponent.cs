using CustomChart.Model;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomChart.View.Base
{
    public abstract class AxisComponent : UserControl
    {
        protected Canvas root;
        public AxisComponent()
        {
            root = new Canvas();
            Content = root;

            /* 范围和步长的默认值 */
            SetCurrentValue(RangeProperty, new Range(0d, 100d));
            SetCurrentValue(StepProperty, 10d);
        }

        [Category("Custom"), Description("数值范围")]
        public Range Range
        {
            get { return (Range)GetValue(RangeProperty); }
            set { SetValue(RangeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Range.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RangeProperty =
            DependencyProperty.Register("Range", typeof(Range), typeof(AxisComponent), new FrameworkPropertyMetadata(null,
        FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnParameterChanged));

        public double Step
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Step.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("Step", typeof(double), typeof(AxisComponent), new FrameworkPropertyMetadata(0d,
        FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnParameterChanged));


        protected static void OnParameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AxisComponent)?.Refresh();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            Refresh();
        }

        protected abstract void Refresh();
        protected abstract double Normalize(double v);

        protected virtual bool CanNotRender()
            => RenderSize.Width <= 0 || RenderSize.Height <= 0 || Range == null || Range.Distance == 0d;
    }
}
