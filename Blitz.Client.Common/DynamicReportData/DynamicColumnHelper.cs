using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Blitz.Client.Common.DynamicReportData
{
    public static class DynamicColumnHelper
    {
        public static List<ExpandoObject> ConvertToExpando<T>(IEnumerable<T> items)
        {
            var properties = typeof(T).GetProperties();

            var results = new List<ExpandoObject>();

            foreach (var item in items)
            {
                IDictionary<string, object> result = new ExpandoObject();
                
                foreach (var propertyInfo in properties)
                {
                    result[propertyInfo.Name] = propertyInfo.GetValue(item, null);
                }

                results.Add((ExpandoObject) result);
            }

            return results;
        }

        public static List<DynamicColumn> GetColumns<T>()
        {
            var properties = typeof (T).GetProperties();

            return properties
                .Select((propertyInfo, index) => new DynamicColumn
                {
                    HeaderName = propertyInfo.Name,
                    PropertyName = propertyInfo.Name,
                    Ordinal = index,
                    IsVisible = true
                })
                .ToList();
        }
    }
}