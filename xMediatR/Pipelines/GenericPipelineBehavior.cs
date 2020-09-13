using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace xMediatR.Pipelines
{
	public class GenericPipelineBehavior<TRequest, TResponse>
		: IPipelineBehavior<TRequest, TResponse>
	{
		public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
		{
			Console.WriteLine("- (PipelineBehavior) handling the request.");
			var response = await next();
			Console.WriteLine("- (PipelineBehavior) finished the request.");
			return response;
		}
	}
}