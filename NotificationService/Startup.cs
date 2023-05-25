using HIOF.Net.V2023.UserIdeProvider;
using HIOF.Net.V2023.Notification;
using HIOF.Net.V2023.Notification.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HIOF.Net.V2023.Controller;

namespace HIOF.Net.V2023.Notification.startup
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("MyScheme")
                .AddScheme<AuthenticationSchemeOptions, AuthenticationHandler>("MyScheme", options =>
                {
                });

            services.AddSingleton<INotificationSink, NotificationService>();
            services.AddHostedService(sp => (NotificationService)sp.GetService<INotificationSink>());
            services.AddSingleton<IUserIdProvider, UserIdProvider>();

            services.AddSignalR();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/notificationHub");
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
    
