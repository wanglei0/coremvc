using Microsoft.AspNetCore.Hosting;

namespace WebApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            new WebAppHostBuilderFactory().Create(args).Build().Run();
        }
    }
}