using Polly;
using Polly.Extensions.Http;
using ThreeAmigos.Products.Services.UnderCutters;
using ThreeAmigosWebPage.Services;
using Auth0.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Register ProductService as a transient service
builder.Services.AddHttpClient<IUnderCuttersService, UnderCuttersService>()
                    .AddPolicyHandler(GetRetryPolicy());


builder.Services.AddAuth0WebAppAuthentication(options => {
    options.Domain = builder.Configuration["Auth:Domain"];
    options.ClientId = builder.Configuration["Auth:ClientId"];
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

<<<<<<< HEAD
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
=======
app.Run();

IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .WaitAndRetryAsync(5, retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}
>>>>>>> 58e1edfb27815dbe3dace9c38b3516b951601cce
