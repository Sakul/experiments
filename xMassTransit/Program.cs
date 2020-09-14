using System;
using System.Threading.Tasks;
using MassTransit;
using xMassTransit.Messages;

namespace xMassTransit
{
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
			var bus = Bus.Factory.CreateUsingInMemory(sbc =>
			{
				sbc.ReceiveEndpoint("test_queue", ep =>
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
			var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
			{
				sbc.Host("rabbitmq://localhost");
				sbc.ReceiveEndpoint("test_queue", ep =>
				{
					ep.Handler<SimpleMessage>(context =>
					{
						return Console.Out.WriteLineAsync($"Received: {context.Message.Text}");
					});
				});
			});

			await bus.StartAsync();
			await bus.Publish(new SimpleMessage { Text = "Hello Rabbit" });

			await Task.Run(() => Console.ReadLine());
			await bus.StopAsync();
		}
	}
}
