using MediatR;

namespace xMediatR.Messages
{
	public class RequestAsync : IRequest<Response>
	{
		public string Message { get; set; }
	}
}