using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BlazorApp.Shared
{
    public class Globals
    {
		public const string AITLtd = "ACCESSible IT Ltd";
		public const string Version = "v2.01";
		public const string DocTypeDDL = "DDL";
        public const string SelectDotDotDot = "Select...";
		public const string NotStated = "Not Stated";
		public const string NotKnown = "(Not Known)";
        public const string Male = "Male";
        public const string Female = "Female";
        public const string AddNew = "(Add New Item)";
        public const string AgeZero = "0";

		public const string LastNameStarts = "Last Name Starts:";
		public const string LastNameContains = "Last Name Contains:";
		public const string FirstNameStarts = "First Name Starts:";
		public const string FirstNameContains = "First Name Contains:";
		public const string IDBegins = "ID Begins:";
		public const string EmailAddressStarts = "Email Address Starts:";
		public const string PhoneContains = "Phone Contains:";
		public const string DateModifiedFilter = "Filter Date Modified:";

        public const string AIT2024Container = "ait2024container";
        public const string BlobContainerDoesNotExist = "Blob container does mot exist";
        public const string OK = "OK";
		public const string SearchDotDotDot = "Search...";
		public const string DLogSizeLarge = "lg";
		public const string DLogSizeSmall = "sm";

		public static bool IsValidEmail(string email)
        {
            //https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format

            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
				Console.WriteLine("IsValidEmail - RegexMatchTimeoutException: " + e.Message);
                return false;
            }
            catch (ArgumentException e)
            {
				Console.WriteLine("IsValidEmail - ArgumentException: " + e.Message);
				return false;
            }
            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException ex)
            {
				Console.WriteLine("IsValidEmail - RegexMatchTimeoutException: " + ex.Message);
				return false;
            }
        }
        public static DateTime GetUKDateTime()
        {
			// https://msdn.microsoft.com/en-us/library/bb397769(v=vs.110).aspx
			// http://stackoverflow.com/questions/5601160/custom-date-time-format
			// https://en.wikipedia.org/wiki/List_of_tz_database_time_zones
			// https://codeshare.co.uk/blog/how-to-display-the-current-time-in-british-summer-time-using-c/
			// get the current UTC time
			DateTime localServerTime = DateTime.UtcNow;

			//Find out if the zone is in daylight saving time or not.
			TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Europe/London");
			bool isDaylightSavingTime = timeZoneInfo.IsDaylightSavingTime(localServerTime);

			//set the time to be the local server time
			DateTime UKDateTime = localServerTime;

			//if the zone is in British Summer Time, add an hour.
			if (isDaylightSavingTime)
			{
				UKDateTime = UKDateTime.AddHours(1);
			}
			return UKDateTime;
		}
		public int CalculateAge(DateTime birthDate)
        {


            DateTime now = GetUKDateTime();
            int age = now.Year - birthDate.Year;

            // For leap years we need this
            if (birthDate > now.AddYears(-age))
                age--;
            return age;
        }
        public static int AgeInYears(DateTime birthday)
        {
            DateTime today = GetUKDateTime().Date;
            // Calculate the age.
            int age = today.Year - birthday.Year;
            // Go back to the year the person was born in case of a leap year
            if (birthday.Date > today.AddYears(-age)) age--;
            return age;
        }
		public static List<string> GetSearchTypeList()
        {
            List<string> list = new List<string>();
            list.Add(LastNameStarts);
            list.Add(LastNameContains);
            list.Add(FirstNameStarts);
			list.Add(FirstNameContains);
            list.Add(IDBegins);
            list.Add(EmailAddressStarts);
            list.Add(PhoneContains);
            list.Add(DateModifiedFilter);
            return list;
        }
        public static bool StringCharsOK(string theString)
        {
            theString = theString.Trim();
            if (theString.Length > 20)
            {
                return false;
            }
            string pattern = @"[a-zA-Z0-9- ]+$";
            return Regex.IsMatch(theString, pattern, RegexOptions.IgnoreCase);
        }
        public static bool PostCodeIsOK(string PostCode)
        {
            // https://stackoverflow.com/questions/164979/regex-for-matching-uk-postcodes
            // https://stackoverflow.com/questions/23679586/regex-for-uk-postcode
            string pattern = "^[A-Za-z]{1,2}[0-9Rr][0-9A-Za-z]? [0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$";
            return Regex.IsMatch(PostCode, pattern, RegexOptions.IgnoreCase);
        }
    }
}
