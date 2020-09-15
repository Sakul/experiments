using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;

namespace ConsoleApp
{
    public class SimpleMessage
    {
        public string Text { get; set; }
    }

	class Program
	{
		static async Task Main(string[] args)
		{
			// await PureMassTransit();

			// Docker install RabbitMQ
			// docker run -p 15672:15672 -p 5672:5672 masstransit/rabbitmq
			await SimpleRabbitMQ();
		}

		static async Task PureMassTransit()
		{
			var bus = Bus.Factory.CreateUsingInMemory(cfg =>
			{
				cfg.ReceiveEndpoint("test_queue", ep =>
				{
					ep.Handler<SimpleMessage>(context =>
					{
						return Console.Out.WriteLineAsync($"Received: {context.Message.Text}");
					});
				});
			});

			await bus.StartAsync();
			await bus.Publish(new SimpleMessage { Text = "Hello" });

			await Task.Run(() => Console.ReadLine());

			await bus.StopAsync();
		}

		static async Task SimpleRabbitMQ()
		{
			var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
			{
				cfg.Host("rabbitmq://localhost");
				cfg.ReceiveEndpoint("event-listener", ep =>
				{
					ep.Consumer<EventConsumer>();
					ep.Handler<SimpleMessage>(context =>
					{
						System.Console.WriteLine($"Handler received: {context.Message.Text}");
						return Task.CompletedTask;
					});
				});
			});
			var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
			await bus.StartAsync(source.Token);

			var continuePublish = true;
			while (continuePublish)
			{
				await bus.Publish<SimpleMessage>(new { Text = "Yahoo!, type c for continue." });
				var value = await Task.Run(() => Console.ReadLine());
				continuePublish = "c".Equals(value, StringComparison.OrdinalIgnoreCase);
			}
			await bus.StopAsync();
		}
		class EventConsumer : IConsumer<SimpleMessage>
		{
			public Task Consume(ConsumeContext<SimpleMessage> context)
			{
				Console.WriteLine($"Consumer received: {context.Message.Text}");
				return Task.CompletedTask;
			}
		}
	}
}
