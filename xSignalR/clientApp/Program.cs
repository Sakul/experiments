using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace clientApp
{
	class Program
	{
		static async Task Main(string[] args)
		{
			var hostUrl = "http://localhost:5000/hub/demo";
			var builder = new HubConnectionBuilder().WithUrl(hostUrl);
			var hub = builder.Build();

			try
			{
				await hub.StartAsync();
			}
			catch (System.Exception)
			{
				System.Console.WriteLine("Can't connect to the server.");
			}

			if (HubConnectionState.Connected != hub.State)
			{
				return;
			}

			var response = await hub.InvokeAsync<string>("SendMessageToServer", "Hello world!");
			System.Console.WriteLine($"Result: {response}");
		}
	}
}
