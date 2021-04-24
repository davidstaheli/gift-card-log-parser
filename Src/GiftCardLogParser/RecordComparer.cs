using System;
using System.Collections.Generic;
using System.Text;

namespace GiftCardLogParser
{
	public class RecordComparer : IComparer<Record>
	{
		public int Compare(Record x, Record y)
		{
			return x.DateTime.CompareTo(y.DateTime);
		}
	}
}
