using System;
using System.ComponentModel;

namespace CustomChart.Model
{
    [TypeConverter(typeof(RangeConverter))]
    public class Range
    {
        public double From { get; set; }
        public double To { get; set; }

        public double Max => Math.Max(From, To);
        public double Min => Math.Min(From, To);
        public double Distance => Math.Abs(Max - Min);

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Range() { }

        /// <summary>
        /// 数据范围定义
        /// </summary>
        /// <param name="from">起始值</param>
        /// <param name="to">终止值</param>
        public Range(double from, double to)
        {
            From = from;
            To = to;
        }

        public override string ToString() => $"{From}, {To}";
    }
}
