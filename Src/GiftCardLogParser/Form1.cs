using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GiftCardLogParser
{
    public partial class Form1 : Form
    {
        private string _filenamesCsv;

        public Form1()
        {
            InitializeComponent();
        }

        private void _browseBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Multiselect = true;
            o.CheckFileExists = true;
            o.Title = "Select Gift Card Transaction Files";
            if (o.ShowDialog(this) == DialogResult.OK &&
                o.FileNames.Length > 0)
            {
                _filenamesCsv = string.Join(",", o.FileNames);
                _logFileTB.Text = _filenamesCsv;
                ParseLogFiles(o.FileNames);
            }
        }

        private void ParseLogFiles(string[] filenames)
        {
            // Locals
            string line;
            Record record;
            StreamReader sr;
            StringBuilder sb;
            List<Record> records;
            //YearOfTransactions year;
            RecordComparer recordComparer;
            string diagnosticMessageIfAny;
            List<string> diagnosticMessages;
            List<MoneyOwedToStore> storesOwed;
            Dictionary<string, List<Record>> decrementsBySerNum;
            Dictionary<string, List<Record>> incrementsBySerNum;
            //Dictionary<int, YearOfTransactions> transactionsByYear;
            Dictionary<string, List<MoneyOwedToStore>> moneyOwedToStoresByStore;
            Dictionary<int, Dictionary<string, List<MoneyOwedToStore>>> moneyOwedBetweenStoresByYear;

            // Initialize
            _textBox.Text = string.Empty;
            recordComparer = new RecordComparer();
            diagnosticMessages = new List<string>();
            decrementsBySerNum = new Dictionary<string, List<Record>>();
            incrementsBySerNum = new Dictionary<string, List<Record>>();
            //transactionsByYear = new Dictionary<int, YearOfTransactions>();
            moneyOwedBetweenStoresByYear = new Dictionary<int, Dictionary<string, List<MoneyOwedToStore>>>();

            #region Collect Records
            foreach (string filename in filenames)
            {
                sr = new StreamReader(filename);
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Trim().Length == 0)
                    {
                        continue;
                    }
                    else
                    {
                        record = new Record(line);
                        if (record.IsCancelled == true)
                        {
                            continue;
                        }

                        //if (transactionsByYear.TryGetValue(record.DateTime.Year, out year) == false)
                        //{
                        //    year = new YearOfTransactions();
                        //    transactionsByYear.Add(record.DateTime.Year, year);
                        //}

                        if (record.Action == "Activate" ||
                            record.Action == "Increment" ||
                            (record.Action == "Adjust" && record.Amount >= 0))
                        {
                            if (incrementsBySerNum.TryGetValue(record.SerialNumber, out records) == false)
                            {
                                records = new List<Record>();
                                incrementsBySerNum.Add(record.SerialNumber, records);
                            }
                            records.Add(record);
                            records.Sort(recordComparer);
                        }
                        else if (record.Action == "Redeem" ||
                                (record.Action == "Adjust" && record.Amount < 0))
                        {
                            if (decrementsBySerNum.TryGetValue(record.SerialNumber, out records) == false)
                            {
                                records = new List<Record>();
                                decrementsBySerNum.Add(record.SerialNumber, records);
                            }
                            records.Add(record);
                            records.Sort(recordComparer);
                        }
                        else
                        {
                            MessageBox.Show(
                                string.Format("The line shown below from file \"{0}\" has an unsupported transaction type called \"{1}\".  " +
                                    "The only supported transaction types are Activate, Increment, Redeem, and Adjust.\r\n\r\n{2}",
                                    filename, record.Action, line),
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                }
                sr.Close();
            }
            #endregion Collect Records

            //foreach (List<Record> incRecords in incrementsBySerNum.Values)
            //{
            //    foreach (Record incRecord in incRecords.Where(r => r.Store == "POS ID" || r.Store == "WEB - Midvale - W. 7200 S."))
            //    {
            //        _textBox.Text += string.Format(
            //            "{0}	{1}	{2}	{3}\r\n",
            //            incRecord.SerialNumber,
            //            incRecord.Action,
            //            incRecord.Store,
            //            incRecord.Amount);
            //    }
            //}

            //foreach (List<Record> decRecords in decrementsBySerNum.Values)
            //{
            //    foreach (Record decRecord in decRecords.Where(r => r.Store == "POS ID" || r.Store == "WEB - Midvale - W. 7200 S."))
            //    {
            //        _textBox.Text += string.Format(
            //            "{0}	{1}	{2}	{3}\r\n",
            //            decRecord.SerialNumber,
            //            decRecord.Action,
            //            decRecord.Store,
            //            decRecord.Amount);
            //    }
            //}

            // Process each year
            //foreach (int yearKey in transactionsByYear.Keys)
            {
                // Initialize
                //year = transactionsByYear[yearKey];
                //moneyOwedToStoresByStore = new Dictionary<string, List<MoneyOwedToStore>>();
                //_textBox.Text += "TRANSACTIONS FOR " + yearKey.ToString() + "\r\n";

                // For each decremented serial number
                foreach (List<Record> decRecords in decrementsBySerNum.Values)
                {
                    // For each decrement
                    foreach (Record decRecord in decRecords)
                    {
                        // Get dictionary of money owed by the decrement's year
                        if (!moneyOwedBetweenStoresByYear.TryGetValue(decRecord.DateTime.Year, out moneyOwedToStoresByStore))
                        {
                            moneyOwedToStoresByStore = new Dictionary<string, List<MoneyOwedToStore>>();
                            moneyOwedBetweenStoresByYear.Add(decRecord.DateTime.Year, moneyOwedToStoresByStore);
                        }

                        // Process the decrement
                        ProcessDecrement(decRecord, incrementsBySerNum, moneyOwedToStoresByStore, out diagnosticMessageIfAny);
                        if (diagnosticMessageIfAny != null)
                        {
                            diagnosticMessages.Add(diagnosticMessageIfAny);
                        }
                    }
                }

                // Output results
                sb = new StringBuilder();
                foreach (int year in moneyOwedBetweenStoresByYear.Keys.OrderBy(k => k))
                {
                    sb.Append(year.ToString() + "\r\n");
                    moneyOwedToStoresByStore = moneyOwedBetweenStoresByYear[year];
                    foreach (string storeThatOwes in moneyOwedToStoresByStore.Keys)
                    {
                        storesOwed = moneyOwedToStoresByStore[storeThatOwes];
                        foreach (MoneyOwedToStore storeOwed in storesOwed)
                        {
                            sb.AppendFormat("{0} owes {1} {2}\r\n",
                                storeThatOwes,
                                storeOwed.Store,
                                storeOwed.AmountOwed.ToString("C"));
                        }
                    }
                    sb.Append("\r\n");
                }
                foreach (string diagnosticMessage in diagnosticMessages)
                {
                    sb.Append(diagnosticMessage);
                }
                _textBox.Text += sb.ToString();
            }
        }

        private static void ProcessDecrement(
            Record decRecord,
            Dictionary<string, List<Record>> incrementsBySerNum,
            Dictionary<string, List<MoneyOwedToStore>> moneyOwedToStoresByStore,
            out string diagnosticMessageIfAny)
        {
            // Locals
            double decAmount;
            Record incRecord;
            string errorMessage;
            double amountToDeduct;
            List<Record> incRecords;
            MoneyOwedToStore moneyOwedToStore;
            List<MoneyOwedToStore> moneyOwedToStores;

            // Initialize
            decAmount = decRecord.Amount;
            diagnosticMessageIfAny = null;

            // Get the increments of the card
            if (incrementsBySerNum.TryGetValue(decRecord.SerialNumber, out incRecords) == false)
            {
                errorMessage = string.Format(
                    "Gift card with serial number {0} has a '{1}' transaction for {2} at {3} for store \"{4}\", " +
                    "but there is no previous record of it being activated or incremented to have a positive balance.  " +
                    "The card's activation or increment might be in an earlier file that was not included in the set of files to process this time.  " +
                    "This transaction will be skipped.",
                    decRecord.SerialNumber, decRecord.Action, decRecord.Amount, decRecord.DateTime, decRecord.Store);
                MessageBox.Show(
                    errorMessage,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            while (decAmount < -.001)
            {
                // Get increment
                if (incRecords == null || incRecords.Count == 0)
                {
                    incRecord = null;
                }
                else
                {
                    incRecord = incRecords[0];
                }

                if (incRecord == null)
                {
                    // Insufficient/no increment found?
                    diagnosticMessageIfAny = string.Format(
                        "⚠ The card with serial number {0} has a negative balance of {1:C} because it has greater debits than its credits.\r\n",
                        decRecord.SerialNumber, decAmount);
                    //MessageBox.Show(
                    //    diagnosticMessageIfAny,
                    //    "Warning",
                    //    MessageBoxButtons.OK,
                    //    MessageBoxIcon.Warning);
                    break;
                }

                // Deduct decrement
                amountToDeduct = Math.Max(decAmount, incRecord.Amount * -1);
                decAmount += Math.Min(decAmount * -1, incRecord.Amount);
                incRecord.Amount += amountToDeduct;
                if (incRecord.Amount == 0)
                {
                    incRecords.Remove(incRecord);
                }

                if (decRecord.Store != incRecord.Store)
                {
                    if (moneyOwedToStoresByStore.TryGetValue(incRecord.Store, out moneyOwedToStores) == false)
                    {
                        moneyOwedToStores = new List<MoneyOwedToStore>();
                        moneyOwedToStoresByStore.Add(incRecord.Store, moneyOwedToStores);
                    }

                    moneyOwedToStore = null;
                    foreach (MoneyOwedToStore mo in moneyOwedToStores)
                    {
                        if (mo.Store == decRecord.Store)
                        {
                            moneyOwedToStore = mo;
                            break;
                        }
                    }
                    if (moneyOwedToStore == null)
                    {
                        moneyOwedToStore = new MoneyOwedToStore() { Store = decRecord.Store, AmountOwed = 0 };
                        moneyOwedToStores.Add(moneyOwedToStore);
                    }
                    moneyOwedToStore.AmountOwed += amountToDeduct * -1;
                }

                //if (decRecord.Store != decRecord.Store)
                //{
                //    if (moneyOwedToStoresByStore.TryGetValue(decRecord.Store, out moneyOwedToStores) == false)
                //    {
                //        moneyOwedToStores = new List<MoneyOwedToStore>();
                //        moneyOwedToStoresByStore.Add(decRecord.Store, moneyOwedToStores);
                //    }

                //    moneyOwedToStore = null;
                //    foreach (MoneyOwedToStore mo in moneyOwedToStores)
                //    {
                //        if (mo.Store == decRecord.Store)
                //        {
                //            moneyOwedToStore = mo;
                //            break;
                //        }
                //    }
                //    if (moneyOwedToStore == null)
                //    {
                //        moneyOwedToStore = new MoneyOwedToStore() { Store = decRecord.Store, AmountOwed = 0 };
                //        moneyOwedToStores.Add(moneyOwedToStore);
                //    }
                //    moneyOwedToStore.AmountOwed += amountToDeduct * -1;
                //}
            }
        }
    }
}
