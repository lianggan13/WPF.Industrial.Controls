using System;
using System.Windows;

namespace CustomDashboard.Common
{
    public static class Interpo
    {
        public static double Linear(double x, double x1, double y1, double x2, double y2)
        {
            return y1 + (y2 - y1) / (x2 - x1) * (x - x1);
        }

        /// <summary>
        /// 获取一段元素基础长度
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static double GetBaseLength(FrameworkElement element)
        {
            double b = Math.Min(element.Height, element.Width) / 2;
            if (b == 0d)
            {
                b = Math.Min(element.ActualWidth, element.ActualHeight) / 2;
            }
            return b;
        }

        /// <summary>
        /// 获取中心点
        /// </summary>
        /// <param name="element"></param>
        /// <param name="p"></param>
        public static Point GetCenterPoint(FrameworkElement element)
        {
            double offsetX = element.Width / 2.0;
            double offsetY = element.Height / 2.0;

            if (double.IsNaN(offsetY * offsetX) || offsetX * offsetY == 0d)
            {
                offsetX = element.ActualWidth / 2.0;
                offsetY = element.ActualHeight / 2.0;
            }

            return new Point(offsetX, offsetY);
        }
    }
}
