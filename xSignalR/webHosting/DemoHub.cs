using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace webHosting
{
	public class DemoHub : Hub
	{
		private readonly ILogger<DemoHub> logger;

		public DemoHub(ILogger<DemoHub> logger)
		{
			this.logger = logger;
		}

		public Task<string> SendMessageToServer(string msg)
		{
			logger.LogInformation($"Server received a message: {msg}");
			return Task.FromResult("Hi, this is a message from the server!");
		}
	}
}