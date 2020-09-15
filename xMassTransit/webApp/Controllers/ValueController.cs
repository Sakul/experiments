using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using webApp.Models;

namespace webApp.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ValueController : ControllerBase
	{
		private readonly IPublishEndpoint publishEndpoint;

		public ValueController(IPublishEndpoint publishEndpoint)
		{
			this.publishEndpoint = publishEndpoint;
		}

		[HttpGet("{msg}")]
		public async Task<ActionResult> Get(string msg)
		{
			await publishEndpoint.Publish<SimpleMessage>(new
			{
				Text = msg
			});
			return Ok(new { Message = msg });
		}
	}
}