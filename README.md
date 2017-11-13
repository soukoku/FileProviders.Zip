# About
Allows using zip files as FS provider for aspnet core's static files abstractions. 
A single zip file can serve as a root file system when used this way.

# Getting It
In an asp.net core project, install its NuGet package with

```cmd
Install-Package Soukoku.Extensions.FileProviders.Zip 
```


# Example
Assuming there's a zip file you want to load, you can do something like the following

```cs

// inside the Startup.cs

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
  var zipProvider = new ZipFileProvider(File.ReadAllBytes(@"path\to\my\zip-file.zip"));

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
