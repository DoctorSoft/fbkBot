using System;
using System.ComponentModel;

namespace Constants.EnumExtension
{
    public static class EnumExtension
    {
        public static string GetDiscription(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return 
                attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}
