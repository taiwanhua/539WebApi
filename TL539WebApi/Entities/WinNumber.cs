using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TL539WebApi.Entities
{
	public class WinNumber
	{
		public int WinNumberID { get; set; }
		public int Date { get; set; }
		public string DayOfWeek { get; set; }

		public string DrawOrder1 { get; set; }
		public string DrawOrder2 { get; set; }
		public string DrawOrder3 { get; set; }
		public string DrawOrder4 { get; set; }
		public string DrawOrder5 { get; set; }
		public string ASC1 { get; set; }
		public string ASC2 { get; set; }
		public string ASC3 { get; set; }
		public string ASC4 { get; set; }
		public string ASC5 { get; set; }

	}
}
