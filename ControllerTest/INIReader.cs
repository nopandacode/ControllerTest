using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    public class INIReader
    {
        public const string GLOBAL_SECTION_NAME = "<global>";

        public string FilePath { get; set; }
        public Dictionary<string, Dictionary<string, string>> Content { get; set; }

        private string[] fileContent;

        public INIReader(string filepath)
        {
            fileContent = File.ReadAllLines(filepath);

            Content = new Dictionary<string, Dictionary<string,string>>();

            Read();
        }

        public void Read()
        {
            string currentSection = GLOBAL_SECTION_NAME;
            Dictionary<string, string> currentSectionContent = new Dictionary<string, string>();

            foreach (var l in fileContent)
            {
                string line = l.Trim();

                if (line.Trim() == string.Empty)
                {
                    continue;
                }

                if (line.StartsWith("["))
                {
                    currentSection = line.Substring(1, line.Length - 2);
                    continue;
                }

                string[] parts = line.Split('=');
                if (parts.Length != 2)
                {
                    throw new Exception($"Line was not in the correct INI format. ('{line}')");
                }

                if (!Content.ContainsKey(currentSection)) Content.Add(currentSection, new Dictionary<string, string>());
                Content[currentSection].Add(parts[0].Trim(), parts[1].Trim());
            }
        }

        public Dictionary<string, string> this[string sectionName]
        {
            get 
            {
                if (!Content.ContainsKey(sectionName)) return null;
                return Content[sectionName];
            }
        }
    }
}
