using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;

namespace xMediatR.Pipelines
{
	public class GenericRequestPreProcessor<TRequest>
		: IRequestPreProcessor<TRequest>
	{
		public Task Process(TRequest request, CancellationToken cancellationToken)
		{
			Console.WriteLine("-- (PreProcessor) starting the process.");
			return Task.CompletedTask;
		}
	}
}