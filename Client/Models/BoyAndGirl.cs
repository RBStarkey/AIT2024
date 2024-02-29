using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Client.Models
{
	public class BoyAndGirl
	{
		public string ChildName { get; set; } = string.Empty;
		public string Gender { get; set; } = string.Empty;
		public string Position { get; set; } = string.Empty;
		public int Registrations { get; set; }

	}
}
