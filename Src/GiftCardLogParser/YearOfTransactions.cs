using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GiftCardLogParser
{
	public class YearOfTransactions
	{
		public Dictionary<string, List<Record>> DecrementsBySerNum { get; set; }
		public Dictionary<string, List<Record>> IncrementsBySerNum { get; set; }


		public YearOfTransactions()
		{
			this.DecrementsBySerNum = new Dictionary<string, List<Record>>();
			this.IncrementsBySerNum = new Dictionary<string, List<Record>>();
		}
	}
}
