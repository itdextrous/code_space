using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InPlayWise.Data.Entities
{
	public class Incident
	{
		public Guid Id { get; set; }
		public int Order { get; set; }
		public string MatchId { get; set; }
		public int TimeInMinutes { get ; set; }
		public int Type { get; set; }
		public int MatchStatus { get; set; }

	}
}
