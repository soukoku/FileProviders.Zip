# About
Allows using zip files as FS provider for aspnet core's 
[StaticFiles middleware](https://github.com/aspnet/StaticFiles). 
A single zip file can serve as a root file system when used this way.


# Getting It
In an asp.net core project, install its 
[NuGet package](https://www.nuget.org/packages/Soukoku.Extensions.FileProviders.Zip/) with

```bat
Install-Package Soukoku.Extensions.FileProviders.Zip 
```


# Example
Assuming there's a zip file you want to load, you can do something like the following

```csharp

// inside the Startup.cs

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
  var zipProvider = new ZipFileProvider(@"path\to\my\zip-file.zip");

  app.UseFileServer(new FileServerOptions()
  {
      FileProvider = zipProvider,
      RequestPath = "/test", // optional
      EnableDirectoryBrowsing = true
  });
  
  // more mvc stuff...
}
```

Then go to the url `http://mysite/test` in the browser to see the zip file content.


# Sample Site
A sample asp.net core site is included in the solution that uses the 
[PDF.js](https://mozilla.github.io/pdf.js/) dist zip file.
Run it or look at the `Startup.cs` file to see how it's configured.
