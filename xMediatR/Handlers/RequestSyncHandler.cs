using System;
using MediatR;
using xMediatR.Messages;

namespace xMediatR.Handlers
{
	public class RequestSyncHandler : RequestHandler<RequestSync, Response>
	{
		protected override Response Handle(RequestSync request)
		{
			Console.WriteLine($"--- (sync) {nameof(RequestSyncHandler)}, received msg: {request.Message}");
			return new Response
			{
				Message = $"(sync) pong2",
			};
		}
	}
}