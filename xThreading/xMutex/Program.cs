using System;
using System.Threading;

namespace xMutex
{
	class Program
	{
		static Mutex mut = new Mutex();

		static void Main(string[] args)
		{
			var numThreads = 3;
			for (int i = 0; i < numThreads; i++)
			{
				var newThread = new Thread(new ThreadStart(() =>
				{
					var numIterations = 2;
					for (int i = 0; i < numIterations; i++)
					{
						UseResource(() => mut.WaitOne(TimeSpan.FromSeconds(1)));
						// UseResource(() => mut.WaitOne());
					}
					Console.WriteLine($"Exit {Thread.CurrentThread.Name}");
				}));
				newThread.Name = string.Format($"Thread{i}");
				newThread.Start();
			}
		}

		private static void UseResource(Func<bool> requestToProcess)
		{
			Console.WriteLine($"Requesting {Thread.CurrentThread.Name}");
			if (requestToProcess())
			{
				Console.WriteLine($">>>> {Thread.CurrentThread.Name} has entered");
				Thread.Sleep(2000);
				Console.WriteLine($"<<<< {Thread.CurrentThread.Name} has released");
				mut.ReleaseMutex();
			}
			else
			{
				Console.WriteLine($"- {Thread.CurrentThread.Name} will not acquire the mutex");
			}
		}
	}
}
