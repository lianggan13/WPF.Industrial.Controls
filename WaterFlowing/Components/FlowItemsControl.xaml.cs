using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WaterFlowing.Components
{
    public partial class FlowItemsControl : UserControl
    {
        public static readonly DependencyProperty CountProperty =
            DependencyProperty.Register(
                nameof(Count),
                typeof(int),
                typeof(FlowItemsControl),
                new PropertyMetadata(8, OnCountChanged));

        public int Count
        {
            get => (int)GetValue(CountProperty);
            set => SetValue(CountProperty, value);
        }

        private static void OnCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FlowItemsControl flow && flow.IsLoaded)
            {
                flow.GenerateFlowItemsAndStoryboard();
            }
        }

        private readonly Color onColor = (Color)ColorConverter.ConvertFromString("#FF56F7E4");
        private readonly Color offColor = (Color)ColorConverter.ConvertFromString("#FF265069");

        public static readonly DependencyProperty LightFlowDirectionProperty =
            DependencyProperty.Register(
                nameof(LightFlowDirection),
                typeof(FlowDirection),
                typeof(FlowItemsControl),
                new PropertyMetadata(FlowDirection.LeftToRight, OnFlowDirectionChanged));

        public FlowDirection LightFlowDirection
        {
            get => (FlowDirection)GetValue(LightFlowDirectionProperty);
            set => SetValue(LightFlowDirectionProperty, value);
        }

        private static void OnFlowDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FlowItemsControl flow && flow.IsLoaded)
            {
                flow.GenerateFlowItemsAndStoryboard();
            }
        }

        public static readonly DependencyProperty FlowItemElementTypeProperty =
            DependencyProperty.Register(
                nameof(FlowItemElementType),
                typeof(Type),
                typeof(FlowItemsControl),
                new PropertyMetadata(typeof(Rectangle), OnFlowItemElementTypeChanged));

        public Type FlowItemElementType
        {
            get => (Type)GetValue(FlowItemElementTypeProperty);
            set => SetValue(FlowItemElementTypeProperty, value);
        }

        private static void OnFlowItemElementTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FlowItemsControl flow && flow.IsLoaded)
            {
                flow.GenerateFlowItemsAndStoryboard();
            }
        }

        public static readonly DependencyProperty FlowItemStyleProperty =
            DependencyProperty.Register(
                nameof(FlowItemStyle),
                typeof(Style),
                typeof(FlowItemsControl),
                new PropertyMetadata(DefaultFlowItemStyle()));

        private static Style DefaultFlowItemStyle()
        {
            var style = new Style(typeof(Rectangle));
            style.Setters.Add(new Setter(Shape.FillProperty, new SolidColorBrush(Color.FromRgb(38, 80, 105))));
            style.Setters.Add(new Setter(Shape.WidthProperty, 18.0));
            style.Setters.Add(new Setter(Shape.HeightProperty, 13.0));
            style.Setters.Add(new Setter(FrameworkElement.MarginProperty, new Thickness(4)));

            return style;
        }

        public Style FlowItemStyle
        {
            get => (Style)GetValue(FlowItemStyleProperty);
            set => SetValue(FlowItemStyleProperty, value);
        }

        public static readonly DependencyProperty IsAnimationEnabledProperty =
            DependencyProperty.Register(
                nameof(IsAnimationEnabled),
                typeof(bool),
                typeof(FlowItemsControl),
                new PropertyMetadata(true, OnIsAnimationEnabledChanged));

        public bool IsAnimationEnabled
        {
            get => (bool)GetValue(IsAnimationEnabledProperty);
            set => SetValue(IsAnimationEnabledProperty, value);
        }

        private static void OnIsAnimationEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FlowItemsControl flow)
            {
                if ((bool)e.NewValue)
                {
                    flow._storyboard?.Begin(flow, true);
                }
                else
                {
                    flow._storyboard?.Stop(flow);
                }
            }
        }

        private Storyboard _storyboard;

        public FlowItemsControl()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            GenerateFlowItemsAndStoryboard();
        }

        private void GenerateFlowItemsAndStoryboard()
        {
            if (this.FindName("stackPanel") is not StackPanel sp)
                return;

            if (_storyboard != null)
                _storyboard.Stop();

            sp.Children.Clear();

            var storyboard = new Storyboard { RepeatBehavior = RepeatBehavior.Forever };

            for (int i = 0; i < Count; i++)
            {
                string rectName = $"rect{i + 1}";
                // 如果已注册，先注销
                try { UnregisterName(rectName); } catch { }

                var rect = Activator.CreateInstance(FlowItemElementType) as FrameworkElement;
                if (rect == null)
                    throw new InvalidOperationException($"Cannot create instance of {FlowItemElementType}");

                rect.Name = rectName;
                if (FlowItemStyle != null)
                    rect.Style = FlowItemStyle;

                if (LightFlowDirection == FlowDirection.LeftToRight)
                {
                    sp.Children.Add(rect);
                }
                else if (LightFlowDirection == FlowDirection.RightToLeft)
                {
                    sp.Children.Insert(0, rect);
                }

                RegisterName(rect.Name, rect);

                // 点亮动画
                var onAnim = new ColorAnimation
                {
                    To = onColor,
                    Duration = TimeSpan.FromSeconds(0.2),
                    BeginTime = TimeSpan.FromSeconds(i * 0.2)
                };
                Storyboard.SetTarget(onAnim, rect);
                Storyboard.SetTargetProperty(onAnim, new PropertyPath("Fill.Color"));
                storyboard.Children.Add(onAnim);

                // 熄灭动画
                var offAnim = new ColorAnimation
                {
                    To = offColor,
                    Duration = TimeSpan.FromSeconds(0.2),
                    BeginTime = TimeSpan.FromSeconds(1.4 + i * 0.2)
                };
                Storyboard.SetTarget(offAnim, rect);
                Storyboard.SetTargetProperty(offAnim, new PropertyPath("Fill.Color"));
                storyboard.Children.Add(offAnim);
            }

            _storyboard = storyboard;
            if (IsAnimationEnabled)
                _storyboard.Begin(this, true);
        }
    }
}