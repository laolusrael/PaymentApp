using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors.Infrastructure;
using PaymentApp.Persistence;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using System;
using Swashbuckle.AspNetCore.Swagger;
using PaymentApp.Api.Middlewares;
using Postal.AspNetCore;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PaymentApp.Api.Helpers;

namespace PaymentApp.Api
{
    public class Startup
    {
        public Startup(IConfiguration conf)
        {
            Configuration = conf;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            var tokenParams = JwtSecurityTokenHelper.GetTokenParameters(Configuration);


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(jwtconfig =>
                    {
                        jwtconfig.TokenValidationParameters = tokenParams;                        
                    });
                    
            _ActivateCORS(services);

           services.AddApplicationInsightsTelemetry();


            services.AddDbContext<PaymentAppContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("PaymentApp"), b => b.MigrationsAssembly("PaymentApp.Api"));
            });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info() { Title = "PaymentApp API", Version = "v1" });
                
            });


            // Setup MVC and Authorization
            _SetupMVC(services);

            // Dependency injection setup

            var autofacBuilder = new ContainerBuilder();
            autofacBuilder.Populate(services);

            autofacBuilder.RegisterModule<DefaultModule>();

            var provider = autofacBuilder.Build(); 
            
            return new AutofacServiceProvider(provider);
        }

        private void _SetupMVC(IServiceCollection services)
        {

            services.AddMvc();
            return;
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddFile(Configuration.GetSection("Logging"));
            

            _DoAutomaticDbMigration(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseErrorResponse();
            app.UseResponseWrapper();



            app.UseMvcWithDefaultRoute();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PaymentApp API v1");
            });
      }

        private void _DoAutomaticDbMigration(IApplicationBuilder app)
        {

            using(var sScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                PaymentAppContext context = sScope.ServiceProvider.GetRequiredService<PaymentAppContext>();
                context.Database.Migrate();
            }
        }

        private void _ActivateCORS(IServiceCollection services)
        {

            // add permission enable cross-origin requests (CORS) from angular
            var corsBuilder = new CorsPolicyBuilder();

            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin();
            corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", corsBuilder.Build());

            });
        }
    }
}
