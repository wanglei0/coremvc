using System.Collections.Generic;

namespace WebApp.Infrastructure.Test
{
    class SimpleRecorder
    {
        readonly List<string> records = new List<string>();
        public void Record(string message) { records.Add(message); }
        public IList<string> Records => records;
    }
}