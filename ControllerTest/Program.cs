using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControllerTest
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.Title = "Controller Test";

            Logger.Log("Checking for 'settings.ini'...");
            if (!File.Exists("settings.ini"))
            {
                Logger.Log("Creating 'settings.ini'");
                File.WriteAllText("settings.ini", "[Settings]\nAlwaysTop=true");
            }

            if (File.ReadAllText("settings.ini").Trim() == string.Empty)
            {
                Logger.Log("Settings file currently empty.", LogStyle.Error);
                Console.ReadKey();
                Environment.Exit(0);
            }

            Logger.Log("Reading settings...");
            INIReader reader = new INIReader("settings.ini");

            Logger.Log("Connecting...");
            XInputController controller;
            do
            {
                controller = new XInputController();
            }
            while (!controller.Connected);

            Logger.Log("Connected.");
            Logger.Log("Opening Debug");
            Logger.Log("Note: Batterylevel could be EMPTY at the start.", LogStyle.Warning);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool topMost = true;

            if (reader["Settings"] == null || !reader["Settings"].ContainsKey("AlwaysTop"))
            {
                Logger.Log("No entry for 'AlwaysTop' in settings.ini. Default 'true'", LogStyle.Warning);
            }
            else
            {
                topMost = reader["Settings"]["AlwaysTop"].ToLower() == "true" ? true : false;
            }

            Debug debug = new Debug(controller);
            debug.TopMost = topMost;

            Application.Run(debug);
        }
    }
}
