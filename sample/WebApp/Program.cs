
using Soukoku.Extensions.FileProviders;

namespace WebApp
{
    static class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            ConfigurePdfJsZip(app);

            app.Run();
        }
        
        static void ConfigurePdfJsZip(WebApplication app)
        {
            var zipFile = Path.Combine(app.Environment.ContentRootPath, "pdfjs-1.9.426-dist.zip");
            var provider = new ZipFileProvider(zipFile);

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = provider,
                RequestPath = "/pdfjs",
                // following are required for extension-less files
                ServeUnknownFileTypes = true, RedirectToAppendTrailingSlash = false,
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