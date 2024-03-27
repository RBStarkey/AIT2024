using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Client.Models
{
    public class Person
    {
        [JsonProperty(PropertyName = "id")]
        [DisplayName("ID")]
        [Required(ErrorMessage = "ID is required")]
        public Guid Id { get; set; }

		[DisplayName("Title")]
		[Required(ErrorMessage = "Select a Title")]
		public string Title { get; set; } = string.Empty;

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, ErrorMessage = "Last Name is too long. (Max chars: 50)")]
        public string LastName { get; set; } = string.Empty;

		[DisplayName("Middle Name")]
        [StringLength(50, ErrorMessage = "Middle Name is too long. (Max chars: 50)")]
        public string? MiddleName { get; set; } = string.Empty;

		[DisplayName("First Name")]
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, ErrorMessage = "First Name is too long. (Max chars: 50)")]
        public string FirstName { get; set; } = string.Empty;

		[DisplayName("Gender")]
		[Required(ErrorMessage = "Gender is required")]
		[StringLength(10, ErrorMessage = "Gender is too long. (Max chars: 10)")]
		public string Gender { get; set; } = string.Empty;

		[DisplayName("Company Name")]
        [Required (ErrorMessage = "Company Name is required")]
        [StringLength(256, ErrorMessage = "Company Name is too long. (Max chars: 256)")]
		public string CompanyName { get; set; } = string.Empty;

        [DisplayName("Phone")]
        [Required(ErrorMessage = "Phone Number is required")]
        [StringLength(25, ErrorMessage = "Phone Number is too long. (Max chars: 25)")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = string.Empty;

		[DisplayName("Email")]
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        //[StringLength(256, ErrorMessage = "Email is too long. (Max chars: 256)")]
        //[RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Email is not well formed")]
        public string? EmailAddress { get; set; }

        [DisplayName("Modified Date-Time")]
        [Range(typeof(DateTime), "1 Jan 1970", "31 Dec 3000", ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime ModifiedDate { get; set; }

		[DisplayName("Search Terms")]
		public string SearchTerms { get; set; } = string.Empty;

		public string UpdateSearchTerms()
		{
			string searchTerms = Id.ToString();
			searchTerms += "|" + Title;
			searchTerms += "|" + LastName;
			searchTerms += "|" + FirstName;
			if(!string.IsNullOrEmpty(MiddleName))
			{
				searchTerms += "|" + MiddleName;
			}
			else
			{
				searchTerms += "|" + string.Empty;
			}
			searchTerms += "|" + Gender;
			searchTerms += "|" + CompanyName;
			searchTerms += "|" + Phone;
			searchTerms += "|" + EmailAddress;
			searchTerms += "|" + ModifiedDate.ToString("dd MMM yyyy");
			return searchTerms;
		}
		public string FullName()
		{
			 return $"{ LastName }, {FirstName}, {MiddleName}";
		}
	}
}
