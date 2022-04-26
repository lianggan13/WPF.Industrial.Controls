using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PipeLine.View
{
    /// <summary>
    /// PipeLine.xaml 的交互逻辑
    /// </summary>
    public partial class PipeLine : UserControl
    {
        public int CapRadius
        {
            get { return (int)GetValue(CapRadiusProperty); }
            set { SetValue(CapRadiusProperty, value); }
        }
        public static readonly DependencyProperty CapRadiusProperty =
            DependencyProperty.Register("CapRadius", typeof(int), typeof(PipeLine), new PropertyMetadata(2));

        // 可以做绑定
        public Brush LiquidColor
        {
            get { return (Brush)GetValue(LiquidColorProperty); }
            set { SetValue(LiquidColorProperty, value); }
        }
        public static readonly DependencyProperty LiquidColorProperty =
            DependencyProperty.Register("LiquidColor", typeof(Brush), typeof(PipeLine), new PropertyMetadata(Brushes.Orange));


        private PipelineDirection _direction = PipelineDirection.LeftToRight;
        /// <summary>
        /// 液体流向，接受1和2两个值
        /// </summary>
        public PipelineDirection Direction
        {
            get { return _direction; }
            set
            {
                _direction = value;
                // 视觉管理器的状态     触发一个动画 
                VisualStateManager.GoToState(this, value.ToString(), true);
            }
        }

        public PipeLine()
        {
            InitializeComponent();
        }
    }

    public enum PipelineDirection
    {
        LeftToRight, RightToLeft
    }
}
