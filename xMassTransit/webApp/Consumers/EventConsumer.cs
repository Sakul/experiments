using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using webApp.Models;

namespace webApp.Consumers
{
	public class EventConsumer : IConsumer<SimpleMessage>
	{
		private readonly ILogger<EventConsumer> logger;

		public EventConsumer(ILogger<EventConsumer> logger)
		{
			this.logger = logger;
		}

		public Task Consume(ConsumeContext<SimpleMessage> context)
		{
			logger.LogInformation($"Consumer received: {context.Message.Text}");
			return Task.CompletedTask;
		}
	}
}