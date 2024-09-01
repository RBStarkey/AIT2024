using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Specialized;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Api
{
	public class GetJsonFileFunction(ILoggerFactory loggerFactory)
	{
		private readonly ILogger _logger = loggerFactory.CreateLogger<GetJsonFileFunction>();


		[Function("GetJsonFile")]
		public static async Task<HttpResponseData> GetJsonFile([HttpTrigger(
			AuthorizationLevel.Anonymous
			, "get"
			)]
				HttpRequestData req)
		{
			try
			{
				NameValueCollection? nameValueCollection = System.Web.HttpUtility.ParseQueryString(req.Url.Query) ?? [];
				var temp = nameValueCollection.GetValues(0);

				string fileName = nameValueCollection["filename"] ?? "";
				if (string.IsNullOrEmpty(fileName))
				{
					var responseBad = req.CreateResponse(HttpStatusCode.BadRequest);
					return responseBad;
				}
				//C:\Users\RaymondStarkey\source\repos\AIT2024\Client\wwwroot\Data\People.json
				// "GetJsonFile - Exception: Could not find file 'C:\\home\\site\\wwwroot\\wwwroot\\Data\\People.json'."

				//http://localhost:7071/api/GetJsonFile?filename=People.json 

				HttpResponseData responseOK = req.CreateResponse(HttpStatusCode.OK);
				using (FileStream stream = File.Open("JsonFiles/" + fileName, FileMode.Open))
				{
					await stream.CopyToAsync(responseOK.Body);
				}
				return responseOK;
			}
			catch (Exception ex)
			{
				Console.Write("GetJsonFile - Exception: " + ex.Message);
				HttpResponseData responseBad = req.CreateResponse(HttpStatusCode.InternalServerError);
				await responseBad.WriteAsJsonAsync("GetJsonFile - Exception: " + ex.Message);
				return responseBad;
			}
		}

		[Function("GetBlobFile")]
		public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
		{
			var response = req.CreateResponse(HttpStatusCode.OK);
			try
			{
				//http://localhost:7071/api/GetBlobFile?key=People.json

				_logger.LogInformation("C# HTTP trigger function - GetJsonFile - processed a request.");
				response.Headers.Add("Content-Type", "application/json; charset=utf-8");

				NameValueCollection? nameValueCollection = System.Web.HttpUtility.ParseQueryString(req.Url.Query) ?? [];
				string fileName = nameValueCollection["key"] ?? "";
				if (string.IsNullOrEmpty(fileName))
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
				BlobClient blobClient = containerClient.GetBlobClient(fileName);   // Person.json  BoysAndGirls.json  Contacts90.json

				Response<BlobDownloadResult> blobDownloadResult = blobClient.DownloadContent();

				string jsonFile = blobDownloadResult.Value.Content.ToString();

				response.WriteString(jsonFile);
				_logger.LogInformation("jsonFile length: " + jsonFile.Length.ToString());
			}
			catch (Exception ex)
			{
				_logger.LogInformation("GetJsonFile - Exception: " + ex.Message);
			}
			return response;
		}
	}
}
