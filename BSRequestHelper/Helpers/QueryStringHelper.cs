using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSRequestHelper.Helpers
{
    public static class QueryStringBuilder
    {
        public static string QueryStringFromObject(object obj)
        {
            if (obj != null)
            {
                var properties = obj.GetType().GetProperties();
                var queryString = string.Join("&", properties
                    .Where(p => p.GetValue(obj) != null && !(p.GetValue(obj) is IList))
                    .Select(p => $"{p.Name}={Uri.EscapeDataString(p.GetValue(obj).ToString())}")
                );

                foreach (var prop in properties.Where(p => p.GetValue(obj) is IList))
                {
                    var valueList = (IEnumerable)prop.GetValue(obj);
                    var propertyName = prop.Name;
                    List<string> values = new List<string>();
                    foreach (var val in valueList)
                    {
                        values.Add(val.ToString());
                    }
                    var valuesToString = string.Join(",", values);
                    queryString += $"&{propertyName}=" + valuesToString;
                }

                return queryString;
            }

            return "";
        }
        public static string QueryStringFromEnum<TEnum>(IEnumerable<TEnum> enumList) where TEnum : Enum
        {
            var enumType = typeof(TEnum);
            var enumName = Enum.GetName(enumType, 0);

            var enumValues = enumList.Select(e => Convert.ChangeType(e, enumType.GetEnumUnderlyingType())).ToList();

            var queryString = new StringBuilder();
            foreach (var value in enumValues)
            {
                if (queryString.Length > 0)
                    queryString.Append("&");

                queryString.Append($"{enumName}={Uri.EscapeDataString(value.ToString())}");
            }

            return queryString.ToString();
        }
    }
}
