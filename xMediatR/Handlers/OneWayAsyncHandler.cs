using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using xMediatR.Messages;

namespace xMediatR.Handlers
{
	public class OneWayAsyncHandler : AsyncRequestHandler<OneWayAsyncRequest>
	{
		protected override Task Handle(OneWayAsyncRequest request, CancellationToken cancellationToken)
		{
			Console.WriteLine($"--- (async) {nameof(OneWayAsyncHandler)}, received msg: {request.Message}");
			return Task.CompletedTask;
		}
	}
}