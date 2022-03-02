using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LoginWithLoading.UserControls
{
    /// <summary>
    /// BubbleControl.xaml 的交互逻辑
    /// </summary>
    public partial class BubbleControl : UserControl
    {
        public BubbleControl()
        {
            InitializeComponent();
        }

        public new  string  Content
        {
            get { return (string)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public new static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(string), typeof(BubbleControl), new PropertyMetadata(0));


    }
}
