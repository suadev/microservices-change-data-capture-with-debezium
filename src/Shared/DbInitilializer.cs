using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Shared
{
    public static class DbInitilializer
    {
        public static void Migrate<T>(IServiceProvider serviceProvider) where T : DbContext
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<T>();
                context.Database.Migrate();
            }
        }
    }
}