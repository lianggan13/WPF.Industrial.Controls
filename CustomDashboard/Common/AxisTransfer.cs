using System;
using System.Windows;

namespace CustomDashboard.Common
{


    /// <summary>
    /// 坐标转换工具类
    /// </summary>
    internal static class AxisTransfer
    {
        private const double D2R = Math.PI / 180.0;

        /// <summary>
        ///     极坐标系转笛卡尔坐标系
        /// </summary>
        /// <param name="deg"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static Point PolarToCartesian(double deg, double radius)
        {
            double num = deg * (Math.PI / 180.0);
            return new Point(Math.Cos(num) * radius, Math.Sin(num) * radius);
        }
    }
}
