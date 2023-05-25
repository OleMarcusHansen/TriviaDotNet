using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HIOF.Net.V2023.Notification.Services
{
    public record Notification(string ForUserId, string Message);

    public interface INotificationSink
    {
        ValueTask PushAsync(Notification notification);
    }

    public class NotificationService : BackgroundService, INotificationSink
    {
        private readonly IServiceProvider _serviceProvider;
        //private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<NotificationService> _logger;
        private readonly Channel<Notification> _channel;

        public ValueTask PushAsync(Notification notification) => _channel.Writer.WriteAsync(notification);

        public NotificationService(
            IServiceProvider serviceProvider,
            ILogger<NotificationService> logger
        )
        {
            _channel = Channel.CreateUnbounded<Notification>();
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Background service started");

            while (true)
            {
                Console.WriteLine("teeeest");
                try
                {
                    if (stoppingToken.IsCancellationRequested)
                    {
                        return;
                    }

                    var (forUserId, message) = await _channel.Reader.ReadAsync(stoppingToken);

                    using var scope = _serviceProvider.CreateScope();

                    var hub = scope.ServiceProvider.GetRequiredService<IHubContext<NotificationHub>>();

                    var payload = new { Message = message };
                    await hub.Clients.All.SendAsync("Notify", new { Message = "tttttt" }, stoppingToken);
                    _logger.LogInformation($"Sending channel notification '{message}' to {forUserId}");
                    await hub.Clients.User(forUserId).SendAsync("Notify", payload, stoppingToken);

                    /*using var scope = _serviceProvider.CreateScope();

                    var hub = scope.ServiceProvider.GetRequiredService<IHubContext<NotificationHub>>();

                    //await hub.Clients.Client("test").SendAsync("Notify", new {Message = "tttttt" }, stoppingToken);
                    //await hub.Clients.All.SendAsync("Notify", new { Message = "tttttt" }, stoppingToken);

                    Console.WriteLine("test");
                    var (forUserId, message) = await _channel.Reader.ReadAsync(stoppingToken);

                    Console.WriteLine("test2");
                    var payload = new { Message = message };
                    _logger.LogInformation($"Sending channel notification '{message}' to {forUserId}");
                    await hub.Clients.Client(forUserId).SendAsync("Notify", payload, stoppingToken);
                    await hub.Clients.All.SendAsync("Notify", payload, stoppingToken);*/
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in notification service");
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                Console.WriteLine("teeeest?");
            }
        }
    }
}