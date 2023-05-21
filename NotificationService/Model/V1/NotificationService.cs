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
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<NotificationService> _logger;
        private readonly Channel<Notification> _channel;

        public ValueTask PushAsync(Notification notification) => _channel.Writer.WriteAsync(notification);

        public NotificationService(
            IHubContext<NotificationHub> hubContext,
            ILogger<NotificationService> logger
        )
        {
            _channel = Channel.CreateUnbounded<Notification>();
            _hubContext = hubContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var (forUserId, message) = await _channel.Reader.ReadAsync(stoppingToken);
                    var payload = new { Message = message };
                    _logger.LogInformation($"Sending channel notification '{message}' to {forUserId}");
                    await _hubContext.Clients.Client(forUserId).SendAsync("Notify", payload, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in notification service");
                }
            }
        }
    }
}