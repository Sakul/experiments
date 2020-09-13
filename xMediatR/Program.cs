using System;
using System.Threading.Tasks;
using MediatR;
using MediatR.Pipeline;
using SimpleInjector;
using xMediatR.Messages;
using xMediatR.Pipelines;

namespace xMediatR
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var mediator = BuildMediator();

			await Demo_RequestResponseAsynchronous();
			await Demo_RequestResponseSynchronous();

			await Demo_RequestOneWayAsync();
			await Demo_RequestOneWaySync();

			await Demo_NotificationAsync();

			async Task Demo_RequestResponseAsynchronous()
			{
				Console.WriteLine("# Request & Response - (asynchronous)");
				var response = await mediator.Send(new RequestAsync { Message = "ping1" });
				Console.WriteLine($"Result: {response.Message}");
				Console.WriteLine();
			}
			async Task Demo_RequestResponseSynchronous()
			{
				Console.WriteLine("# Request & Response - (synchronous)");
				var response = await mediator.Send(new RequestSync { Message = "ping2" });
				Console.WriteLine($"Result: {response.Message}");
				Console.WriteLine();
			}
			async Task Demo_RequestOneWayAsync()
			{
				Console.WriteLine("# Request - One way (asynchronous)");
				await mediator.Send(new OneWayAsyncRequest { Message = "Hello" });
				Console.WriteLine();
			}
			async Task Demo_RequestOneWaySync()
			{
				Console.WriteLine("# Request - One way (synchronous)");
				await mediator.Send(new OneWaySyncRequest { Message = "Hi" });
				Console.WriteLine();
			}

			async Task Demo_NotificationAsync()
			{
				Console.WriteLine("# Notification (asynchronous)");
				await mediator.Publish(new News { Message = "gossip" });
				Console.WriteLine();
			}
		}

		static IMediator BuildMediator()
		{
			var container = new SimpleInjector.Container();

			// Mediator
			container.RegisterSingleton<IMediator, Mediator>();
			container.Register(() => new ServiceFactory(container.GetInstance), Lifestyle.Singleton);

			// Handlers
			var assemblies = new[] { typeof(RequestAsync).Assembly };
			container.Register(typeof(IRequestHandler<,>), assemblies);
			container.Collection.Register(typeof(INotificationHandler<>), assemblies);

			// Pipeline behaviours
			container.Collection.Register(typeof(IPipelineBehavior<,>), new[]
			{
				typeof(GenericPipelineBehavior<,>)
				// optionals can be register in this line.
			});

			// ------ Optionals ------

			// Pre processor
			container.Collection.Append(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
			container.Collection.Register(typeof(IRequestPreProcessor<>), new[] { typeof(GenericRequestPreProcessor<>) });

			// Post processor
			container.Collection.Append(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));
			container.Collection.Register(typeof(IRequestPostProcessor<,>), new[] { typeof(GenericRequestPostProcessor<,>) });

			container.Verify();

			var mediator = container.GetInstance<IMediator>();
			return mediator;
		}
	}
}
