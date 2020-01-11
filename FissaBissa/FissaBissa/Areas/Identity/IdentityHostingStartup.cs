using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(FissaBissa.Areas.Identity.IdentityHostingStartup))]

namespace FissaBissa.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => { });
        }
    }
}
