using System;
using System.Linq;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Diagnostics;
using System.Threading.Tasks;

namespace xReactiveExtension
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var counter = 0;
			var subject = new Subject<int>();
			var observable = subject
				.Throttle(TimeSpan.FromSeconds(1))
				.Where(it => it % 2 == 0)
				.Do(it => counter++)
				.Delay(TimeSpan.FromSeconds(2))
				.Do(it => counter += 100);

			observable.Subscribe(onNext => Console.WriteLine(onNext));

			var random = new Random();
			var watch = new Stopwatch();
			watch.Start();
			while (true)
			{
				var number = random.Next();
				subject.OnNext(number);
				Console.WriteLine($"{watch.Elapsed}, send: {number}");
				if (watch.Elapsed >= TimeSpan.FromSeconds(2))
				{
					Console.WriteLine($"Current counter: {counter}");
					break;
				}
			}

			Console.WriteLine("Waiting for 3 sec");
			await Task.Delay(3005);
			Console.WriteLine($"Current counter: {counter}");

			Console.WriteLine("Waiting for 1 sec");
			await Task.Delay(1000);
			Console.WriteLine($"Current counter: {counter}");
		}
	}
}
