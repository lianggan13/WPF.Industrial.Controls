using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace CustomChart.View.Base
{
    public abstract class Tick : AxisComponent
    {
        public Tick()
        {
            //StrokeThicknessProperty
            /* 颜色、长度、宽度默认值 */
            SetCurrentValue(StrokeProperty, Brushes.Lime);
            SetCurrentValue(StrokeLengthProperty, 10d);
            SetCurrentValue(StrokeThicknessProperty, 2d);
        }

        /// <summary>
        /// 刻度线颜色
        /// </summary>
        [Category("Custom"), Description("刻度线颜色")]
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }
        /// <summary>
        /// <see cref="Stroke"/>
        /// </summary>
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register
                                                                        (
                                                                            nameof(Stroke),
                                                                            typeof(Brush),
                                                                            typeof(Tick),
                                                                            new PropertyMetadata(
                                                                                default(Brush),
                                                                                OnParameterChanged)
                                                                        );
        /// <summary>
        /// 刻度线长度
        /// </summary>
        [Category("Custom"), Description("刻度线长度")]
        public double StrokeLength
        {
            get { return (double)GetValue(StrokeLengthProperty); }
            set { SetValue(StrokeLengthProperty, value); }
        }
        /// <summary>
        /// <see cref="StrokeLength"/>
        /// </summary>
        public static readonly DependencyProperty StrokeLengthProperty = DependencyProperty.Register
                                                                        (
                                                                            nameof(StrokeLength),
                                                                            typeof(double),
                                                                            typeof(Tick),
                                                                            new PropertyMetadata(
                                                                                default(double),
                                                                                OnParameterChanged)
                                                                        );

        /// <summary>
        /// 刻度线宽度
        /// </summary>
        [Category("Custom"), Description("刻度线宽度")]
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        /// <summary>
        /// <see cref="StrokeThickness"/>
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register
                                                                        (
                                                                            nameof(StrokeThickness),
                                                                            typeof(double),
                                                                            typeof(Tick),
                                                                            new PropertyMetadata(
                                                                                default(double),
                                                                                OnParameterChanged)
                                                                        );


        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            Refresh();
        }

        protected override bool CanNotRender()
                => base.CanNotRender() || Step == 0d;
    }
}