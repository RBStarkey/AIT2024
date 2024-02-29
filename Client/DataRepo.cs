using BlazorApp.Client.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Reflection;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace BlazorApp.Client.Data
{
	public class DataRepo
    {
        public List<Person> PersonList { get; set; }
        public List<Contact> ContactList { get; set; }
        public List<BoyAndGirl> BoyAndGirlList { get; set; }

        public DataRepo()
        {
            PersonList = [];
            ContactList = [];
            BoyAndGirlList = [];
        }
        public async Task LoadPersonList()
        {
			try
			{
				//string json = await Http.GetFromJsonAsync<Person[]>("/api/WeatherForecast") ?? new Person[] { };
				//var assemblyLocation = Directory.GetParent(Assembly.GetExecutingAssembly().Location)?.FullName;
				//var wwwrootLocation = Path.Combine(assemblyLocation, "wwwroot");
				//var webRoot = wwwrootLocation;


				//Assembly assem = typeof(DataRepo).Assembly;
				//Console.WriteLine("Assembly name: {0}", assem.FullName);

				// Get the currently executing assembly.
				//Assembly? currentAssem = Assembly.GetExecutingAssembly();
				//Console.WriteLine("Currently executing assembly:");
				//Console.WriteLine("   {0}\n", currentAssem.FullName);
				//string? path = currentAssem.FullName;


				string? people = Assembly.LoadFile("wwwroot\\Data\\People.json").ToString();

				//string json = File.ReadAllText("wwwroot/Data/People.json");
				//PersonList = JsonConvert.DeserializeObject<List<Person>>(json);
				//List<Person> PersonList = JsonConvert.DeserializeObject<List<Person>>(json);
				//PersonList = PersonList.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

        }
        public void LoadContactList()
        {
            string json = System.IO.File.ReadAllText("Data/Contacts90.json");
            ContactList = JsonConvert.DeserializeObject<List<Contact>>(json);
            //PersonList = PersonList.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();
        }

        public void LoadBoyAndGirlList()
        {
            string json = System.IO.File.ReadAllText("Data/BoysAndGirls.json");
            BoyAndGirlList = JsonConvert.DeserializeObject<List<BoyAndGirl>>(json);
        }
    }
}
