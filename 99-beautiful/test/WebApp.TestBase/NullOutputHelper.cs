using Xunit.Abstractions;

namespace WebApp.TestBase
{
    class NullOutputHelper : ITestOutputHelper
    {
        public void WriteLine(string message) { }
        public void WriteLine(string format, params object[] args) { }
    }
}