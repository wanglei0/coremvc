using System.Diagnostics.CodeAnalysis;

namespace WebModule.SampleModule.Domain
{
    [SuppressMessage(
        "ReSharper",
        "UnusedAutoPropertyAccessor.Global",
        Justification = "Will be used on serialization")]
    class Message
    {
        public string Text { get; }

        public Message(string text) { Text = text; }
    }
}