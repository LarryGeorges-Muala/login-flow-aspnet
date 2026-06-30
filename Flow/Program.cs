using System.Text;
using Microsoft.EntityFrameworkCore;
using Flow.Components;
using Flow.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddInteractiveServerComponents();

// Add controller support
builder.Services.AddControllers(); 

// Add HttpClient to make API calls to yourself
builder.Services.AddScoped(sp => new HttpClient 
{ 
    // BaseAddress = new Uri("https://localhost:7000/") // Match your app's actual URL
    BaseAddress = new Uri("http://localhost:5290/")
    // BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddDbContext<ClientContext>(opt =>
    opt.UseInMemoryDatabase("ClientList"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Map your API routes
app.MapControllers(); 

// Encryption Key
var encryptionKey = AesProtector.GenerateRandomKey();
string? encryptionKeyString = Convert.ToBase64String(encryptionKey);
Environment.SetEnvironmentVariable("AesProtector", encryptionKeyString);

app.Run();
