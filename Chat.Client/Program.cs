    using Blazored.LocalStorage;
using Chat.Client;
using Chat.Client.BlazorCustomAuth;
using Chat.Client.Integrations.Message;
using Chat.Client.Integrations.User;
using Chat.Client.Integrations.UserChat;
using Chat.Client.LocalStorage;
using Chat.Client.Razor_Page_Behind_Code_Source;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7175") });
builder.Services.AddScoped<IUserIntegration, UserIntegration>();
builder.Services.AddScoped<IMessageIntegration, MessageIntegration>();
builder.Services.AddScoped<IUserChatIntegration, UserChatIntegration>();

builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthorizationCore();
await builder.Build().RunAsync();   