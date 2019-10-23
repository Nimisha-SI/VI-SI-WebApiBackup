using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApis.BAL;
using WebApis.BOL;
using WebApis.DAL;

namespace WebApis
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
            services.AddCors();
            services.AddOptions();
             //var section = Configuration.GetSection("CRICKET");
            //var section2 = Configuration.GetSection("elasticsearch");
            services.Configure<AppConfig>(Configuration);
            //services.Configure<AppConfig>(Configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //services.AddSingleton<ISportDetails, sport>();
            services.AddSingleton<Isportz, sportzBal>();
            services.AddSingleton<ISearchDataFilter, GetMatchDetails>();
            //var settings = Configuration.GetSection("CRICKET");
            // services.AddElasticsearch(Configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors(Options => Options.WithOrigins("http://localhost:57271/").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());
            app.UseHttpsRedirection();
            app.UseMvc();
            
        }
    }
}
