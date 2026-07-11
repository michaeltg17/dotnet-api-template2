using Core.Domain;
using CrossCutting.Settings;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistance.Interceptors;

namespace Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options, IApiSettings apiSettings) : DbContext(options)
    {
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(apiSettings.SqlServerConnectionString, options => options.EnableRetryOnFailure())
                .AddInterceptors(new SetAuditInfoSaveChangesInterceptor());
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(builder);
        }

        public Task<int> Delete<T>(long id) where T : class, IIdentifiable
        {
            return Set<T>().Where(e => e.Id == id).ExecuteDeleteAsync();
        }
    }
}
