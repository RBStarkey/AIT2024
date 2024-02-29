using System.Net;
using Azure.Storage.Blobs;
using Azure.Storage;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using BlazorApp.Client.Models;
using Azure.Storage.Blobs.Models;
using Azure;
using Newtonsoft.Json;
using System.Text;
using System.Reflection.Metadata;
using System.Collections.Specialized;

namespace Api
{
	public class GetJsonFileFunction(ILoggerFactory loggerFactory)
	{
		private readonly ILogger _logger = loggerFactory.CreateLogger<GetJsonFileFunction>();

		[Function("GetJsonFile")]
		public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req)
		{
			var response = req.CreateResponse(HttpStatusCode.OK);
			try
			{
				//http://localhost:7071/api/GetJsonFile?key=People.json

				_logger.LogInformation("C# HTTP trigger function - GetJsonFile - processed a request.");
				response.Headers.Add("Content-Type", "application/json; charset=utf-8");

				NameValueCollection? nameValueCollection = System.Web.HttpUtility.ParseQueryString(req.Url.Query) ?? [];
				string fileName = nameValueCollection["fileName"];
				if(string.IsNullOrEmpty(fileName))
				{
					_logger.LogError("GetJsonFile - NameValueCollection: Filename is null");
					return response;
				}
				string accountName = "ait2024";
				string accountKey = "4kc/MlYoJMWJsVzOM9+zYQk0gQDA9XvJUls1Ls2vwZu3T7RG7XocsYUrZ5Ay729m4Fu2F0iU9Ms0+ASt7i/2Cg==";
				StorageSharedKeyCredential sharedKeyCredential = new(accountName, accountKey);

				string blobUri = "https://" + accountName + ".blob.core.windows.net";

				BlobServiceClient blobServiceClient = new(new Uri(blobUri), sharedKeyCredential);

				string containerName = "ait2024container";
				BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
				BlobClient blobClient = containerClient.GetBlobClient(fileName);   // People.json  BoysAndGirls.json  Contacts90.json

				Response<BlobDownloadResult> blobDownloadResult = blobClient.DownloadContent();

				string jsonFile = blobDownloadResult.Value.Content.ToString();

				response.WriteString(jsonFile);
			}
			catch (Exception ex)
			{
				_logger.LogError("GetJsonFile - Exception: " + ex.Message);
			}
			return response;
		}
	}
}
