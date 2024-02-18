using CrystalOS_Alpha;
using CrystalOSAlpha.SystemApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOSAlpha.Programming
{
    class CodeGenerator
    {
        public static string Generate(string input, string newComponent)
        {
            string Modified_Code = input;
            if(Modified_Code == "")
            {
                Modified_Code = "#Define Window_Main\n" +
                                "{\n" +
                                "    this.Title = \"New window\";\n" +
                                "    this.Width = 500;\n" +
                                "    this.Height = 300;\n" +
                                "    this.Titlebar = true;\n" +
                                "    this.RGB = 60, 60, 60;\n" +
                                "    Table table = new Table(10, 50, 10, 10, 480, 200);\n" +
                                "}\n" +
                                "#void Looping\n" + 
                                "{\n" +
                                "    \n" +
                                "}";
            }
            if (newComponent != "")
            {
                List<string> Code = Separate(Modified_Code);
                string temp = "";
                string[] block = Code[0].Split('\n');
                if (newComponent.StartsWith("this."))
                {
                    if (newComponent.Trim().StartsWith("this.Width"))
                    {
                        //Kernel.Clipboard = "It comes here just fine";
                        for (int i = 0; i < block.Length; i++)
                        {
                            if (block[i].Trim().StartsWith("this.Width"))
                            {
                                string[] clean = newComponent.Split("=");
                                if (int.TryParse(clean[1].Trim().Replace(";", ""), out int t))
                                {
                                    if(t >= 50)
                                    {
                                        block[i] = "    this.Width = " + clean[1].Trim();
                                    }
                                }
                            }
                        }
                    }
                    else if (newComponent.StartsWith("this.Height"))
                    {
                        for (int i = 0; i < block.Length; i++)
                        {
                            if (block[i].Trim().StartsWith("this.Height"))
                            {
                                string[] clean = newComponent.Split("=");
                                if (int.TryParse(clean[1].Trim().Replace(";", ""), out int t))
                                {
                                    if (t >= 50)
                                    {
                                        block[i] = "    this.Height = " + clean[1].Trim();
                                    }
                                }
                            }
                        }
                    }
                    else if (newComponent.StartsWith("this.RGB"))
                    {
                        for (int i = 0; i < block.Length; i++)
                        {
                            if (block[i].Trim().StartsWith("this.RGB"))
                            {
                                string[] clean = newComponent.Split("=");
                                //Kernel.Clipboard = clean[1];
                                string[] miert = clean[1].Split(", ");
                                if (miert.Length == 3)
                                {
                                    if (int.TryParse(miert[^1].Replace(";", ""), out int num))
                                    {
                                        block[i] = "    this.RGB = " + clean[1].Trim();
                                    }
                                }
                                //block[i] = "    this.RGB = 60, 60, 60;";
                            }
                        }
                    }
                    else if (newComponent.StartsWith("this.Titlebar"))
                    {
                        for (int i = 0; i < block.Length; i++)
                        {
                            if (block[i].Trim().StartsWith("this.Titlebar"))
                            {
                                string[] clean = newComponent.Split("=");
                                block[i] = "    this.Titlebar = " + clean[1].Trim();
                            }
                        }
                    }
                    for (int i = 0; i < block.Length; i++)
                    {
                        temp += block[i] + "\n";
                    }
                }
                else if (newComponent.StartsWith("Button") || newComponent.StartsWith("Label") || newComponent.StartsWith("TextBox") || newComponent.StartsWith("Slider") || newComponent.StartsWith("CheckBox") || newComponent.StartsWith("Table"))
                {
                    string[] parts = newComponent.Split('=');
                    bool found = false;
                    for(int j = 0; j < block.Length; j++)
                    {
                        if (block[j].Trim().StartsWith(parts[0]))
                        {
                            block[j] = "    " + newComponent;
                            found = true;
                        }
                    }
                    for(int i = 0; i < block.Length; i++)
                    {
                        if(i < block.Length - 1)
                        {
                            temp += block[i] + "\n";
                        }
                        else
                        {
                            if(found == false)
                            {
                                temp += "    " + newComponent + "\n";
                            }
                            temp += block[i] + "\n";
                        }
                    }
                }
                for(int i = 1; i < Code.Count; i++)
                {
                    temp += Code[i] + "\n";
                }
                string[] CleanUp = temp.Split('\n');
                temp = "";
                foreach(string s in CleanUp)
                {
                    if(s != "")
                    {
                        temp += s + "\n";
                    }
                }
                Modified_Code = temp;
            }
            return Modified_Code;
        }
        public static string RemoveLine(string input, string LineToRemove)
        {
            string[] ls = input.Split('\n');
            List<string> lines = ToList(ls);
            foreach(string line in lines)
            {
                if(line.Trim() == LineToRemove)
                {
                    lines.Remove(line);
                }
            }
            string result = "";
            for (int i = 0; i < lines.Count; i++)
            {
                result += lines[i];
                if (i < lines.Count - 1)
                {
                    result += "\n";
                }
            }
            return result; //string.Join("\n", lines);
        }
        public static string RemoveLineByID(string input, string ID)
        {
            string[] ls = input.Split('\n');
            List<string> lines = ToList(ls);
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Trim().Contains("\"" + ID + "\","))
                {
                    lines.RemoveAt(i);
                }
            }
            string result = "";
            for (int i = 0; i < lines.Count; i++)
            {
                result += lines[i];
                if (i < lines.Count - 1)
                {
                    result += "\n";
                }
            }
            return result; //string.Join("\n", lines);
        }
        public static List<string> Separate(string In)
        {
            List<string> parts = new List<string>();

            //string current = "";
            //int openedRemainingBrackets = 0;
            //bool Started = false;
            //bool wasWhiteSpace = false;
            //for (int i = 0; i < In.Length; i++)
            //{
            //    current += In[i];
            //    if (In[i] == '{')
            //    {
            //        openedRemainingBrackets++;
            //        Started = true;
            //    }
            //    else if (In[i] == '}')
            //    {
            //        openedRemainingBrackets--;
            //    }
            //    if (i != 0 && openedRemainingBrackets == 0 && Started == true)
            //    {
            //        parts.Add(current);
            //        Started = false;
            //        current = "";
            //    }
            //}
            parts = CodeGenerator.ToList(In.Split("\n#"));
            for (int i = 0; i < parts.Count; i++)
            {
                if (i != 0)
                {
                    parts[i] = parts[i].Insert(0, "\n#");
                }
            }
            return parts;        }
        public static List<string> ToList(string[] content)
        {
            List<string> parts = new List<string>();
            foreach(string s in content)
            {
                parts.Add(s);
            }
            return parts;
        }
    }
}
