using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HHParser.Domain.Attributes;

namespace HHParser.Application.Services.CommonClasses
{
    public static class QueryParameterHelper
    {
        public static Dictionary<string, string> ToDictionary<T>(T parameters)
        {
            var dict = new Dictionary<string, string>();
            var type = typeof(T);
            foreach (var property in type.GetProperties())
            {
                var value = property.GetValue(parameters);
                if (value == null)
                {
                    continue;
                }

                string valueAsString = value.ToString()!;
                if (string.IsNullOrWhiteSpace(valueAsString))
                {
                    continue;
                }

                // Если атрибут указан, используем его значение, иначе имя свойства
                var attribute = property.GetCustomAttribute<QueryParameterAttribute>();
                string key = attribute?.Key ?? property.Name;
                dict[key] = valueAsString;
            }
            return dict;
        }
    }

}
