using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Config
// - PatchSite:RequestPath (e.g. "/patchsite" or "/")
// - PatchSite:PhysicalPath (optional; absolute or relative to ContentRootPath)
var requestPathValue = builder.Configuration["PatchSite:RequestPath"];
if (string.IsNullOrWhiteSpace(requestPathValue))
    requestPathValue = "/patchsite";
if (!requestPathValue.StartsWith('/'))
    requestPathValue = "/" + requestPathValue;

var requestPath = requestPathValue == "/" ? PathString.Empty : new PathString(requestPathValue);

var physicalPathValue = builder.Configuration["PatchSite:PhysicalPath"];
string patchSitePhysicalPath;

if (!string.IsNullOrWhiteSpace(physicalPathValue))
{
    patchSitePhysicalPath = Path.IsPathRooted(physicalPathValue)
        ? physicalPathValue
        : Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, physicalPathValue));
}
else
{
    // 1) Preferred in publish/output: copied content (see csproj) lives in ./patchsite
    var copied = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "patchsite"));

    // 2) During dev runs from repo: serve directly from the source folder
    var source = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "PatcherWebSite", "mir2-patchsite"));

    patchSitePhysicalPath = Directory.Exists(copied) ? copied : source;
}

if (!Directory.Exists(patchSitePhysicalPath))
{
    throw new DirectoryNotFoundException(
        $"Patch site folder not found: '{patchSitePhysicalPath}'. " +
        "Set PatchSite:PhysicalPath to a valid directory.");
}

var patchSiteFileProvider = new PhysicalFileProvider(patchSitePhysicalPath);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

// Serve index.html at the configured request path.
var defaultFiles = new DefaultFilesOptions
{
    FileProvider = patchSiteFileProvider,
    RequestPath = requestPath
};
defaultFiles.DefaultFileNames.Clear();
defaultFiles.DefaultFileNames.Add("index.html");

app.UseDefaultFiles(defaultFiles);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = patchSiteFileProvider,
    RequestPath = requestPath
});

// Convenience redirect from / to the configured mount path (when not mounted at /).
if (requestPath != PathString.Empty)
{
    app.MapGet("/", (HttpContext ctx) =>
    {
        var target = requestPath.Value!.TrimEnd('/') + "/";
        ctx.Response.Redirect(target);
        return Results.Empty;
    });
}

app.MapGet("/healthz", () => Results.Ok("ok"));

app.Run();

