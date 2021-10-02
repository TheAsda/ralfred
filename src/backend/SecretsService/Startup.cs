using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Ralfred.Common.DataAccess.Repositories;
using Ralfred.Common.DependencyInjection;
using Ralfred.Common.Helpers;
using Ralfred.Common.Helpers.Serialization;
using Ralfred.Common.Managers;
using Ralfred.Common.Security;
using Ralfred.Common.Types;
using Ralfred.SecretsService.Middleware;
using Ralfred.SecretsService.Services;

using Serilog;


namespace Ralfred.SecretsService
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public static void ConfigureServices(IServiceCollection services)
		{
			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(_configuration, "Serilog")
				.CreateLogger();

			/* Preparing common and miscellaneous */
			services.AddLogging(builder =>
			{
				builder.ClearProviders();
				builder.AddSerilog(Log.Logger);
			});

			ConfigureSerialization(services);
			ConfigureCommon(services);

			/* Preparing application configuration  */
			services.AddTransient<IConfigurationManager, ConfigurationManager>(serviceProvider =>
				new ConfigurationManager(serviceProvider.GetService<YamlSerializer>()!, serviceProvider.GetService<IContentProvider>()!));

			var configuration = RegisterApplicationConfiguration(services);
			ConfigureTokenAuthentication(configuration, services);

			/* Preparing Logic and DAL */
			ConfigureDataAccess(configuration, services);

			services.AddTransient<IPathResolver, PathResolver>();
			services.AddTransient<IFileConverter, FileConverter>();
			services.AddTransient<ISecretsManager, SecretsManager>();
			services.AddTransient<IFormatterResolver, FormatterResolver>();

			services.AddControllers(options =>
			{
				options.Filters.Add<ExceptionFilter>();
				options.InputFormatters.Add(new BypassFormDataInputFormatter());
			});
		}

		public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();
			app.UseMiddleware<LoggingMiddleware>();
			app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

			applicationLifetime.ApplicationStopped.Register(() => { Log.Logger.Information("Application stopped."); });

			Log.Logger.Information("Application started.");
		}

		/* Service collection registration */
		private static void ConfigureDataAccess(Configuration appConfiguration, IServiceCollection services)
		{
			services.ConfigureStorageContext(appConfiguration.Engine!.Value);

			services.AddTransient<IGroupRepository, GroupRepository>();
			services.AddTransient<ISecretsRepository, SecretsRepository>();
		}

		private static void ConfigureSerialization(IServiceCollection services)
		{
			services.AddTransient<JsonSerializer>();
			services.AddTransient<XmlSerializer>();
			services.AddTransient<YamlSerializer>();

			services.AddTransient<StorageResolvingExtensions.SerializerResolver>(serviceProvider => key =>
			{
				return key switch
				{
					FormatType.Json => serviceProvider.GetService<JsonSerializer>()!,
					FormatType.Xml  => serviceProvider.GetService<XmlSerializer>()!,
					FormatType.Yaml => serviceProvider.GetService<YamlSerializer>()!,

					_ => null
				};
			});
		}

		private static void ConfigureCommon(IServiceCollection services)
		{
			services.AddTransient<IContentProvider, ContentProvider>();
			services.AddTransient<IDateTimeProvider, DateTimeProvider>();
			services.AddTransient<IGuidProvider, GuidProvider>();
			services.AddTransient<ITokenValidator, TokenValidator>();
			services.AddTransient<ITokenProvider, TokenProvider>();
		}

		/* Startup tasks */
		private static Configuration RegisterApplicationConfiguration(IServiceCollection services)
		{
			// TODO: add config validation

			var serviceProvider = services.BuildServiceProvider();
			var configurationManager = serviceProvider.GetService<IConfigurationManager>()!;

			var appConfigurationDefaults = configurationManager.Get(_configuration!["Settings:DefaultPath"]);
			var appConfigurationOverrides = configurationManager.Get(_configuration!["Settings:OverridePath"]);

			if (appConfigurationDefaults is null)
				throw new Exception("Cannot read configuration file. Application stopped.");

			var appConfiguration = appConfigurationDefaults;

			if (appConfigurationOverrides is not null)
				appConfiguration = configurationManager.Merge(appConfigurationDefaults, appConfigurationOverrides);

			services.AddSingleton(appConfiguration);

			Log.Logger.Information("Loading application configuration.");

			return appConfiguration;
		}

		private static void ConfigureTokenAuthentication(Configuration appConfiguration, IServiceCollection services)
		{
			if (!string.IsNullOrWhiteSpace(appConfiguration.RootToken))
				return;

			var serviceProvider = services.BuildServiceProvider();
			var tokenProvider = serviceProvider.GetService<ITokenProvider>()!;

			var token = tokenProvider.Generate(_configuration.GetValue<int>("TokenLifetimeInDays"));

			Log.Logger.Information($"Token: {token}");

			appConfiguration.RootToken = token;
		}

		private static IConfiguration? _configuration;
	}
}