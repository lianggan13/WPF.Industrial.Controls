using CustomDashboard.Common;
using CustomDashboard.Model;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace CustomDashboard.View
{
    /// <summary>
    /// SpeedMeter.xaml 的交互逻辑
    /// </summary>
    [TemplatePart(Name = nameof(PART_MeterArcsCanvas), Type = typeof(Canvas))]
    [TemplatePart(Name = nameof(PART_MeterTicksCanvas), Type = typeof(Canvas))]
    [TemplatePart(Name = nameof(PART_MeterLablesCanvas), Type = typeof(Canvas))]
    public partial class SpeedMeter : Control
    {
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(SpeedMeter), new PropertyMetadata(0d, ValuePropertyChanged));

        [Category("Custom")]
        public double ValueMax
        {
            get { return (double)GetValue(ValueMaxProperty); }
            set { SetValue(ValueMaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValueMax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueMaxProperty =
            DependencyProperty.Register("ValueMax", typeof(double), typeof(SpeedMeter), new FrameworkPropertyMetadata(180d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        [Category("Custom")]
        public double ValueMin
        {
            get { return (double)GetValue(ValueMinProperty); }
            set { SetValue(ValueMinProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValueMin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueMinProperty =
            DependencyProperty.Register("ValueMin", typeof(double), typeof(SpeedMeter), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));


        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Angle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(SpeedMeter), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));


        [Category("Custom")]
        public double AngleMax
        {
            get { return (double)GetValue(AngleMaxProperty); }
            set { SetValue(AngleMaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AngleMax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AngleMaxProperty =
            DependencyProperty.Register("AngleMax", typeof(double), typeof(SpeedMeter), new FrameworkPropertyMetadata(420d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));



        [Category("Custom")]
        public double AngleMin
        {
            get { return (double)GetValue(AngleMinProperty); }
            set { SetValue(AngleMinProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AngleMin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AngleMinProperty =
            DependencyProperty.Register("AngleMin", typeof(double), typeof(SpeedMeter), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));


        [Category("Custom")]
        [Description("仪表色盘集合")]
        public MeterArcs MeterArcs
        {
            get { return (MeterArcs)GetValue(MeterArcsProperty); }
            set { SetValue(MeterArcsProperty, value); }
        }


        // Using a DependencyProperty as the backing store for MeterArcs.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MeterArcsProperty = DependencyProperty.Register("MeterArcs", typeof(MeterArcs), typeof(SpeedMeter), new PropertyMetadata(null, DependencyPropChanged));


        [Category("Custom")]
        [Description("仪表刻度集合")]
        public MeterTicks MeterTicks
        {
            get { return (MeterTicks)GetValue(MeterTicksProperty); }
            set { SetValue(MeterTicksProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MeterTicks.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MeterTicksProperty =
            DependencyProperty.Register("MeterTicks", typeof(MeterTicks), typeof(SpeedMeter), new PropertyMetadata(null, DependencyPropChanged));

        public MeterLableStyle MeterLableStyle
        {
            get { return (MeterLableStyle)GetValue(MeterLableStyleProperty); }
            set { SetValue(MeterLableStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MeterLable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MeterLableStyleProperty =
            DependencyProperty.Register("MeterLableStyle", typeof(MeterLableStyle), typeof(SpeedMeter), new PropertyMetadata(null, DependencyPropChanged));


        public Canvas PART_MeterArcsCanvas { get; private set; }
        public Canvas PART_MeterTicksCanvas { get; private set; }
        public Canvas PART_MeterLablesCanvas { get; private set; }


        public SpeedMeter()
        {
            InitializeComponent();

            // TODO: 可考虑重写 ArrangeOverride 和 MeasureOverride
            base.SizeChanged += (s, e) =>
            {
                SpeedMeter speedMeter;
                if ((speedMeter = s as SpeedMeter) != null)
                {
                    //UpateMeterArcsCanvas(speedMeter);
                    //UpateMeterTicksCanvas(speedMeter);
                    //UpateMeterLabelsCanvas(speedMeter);
                }
            };
            UpateAngle(Value);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            UpateMeterArcsCanvas(this);
            UpateMeterTicksCanvas(this);
            UpateMeterLabelsCanvas(this);

            base.OnRenderSizeChanged(sizeInfo);


        }

        static SpeedMeter()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(SpeedMeter), new FrameworkPropertyMetadata(typeof(SpeedMeter)));
        }


        private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SpeedMeter meter && e.NewValue is double arc)
            {
                meter.UpateAngle(arc);
            }
        }

        private static void DependencyPropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SpeedMeter autometer)
            {
                if (e.Property == MeterArcsProperty)
                {
                    INotifyCollectionChanged notifyCollectionChanged;
                    if ((notifyCollectionChanged = e.OldValue as INotifyCollectionChanged) != null)
                    {
                        notifyCollectionChanged.CollectionChanged -= autometer.MeterArcs_CollectionChanged;
                    }
                    if ((notifyCollectionChanged = e.NewValue as INotifyCollectionChanged) != null)
                    {
                        notifyCollectionChanged.CollectionChanged += autometer.MeterArcs_CollectionChanged;
                    }
                }
                else if (e.Property == MeterTicksProperty)
                {
                    INotifyCollectionChanged notifyCollectionChanged;
                    if ((notifyCollectionChanged = e.OldValue as INotifyCollectionChanged) != null)
                    {
                        notifyCollectionChanged.CollectionChanged -= autometer.MeterTicks_CollectionChanged;
                    }
                    if ((notifyCollectionChanged = e.NewValue as INotifyCollectionChanged) != null)
                    {
                        notifyCollectionChanged.CollectionChanged += autometer.MeterTicks_CollectionChanged;
                    }
                }
                else if (e.Property == MeterLableStyleProperty)
                {
                    autometer.UpateMeterLabelsCanvas(autometer);
                }
            }
        }

        private void MeterArcs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpateMeterArcsCanvas(this);
        }

        private void MeterTicks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpateMeterTicksCanvas(this);
        }

        private void UpateAngle(double arc)
        {
            Angle = Interpo.Linear(x: arc, x1: this.ValueMin, y1: this.AngleMin, x2: this.ValueMax, y2: this.AngleMax);
        }

        public void UpateMeterArcsCanvas(SpeedMeter meter)
        {
            if (meter.PART_MeterArcsCanvas != null
                && meter.MeterArcs != null
                && (meter.ActualHeight * meter.ActualWidth != 0d || meter.Height * meter.Width != 0d))
            {
                meter.PART_MeterArcsCanvas.Children.Clear();
                foreach (var s in meter.MeterArcs)
                {
                    // 获取弧长
                    var arc1 = ValueRange.GetValueMin(s);   // 获取最大弧长
                    var arc2 = ValueRange.GetValueMax(s);   // 获取最小弧长

                    s.Width = (meter.ActualWidth != 0.0) ? meter.ActualWidth : meter.Width;
                    s.Height = (meter.ActualHeight != 0.0) ? meter.ActualHeight : meter.Height;

                    // 设置角度
                    s.AngleFrom = Interpo.Linear(x: arc1, x1: meter.ValueMin, y1: meter.AngleMin, x2: meter.ValueMax, y2: meter.AngleMax);
                    s.AngleTo = Interpo.Linear(x: arc2, x1: meter.ValueMin, y1: meter.AngleMin, x2: meter.ValueMax, y2: meter.AngleMax);


                    //meter.PART_MeterArcsCanvas.Children.Add(s);

                    //MeterArc shape = new MeterArc()
                    //{
                    //    Width = s.Width,
                    //    Height = s.Height,
                    //    AngleFrom = s.AngleFrom,
                    //    AngleTo = s.AngleTo,
                    //    RadiusFrom = s.RadiusFrom,
                    //    RadiusTo = s.RadiusTo,
                    //    Fill = s.Fill,
                    //};

                    meter.PART_MeterArcsCanvas.Children.Add(s);
                    s.UpdateLayout();

                    //meter.PART_MeterArcsCanvas.Children.Add(shape);

                }
            }
        }

        public void UpateMeterTicksCanvas(SpeedMeter meter)
        {
            if (meter.PART_MeterTicksCanvas != null
                && meter.MeterTicks != null
                && (meter.ActualHeight * meter.ActualWidth != 0d || meter.Height * meter.Width != 0d))
            {
                meter.PART_MeterTicksCanvas.Children.Clear();
                foreach (var s in meter.MeterTicks)
                {
                    // 获取弧长
                    var arc1 = ValueRange.GetValueMin(s);   // 获取最大弧长
                    var arc2 = ValueRange.GetValueMax(s);   // 获取最小弧长

                    // 设置角度 arc length --> angle
                    s.AngleFrom = Interpo.Linear(x: arc1, x1: meter.ValueMin, y1: meter.AngleMin, x2: meter.ValueMax, y2: meter.AngleMax);
                    s.AngleTo = Interpo.Linear(x: arc2, x1: meter.ValueMin, y1: meter.AngleMin, x2: meter.ValueMax, y2: meter.AngleMax);

                    s.Width = (meter.ActualWidth != 0.0) ? meter.ActualWidth : meter.Width;
                    s.Height = (meter.ActualHeight != 0.0) ? meter.ActualHeight : meter.Height;
                    meter.PART_MeterTicksCanvas.Children.Add(s);
                }
            }


        }

        public void UpateMeterLabelsCanvas(SpeedMeter meter)
        {
            if (meter.PART_MeterLablesCanvas != null
             && meter.MeterTicks != null
             && (meter.ActualHeight * meter.ActualWidth != 0d || meter.Height * meter.Width != 0d))
            {
                double b = Math.Min(base.ActualWidth, base.ActualHeight) / 2;    // 取一段基础长度 base length
                double r = b * MeterLableStyle.Offset;

                meter.PART_MeterLablesCanvas.Children.Clear();
                for (double v = ValueMin; v <= ValueMax; v += MeterLableStyle.Step)
                {
                    TextBlock text = new TextBlock();
                    text.Text = $"{v}";
                    text.FontSize = MeterLableStyle.FontSize;
                    text.Foreground = MeterLableStyle.Foreground;
                    text.VerticalAlignment = VerticalAlignment.Center;
                    text.HorizontalAlignment = HorizontalAlignment.Center;
                    text.TextAlignment = TextAlignment.Center;

                    double angle = Interpo.Linear(x: v, x1: meter.ValueMin, y1: meter.AngleMin, x2: ValueMax, y2: AngleMax);

                    Point p1 = AxisTransfer.PolarToCartesian(angle, r);

                    Point offset = Interpo.GetCenterPoint(this);

                    // 偏移至中心点
                    p1.Offset(offset.X, offset.Y);
                    meter.PART_MeterLablesCanvas.Children.Add(text);

                    text.UpdateLayout();

                    Canvas.SetLeft(text, p1.X - text.ActualWidth / 2);  // 相对大小修正位置 - text.ActualWidth / 2
                    Canvas.SetTop(text, p1.Y - text.ActualHeight / 2);  // 相对大小修正位置 - text.ActualHeight / 2
                }
            }
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_MeterArcsCanvas = GetTemplateChild(nameof(PART_MeterArcsCanvas)) as Canvas;
            PART_MeterTicksCanvas = GetTemplateChild(nameof(PART_MeterTicksCanvas)) as Canvas;
            PART_MeterLablesCanvas = GetTemplateChild(nameof(PART_MeterLablesCanvas)) as Canvas;
        }


        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            return base.ArrangeOverride(arrangeBounds);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            return base.MeasureOverride(constraint);
        }
    }

}
