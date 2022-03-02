using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomDashboard.Asserts.Style
{

    public partial class StyleWindow : ResourceDictionary
    {
        public StyleWindow()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Window win = sender as Window;
        }

        private void Title_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (((FrameworkElement)sender).TemplatedParent is Window self)
                self.DragMove();
        }

        private void WindowStateButton_Click(object sender, RoutedEventArgs e)
        {
            if (((FrameworkElement)sender).TemplatedParent is Window self)
            {
                if (e.Source is Button btn)
                {
                    //string cmd = btn.Content.ToString().ToLower();
                    string cmd = btn.Tag.ToString().ToLower();
                    switch (cmd)
                    {
                        case "minimize":
                            {
                                self.Margin = new Thickness(0);
                                self.WindowState = WindowState.Minimized; break;
                            }
                        case "maximize":
                            {
                                self.WindowState ^= WindowState.Maximized;
                                break;
                            }
                        case "close": self.Close(); break;
                        default:
                            Debugger.Break();
                            break;
                    }
                }
            }
        }

        bool _IsWiden = false;

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border sizeGap)
            {
                _IsWiden = true;
            }
        }

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border sizeGap && _IsWiden)
            {
                sizeGap.ReleaseMouseCapture();
                _IsWiden = false;
            }
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is Border sizeGap && _IsWiden)
            {
                sizeGap.CaptureMouse();
                Window window = Window.GetWindow(sizeGap);
                double width = e.GetPosition(window).X;
                double height = e.GetPosition(window).Y;
                if (width > window.MinWidth && height > window.MinHeight)
                {
                    window.Width = width;
                    window.Height = height;
                }
            }
        }

        private void Border_LostFocus(object sender, RoutedEventArgs e)
        {
            Border_MouseUp(sender, null);
        }
    }
}
