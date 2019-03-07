using System.Collections.Generic;
using System.Linq;

namespace WebApp.Deployment.Initialization
{
    static class EnumerableExtensions
    {
        public static bool ContainsDuplication<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer = null)
        {
            var set = new HashSet<T>(comparer ?? EqualityComparer<T>.Default);
            return source.Any(item => !set.Add(item));
        }
    }
}