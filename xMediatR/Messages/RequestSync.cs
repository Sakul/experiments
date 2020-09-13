using MediatR;

namespace xMediatR.Messages
{
	public class RequestSync : IRequest<Response>
	{
		public string Message { get; set; }
	}
}