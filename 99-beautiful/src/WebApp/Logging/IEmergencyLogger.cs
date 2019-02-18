using System;

namespace WebApp.Logging
{
    interface IEmergencyLogger : IDisposable
    {
        void Fatal(Exception exception, string messageTemplate, params object[] args);
        void Warning(string messageTemplate, params object[] args);
    }
}