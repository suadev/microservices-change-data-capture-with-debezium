using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Notification.Data;
using Services.Notification.Events;
using Services.Notification.Handlers;
using Shared;
using Shared.Kafka;

namespace Services.Notification
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NotificationDBContext>(options => options
                .UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))
                .UseSnakeCaseNamingConvention()
            );

            services.AddControllers();

            services.AddKafkaConsumer<string, UserCreatedEvent, UserCreatedEventHandler>(p =>
            {
                p.Topic = "user_events";
                p.GroupId = "user_events_notification_group";
                p.BootstrapServers = "localhost:9092";
            });

            services.AddKafkaConsumer<string, CustomerUpdatedEvent, CustomerUpdatedEventHandler>(p =>
            {
                p.Topic = "customer_events";
                p.GroupId = "customer_events_notification_group";
                p.BootstrapServers = "localhost:9092";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            DbInitilializer.Migrate<NotificationDBContext>(app.ApplicationServices);

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("", async context => await context.Response.WriteAsync("Notification service is up."));
            });
        }
    }
}
