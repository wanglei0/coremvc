using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace WebApp.TestBase
{
    public interface ITestServiceConfigurator
    {
        void ConfigureService(
            IServiceCollection services,
            IDictionary<string, object> dependencies);
    }
}