using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Routing;

namespace WebApp.Routing
{
    public class HttpEat : HttpMethodAttribute
    {
        static readonly IEnumerable<string> Supported = new[] {"EAT"};
        
        public HttpEat() : base(Supported)
        {
        }

        public HttpEat(string template) : base(Supported, template)
        {
        }
    }
}