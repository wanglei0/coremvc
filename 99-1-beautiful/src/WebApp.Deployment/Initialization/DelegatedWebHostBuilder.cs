using System;
using Microsoft.AspNetCore.Hosting;

namespace WebApp.Deployment.Initialization
{
    class DelegatedWebHostBuilder : EnvironmentAwareWebHostBuilder
    {
        readonly Func<IHostingEnvironment, bool> isSupported;

        public DelegatedWebHostBuilder(
            IWebHostBuilder underlyingBuilder,
            Func<IHostingEnvironment, bool> isSupported) : base(underlyingBuilder)
        {
            this.isSupported = isSupported;
        }

        protected override bool IsSupported(IHostingEnvironment hostingEnvironment) =>
            isSupported(hostingEnvironment);
    }
}