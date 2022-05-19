using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Box.View
{
    /// <summary>
    /// CoolingTower.xaml 的交互逻辑
    /// </summary>
    public partial class CoolingTower : Page
    {
        public CoolingTower()
        {
            InitializeComponent();
            this.PreviewMouseLeftButtonDown += CoolingTower_PreviewMouseLeftButtonDown;
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;

                if (value)
                {
                    var parent = VisualTreeHelper.GetParent(this) as Grid;
                    if (parent != null)
                    {
                        foreach (var item in parent.Children)
                        {
                            if (item is CoolingTower)
                                (item as CoolingTower).IsSelected = false;
                        }
                    }
                }
                VisualStateManager.GoToState(this, value ? "Selected" : "Unselected", false);
            }
        }

        public bool IsRunning
        {
            get { return (bool)GetValue(IsRunningProperty); }
            set { SetValue(IsRunningProperty, value); }
        }
        public static readonly DependencyProperty IsRunningProperty =
            DependencyProperty.Register("IsRunning", typeof(bool), typeof(CoolingTower), new PropertyMetadata(default(bool), new PropertyChangedCallback(OnRunningChanged)));

        private static void OnRunningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VisualStateManager.GoToState(d as CoolingTower, (bool)e.NewValue ? "RunState" : "Stop", false);
        }

        public bool IsFaultState
        {
            get { return (bool)GetValue(IsFaultStateProperty); }
            set { SetValue(IsFaultStateProperty, value); }
        }
        public static readonly DependencyProperty IsFaultStateProperty =
            DependencyProperty.Register("IsFaultState", typeof(bool), typeof(CoolingTower), new PropertyMetadata(default(bool), new PropertyChangedCallback(OnFaultStateChanged)));

        private static void OnFaultStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VisualStateManager.GoToState(d as CoolingTower, (bool)e.NewValue ? "FaultState" : "NormalState", false);
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(CoolingTower), new PropertyMetadata(default(ICommand)));

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(CoolingTower), new PropertyMetadata(default(object)));



        private void CoolingTower_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.IsSelected = !this.IsSelected;
            this.Command?.Execute(this.CommandParameter);
            e.Handled = true;
        }
    }
}
