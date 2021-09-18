using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Ralfred.Common.DataAccess.Repositories;
using Ralfred.Common.DependencyInjection;
using Ralfred.Common.Helpers;
using Ralfred.Common.Managers;
using Ralfred.Common.Types;
using Ralfred.SecretsProvider.Services;


namespace Ralfred.SecretsProvider
{
	public class Startup
	{
		public static void ConfigureServices(IServiceCollection services)
		{
			services.ConfigureStorageContext(StorageEngineType.InMemory /* storage type from configuration */);

			services.AddTransient<ISecretsRepository, SecretsRepository>();
			services.AddTransient<IGroupRepository, GroupRepository>();
			services.AddTransient<IFileConverter, FileConverter>();
			services.AddTransient<IPathResolver, PathResolver>();
			services.AddTransient<ISecretsManager, SecretsManager>();
			services.AddControllers(options => { options.InputFormatters.Add(new BypassFormDataInputFormatter()); });
		}

		public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
		}
	}
}