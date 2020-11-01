using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace filemanager
{
    class sourceHandler
    {
        private string pathIn, pathOut = "";
        public string content = "";


        public string PathIn
        {
            get { return pathIn; }
            set { pathIn = value; }
        }

        public string PathOut
        {
            get { return pathOut; }
            set { pathOut = value; }
        }

        public sourceHandler(string pathIn, string pathOut)
        {
            this.pathIn = pathIn;
            this.pathOut = pathOut;
        }

        public void openFileToRead()
        {
            try
            {
                StreamReader sr = new StreamReader(File.OpenRead(this.pathIn));
                content = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        //replaceText("  "," ");
        public void replaceText(string from, string to)
        {
            while (content.Contains(from))
            {
                content = content.Replace(from, to);
            }
        }

        List<string> symbolTable = new List<string>();
        int symbolIndex = 0;

        string changeVariableAndConstants(string varAndConstName)
        {
            symbolTable.Add(varAndConstName);
            symbolIndex += 1;
            string result = "00" + symbolIndex.ToString();
            return result.Substring(result.Length - 3);
        }
        Dictionary<string, string> fromTo = new Dictionary<string, string>();

        public void replaceFirst()
        {

            content = Regex.Replace(content, @"//(.*?)\r?\n", "");
            content = Regex.Replace(content, @"/\*(.*?)\*/", "");
            content = Regex.Replace(content, @"/\*[\w\W]*\*/", "");

            fromTo.Add("  ", " ");
            fromTo.Add("\r\n", " ");
            fromTo.Add("    ", " "); //Tab
            fromTo.Add(" {", "{");
            fromTo.Add(" }", "}");
            fromTo.Add("{ ", "{");
            fromTo.Add("} ", "}");
            fromTo.Add(" (", "(");
            fromTo.Add(" )", ")");
            fromTo.Add("( ", "(");
            fromTo.Add(") ", ")");
            fromTo.Add(" ;", ";");
            fromTo.Add("; ", ";");
            fromTo.Add(" =", "=");
            fromTo.Add("= ", "=");
            fromTo.Add(" ,", ",");
            fromTo.Add(", ", ",");
            fromTo.Add(" -", "-");
            fromTo.Add("- ", "-");
            fromTo.Add("IF", " 10 ");
            fromTo.Add("for", " 20 ");
            fromTo.Add("while", " 30 ");
            fromTo.Add("switch", " 31 ");
            fromTo.Add("case", " 32 ");
            fromTo.Add("else", " 34 ");
            fromTo.Add("(", " 40 ");
            fromTo.Add(")", " 50 ");
            fromTo.Add("==", " 60 ");
            fromTo.Add("=", " 61 ");
            fromTo.Add("{", " 70 ");
            fromTo.Add("}", " 80 ");
            fromTo.Add("+", " 81 ");
            fromTo.Add("++", " 82 ");
            fromTo.Add("-", " 83 ");
            fromTo.Add("--", " 84 ");
            fromTo.Add("-=", " 85 ");
            fromTo.Add("+=", " 86 ");
            fromTo.Add(">", " 87 ");
            fromTo.Add("<", " 88 ");
            fromTo.Add(">=", " 89 ");
            fromTo.Add("<=", " 90 ");


            foreach (KeyValuePair<string, string> kvp in fromTo)
            {
                replaceText(kvp.Key, kvp.Value);
            }
        }
        public void replaceContent()
        {
            var blockComment = @"/[*][\w\d\s]+[*]/";
            var lineComment = @"//.*?\n";

            string fromNum = @"([0-9]+)";
            string fromVar = @"([a-z-_]+)";

            content = Regex.Replace(content, blockComment, "  ");
            content = Regex.Replace(content, lineComment, "  ");


            content = Regex.Replace(content, fromNum, changeVariableAndConstants("$1"));
            content = Regex.Replace(content, fromVar, changeVariableAndConstants("$1"));

            foreach (var x in fromTo)
            {
                while (content.Contains(x.Key))
                {
                    content = content.Replace(x.Key, x.Value);
                }
            }

        }

        public void openFileToWrite()
        {
            try
            {
                StreamWriter sw = new StreamWriter(File.Open(this.pathOut, FileMode.Create));
                sw.WriteLine(content);
                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
