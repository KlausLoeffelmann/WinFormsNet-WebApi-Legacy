using System;
using System.Collections.Generic;

namespace SimpleInvokeDemo.Data
{
	public class Contact
	{
		private static string[] myFirstNames = new string[] { "Adriana", "Stephan", "Andreas", "Lisa", "Daniel", "Guido", "Paul", "Lars", "Carola", "Mads" };

		private static string[] myLastNames = new string[] { "Mayor", "Spooner", "Miller", "Doe", "Löffelmann", "Landwerth", "Torgersen" };

		private static string[] myAddressLine = new string[] { "1719 Bellevue Way SE", "1249 110th Ave NE", "4780 42th Ave NE", "1810 130th Ave NE", "929 4th Ave", "1052 W Seneca St", "7764 32nd Avenue NE", "NE 140th St", "68th Ave NE" };

		private static string[] myCities = new string[] { "Kirkland", "Bellevue", "Monroe", "Woodinville", "Pleasanton", "Seattle", "Issaquah", "Sammamish", "Redmond" };

		private static string[] myZips = new string[] { "98033", "98034", "98040", "98123", "94566", "94567", "98027", "98028", "98052" };

		private static TimeSpan EARLIEST_AGE = DateTime.Now - new DateTime(1950, 01, 01);

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string Addressline1 { get; set; }
		public string Addressline2 { get; set; }
		public string City { get; set; }
		public string Zip { get; set; }
		public string State { get; set; }

		public int? Age
		{
			get
			{
				return Convert.ToInt32((DateTime.Now - DateOfBirth)?.TotalDays / 365);
			}
		}

		public override string ToString()
		{
			return $"{(LastName ?? " - - -")}, " + $"{1}: " + $"{1} years.";
		}

		public static IEnumerable<Contact> GetDemoData(int count)
		{

			//We need that to demo the new Exception Helper!
			if (count < 1)
			{
				yield break;
			}

			var randomGen = new Random(DateTime.Now.Millisecond);
			for (var counter = 1; counter <= count; counter++)
			{
				var birthDate = new DateTime(EARLIEST_AGE.Ticks + randomGen.Next(Convert.ToInt32(((new DateTime(2002, 01, 01)).Ticks - EARLIEST_AGE.Ticks) / 10000000000)) * 10000000000);
				yield return new Contact
				{
					FirstName = myFirstNames[randomGen.Next(myFirstNames.Length - 1)],
					LastName = myLastNames[randomGen.Next(myLastNames.Length - 1)],
					Addressline1 = myAddressLine[randomGen.Next(myAddressLine.Length - 1)],
					City = myCities[randomGen.Next(myCities.Length - 1)],
					Zip = myZips[randomGen.Next(myZips.Length - 1)],
					State = "WA",
					DateOfBirth = birthDate
				};
			}
		}
	}
}