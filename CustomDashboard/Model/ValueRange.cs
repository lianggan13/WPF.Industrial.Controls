using System.Windows;

namespace CustomDashboard.Model
{
    public class ValueRange : DependencyObject
    {
        public static double GetValueMax(DependencyObject obj)
        {
            return (double)obj.GetValue(ValueMaxProperty);
        }

        public static void SetValueMax(DependencyObject obj, double value)
        {
            obj.SetValue(ValueMaxProperty, value);
        }

        // Using a DependencyProperty as the backing store for ValueMax.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueMaxProperty =
            DependencyProperty.RegisterAttached("ValueMax", typeof(double), typeof(ValueRange), new PropertyMetadata(0d, PropertyChangedCallback));


        public static double GetValueMin(DependencyObject obj)
        {
            return (double)obj.GetValue(ValueMinProperty);
        }

        public static void SetValueMin(DependencyObject obj, double value)
        {
            obj.SetValue(ValueMinProperty, value);
        }

        // Using a DependencyProperty as the backing store for ValueMin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueMinProperty =
            DependencyProperty.RegisterAttached("ValueMin", typeof(double), typeof(ValueRange), new PropertyMetadata(0d, PropertyChangedCallback));




        public static double GetValueStep(DependencyObject obj)
        {
            return (double)obj.GetValue(ValueStepProperty);
        }

        public static void SetValueStep(DependencyObject obj, double value)
        {
            obj.SetValue(ValueStepProperty, value);
        }

        // Using a DependencyProperty as the backing store for ValueStep.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueStepProperty =
            DependencyProperty.RegisterAttached("ValueStep", typeof(double), typeof(ValueRange), new PropertyMetadata(0d, PropertyChangedCallback));




        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}
