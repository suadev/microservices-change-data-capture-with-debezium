using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Identity.Commands.Handlers;
using Services.Identity.Data;
using Services.Identity.Events;
using Services.Identity.Handlers;
using Shared;
using Shared.Kafka;

namespace Services.Identity
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
            services.AddDbContext<IdentityDBContext>(options => options
                .UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))
                .UseSnakeCaseNamingConvention()
            );

            services.AddMediatR(typeof(RegisterUserCommandHandler).GetTypeInfo().Assembly);

            services.AddControllers();

            services.AddKafkaConsumer<string, CustomerUpdatedEvent, CustomerUpdatedEventHandler>(p =>
            {
                p.Topic = "customer_events";
                p.GroupId = "customer_events_identity_group";
                p.BootstrapServers = "localhost:9092";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            DbInitilializer.Migrate<IdentityDBContext>(app.ApplicationServices);

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("", async context => await context.Response.WriteAsync("Identity service is up."));
            });
        }
    }
}
