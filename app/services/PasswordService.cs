using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cons = ShadAhm.Katalalu.App.Services.ConsoleService;

namespace ShadAhm.Katalalu.App.Services
{
    public class PasswordService
    {
        private readonly SettingService _settingService;

        public PasswordService()
        {
            _settingService = SettingService.Instance;
        }

        public void List(string masterPassword)
        {
            int counter = 0;
            string line;
            Dictionary<int, int> columnMaxLengths = new Dictionary<int, int>();
            Dictionary<int, string> columnHeaders = new Dictionary<int, string>();
            List<string[]> rows = new List<string[]>();

            using var reader = new StreamReader(_settingService.GetCsvLocation());
            while ((line = reader.ReadLine()) != null)
            {
                string[] cells = line.Split(',');

                if (counter == 0)
                {
                    for (int i = 0; i < cells.Length; i++)
                    {
                        columnMaxLengths[i] = cells[i].Length;
                        columnHeaders[i] = cells[i];
                    }
                }
                else
                {
                    int serviceColIndex = columnHeaders.SingleOrDefault(o => o.Value.Equals("service", StringComparison.InvariantCultureIgnoreCase)).Key;
                    int usernameColIndex = columnHeaders.SingleOrDefault(o => o.Value.Equals("username", StringComparison.InvariantCultureIgnoreCase)).Key;
                    int passwordColIndex = columnHeaders.SingleOrDefault(o => o.Value.Equals("password", StringComparison.InvariantCultureIgnoreCase)).Key;
                    int isEncryptedColIndex = columnHeaders.SingleOrDefault(o => o.Value.Equals("isencrypted", StringComparison.InvariantCultureIgnoreCase)).Key;

                    string service = cells[serviceColIndex];
                    string username = cells[usernameColIndex];
                    string password = cells[passwordColIndex];
                    bool isEncrypted = Convert.ToBoolean(cells[isEncryptedColIndex]);

                    if (isEncrypted)
                    {
                        cells[usernameColIndex] = StringCipher.Decrypt(masterPassword, username, service);
                        cells[passwordColIndex] = StringCipher.Decrypt(masterPassword, password, service);
                    }

                    for (int i = 0; i < cells.Length; i++)
                    {
                        if (cells[i].Length > columnMaxLengths[i])
                            columnMaxLengths[i] = cells[i].Length;
                    }

                    rows.Add(cells);
                }

                counter++;
            }

            List<string> headers = columnHeaders.Select(o => o.Value).ToList();
            Cons.DrawTable(columnMaxLengths, headers, rows);
        }

        public void EncryptAll(string masterPassword)
        {
            int counter = 0;
            string firstLine = string.Empty;
            string line;
            Dictionary<int, string> columnHeaders = new Dictionary<int, string>();
            List<string> rows = new List<string>();

            using var reader = new StreamReader(_settingService.GetCsvLocation());
            while ((line = reader.ReadLine()) != null)
            {
                string[] cells = line.Split(',');

                if (counter == 0)
                {
                    firstLine = line;
                    for (int i = 0; i < cells.Length; i++)
                    {
                        columnHeaders[i] = cells[i];
                    }
                }
                else
                {
                    int serviceColIndex = columnHeaders.SingleOrDefault(o => o.Value.Equals("service", StringComparison.InvariantCultureIgnoreCase)).Key;
                    int usernameColIndex = columnHeaders.SingleOrDefault(o => o.Value.Equals("username", StringComparison.InvariantCultureIgnoreCase)).Key;
                    int passwordColIndex = columnHeaders.SingleOrDefault(o => o.Value.Equals("password", StringComparison.InvariantCultureIgnoreCase)).Key;
                    int isEncryptedColIndex = columnHeaders.SingleOrDefault(o => o.Value.Equals("isencrypted", StringComparison.InvariantCultureIgnoreCase)).Key;

                    string service = cells[serviceColIndex];
                    string username = cells[usernameColIndex];
                    string password = cells[passwordColIndex];
                    bool isEncrypted = Convert.ToBoolean(cells[isEncryptedColIndex]);

                    if (!isEncrypted)
                    {
                        cells[usernameColIndex] = StringCipher.Encrypt(masterPassword, username, service);
                        cells[passwordColIndex] = StringCipher.Encrypt(masterPassword, password, service);
                        cells[isEncryptedColIndex] = "TRUE";
                    }

                    rows.Add(string.Join(',', cells));
                }

                counter++;
            }

            reader.Close();

            using var writer = new StreamWriter(_settingService.GetCsvLocation());
            writer.WriteLine(firstLine);

            foreach (var row in rows)
            {
                writer.WriteLine(row);
            }

            writer.Close();
        }
    }
}