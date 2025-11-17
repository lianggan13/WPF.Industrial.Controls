using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace LiquidLine.Components
{
    /// <summary>
    /// LiquidLine.xaml 的交互逻辑
    /// </summary>
    public partial class LiquidLine : UserControl
    {
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(
            nameof(Direction), typeof(FlowDirection), typeof(LiquidLine),
            new PropertyMetadata(FlowDirection.LeftToRight, OnDirectionChanged));

        public FlowDirection Direction
        {
            get => (FlowDirection)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }

        private static void OnDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            return;
            if (d is LiquidLine ctrl)
            {
                var stateName = e.NewValue.ToString();
                // Use GoToElementState targeting the named Root element where VisualStateGroups live
                //VisualStateManager.GoToElementState(ctrl, stateName, true);
                VisualStateManager.GoToState(ctrl, stateName, true);
            }
        }

        // IsAnimationEnabled dependency property
        public static readonly DependencyProperty IsAnimationEnabledProperty = DependencyProperty.Register(
            nameof(IsAnimationEnabled), typeof(bool), typeof(LiquidLine),
            new PropertyMetadata(false, OnIsAnimationEnabledChanged));

        public bool IsAnimationEnabled
        {
            get => (bool)GetValue(IsAnimationEnabledProperty);
            set => SetValue(IsAnimationEnabledProperty, value);
        }

        private static void OnIsAnimationEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LiquidLine ctrl)
            {
                var enabled = (bool)e.NewValue;
                var stateName = enabled ? ctrl.Direction.ToString() : "Normal";
                //VisualStateManager.GoToElementState(ctrl, stateName, true);
                bool ok = VisualStateManager.GoToState(ctrl, stateName, true);

                if (!enabled)
                    ctrl.StopAnimationAndHoldCurrentValue(); // 停止动画并保持当前可视值
            }
        }

        public Style BorderStyle
        {
            get { return (Style)GetValue(BorderStyleProperty); }
            set { SetValue(BorderStyleProperty, value); }
        }

        public static readonly DependencyProperty BorderStyleProperty =
            DependencyProperty.Register(nameof(BorderStyle), typeof(Style), typeof(LiquidLine), new PropertyMetadata(null));

        public Style LineStyle
        {
            get { return (Style)GetValue(LineStyleProperty); }
            set { SetValue(LineStyleProperty, value); }
        }

        public static readonly DependencyProperty LineStyleProperty =
            DependencyProperty.Register(nameof(LineStyle), typeof(Style), typeof(LiquidLine), new PropertyMetadata(null));


        /// <summary>
        /// 在停止动画前读取当前受动画影响的值，移除动画并设置为该值，从而“停止并保持”视觉状态
        /// </summary>
        private void StopAnimationAndHoldCurrentValue()
        {
            if (line == null) return;
            // 读取当前有效值（若动画在运行，这里得到动画当前帧的值）
            var currentOffsetObj = line.GetValue(Shape.StrokeDashOffsetProperty);
            if (currentOffsetObj is double currentOffset)
            {
                // 移除任何动画（等价于 BeginAnimation(..., null)）
                //liquidLine.BeginAnimation(Shape.StrokeDashOffsetProperty, null);

                // 写回当前值，使其成为本地值，从而保持视觉效果
                line.SetValue(Shape.StrokeDashOffsetProperty, currentOffset);
            }
        }

        public LiquidLine()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // set border style
            if (border != null && BorderStyle != null)
                border.Style = BorderStyle;
            // set line style
            if (line != null && LineStyle != null)
                line.Style = LineStyle;
        }
    }
}
