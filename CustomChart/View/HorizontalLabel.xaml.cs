using CustomChart.View.Base;
using System.Windows.Controls;

namespace CustomChart.View
{
    /// <summary>
    /// HorizontalLabel.xaml 的交互逻辑
    /// </summary>
    public partial class HorizontalLabel : AxisLabel
    {
        public HorizontalLabel()
        {
            InitializeComponent();
        }

        protected override void Refresh()
        {
            if (CanNotRender())
                return;

            double min = Range.Min;
            double max = Range.Max;

            root.Children.Clear();
            for (double i = min; i <= max; i += Step)
            {
                Label label = new Label();
                label.Content = $"{i}";
                label.FontSize = FontSize;
                label.FontFamily = FontFamily;
                label.Foreground = Foreground;
                label.HorizontalContentAlignment = HorizontalLabelAlignment;
                root.Children.Add(label);

                label.UpdateLayout();
                Canvas.SetLeft(label, Normalize(i) - label.ActualWidth / 2d);
            }
        }

        /// <summary>
        /// 数据点映射到视图点
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        protected override double Normalize(double v) => (v - Range.Min) / (Range.Max - Range.Min) * RenderSize.Width;
    }
}
