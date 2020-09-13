using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using xMediatR.Messages;

namespace xMediatR.Handlers
{
	public class NewsPaperHandler : INotificationHandler<News>
	{
		public Task Handle(News notification, CancellationToken cancellationToken)
		{
			Console.WriteLine($"- (async) {nameof(NewsPaperHandler)} got a message: {notification.Message}");
			return Task.CompletedTask;
		}
	}

	public class NewsTelevisionHandler : INotificationHandler<News>
	{
		public Task Handle(News notification, CancellationToken cancellationToken)
		{
			Console.WriteLine($"- (async) {nameof(NewsTelevisionHandler)} got a message: {notification.Message}");
			return Task.CompletedTask;
		}
	}
}