using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControllerTest
{
    public class Logger
    {
        public static Label StatusLabel { get; set; }
        public static ToolStripStatusLabel TS_StatusLabel { get; set; }

        public static void Log(string message)
        {
            Log(message, LogStyle.Info);
        }
        public static void Log(string message, LogStyle logStyle)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            var now = DateTime.Now;

            string d = now.Day.ToString();
            string m = now.Month.ToString();
            string day = d.Length < 2 ? "0" + d : d;
            string month = m.Length < 2 ? "0" + m : m;
            string year = now.Year.ToString();

            string h = now.Hour.ToString();
            string M = now.Minute.ToString();
            string s = now.Second.ToString();
            string ms = now.Millisecond.ToString();
            string hour = h.Length < 2 ? "0" + h : h;
            string minute = M.Length < 2 ? "0" + M : M;
            string second = s.Length < 2 ? "0" + s : s;
            string millisecond = ms.Length < 2 ? "0" + ms : ms.Substring(0, 2);

            Console.Write($"[{day}.{month}.{year} - {hour}:{minute}:{second}:{millisecond}] ");

            Console.ForegroundColor = logStyle.PrefixColor;
            Console.Write($"[{logStyle.PrefixName}] ");

            Console.ForegroundColor = logStyle.MessageColor;
            Console.WriteLine(message);

            Console.ResetColor();

            if (StatusLabel != null)
            {
                StatusLabel.Text = message;
            }

            if (TS_StatusLabel != null)
            {
                TS_StatusLabel.Text = message;
            }
        }
    }

    public struct LogStyle
    {
        public LogStyle(string prefixName, ConsoleColor prefixColor, ConsoleColor messageColor)
        {
            PrefixName = prefixName;
            PrefixColor = prefixColor;
            MessageColor = messageColor;
        }

        public string PrefixName { get; set; }
        public ConsoleColor PrefixColor { get; set; }
        public ConsoleColor MessageColor { get; set; }

        public static LogStyle Info { get; } = new LogStyle("Info", ConsoleColor.DarkGray, ConsoleColor.Gray);
        public static LogStyle Warning { get; } = new LogStyle("Warning", ConsoleColor.DarkYellow, ConsoleColor.Yellow);
        public static LogStyle Error { get; } = new LogStyle("Error", ConsoleColor.DarkRed, ConsoleColor.Red);
    }
}
