using Common.MVVMFoundation;
using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Progress.View
{
    /// <summary>
    /// CircularProgressBar.xaml 的交互逻辑
    /// </summary>
    public partial class CircularBar : UserControl
    {
        public CircularBar()
        {
            InitializeComponent();

            this.SizeChanged += CircularProgressBar_SizeChanged;
        }

        private void CircularProgressBar_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            //this.layout.Width = Math.Min(this.RenderSize.Width, this.RenderSize.Height);
            //this.layout.Height = Math.Min(this.RenderSize.Width, this.RenderSize.Height);
            this.VM.R = Math.Min(this.RenderSize.Width, this.RenderSize.Height) / 2;
        }
    }

    public class CircularProgressBarVM : NotifyPropertyChanged
    {
        private double r;

        public double R
        {
            get { return r; }
            set { r = value; OnPropertyChanged(); }
        }


        private int currentAngle;

        public int CurrentAngle
        {
            get { return currentAngle; }
            set
            {
                currentAngle = value; OnPropertyChanged();
            }
        }

        private string pathData;

        public string PathData
        {
            get { return pathData; }
            set { pathData = value; OnPropertyChanged(); }
        }


        public ICommand AngleChangedCmd { get; private set; }

        public ICommand RotateAngleCmd { get; private set; }


        private Timer timer = new Timer();

        public CircularProgressBarVM()
        {
            //Data = "M 75 3 A 75 75 0 0 1 147 75" angle = 90°

            //PathData = $"M {R + 0.01} 3 A {r} {r} 0 {0} 1 {x} {y}";
            timer.Interval = 200;
            timer.Elapsed += Timer_Elapsed;

            this.PropertyChanged += CircularProgressBarVM_PropertyChanged;

            AngleChangedCmd = new RelayCommand<object>((para) =>
            {
                if (para is TextBox txtAngle)
                {
                    if (int.TryParse(txtAngle.Text, out int angle))
                    {
                        CurrentAngle = angle;
                        ChangAngle(angle);
                    }
                }

            }, (para) => true);

            RotateAngleCmd = new RelayCommand<object>((para) =>
            {
                if (para is ToggleButton btnRotate)
                {
                    RoateAngle(btnRotate.IsChecked);
                }
            }, (para) => true);
        }

        private void RoateAngle(bool? isChecked)
        {
            if (isChecked == true)
            {
                timer.Start();
            }
            else
            {
                timer.Stop();
            }
        }


        private void CircularProgressBarVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(R))
            {
                ChangAngle(CurrentAngle);
            }
        }


        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                CurrentAngle++;
                ChangAngle(CurrentAngle);
            });
        }

        public void ChangAngle(int angle)
        {
            angle = angle % 360;

            double inR = R - 3; // R = 75
            double x = R + inR * Math.Sin(angle * Math.PI / 180);
            double y = R - inR * Math.Cos(angle * Math.PI / 180);

            int isLargeArcFlag = angle < 180 ? 0 : 1;

            //Data = "M 75 3 A 72 72 0 0 1 75 3" angle = 0°
            //Data = "M 75 3 A 75 75 0 0 1 147 75" angle = 90°

            //PathData = $"M {R + 0.01} 3 A {inR} {inR} 0 {isLargeArcFlag} 1 {x} {y}";
            PathData = $"M {R} 3 A {inR} {inR} 0 {isLargeArcFlag} 1 {x} {y}";
        }

    }
}
