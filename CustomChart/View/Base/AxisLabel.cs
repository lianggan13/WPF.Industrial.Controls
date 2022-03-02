using System.Windows;
using System.Windows.Media;

namespace CustomChart.View.Base
{
    public abstract class AxisLabel : AxisComponent
    {

        public AxisLabel()
        {
            SetCurrentValue(FontFamilyProperty, new FontFamily("Consolas"));
        }

        public HorizontalAlignment HorizontalLabelAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalLabelAlignmentProperty); }
            set { SetValue(HorizontalLabelAlignmentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HorizontalLabelAlignment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HorizontalLabelAlignmentProperty =
            DependencyProperty.Register("HorizontalLabelAlignment", typeof(HorizontalAlignment), typeof(AxisLabel), new FrameworkPropertyMetadata(HorizontalAlignment.Center,
        FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, OnParameterChanged));

        /// <summary>
        /// 追加对前景色、字号、字体改变的响应
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == ForegroundProperty
                || e.Property == FontSizeProperty
                || e.Property == FontFamilyProperty)
                Refresh();
        }

        protected override bool CanNotRender()
             => base.CanNotRender() || Step == 0d;
    }
}
