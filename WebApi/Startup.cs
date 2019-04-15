namespace WebApi
{
    using MediatR;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using WebApi.Infra;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFeatureFolders();

            services.AddMediatR();

            services.AddDbContext<ApiContext>(opts =>
            {
                opts.UseSqlServer(Configuration.GetConnectionString("ApiConnection"), opt => opt.EnableRetryOnFailure());
            });

            services.AddCors(opts =>
            {
                opts.AddPolicy("Dev", opt => opt.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseCors("Dev");
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseMvc(opts =>
            {
                opts.MapRoute(
                    name: "default",
                    template: "api/{controller}/{action}/{id?}");
            });
        }
    }
}