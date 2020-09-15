using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using webApp.Consumers;

namespace webApp
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMassTransit(busCfg =>
			{
				busCfg.AddConsumer<EventConsumer>();
				busCfg.SetKebabCaseEndpointNameFormatter();
				busCfg.UsingRabbitMq((ctx, cfg) =>
				{
					// Manual endpoint
					cfg.ReceiveEndpoint("event-listener", e =>
					{
						e.ConfigureConsumer<EventConsumer>(ctx);
					});

					// Automate endpoint
					// cfg.ConfigureEndpoints(ctx);
				});
			});
			services.AddMassTransitHostedService();

			// (Optional) Health check, require extra setup steps in the Configure's endpoint mapper.
			services.AddHealthChecks();
			services.Configure<HealthCheckPublisherOptions>(opt =>
			{
				opt.Delay = TimeSpan.FromSeconds(2);
				opt.Predicate = it => it.Tags.Contains("ready");
			});

			services.AddControllers();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				// Extra steps for health check endpoint mapping.
				endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions()
				{
					Predicate = it => it.Tags.Contains("ready")
				});
				endpoints.MapHealthChecks("/health/live", new HealthCheckOptions());

				endpoints.MapControllers();
			});
		}
	}
}
