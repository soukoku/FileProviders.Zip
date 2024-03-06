using Soukoku.Extensions.FileProviders;

namespace WebApp;

internal static class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.ConfigurePdfJsZip();

        app.Run();
    }

    private static void ConfigurePdfJsZip(this WebApplication app)
    {
        var zipFile = Path.Combine(app.Environment.ContentRootPath, "pdfjs-1.9.426-dist.zip");
        var provider = new ZipFileProvider(zipFile);

        var options = new FileServerOptions
        {
            FileProvider = provider,
            //RequestPath = "/pdfjs",
            EnableDirectoryBrowsing = true,
        };
        // required for extension-less files
        options.StaticFileOptions.ServeUnknownFileTypes = true;
        //options.StaticFileOptions.DefaultContentType = "text/plain";
        app.UseFileServer(options);
    }
}