using BlazorApp.Client.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace BlazorApp.Client.Services
{
	public class DataRepo
	{
		public List<Person> PersonList { get; set; }
		public List<Contact> ContactList { get; set; }
		public List<BoyAndGirl> BoyAndGirlList { get; set; }

		private static readonly HttpClient httpClient = new(); 
		public DataRepo()
		{
			PersonList = [];
			ContactList = [];
			BoyAndGirlList = [];
		}
		public async Task LoadPersonList(string URLString)
		{
			HttpResponseMessage response = await httpClient.GetAsync(URLString);
			if(!response.IsSuccessStatusCode)
			{
				return;
			}
			PersonList = JsonConvert.DeserializeObject<List<Person>>(response.ToString()) ?? [];
		}
		public async Task LoadContactList()
		{
			try
			{
				//HttpResponseMessage response = await httpClient.GetAsync("http://wwwroot/Data//Contacts90.json");
				HttpResponseMessage response = await httpClient.GetAsync("http://localhost:7071/api/GetBlobFile?key=Contacts90.json");
				string responseBody = await response.Content.ReadAsStringAsync();
				ContactList = JsonConvert.DeserializeObject<List<Contact>>(responseBody);
			}
			catch (Exception ex)
			{
				Console.WriteLine("LoadContactList - Exception: " + ex.Message);
			}
		}

	public async Task LoadBoyAndGirlList()
	{
		var response = await httpClient.GetAsync("https://localhost:7071/api/GetBlobFile?key=Data/BoysAndGirls.json");
		//string json = File.ReadAllText("AIT2024/Client/wwwroot/Data/Contacts90.json");
		BoyAndGirlList = JsonConvert.DeserializeObject<List<BoyAndGirl>>(response.ToString());
	}
	}
}
