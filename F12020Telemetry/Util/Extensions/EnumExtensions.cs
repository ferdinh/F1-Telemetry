using System;
using System.ComponentModel.DataAnnotations;

namespace F12020Telemetry.Util.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                var field = type.GetField(name);
                if (field != null)
                {
                    var attr = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) as DisplayAttribute;

                    if (attr != null)
                    {
                        return attr.Name;
                    }
                }
            }
            return value.ToString();
        }
    }
}