using System;
using System.ComponentModel;
using System.Globalization;

namespace CustomChart.Model
{
    public class RangeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            /* 剔除空格 */
            string spaceRemoved = value.ToString().Replace(" ", string.Empty);

            /* 逗号分隔 */
            string[] splited = spaceRemoved.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var strs = splited;

            double d0, d1;
            if (strs.Length == 2
                && double.TryParse(strs[0], out d0)
                && double.TryParse(strs[1], out d1))
            {
                return new Range { From = d0, To = d1 };
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
