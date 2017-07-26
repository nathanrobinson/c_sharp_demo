using System.Runtime.InteropServices.ComTypes;
using Newtonsoft.Json;

namespace cs_tests
{
    public static class Extensions
    {
        public static T Clone<T>(this T source)
        {
            if (ReferenceEquals(source, null))
                return default(T);

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }
    }
}