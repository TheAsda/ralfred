using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Ralfred.Common.DependencyInjection;
using Ralfred.Common.Types;


namespace Ralfred.SecretsProvider
{
	public class Startup
	{
		public static void ConfigureServices(IServiceCollection services)
		{
			services.ConfigureStorageContext(StorageEngineType.InMemory /* storage type from configuration */);

			services.AddControllers();
		}

		public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}