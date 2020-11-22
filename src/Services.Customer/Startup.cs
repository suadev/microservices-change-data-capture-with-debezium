using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Customer.Commands.Handlers;
using Services.Customer.Data;
using Services.Customer.Events;
using Services.Customer.Handlers;
using Shared.Kafka;
using MediatR;
using Shared;

namespace Services.Customer
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
            services.AddDbContext<CustomerDBContext>(options => options
                .UseNpgsql(Configuration.GetConnectionString("DefaultConnection"))
                .UseSnakeCaseNamingConvention()
            );

            services.AddMediatR(typeof(UpdateCustomerCommandHandler).GetTypeInfo().Assembly);

            services.AddControllers();

            services.AddKafkaConsumer<string, UserCreatedEvent, UserCreatedEventHandler>(p =>
            {
                p.Topic = "user_events";
                p.GroupId = "user_events_customer_group";
                p.BootstrapServers = "localhost:9092";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            DbInitilializer.Migrate<CustomerDBContext>(app.ApplicationServices);

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("", async context => await context.Response.WriteAsync("Customer service is up."));
            });
        }
    }
}
