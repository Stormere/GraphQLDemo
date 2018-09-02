using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;

namespace GraphQLDemo
{
    public class Program
    {
       


        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
.UseStartup<Startup>()
.ConfigureServices(services => services.AddAutofac())
.Build();
        }
    }
}
