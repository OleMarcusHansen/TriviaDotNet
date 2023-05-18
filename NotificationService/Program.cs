using HIOF.Net.V2023.startup;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace program
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        // CreateHostBuilder method

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    if (args.Length > 0)
                    {
                        if (int.TryParse(args[0], out int port))
                        {
                            webBuilder.UseUrls($"http://127.0.0.1:{port}/notification");
                        }
                        else
                        {
                            Console.WriteLine("Invalid port number. Using the default URL.");
                        }
                    }
                });

            return builder;
        }
    }
}