using Blazorise;
using Blazorise.Bulma;
using Blazorise.Icons.FontAwesome;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using JOHNNYbeGOOD.Home.Api.Contracts;
using JOHNNYbeGOOD.Home.Client.Services;

namespace JOHNNYbeGOOD.Home.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services
                .AddBlazorise(options =>
                 {
                     options.ChangeTextOnKeyPress = true;
                 })
                .AddBulmaProviders()
                .AddFontAwesomeIcons();

            builder.RootComponents.Add<App>("app");

            builder.Services.AddBaseAddressHttpClient();

            builder.Services.AddTransient<IFeedingService, FeedingServiceProxy>();

            await builder.Build().RunAsync();
        }
    }
}
