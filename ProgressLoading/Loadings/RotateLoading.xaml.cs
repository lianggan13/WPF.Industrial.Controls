using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace ProgressLoading.Loadings
{
    /// <summary>
    /// RotateLoading.xaml 的交互逻辑
    /// </summary>
    [TemplatePart(Name = "PART_Canvas", Type = typeof(Canvas))]
    public partial class RotateLoading : Control
    {
        static RotateLoading()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RotateLoading), new FrameworkPropertyMetadata(typeof(RotateLoading)));
        }

        public RotateLoading()
        {
            InitializeComponent();
        }

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty RadiusProperty =
          DependencyProperty.Register(nameof(Radius), typeof(double), typeof(RotateLoading), new FrameworkPropertyMetadata(12d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public bool Run
        {
            get { return (bool)GetValue(RunProperty); }
            set
            {
                SetValue(RunProperty, value);
                ChangeVisualState();
            }
        }

        public static readonly DependencyProperty RunProperty =
            DependencyProperty.Register(nameof(Run), typeof(bool), typeof(RotateLoading), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.AffectsArrange));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Canvas canvas = base.GetTemplateChild("PART_Canvas") as Canvas;
            canvas.Width = (Radius + 10) * 2;
            canvas.Height = (Radius + 10) * 2;

            return base.MeasureOverride(constraint);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            ArrangeEllipse();
            ChangeVisualState();
            return base.ArrangeOverride(arrangeBounds);
        }

        private void ArrangeEllipse()
        {
            Canvas canvas = base.GetTemplateChild("PART_Canvas") as Canvas;
            var ellipses = canvas.Children.OfType<UIElement>().Where(u => u is Ellipse);
            for (int i = 0; i < ellipses.Count(); i++)
            {
                var ellipse = ellipses.ElementAt(i);
                MeasureXY(i, out double x, out double y);

                Canvas.SetLeft(ellipse, x);
                Canvas.SetTop(ellipse, y);
            }
        }


        private void MeasureXY(int i, out double x, out double y)
        {
            double Virtual_Radius = Radius;// 50;
            x = Virtual_Radius + Virtual_Radius * Math.Sin(i * Math.PI * 2 / 10.0);
            y = Virtual_Radius + Virtual_Radius * Math.Cos(i * Math.PI * 2 / 10.0);
        }


        private void ChangeVisualState(bool useTransitions = false)
        {
            if (Run)
            {
                VisualStateManager.GoToState(this, "Run", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Stop", useTransitions);
            }
        }
    }
}
