using Portfolio.Marvin.Components;
using Portfolio.Marvin.Extensions;
using Portfolio.Marvin.Providers.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services
    .AddResponseCompression();

builder.Services
    .AddPortfolioServices(builder.Configuration);

var app = builder.Build();

var blogProvider = app.Services.GetRequiredService<IBlogProvider>();
await blogProvider.Reload();

app.UseResponseCompression(); 

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode(options =>
    {
        options.DisableWebSocketCompression = true;
    });

app.Run();
