using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;


namespace CustomDashboard.Model
{
    public abstract class ArcShape : Shape
    {

        [Category("Custom")]
        [Description("起始角度,角度范围 0~360")]
        public double AngleFrom
        {
            get { return (double)GetValue(AngleFromProperty); }
            set { SetValue(AngleFromProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AngleFrom.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AngleFromProperty =
            DependencyProperty.Register("AngleFrom", typeof(double), typeof(ArcShape), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        [Category("Custom")]
        [Description("终止角度,角度范围 0~360")]
        public double AngleTo
        {
            get { return (double)GetValue(AngleToProperty); }
            set { SetValue(AngleToProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AngleTo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AngleToProperty =
            DependencyProperty.Register("AngleTo", typeof(double), typeof(ArcShape), new FrameworkPropertyMetadata(90d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));


        [Category("Custom")]
        [Description("起始半径,相对范围 0~1")]
        public double RadiusFrom
        {
            get { return (double)GetValue(RadiusFromProperty); }
            set { SetValue(RadiusFromProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RadiusFrom.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusFromProperty =
            DependencyProperty.Register("RadiusFrom", typeof(double), typeof(ArcShape), new FrameworkPropertyMetadata(0.65d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        [Category("Custom")]
        [Description("终止半径,相对范围 0~1")]
        public double RadiusTo
        {
            get { return (double)GetValue(RadiusToProperty); }
            set { SetValue(RadiusToProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RadiusTo.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusToProperty =
            DependencyProperty.Register("RadiusTo", typeof(double), typeof(ArcShape), new FrameworkPropertyMetadata(0.75d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));



        protected override Geometry DefiningGeometry => GeometryGenerator();

        protected abstract Geometry GeometryGenerator();
    }
}
