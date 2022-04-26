using System.Windows;
using System.Windows.Controls;

namespace Pump.View
{
    /// <summary>
    /// FanPump.xaml 的交互逻辑
    /// </summary>
    public partial class FanPump : UserControl
    {
        public FanPump()
        {
            InitializeComponent();
        }

        public bool IsRun
        {
            get { return (bool)GetValue(IsRunProperty); }
            set { SetValue(IsRunProperty, value); }
        }
        public static readonly DependencyProperty IsRunProperty =
            DependencyProperty.Register(
                "IsRun",
                typeof(bool),
                typeof(FanPump),
                new PropertyMetadata(default(bool), new PropertyChangedCallback(OnRunStateChanged)));

        private static void OnRunStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var v = bool.Parse(e.NewValue.ToString());
            if (v)
            {
                VisualStateManager.GoToState(d as FanPump, "RunState", true);
            }
            else
            {
                VisualStateManager.GoToState(d as FanPump, "Stop", true);
            }
        }
    }
}
