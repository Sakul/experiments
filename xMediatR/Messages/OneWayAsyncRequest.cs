using MediatR;

namespace xMediatR.Messages
{
    public class OneWayAsyncRequest : IRequest
    {
        public string Message { get; set; }
    }
}