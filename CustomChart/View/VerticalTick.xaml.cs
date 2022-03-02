using CustomChart.View.Base;
using System.Windows.Shapes;

namespace CustomChart.View
{
    /// <summary>
    /// VerticalTick.xaml 的交互逻辑
    /// </summary>
    public partial class VerticalTick : Tick
    {
        public VerticalTick()
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
                Line line = new Line();
                line.Stroke = Stroke;
                line.StrokeThickness = StrokeThickness;
                line.X1 = 0;
                line.Y1 = Normalize(i);
                line.X2 = line.X1 + StrokeLength;
                line.Y2 = line.Y1;

                root.Children.Add(line);
            }
        }

        /// <summary>
        /// 数据点映射到视图点
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        protected override double Normalize(double v) => (v - Range.Min) / (Range.Max - Range.Min) * RenderSize.Height;

    }
}
