using MediatR;

namespace xMediatR.Messages
{
    public class OneWaySyncRequest : IRequest
    {
        public string Message { get; set; }
    }
}