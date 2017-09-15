using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spring.Context.Support;
using Spring.Context;
using DemoNH.Core.Services;

namespace DemoNH.Apps.AspNetCoreHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IApplicationContext springContext = new Spring.Context.Support.XmlApplicationContext(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,  "[IoC.Infrastructure].xml"));

            //Configuração de referência: https://github.com/luizcarlosfaria/Hangout-NH-EF-DAPPER--demo-nh/blob/master/DemoNH.Core/Services/%5BIoC%5D.Services.xml
            //Aqui estou subindo o host WCF para o serviço AlunoService
            //Caso descomente, precisa executar o visual studio como administrador.
            //springContext.GetObject<Core.Infrastructure.Services.IService>("AlunoServiceHostAdapter").Start();

            //Aqui estou trazendo do container padrão para o container do ASP.NET Core.
            services.AddSingleton(springContext.GetObject<IAlunoService>());


            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
