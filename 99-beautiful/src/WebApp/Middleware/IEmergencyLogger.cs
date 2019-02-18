using System;

namespace WebApp.Middleware
{
    interface IEmergencyLogger : IDisposable
    {
        void Fatal(Exception exception, string messageTemplate, params object[] args);
        void Warning(string messageTemplate, params object[] args);
    }
}