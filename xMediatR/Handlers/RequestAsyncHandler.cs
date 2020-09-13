using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using xMediatR.Messages;

namespace xMediatR.Handlers
{
	public class RequestAsyncHandler : IRequestHandler<RequestAsync, Response>
	{
		public Task<Response> Handle(RequestAsync request, CancellationToken cancellationToken)
		{
			Console.WriteLine($"--- (async) {nameof(RequestAsyncHandler)}, received msg: {request.Message}");
			return Task.FromResult(new Response
			{
				Message = $"(async) pong",
			});
		}
	}
}