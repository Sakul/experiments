using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace xSimpleInjector
{
	class Program
	{
		static void Main(string[] args)
		{
			// Create a new container
			var container = new SimpleInjector.Container();

			// Configure the container (Register)
			container.Register<LogConfiguration>();
			container.Register<IAccount, AccountController>();

			container.Collection.Register<ILogger>(typeof(ConsoleLogger), typeof(CompositeLogger));
			container.Collection.Append<ILogger, FileLoger>();

			var assemblies = new[] { typeof(ICommand<>).Assembly };
			//container.Register(typeof(ICommand<>), assemblies);
			container.Collection.Register(assemblies);

			container.Register<IHandler<AHandler>, AHandler>();
			container.Register<IHandler<BHandler>, BHandler>();
			container.RegisterInitializer<IHandler>(hdl => hdl.ExecuteAsynchronously = true);

			container.Register<Transient>(Lifestyle.Transient);
			//container.Register<Scoped>(Lifestyle.Scoped);
			container.Register<Singleton1>(Lifestyle.Singleton);
			container.RegisterInstance(new Singleton2());
			container.RegisterSingleton<Singleton3>();

			// Solve: Cycling dependency
			container.RegisterInstance<IServiceProvider>(container);
			container.Register<ServiceFactory>();
			container.Register<AService>();
			container.Register<BService>();

			// Verify the container (optional)
			container.Verify();
			Console.WriteLine("Verified ====>");

			// Retrieve instances from the container (Resolve)
			DemoRegisterInitializer(container);

			DemoChangeValue<Transient>(container);
			//DemoChangeValue<Scoped>(container);
			DemoChangeValue<Singleton1>(container);
			DemoChangeValue<Singleton2>(container);
			DemoChangeValue<Singleton3>(container);

			var depenOnB = container.GetInstance<AService>();
			var depenOnA = container.GetInstance<BService>();
		}

		static void DemoChangeValue<T>(Container container) where T : class
		{
			Console.WriteLine();
			var obj1 = (IHaveValue)container.GetInstance<T>();
			Console.WriteLine($"{typeof(T).Name} object before change the value, valueL {obj1.Value}");
			obj1.Value = 99;
			var obj2 = (IHaveValue)container.GetInstance<T>();
			Console.WriteLine($"{typeof(T).Name} object after value changed, valueL {obj2.Value}");
		}

		static void DemoRegisterInitializer(Container container)
		{
			Console.WriteLine();
			Console.WriteLine($"A handler ExecuteAsynchronously: {container.GetInstance<IHandler<AHandler>>().ExecuteAsynchronously}");
			Console.WriteLine($"B handler ExecuteAsynchronously: {container.GetInstance<IHandler<BHandler>>().ExecuteAsynchronously}");
		}
	}

	public interface ILogger { }
	public class ConsoleLogger : ILogger
	{
		public ConsoleLogger(LogConfiguration configuration)
		{
			Console.WriteLine($"Constructing {nameof(ConsoleLogger)}");
		}
	}
	public class CompositeLogger : ILogger
	{
		public CompositeLogger(LogConfiguration configuration, IEnumerable<ILogger> loggers)
		{
			Console.WriteLine($"Constructing {nameof(CompositeLogger)}, Loggers: {loggers.Count()}");
		}
	}
	public class FileLoger : ILogger
	{
		public FileLoger(LogConfiguration configuration)
		{
			Console.WriteLine($"Constructing {nameof(FileLoger)}");
		}
	}
	public class LogConfiguration { }

	public interface IAccount { }
	public class AccountController : IAccount
	{
		public AccountController(IEnumerable<ILogger> dacs, IList<ILogger> loggers)
		{
			Console.WriteLine($"Constructing {nameof(AccountController)}, Loggers: {loggers.Count()}");
		}
	}

	public interface IHandler
	{
		bool ExecuteAsynchronously { get; set; }
	}
	public interface IHandler<T> : IHandler { }
	public class AHandler : IHandler<AHandler>
	{
		public bool ExecuteAsynchronously { get; set; }

		public AHandler()
		{
			Console.WriteLine($"Constructing {nameof(AHandler)}, ExecuteAsynchronously: {ExecuteAsynchronously}");
		}
	}
	public class BHandler : IHandler<BHandler>
	{
		public bool ExecuteAsynchronously { get; set; }

		public BHandler()
		{
			Console.WriteLine($"Constructing {nameof(BHandler)}, ExecuteAsynchronously: {ExecuteAsynchronously}");
		}
	}

	public interface ICommand { }
	public class NotRegisteredCommand : ICommand
	{
		public NotRegisteredCommand()
		{
			throw new Exception("This command must not be create!");
		}
	}
	public class DeleteCommand : ICommand<DeleteCommand>
	{
		public DeleteCommand()
		{
			Console.WriteLine($"Constructing {nameof(DeleteCommand)}");
		}
	}
	public interface ICommand<T> : ICommand { }
	public class CreateCommand : ICommand<CreateCommand>
	{
		public CreateCommand()
		{
			Console.WriteLine($"Constructing {nameof(CreateCommand)}");
		}
	}

	public interface IHaveValue
	{
		int Value { get; set; }
	}
	public class Transient : IHaveValue
	{
		public int Value { get; set; }

		public Transient()
		{
			Console.WriteLine($"Constructing {nameof(Transient)}");
		}
	}
	public class Scoped : IHaveValue
	{
		public int Value { get; set; }
	}
	public class Singleton1 : IHaveValue
	{
		public int Value { get; set; }

		public Singleton1()
		{
			Console.WriteLine($"Constructing {nameof(Singleton1)}");
		}
	}
	public class Singleton2 : IHaveValue
	{
		public int Value { get; set; }

		public Singleton2()
		{
			Console.WriteLine($"Constructing {nameof(Singleton2)}");
		}
	}
	public class Singleton3 : IHaveValue
	{
		public int Value { get; set; }

		public Singleton3()
		{
			Console.WriteLine($"Constructing {nameof(Singleton3)}");
		}
	}

	public class AService
	{
		private readonly ServiceFactory serviceFactory;
		private BService bService;
		private BService BService => bService ??= serviceFactory.GetService<BService>();

		public AService(ServiceFactory serviceFactory)
		{
			this.serviceFactory = serviceFactory;
		}
	}
	public class BService
	{
		private readonly ServiceFactory serviceFactory;
		private AService aService;
		private AService AService => aService ??= serviceFactory.GetService<AService>();

		public BService(ServiceFactory serviceFactory)
		{
			this.serviceFactory = serviceFactory;
		}
	}
	public class ServiceFactory
	{
		private readonly IServiceProvider serviceProvider;

		public ServiceFactory(IServiceProvider serviceProvider)
		{
			this.serviceProvider = serviceProvider;
		}

		public T GetService<T>()
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException(nameof(serviceProvider));
			}

			return (T)serviceProvider.GetService(typeof(T));
		}
	}
}
