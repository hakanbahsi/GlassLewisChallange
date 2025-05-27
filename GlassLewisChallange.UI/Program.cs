using GlassLewisChallange.UI.Models;
using GlassLewisChallange.UI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

builder.Services.AddHttpClient();
builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection("ApiSettings"));
builder.Services.AddScoped<CompanyService>();

builder.Services.AddHttpClient<CompanyService>((client) =>
{
    client.DefaultRequestHeaders.Add("X-API-KEY", "secret-key-123");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
