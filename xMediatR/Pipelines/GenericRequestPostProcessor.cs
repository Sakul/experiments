using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;

namespace xMediatR.Pipelines
{
	public class GenericRequestPostProcessor<TRequest, TResponse>
	    : IRequestPostProcessor<TRequest, TResponse>
	{
		public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
		{
			Console.WriteLine("-- (PostProcessor) finished the process.");
			return Task.CompletedTask;
		}
	}
}