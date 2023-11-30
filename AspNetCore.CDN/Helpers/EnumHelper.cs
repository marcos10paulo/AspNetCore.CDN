using System;
using System.ComponentModel;

namespace AspNetCore.CDN.Helpers
{
    public static class EnumHelper
    {
        public static string GetEnumDescription(this Enum enumValue)
        {
            var field = enumValue.GetType().GetField(enumValue.ToString());
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }

            throw new NotSupportedException($"{enumValue} value in the {enumValue.GetType().Name} enum, does not contains DescriptionAttribute!");
        }

        public static T GetEnumByDescription<T>(this string description) where T : Enum 
        {
            foreach (Enum enumItem in Enum.GetValues(typeof(T)))
            {
                if (enumItem.GetEnumDescription() == description)
                {
                    return (T)enumItem;
                }
            }

            throw new NotSupportedException($"Description '{description}' not supported for enum {typeof(T).Name}");
        }
    }
}
