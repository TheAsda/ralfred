using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Ralfred
{
	public class Startup
	{
		public static void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers(options =>
			{
				options.InputFormatters.Add(new BypassFormDataInputFormatter());
			});
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