using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CarMechanic.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using CarMechanic.Client.Services;
using CarMechanic.Client.Auth;
using Microsoft.Extensions.Http;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddAuthorizationCore();

        builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
        builder.Services.AddScoped<IAuthService, AuthService>();

        builder.Services.AddScoped<AuthHeaderHandler>();

        builder.Services.AddHttpClient(Microsoft.Extensions.Options.Options.DefaultName, client =>
        {
            client.BaseAddress = new Uri("https://localhost:7184");
        })
        .AddHttpMessageHandler<AuthHeaderHandler>();

        builder.Services.AddScoped(sp =>
            sp.GetRequiredService<IHttpClientFactory>().CreateClient(Microsoft.Extensions.Options.Options.DefaultName));


        await builder.Build().RunAsync();
    }
}