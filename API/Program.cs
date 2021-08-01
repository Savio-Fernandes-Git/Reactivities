using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Program
    {
        //make main async to allow await function
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            //using dependency injection
            //keyword using here will dispose var scope
            //scope is where we will be storing our services so it needs to be disposed after use
            //there are things we need to dispose after we finish using it
            using var scope = host.Services.CreateScope();

            //service fetcher
            var services = scope.ServiceProvider;
            
            try{
                var context = services.GetRequiredService<DataContext>();
                //will create database if it doesn't already exist
                await context.Database.MigrateAsync();
                await Seed.SeedData(context);
            } catch(Exception ex){
                //ILogger is a service, ILogger takes a type <Program> is the class we're logging from
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occured during migration");
            } 
            // we need to make sure we run the application (CreateHostBuilder)
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
