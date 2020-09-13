using System;
using MediatR;
using xMediatR.Messages;

namespace xMediatR.Handlers
{
	public class OneWaySyncHandler : RequestHandler<OneWaySyncRequest>
	{
		protected override void Handle(OneWaySyncRequest request)
		{
			Console.WriteLine($"--- (sync) {nameof(OneWaySyncHandler)}, received msg: {request.Message}");
		}
	}
}