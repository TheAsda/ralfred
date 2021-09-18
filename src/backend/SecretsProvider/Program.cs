using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;


namespace Ralfred.SecretsProvider
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		private static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((context, configurationBuilder) =>
				{
					configurationBuilder.Sources.Clear();

					configurationBuilder
						.SetBasePath(context.HostingEnvironment.ContentRootPath)
						.AddJsonFile("appsettings.json", false, true)
						.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, false);

					configurationBuilder.AddCommandLine(args);
				})
				.ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
	}
}