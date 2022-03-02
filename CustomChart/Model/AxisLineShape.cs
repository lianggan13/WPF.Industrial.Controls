using CustomChart.View.Base;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;


namespace CustomChart.Model
{
    public class AxisLineShape : Tick
    {
        public AxisLineShape()
        {
            /* 颜色、宽度、默认值 */
            SetCurrentValue(StrokeProperty, Brushes.Lime);
            SetCurrentValue(StrokeThicknessProperty, 2d);
            //SetCurrentValue(StrokeDashArrayProperty, new DoubleCollection() { 0 });

            // binding parent's width & height
            //Loaded += (s, e) =>
            //{
            //    var panel = this.Parent as Panel;
            //    if (panel == null)
            //        return;

            //    SetBinding(WidthProperty, new Binding(nameof(ActualWidth)) { Source = panel });
            //    SetBinding(HeightProperty, new Binding(nameof(ActualHeight)) { Source = panel });
            //};

        }

        //protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        //{
        //    var panel = this.Parent as Panel;
        //    if (panel == null)
        //        return;

        //    SetBinding(WidthProperty, new Binding(nameof(ActualWidth)) { Source = panel });
        //    SetBinding(HeightProperty, new Binding(nameof(ActualHeight)) { Source = panel });
        //}

        //protected override Geometry DefiningGeometry
        //{
        //    get
        //    {
        //        double width = double.IsNaN(Width) ? RenderSize.Width : Width;
        //        double height = double.IsNaN(Height) ? RenderSize.Height : Height;
        //        if (width <= 0 || height <= 0) return StreamGeometry.Empty;

        //        StreamGeometry stream = new StreamGeometry();

        //        using (StreamGeometryContext geo = stream.Open())
        //        {
        //            // 纵向轴线
        //            Point p1, p2;
        //            //p1 = new Point(0, 0);
        //            //p2 = new Point(0, height);
        //            //geo.BeginFigure(p1, false, false);
        //            //geo.LineTo(p2, true, false);

        //            p1 = new Point(width - 0.5, 0);
        //            p2 = new Point(width - 0.5, height);
        //            geo.BeginFigure(p1, false, false);
        //            geo.LineTo(p2, true, false);
        //        }

        //        stream.Freeze();
        //        return stream;
        //    }
        //}

        protected override double Normalize(double v)
        {
            throw new System.NotImplementedException();
        }

        protected override void Refresh()
        {
            if (CanNotRender())
                return;

            double min = Range.Min;
            double max = Range.Max;

            root.Children.Clear();

            //p1 = new Point(width, 0);
            //p2 = new Point(width, height);

            double w = RenderSize.Width;
            double h = RenderSize.Height;
            Point p1 = new Point(0, 0);
            Point p2 = new Point(w, 0);
            Point p3 = new Point(w, h);
            Point p4 = new Point(0, h);

            List<Tuple<Point, Point>> points = new List<Tuple<Point, Point>>()
            {
                new Tuple<Point, Point>(p1, p2),
                new Tuple<Point, Point>(p2, p3),
                new Tuple<Point, Point>(p3, p4),
                new Tuple<Point, Point>(p4, p1),
            };

            foreach (var p in points)
            {
                Line line = new Line();
                line.Stroke = Stroke;
                line.StrokeThickness = StrokeThickness;
                line.X1 = p.Item1.X;// RenderSize.Width; //Normalize(i);//- StrokeThickness / 2d;
                line.Y1 = p.Item1.Y;
                line.X2 = p.Item2.X;
                line.Y2 = p.Item2.Y; // RenderSize.Height;//StrokeLength;

                root.Children.Add(line);
            }

            //Line line = new Line();
            //line.Stroke = Stroke;
            //line.StrokeThickness = StrokeThickness;
            //line.X1 = 0;// RenderSize.Width; //Normalize(i);//- StrokeThickness / 2d;
            //line.Y1 = 0;
            //line.X2 = line.X1;
            //line.Y2 = line.Y1 + RenderSize.Height;//StrokeLength;

            //Line line2 = new Line();
            //line2.Stroke = Stroke;
            //line2.StrokeThickness = StrokeThickness;
            //line2.X1 = RenderSize.Width; //Normalize(i);//- StrokeThickness / 2d;
            //line2.Y1 = 0;
            //line2.X2 = line2.X1;
            //line2.Y2 = line2.Y1 + RenderSize.Height;//StrokeLength;

            //root.Children.Add(line);
            //root.Children.Add(line2);
        }
    }
}
