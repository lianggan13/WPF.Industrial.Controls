using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProgressLoading.Progresses
{
    public partial class LiquidWaveProgress : UserControl
    {
        // 依赖属性
        public static readonly DependencyProperty WavePhaseProperty =
            DependencyProperty.Register(
                nameof(WavePhase),
                typeof(double),
                typeof(LiquidWaveProgress),
                new PropertyMetadata(0.0, OnWavePhaseChanged));

        public double WavePhase
        {
            get => (double)GetValue(WavePhaseProperty);
            set => SetValue(WavePhaseProperty, value);
        }

        private static void OnWavePhaseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LiquidWaveProgress)d).UpdateWavePath();
        }

        private readonly double _waveAmplitude = 20;
        private readonly double _waveLength = 200;
        private readonly int _points = 200;

        public LiquidWaveProgress()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(LiquidWaveProgress),
                new PropertyMetadata(40.0, OnValueChanged));

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LiquidWaveProgress)d).UpdateWavePath();
        }

        private void UpdateWavePath()
        {
            double percent = Math.Max(0, Math.Min(1, Value / 100));
            double width = this.ActualWidth;
            double height = this.ActualHeight;

            if (percent >= 1.0)
            {
                var ellipse = new EllipseGeometry(new Point(width / 2, height / 2), width / 2, height / 2);
                WavePath.Data = ellipse;
                //WavePath2.Data = ellipse;
                return;
            }

            double baseY = height - percent * height;

            // 第一条波浪
            var figure1 = new PathFigure { StartPoint = new Point(0, baseY) };
            // 第二条波浪
            var figure2 = new PathFigure { StartPoint = new Point(0, baseY) };

            for (int i = 0; i <= _points; i++)
            {
                double x = i * width / _points;
                // WavePath 向右移动（正相位）
                double y1 = baseY + Math.Sin((x / _waveLength * 2 * Math.PI) + WavePhase) * _waveAmplitude;
                // WavePath2 向左移动（负相位）
                double y2 = baseY + Math.Sin((x / _waveLength * 2 * Math.PI) - WavePhase) * _waveAmplitude;
                figure1.Segments.Add(new LineSegment(new Point(x, y1), true));
                figure2.Segments.Add(new LineSegment(new Point(x, y2), true));
            }
            figure1.Segments.Add(new LineSegment(new Point(width, height), true));
            figure1.Segments.Add(new LineSegment(new Point(0, height), true));
            figure1.IsClosed = true;

            figure2.Segments.Add(new LineSegment(new Point(width, height), true));
            figure2.Segments.Add(new LineSegment(new Point(0, height), true));
            figure2.IsClosed = true;

            var geometry1 = new PathGeometry();
            geometry1.Figures.Add(figure1);
            WavePath.Data = geometry1;

            var geometry2 = new PathGeometry();
            geometry2.Figures.Add(figure2);
            WavePath2.Data = geometry2;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var size = base.MeasureOverride(availableSize);

            double centerX = size.Width / 2;
            double centerY = size.Height / 2;
            double radius = Math.Min(centerX, centerY);

            if (WavePath != null)
                WavePath.Clip = new EllipseGeometry(new Point(centerX, centerY), radius, radius);
            if (WavePath2 != null)
                WavePath2.Clip = new EllipseGeometry(new Point(centerX, centerY), radius, radius);

            return size;
        }
    }
}