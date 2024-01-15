using Auth0.AspNetCore.Authentication;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using ThreeAmigosWebsite.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddTransient<IProductService, ProductService>();

builder.Services.AddHttpClient<IUserService, UserService>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddAuth0WebAppAuthentication(options => {
    options.Domain = builder.Configuration["Auth:Domain"];
    options.ClientId = builder.Configuration["Auth:ClientId"];
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHostedService<RefreshService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// TEST FOR GIT PUSH AND DEPLOY
