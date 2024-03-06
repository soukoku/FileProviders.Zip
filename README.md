# About
Allows using zip files as a FS provider for aspnet core's 
[StaticFiles middleware](https://github.com/dotnet/aspnetcore/tree/main/src/Middleware/StaticFiles). 
A single zip file can serve as a root file system when used this way.


# Getting It
In an asp.net core project, install the 
[Soukoku.Extensions.FileProviders.Zip](https://www.nuget.org/packages/Soukoku.Extensions.FileProviders.Zip/) 
NuGet package.

# Example
Assuming there's a zip file you want to load, you can do something like the following

```csharp

// inside the Program.cs or Startup.cs
IApplicationBuilder app = ...

var zipProvider = new ZipFileProvider(@"path\to\my\zip-file.zip");
var options = new FileServerOptions
{
    FileProvider = provider,
    RequestPath = "/test", // optional
    EnableDirectoryBrowsing = true,
};
// required for extension-less files
options.StaticFileOptions.ServeUnknownFileTypes = true;
app.UseFileServer(options);
  

```

Then go to the url `http://mysite/test` in the browser to see the zip file content.


# Sample Site
A sample asp.net core site is included in the solution that uses an old
[PDF.js](https://mozilla.github.io/pdf.js/) dist zip file.
Run it or look at the `Startup.cs` file to see how it's configured.
