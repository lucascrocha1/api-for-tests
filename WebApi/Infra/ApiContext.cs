namespace WebApi.Infra
{
    using Microsoft.EntityFrameworkCore;
    using WebApi.Domain;

    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>();

            base.OnModelCreating(modelBuilder);
        }
    }
}