using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication.Db;
using WebApplication.DI;
using WebApplication.Exception;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
	        services.AddHttpContextAccessor();
	        services.AddResponseCaching();
	        services.AddDirectoryBrowser();
	        services.AddMvc(
		        config => { config.Filters.Add(typeof(ExceptionHandler)); });
	        
	        services.AddCors(options => { options.AddPolicy("TurboPolicy", builder => CorsPolicyBuilder(builder)); });
	        services.AddEntityFrameworkSqlite().AddDbContext<DbLocalContext>();
	        var containerBuilder = new ContainerBuilder();
	        containerBuilder.RegisterModule(new WebApiAutofacModule());
	        containerBuilder.Populate(services);
	        return new AutofacServiceProvider(containerBuilder.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            app.Use(async (context, next) =>
            {
	            context.Response.GetTypedHeaders().CacheControl =
		            new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
		            {
			            NoCache = true,
			            NoStore = true,
			            MustRevalidate = true,
			            MaxAge = TimeSpan.Zero
		            };
	            context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
		            new[] { "Accept-Encoding" };

	            await next().ConfigureAwait(false);
            });

            app.UseCors("TurboPolicy");
        }
        
        private CorsPolicyBuilder CorsPolicyBuilder(CorsPolicyBuilder builder)
        {
	        builder.AllowAnyHeader();
	        builder.AllowAnyMethod();
	        builder.AllowCredentials();
	        builder.SetIsOriginAllowed(origin => true);
	        return builder;
        }
    }
}
