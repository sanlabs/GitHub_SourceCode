using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataRepository;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TodoWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {                        
            var host = CreateWebHostBuilder(args).Build();

            //Inject Repository in the Host Service
            using (var scope = host.Services.CreateScope())
            {
                //Inject DBContext
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<TodoDBContext>();

                //Create Sample Data to Start with
                SampleData.Initialize(services);
            }

            //Continue to run the application
            host.Run();


        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
