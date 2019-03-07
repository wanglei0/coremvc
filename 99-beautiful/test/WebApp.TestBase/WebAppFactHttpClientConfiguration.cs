using System;
using Microsoft.AspNetCore.Mvc.Testing;

namespace WebApp.TestBase
{
    static class WebAppFactHttpClientConfiguration
    {
        const string DefaultBaseAddress = "http://www.baseaddress.com";

        public static WebApplicationFactoryClientOptions DefaultHttpClientOptions { get; } =
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri(DefaultBaseAddress),
                HandleCookies = true
            };
    }
}