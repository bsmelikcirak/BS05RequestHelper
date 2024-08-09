using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

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
        public static string CreateSoapEnvelope<T>(T requestObj)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter))
                {
                    xmlSerializer.Serialize(xmlWriter, requestObj);
                    string xmlBody = stringWriter.ToString();
                    return $@"<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:web='http://www.example.com/webservice'>
                        <soapenv:Header/>
                        <soapenv:Body>
                            {xmlBody}
                        </soapenv:Body>
                      </soapenv:Envelope>";
                }
            }
        }

    }
}
