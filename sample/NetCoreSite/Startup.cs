using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Soukoku.Extensions.FileProviders;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;

namespace NetCoreSite
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
            //services.AddTransient<IFileProvider, ZipFileProvider>();
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

            ConfigurePdfJsZip(app);
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }

        private void ConfigurePdfJsZip(IApplicationBuilder app)
        {
            var zipFile = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "pdfjs-1.9.426-dist.zip");
            var provider = new ZipFileProvider(File.ReadAllBytes(zipFile));
            
            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = provider,
                RequestPath = "/pdfjs",
                EnableDirectoryBrowsing = true
            });
        }
    }
}
