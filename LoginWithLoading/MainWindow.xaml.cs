using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Media.Animation;
using LoginWithLoading.UserControls;
using MenuWindowChrome;

namespace LoginWithLoading
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            loading.IsLoading = true;
            Task.Run(() =>
            {
                Thread.Sleep(3000);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (UserNumber.Text == "20201224" && UserPwd.Password == "123")
                    {

                        loading.IsLoading = false;
                        new MenuWindowChrome.MainWindow().Show();
                        this.Hide();

                    }
                    else
                    {
                        loading.IsLoading = false;
                        Alert("用户名或密码错误！");
                    }
                });
            });

        }
        private void Alert(string Content)
        {

            //查找当前窗体中是否存在Body的Grid控件
            if (this.FindName("Body") is Grid)
            {
                Grid control = this.FindName("Body") as Grid;
                //声明气泡控件，并赋值
                BubbleControl data = new BubbleControl()
                {
                    Content = Content,
                    Name = "Bubble"
                };
                //将当前气泡置顶
                Panel.SetZIndex(data, 100000);
                //判断当前气泡控件是否存在，存在就不创建直到气泡消失
                if (control.FindName("Bubble") is BubbleControl) return;
                //在Grid中注册气泡控件
                control.RegisterName(data.Name, data);
                //在Grid中添加气泡控件
                control.Children.Add(data);
                //创建画布
                Storyboard storyboard = new Storyboard();
                //创建关键帧动画
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                //起始透明色为0
                doubleAnimation.From = 0;
                //结束透明色为1
                doubleAnimation.To = 1;
                //动画完成的持续时间
                doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.1));
                //将动画放入画布中
                storyboard.Children.Add(doubleAnimation);
                //指定该动画作用在气泡控件(BubbleControl)上
                Storyboard.SetTarget(doubleAnimation, data);
                //指定该动画生效属性
                Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(OpacityProperty.Name));
                //该动画执行完后执行的事件
                storyboard.Completed += async (obj, ea) =>
                {
                    //停留3秒
                    await Task.Delay(3000);
                    //清除气泡控件
                    control.Children.Remove(data);
                    //注销气泡控件
                    control.UnregisterName(data.Name);
                };
                //开始执行动画
                storyboard.Begin();

            }
        }
    }
}
