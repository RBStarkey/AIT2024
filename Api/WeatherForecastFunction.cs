using BlazorApp.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net;
using System.Reflection;
using System.Net.Http.Json;
using Azure;
using BlazorApp.Client.Models;
using System.IO;


namespace Api
{
	public class HttpTrigger
	{
		private readonly ILogger _logger;

		public HttpTrigger(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<HttpTrigger>();
		}

		[Function("WeatherForecast")]
		public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
		{
			var randomNumber = new Random();
			var temp = 0;

			var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateTime.Now.AddDays(index),
				TemperatureC = temp = randomNumber.Next(-20, 55),
				Summary = GetSummary(temp)
			}).ToArray();


			string path = Directory.GetCurrentDirectory();
			Console.WriteLine("The current directory is {0}", path);

			var response = req.CreateResponse(HttpStatusCode.OK);
			response.WriteAsJsonAsync(result);
			return response;
		}

		private string GetSummary(int temp)
		{
			var summary = "Mild";

			if (temp >= 32)
			{
				summary = "Hot";
			}
			else if (temp <= 16 && temp > 0)
			{
				summary = "Cold";
			}
			else if (temp <= 0)
			{
				summary = "Freezing";
			}

			return summary;
		}

	}
}
