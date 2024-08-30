using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlazorApp.Client.Models;
using Newtonsoft.Json;

namespace BlazorApp.Client.Services
{
	public class JsonBlobService
	{
		//https://learn.microsoft.com/en-us/azure/storage/blobs/storage-blob-dotnet-get-started?tabs=account-key#authorize-access-and-connect-to-blob-storage
		//https://learn.microsoft.com/en-us/dotnet/api/microsoft.azure.storage.cloudstorageaccount?view=azure-dotnet
		//https://learn.microsoft.com/en-us/aspnet/core/blazor/components/quickgrid?view=aspnetcore-8.0

		static readonly string accountName = "ait2024";
		static readonly string accountKey = "4kc/MlYoJMWJsVzOM9+zYQk0gQDA9XvJUls1Ls2vwZu3T7RG7XocsYUrZ5Ay729m4Fu2F0iU9Ms0+ASt7i/2Cg==";
		static readonly StorageSharedKeyCredential sharedKeyCredential = new(accountName, accountKey);

		static readonly string blobUri = "https://" + accountName + ".blob.core.windows.net";

		static readonly BlobServiceClient blobServiceClient = new(new Uri(blobUri), sharedKeyCredential);

		static readonly string containerName = "ait2024container";
		static readonly BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

		public async Task<List<Person>> GetPersonList()
		{
			List<Person> personList = [];
			try
			{

				BlobClient blobClient = containerClient.GetBlobClient("Persons.json");   //  Persons.json  BoysAndGirls.json  Contacts90.json

				Response<BlobDownloadResult> blobDownloadResult = await blobClient.DownloadContentAsync();

				string jsonFile = blobDownloadResult.Value.Content.ToString();
				personList = JsonConvert.DeserializeObject<List<Person>>(jsonFile);
				if (personList == null)
				{
					personList = [];
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("GetPersonList - Exception: " + ex.Message.ToString());
			}

			return personList;
		}
	}
}
