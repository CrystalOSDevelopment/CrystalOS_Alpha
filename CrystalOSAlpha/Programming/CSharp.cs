using Cosmos.Core_Asm;
using CrystalOSAlpha.Applications;
using CrystalOSAlpha.Applications.Terminal;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskScheduler = CrystalOSAlpha.Graphics.TaskScheduler;

namespace CrystalOSAlpha.Programming
{
    class CSharp
    {
        public static List<Variables> Variables = new List<Variables>();
        public static bool firstline = true;
        public static int indic = 0;
        public static bool WaitForResponse = false;

        public static string Returning_Value = null;
        public static string name = null;
        public static List<bool> statements = new List<bool>() { true };
        public static string Executor(string input)
        {
            string output = "";
            /*
            string[] lines = input.Split('\n');
            for(int i = indic; i < lines.Length && WaitForResponse; i++)
            {
                output += Returning_methods(lines[i]);
                indic++;
            }
            */
            Programming_Term c = new Programming_Term();
            c.x = 100;
            c.y = 100;
            c.width = 700;
            c.height = 420;
            c.name = "Term...";
            c.z = 999;
            c.echo_off = true;
            c.code = input;
            c.icon = ImprovedVBE.ScaleImageStock(Resources.Terminal, 56, 56);
            TaskScheduler.Apps.Add(c);
            return output;
        }

        public static string Returning_methods(string input)
        {
            int i = 0;
            string output = "";
            bool jmp = false;
            bool strin = false;
            input = input.Trim();
            while(i < input.Length)
            {
                if(jmp == true)
                {
                    jmp = false;
                }
                else {

                    switch(input[i])
                    {
                        case ',':
                            if(input.Length > i + 1)
                            {
                                if (input[i + 1] == ' ' && strin != true)
                                {
                                    jmp = true;
                                }
                            }
                            output += input[i];
                            break;
                        case ' ':
                            if (input.Length > i + 1)
                            {
                                if (strin == true)
                                {
                                    output += input[i];
                                }
                            }
                            break;
                        case '\"':
                            if(strin == true)
                            {
                                strin = false;
                            }
                            else
                            {
                                strin = true;
                            }
                            output += input[i];
                            break;
                        default:
                            output += input[i];
                            break;
                    }
                }
                i++;
            }
            output = Interpreting(output, "");
            return output;
        }

        public static string Interpreting(string input, string directory)
        {
            string output = "";
            string[] functions = input.Split(';');
            foreach(string line in functions)
            {
                if (statements[^1] == true)
                {
                    if (line.StartsWith("string"))
                    {
                        string temp = line.Replace("string", "");
                        //temp = temp.Remove(temp.Length - 1);
                        string[] values = temp.Split("=");
                        if (values[1].Contains("\""))
                        {
                            Variables.Add(new Programming.Variables(values[0], values[1].Replace("\"", "")));
                        }
                        else
                        {
                            name = values[0];
                            if(Returning_Value == null)
                            {
                                WaitForResponse = true;
                            }
                        }
                    }
                    if(Returning_Value != null)
                    {
                        Variables.Add(new Programming.Variables(name, Returning_Value));
                        Returning_Value = null;
                    }
                    if (line.StartsWith("Console"))
                    {
                        WaitForResponse = false;
                        string temp = line.Replace("Console.", "");
                        if (temp.StartsWith("WriteLine("))
                        {
                            temp = temp.Replace("WriteLine(", "");
                            temp = temp.Replace(")", "");
                            if (!temp.Contains("\""))
                            {
                                if (temp.Contains("+"))
                                {
                                    string[] container = temp.Split("+");
                                    string outing = "";
                                    foreach(string s in container)
                                    {
                                        foreach(var item in Variables)
                                        {
                                            if(item.S_Name == s)
                                            {
                                                outing += item.S_Value;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (var item in Variables)
                                    {
                                        if (item.S_Name == temp)
                                        {
                                            output = item.S_Value;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (temp.Contains("+"))
                                {
                                    foreach(string s in temp.Split('+'))
                                    {
                                        output += s.Replace("\"", "");
                                    }
                                }
                                else
                                {
                                    if(firstline == true)
                                    {
                                        output += temp.Replace("\"", "");
                                    }
                                    else
                                    {
                                        output += "\n" + temp.Replace("\"", "");
                                    }
                                }
                            }
                        }
                        else if (temp.StartsWith("Write("))
                        {
                            temp = temp.Replace("Write(", "");
                            temp = temp.Replace(")", "");
                            if (!temp.Contains("\""))
                            {

                            }
                            else
                            {
                                if (temp.Contains("+"))
                                {
                                    foreach (string s in temp.Split('+'))
                                    {
                                        output += s.Replace("\"", "");
                                    }
                                }
                                else
                                {
                                    output += temp.Replace("\"", "");
                                }
                            }
                        }
                        else if (temp.StartsWith("ReadLine("))
                        {
                            WaitForResponse = true;
                        }
                    }
                    if (line.StartsWith("if"))
                    {
                        string temp = line.Replace("if(", "").Replace(")", "");
                        if (temp.Contains("=="))
                        {
                            //Checking the left-side
                            string[] sides = temp.Split("==");
                            if (sides[0].Contains("\""))
                            {
                                sides[0] = sides[0].Replace("\"", "");
                            }
                            else
                            {
                                foreach(var Item in Variables)
                                {
                                    if (sides[0] == Item.S_Name)
                                    {
                                        sides[0] = Item.S_Value;
                                    }
                                }
                            }
                            //Checking the right side
                        }
                    }
                    if (line.StartsWith("//")) { }
                }
                firstline = false;
            }
            return output;
        }
    }

    class Variables
    {
        public string S_Name { get; set; }
        public string S_Value { get; set; }

        public string I_Name { get; set; }
        public int I_Value { get; set; }

        public Variables(string name, string value)
        {
            this.S_Name = name;
            this.S_Value = value;
        }
        public Variables(string name, int value)
        {
            this.I_Name = name;
            this.I_Value = value;
        }
        public string GetString()
        {
            return S_Value;
        }
        public int GetInt()
        {
            return I_Value;
        }
    }
}
