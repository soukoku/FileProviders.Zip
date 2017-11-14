using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Soukoku.Extensions.FileProviders;
using System.IO;

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

            ConfigurePdfJsZip(app, env);
            
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }

        private void ConfigurePdfJsZip(IApplicationBuilder app, IHostingEnvironment env)
        {
            var zipFile = Path.Combine(env.ContentRootPath, "pdfjs-1.9.426-dist.zip");
            var provider = new ZipFileProvider(File.ReadAllBytes(zipFile));

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = provider,
                RequestPath = "/pdfjs",

                // following are required for extension-less files
                ServeUnknownFileTypes = true,
                DefaultContentType = "text/plain"
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = provider,
                RequestPath = "/pdfjs",
            });


            //app.UseFileServer(new FileServerOptions()
            //{
            //    FileProvider = provider,
            //    RequestPath = "/pdfjs",
            //    EnableDirectoryBrowsing = true,
            //});
        }
    }
}
