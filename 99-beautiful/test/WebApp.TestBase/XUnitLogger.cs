using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using Xunit.Abstractions;

namespace WebApp.TestBase
{
    class XUnitLogger : ILogger
    {
        readonly string categoryName;
        readonly ITestOutputHelper output;
        readonly IExternalScopeProvider scopeProvider;

        const string LogLevelPadding = ": ";
        const string MessagePadding = "     ";
        static readonly string newLineWithMessagePadding = $"{Environment.NewLine}{MessagePadding}";

        public XUnitLogger(
            string categoryName, 
            ITestOutputHelper output,
            IExternalScopeProvider scopeProvider = null)
        {
            this.categoryName = categoryName;
            this.output = output;
            this.scopeProvider = scopeProvider;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) { return; }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            string message = formatter(state, exception);

            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                WriteMessage(logLevel, categoryName, eventId.Id, message, exception);
            }
        }

        void WriteMessage(
            LogLevel logLevel,
            string name,
            int eventId,
            string message,
            Exception exception)
        {
            var contentBuilder = new StringBuilder(1024);

            string logLevelString = GetLogLevelString(logLevel);
            contentBuilder
                .Append(logLevelString)
                .Append(LogLevelPadding)
                .Append(name)
                .Append('[')
                .Append(eventId)
                .Append("] ")
                .AppendLine(DateTime.Now.ToString("T"));

            GetScopeInformation(contentBuilder);

            if (!string.IsNullOrEmpty(message))
            {
                contentBuilder.Append(MessagePadding);
                int length = contentBuilder.Length;
                
                contentBuilder.AppendLine(message);
                contentBuilder.Replace(
                    Environment.NewLine,
                    newLineWithMessagePadding,
                    length,
                    message.Length);
            }

            if (exception != null)
            {
                contentBuilder.AppendLine(exception.ToString());
            }
            
            output.WriteLine(contentBuilder.ToString());
        }

        void GetScopeInformation(StringBuilder contentBuilder)
        {
            var provider = scopeProvider;
            
            if (provider != null)
            {
                var initialLength = contentBuilder.Length;

                provider.ForEachScope((scope, state) =>
                {
                    var (builder, length) = state;
                    var first = length == builder.Length;
                    builder.Append(first ? "=> " : " => ").Append(scope);
                }, (contentBuilder, initialLength));

                if (contentBuilder.Length > initialLength)
                {
                    contentBuilder.Insert(initialLength, MessagePadding);
                    contentBuilder.AppendLine();
                }
            }
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public IDisposable BeginScope<TState>(TState state)
        {
            return scopeProvider != null ? scopeProvider.Push(state) : NullScope.Instance;
        }

        static string GetLogLevelString(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Trace:
                    return "VEB";
                case LogLevel.Debug:
                    return "DBG";
                case LogLevel.Information:
                    return "INF";
                case LogLevel.Warning:
                    return "WRN";
                case LogLevel.Error:
                    return "ERR";
                case LogLevel.Critical:
                    return "FAT";
                case LogLevel.None:
                    return "NON";
                default:
                    throw new ArgumentOutOfRangeException(nameof(level));
            }
        }
    }
}