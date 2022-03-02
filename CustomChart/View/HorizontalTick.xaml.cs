using CustomChart.Model;
using CustomChart.View.Base;
using System.Windows.Shapes;

namespace CustomChart.View
{
    /// <summary>
    /// HorizontalTick.xaml 的交互逻辑
    /// </summary>
    public partial class HorizontalTick : Tick
    {
        public HorizontalTick()
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
            var w = RenderSize.Width;
            for (double i = min; i <= max; i += Step)
            {
                Line line = new Line();
                line.Stroke = Stroke;
                line.StrokeThickness = StrokeThickness;
                line.X1 = Normalize(i);//- StrokeThickness / 2d;
                line.Y1 = 0;
                line.X2 = line.X1;
                line.Y2 = line.Y1 + StrokeLength;

                root.Children.Add(line);
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
