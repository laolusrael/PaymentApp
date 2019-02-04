using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;

namespace PaymentApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }
        private static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                        //.ConfigureLogging((hcontext, builder) => { builder.AddXmlFile(); })
                        //.UseKestrel()
                        //.UseIISIntegration()
                        .UseStartup<Startup>()
                        .Build();
        }
    }
}
