using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GiftCardLogParser
{
	public class Record
	{
		private static char[] SEPARATOR = new char[] { '\t' };

		public string SerialNumber;
		public DateTime DateTime;
		public string Action;
		public double Amount;
		public string Store;
		public string UserID;
		public bool IsCancelled;

		public Record(string line)
		{
			string[] tokens = line.Split(SEPARATOR);
			this.SerialNumber = tokens[0];
			this.DateTime = DateTime.Parse(tokens[1]);
			this.Action = tokens[2];
			this.Amount = double.Parse(tokens[3]);
			this.Store = tokens[4];
			this.UserID = tokens[5];
			this.DateTime = this.DateTime.Add(TimeSpan.Parse(tokens[6]));
			if (tokens.Length > 7)
			{
				this.IsCancelled = tokens[7].ToLower().StartsWith("cancel");
			}

            //File.AppendAllText(@"c:\GiftCard\" + this.DateTime.Year + ".txt", line + "\r\n");
		}
	}
}
