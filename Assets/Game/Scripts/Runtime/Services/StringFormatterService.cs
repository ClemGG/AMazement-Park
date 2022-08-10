
using System.Text.RegularExpressions;

namespace Project.Services
{
    public static class StringFormatterService
    {
        public static string[] AddSpaces(this string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = Regex.Replace(values[i], @"([a-z])([A-Z])", "$1 $2");
            }

            return values;
        }

        public static string AddSpaces(this string value)
        {
            value = Regex.Replace(value, @"([a-z])([A-Z])", "$1 $2");
            return value;
        }
    }
}