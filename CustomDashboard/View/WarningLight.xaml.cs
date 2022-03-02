using System.Windows;
using System.Windows.Controls;

namespace CustomDashboard.View
{
    /// <summary>
    /// WarningLight.xaml 的交互逻辑
    /// </summary>
    public partial class WarningLight : UserControl
    {
        public LightState LightState
        {
            get { return (LightState)GetValue(LightStateProperty); }
            set { SetValue(LightStateProperty, value); }
        }
        public static readonly DependencyProperty LightStateProperty =
            DependencyProperty.Register("LightState", typeof(LightState), typeof(WarningLight), new PropertyMetadata(LightState.None, new PropertyChangedCallback(OnLightTypeChanged)));

        private static void OnLightTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // 执行视觉状态的更新 

            // 属性需要变化
            // 这个变化体现界面上：视觉状态来触发一个动画
            var state = e.NewValue.ToString();
            VisualStateManager.GoToState(d as WarningLight, state, false);
        }

        public WarningLight()
        {
            InitializeComponent();

            this.LightState = LightState.Fault;
            this.LightState.ToString();  // Fault
        }
    }

    public enum LightState
    {
        None, Fault, Warning, Run
    }
}
