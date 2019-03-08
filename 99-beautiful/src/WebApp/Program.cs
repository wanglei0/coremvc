using Microsoft.AspNetCore.Hosting;

namespace WebApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            new AppWebHostBuilderFactory().Create(args).Build().Run();
        }
    }
}