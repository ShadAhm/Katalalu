using System;
using System.Collections.Generic;
using System.Text;

namespace ShadAhm.Katalalu.App.Services
{
    public static class ConsoleService
    {
        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void Clear()
        {
            Console.Clear();
        }

        public static void WriteLines(params string[] messages)
        {
            for (int i = 0; i < messages.Length; ++i)
                Console.WriteLine(messages[i]);
        }

        public static void AnyKeyToExit()
        {
            Console.WriteLine("Press any key to exit");
            Console.Read();
        }

        public static void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        public static string ReadLine()
        {
            return Console.ReadLine();
        }

        public static string ReadCommand(string label = null)
        {
            Console.Write(label ?? "> ");
            return Console.ReadLine().ToLowerInvariant();
        }

        public static void DrawTable(Dictionary<int, int> maxColumnLenghts, IList<string> headers, IEnumerable<string[]> rows)
        {
            int columnCount = headers.Count;
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < columnCount; i++)
            {
                sb.Append("| ");
                sb.Append(headers[i].PadRight(maxColumnLenghts[i]));
                sb.Append(" ");

                if (isLastColumn(i))
                    sb.Append("|");
            }

            sb.AppendLine();

            for (int i = 0; i < columnCount; i++)
            {
                sb.Append("+");
                sb.Append(new String('-', maxColumnLenghts[i] + 2));

                if (isLastColumn(i))
                    sb.Append("+");
            }

            sb.AppendLine();

            foreach (string[] row in rows)
            {
                for (int j = 0; j < row.Length; j++)
                {
                    sb.Append("| ");
                    sb.Append(row[j].PadRight(maxColumnLenghts[j]));
                    sb.Append(" ");

                }
                sb.Append("|");
                sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());

            bool isLastColumn(int i) => i == columnCount - 1;
        }
    }
}