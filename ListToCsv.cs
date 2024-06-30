using System.Reflection;
using System.Text;


namespace ListToCsv
{
    public static class ListExtensions
    {
        public static string ToCsv<T>(this List<T> list, string delimiter = ",", bool includeHeaders = true )
        {
            if(list == null || list.Count == 0)
            {
                return string.Empty;
            }
            
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            StringBuilder sb = new StringBuilder();

            if(includeHeaders)
            {
                sb.AppendLine(string.Join(delimiter, properties.Select(p => p.Name)));
            }

            foreach (T item in list)
            {
                var value = properties.Select(p => EscapeForCsv(p.GetValue(item)?.ToString() ?? string.Empty));
                sb.AppendLine(string.Join(delimiter, value));
            }

            return sb.ToString();
        }

        private static string EscapeForCsv(string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            if(value.Contains(",") || value.Contains("\"") || value.Contains("\n") || value.Contains("\r"))
            {
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            }

            return value;
        }
    }
}