using MediatR;

namespace xMediatR.Messages
{
    public class News : INotification
    {
        public string Message { get; set; }
    }
}