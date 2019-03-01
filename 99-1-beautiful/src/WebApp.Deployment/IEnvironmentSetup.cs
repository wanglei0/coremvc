using System;

namespace WebApp.Deployment
{
    public interface IEnvironmentSetup
    {
        Type ConfiguratorType { get; }
        Type StartupType { get; }
    }
}