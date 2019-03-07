using Microsoft.Extensions.Options;

namespace WebApp.Test.Utils
{
    class OptionsSnapshot<T> : IOptionsSnapshot<T>
        where T: class, new()
    {
        public OptionsSnapshot(T value) { Value = value; }

        public T Value { get; }
        public T Get(string name) { throw new System.NotImplementedException(); }
    }
}