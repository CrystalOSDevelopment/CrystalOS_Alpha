using Cosmos.Core_Asm;
using Cosmos.System.Graphics.Fonts;
using CrystalOSAlpha.Applications;
using CrystalOSAlpha.Applications.Terminal;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.Graphics.Widgets;
using CrystalOSAlpha.SystemApps;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
        public static bool WaitForResponse = false;

        public static string Returning_Value = null;
        public static string name = null;
        public static List<bool> statements = new List<bool>() { true };

        public static string format = "string";

        public static bool WasIf = false;
        public static bool WasTrue = false;
        public static bool Checker = false;

        public static bool blank = false;

        public static string Clipboard = "";

        public static int Count = 0;

        public static bool looping = false;
        public static int Bracket = 0;
        public static int Bookmark = 0;
        public static int Cycles = 0;
        public static int MaxCycle = 0;
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
            foreach (string line in functions)
            {
                if (Count < 0)//statements.Count == 0
                {
                    Count = 0;
                    //statements.Add(true);
                }
                if (Count == 0)//statements[^1] == true
                {
                    #region variables
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
                                format = "string";
                                WaitForResponse = true;
                            }
                        }
                    }
                    if (line.StartsWith("int"))
                    {
                        string temp = line.Replace("int", "");
                        //temp = temp.Remove(temp.Length - 1);
                        string[] values = temp.Split("=");
                        if (int.TryParse(values[1], out int s) == true)
                        {
                            Variables.Add(new Programming.Variables(values[0], s));
                        }
                        else if (values[1] == "DateTime.UtcNow.Minute")
                        {
                            bool found = false;
                            foreach (var v in Variables)
                            {
                                if (v.I_Name == values[0])
                                {
                                    found = true;
                                    v.I_Value = DateTime.UtcNow.Minute;
                                }
                            }
                            if(found == false)
                            {
                                Variables.Add(new Programming.Variables(values[0], DateTime.UtcNow.Minute));
                            }
                        }
                        else if (values[1] == "DateTime.UtcNow.Year")
                        {
                            bool found = false;
                            foreach (var v in Variables)
                            {
                                if (v.I_Name == values[0])
                                {
                                    found = true;
                                    v.I_Value = DateTime.UtcNow.Year;
                                }
                            }
                            if (found == false)
                            {
                                Variables.Add(new Programming.Variables(values[0], DateTime.UtcNow.Year));
                            }
                        }
                        else if (values[1].StartsWith("Random.Next("))
                        {
                            string value = values[1].Replace("Random.Next(", "").Replace(")", "");
                            string[] Sides = value.Split(',');

                            Random rnd = new Random();

                            int generated = rnd.Next(int.Parse(Sides[0]), int.Parse(Sides[1]));

                            bool found = false;
                            foreach (var v in Variables)
                            {
                                if (v.I_Name == values[0])
                                {
                                    found = true;
                                    
                                    v.I_Value = generated;
                                }
                            }
                            if (found == false)
                            {
                                try
                                {
                                    Variables.Add(new Programming.Variables(values[0], generated));
                                }
                                catch(Exception e)
                                {
                                    Clipboard = e.Message;
                                }
                            }
                            
                            //Clipboard = generated.ToString();
                        }
                        else
                        {
                            //bool found = false;
                            //foreach(var v in Variables)
                            //{
                            //    if(v.I_Name == values[0])
                            //    {
                            //        found = true;
                            //    }
                            //}
                            //if(found == false)
                            //{
                            //    name = values[0];
                            //    if (Returning_Value == null)
                            //    {
                            //        format = "int";
                            //        WaitForResponse = true;
                            //    }
                            //}
                            name = values[0];
                            Variables.RemoveAll(d => d.I_Name == name);
                            if (Returning_Value == null)
                            {
                                format = "int";
                                WaitForResponse = true;
                            }
                        }
                    }
                    if (line.StartsWith("bool"))
                    {
                        string temp = line.Replace("bool", "");
                        //temp = temp.Remove(temp.Length - 1);
                        string[] values = temp.Split("=");
                        if (bool.TryParse(values[1], out bool s) == true)
                        {
                            Variables.RemoveAll(d => d.B_Name == values[0]);
                            Variables.Add(new Variables(values[0], s));
                        }
                        else
                        {
                            name = values[0];
                            if (Returning_Value == null)
                            {
                                format = "bool";
                                WaitForResponse = true;
                            }
                        }
                    }
                    if (line.StartsWith("float"))
                    {
                        string temp = line.Replace("float", "");
                        //temp = temp.Remove(temp.Length - 1);
                        string[] values = temp.Split("=");
                        if (float.TryParse(values[1], out float f) == true)
                        {
                            Variables.Add(new Programming.Variables(values[0], f));
                        }
                        else
                        {
                            name = values[0];
                            if (Returning_Value == null)
                            {
                                format = "float";
                                WaitForResponse = true;
                            }
                        }
                    }
                    if (line.StartsWith("double"))
                    {
                        string temp = line.Replace("double", "");
                        //temp = temp.Remove(temp.Length - 1);
                        string[] values = temp.Split("=");
                        if (double.TryParse(values[1], out double f) == true)
                        {
                            Variables.Add(new Programming.Variables(values[0], f));
                        }
                        else
                        {
                            name = values[0];
                            if (Returning_Value == null)
                            {
                                format = "double";
                                WaitForResponse = true;
                            }
                        }
                    }
                    if (Returning_Value != null)
                    {
                        if(format == "string")
                        {
                            Variables.Add(new Programming.Variables(name, Returning_Value));
                        }
                        else if(format == "int")
                        {
                            if(int.TryParse(Returning_Value, out int num))
                            {
                                Variables.Add(new Programming.Variables(name, num));
                            }
                        }
                        else if (format == "bool")
                        {
                            if (bool.TryParse(Returning_Value, out bool b))
                            {
                                Variables.Add(new Programming.Variables(name, b));
                            }
                        }
                        else if (format == "float")
                        {
                            if (float.TryParse(Returning_Value, out float b))
                            {
                                Variables.Add(new Programming.Variables(name, b));
                            }
                        }
                        else if (format == "double")
                        {
                            if (double.TryParse(Returning_Value, out double b))
                            {
                                Variables.Add(new Programming.Variables(name, b));
                            }
                        }
                        Returning_Value = null;
                    }
                    #endregion variables

                    #region Console out
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
                                            if (item.S_Name == temp)
                                            {
                                                output = item.S_Value;
                                            }
                                            if (item.I_Name == temp)
                                            {
                                                output = item.I_Value.ToString();
                                            }
                                            if (item.B_Name == temp)
                                            {
                                                output = item.B_Value.ToString();
                                            }
                                            if (item.F_Name == temp)
                                            {
                                                output = item.F_Value.ToString();
                                            }
                                            if (item.D_Name == temp)
                                            {
                                                output = item.D_Value.ToString();
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
                                        if (item.I_Name == temp)
                                        {
                                            output = item.I_Value.ToString();
                                        }
                                        if(item.B_Name == temp)
                                        {
                                            output = item.B_Value.ToString();
                                        }
                                        if (item.F_Name == temp)
                                        {
                                            output = item.F_Value.ToString();
                                        }
                                        if (item.D_Name == temp)
                                        {
                                            output = item.D_Value.ToString();
                                        }
                                    }
                                    if (firstline == true)
                                    {

                                    }
                                    else
                                    {
                                        output = output.Insert(0, "\n");
                                    }
                                }
                            }
                            else
                            {
                                if (temp.Contains("+"))
                                {
                                    foreach(string s in temp.Split('+'))
                                    {
                                        if (s.Contains("\""))
                                        {
                                            output += s.Replace("\"", "");
                                        }
                                        else
                                        {
                                            foreach (var item in Variables)
                                            {
                                                if (item.S_Name == s)
                                                {
                                                    output += item.S_Value;
                                                }
                                                if (item.I_Name == s)
                                                {
                                                    output += item.I_Value.ToString();
                                                }
                                                if (item.B_Name == s)
                                                {
                                                    output += item.B_Value.ToString();
                                                }
                                                if (item.F_Name == s)
                                                {
                                                    output += item.F_Value.ToString();
                                                }
                                                if (item.D_Name == s)
                                                {
                                                    output += item.D_Value.ToString();
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    /*
                                    if(firstline == true)
                                    {
                                        output += temp.Replace("\"", "");
                                    }
                                    else
                                    {
                                        output += "\n" + temp.Replace("\"", "");
                                    }
                                    */
                                    output += temp.Replace("\"", "");
                                }
                            }
                            output += "\n";
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
                    #endregion Console out

                    #region Conditionals
                    if (line.StartsWith("if"))
                    {
                        WasIf = true;
                        WasTrue = false;
                        try
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
                                    if (bool.TryParse(sides[0], out bool s))
                                    {
                                        sides[0] = s.ToString();
                                    }
                                    else
                                    {
                                        foreach (var Item in Variables)
                                        {
                                            if (sides[0] == Item.S_Name)
                                            {
                                                sides[0] = Item.S_Value;
                                            }
                                            if (sides[0] == Item.I_Name)
                                            {
                                                sides[0] = Item.I_Value.ToString();
                                            }
                                            if (sides[0] == Item.B_Name)
                                            {
                                                sides[0] = Item.B_Value.ToString();
                                            }
                                            if (sides[0] == Item.F_Name)
                                            {
                                                sides[0] = Item.F_Value.ToString();
                                            }
                                            if (sides[0] == Item.D_Name)
                                            {
                                                sides[0] = Item.D_Value.ToString();
                                            }
                                        }
                                    }
                                }
                                //Checking the right side
                                if (sides[1].Contains("\""))
                                {
                                    sides[1] = sides[1].Replace("\"", "");
                                }
                                else
                                {
                                    if (bool.TryParse(sides[1], out bool s))
                                    {
                                        sides[1] = s.ToString();
                                    }
                                    else
                                    {
                                        foreach (var Item in Variables)
                                        {
                                            if (sides[1] == Item.S_Name)
                                            {
                                                sides[1] = Item.S_Value;
                                            }
                                            if (sides[1] == Item.I_Name)
                                            {
                                                sides[1] = Item.I_Value.ToString();
                                            }
                                            if (sides[1] == Item.B_Name)
                                            {
                                                sides[1] = Item.B_Value.ToString();
                                            }
                                            if (sides[1] == Item.F_Name)
                                            {
                                                sides[1] = Item.F_Value.ToString();
                                            }
                                            if (sides[1] == Item.D_Name)
                                            {
                                                sides[1] = Item.D_Value.ToString();
                                            }
                                        }
                                    }
                                }
                                if (sides[0] == sides[1])
                                {
                                    blank = true;
                                }
                                else
                                {
                                    Count++;
                                    Checker = true;
                                    blank = false;
                                }
                            }
                            if (temp.Contains("!="))
                            {
                                //Checking the left-side
                                string[] sides = temp.Split("!=");
                                if (sides[0].Contains("\""))
                                {
                                    sides[0] = sides[0].Replace("\"", "");
                                }
                                else
                                {
                                    foreach (var Item in Variables)
                                    {
                                        if (sides[0] == Item.S_Name)
                                        {
                                            sides[0] = Item.S_Value;
                                        }
                                        if (sides[0] == Item.I_Name)
                                        {
                                            sides[0] = Item.I_Value.ToString();
                                        }
                                        if (sides[0] == Item.B_Name)
                                        {
                                            sides[0] = Item.B_Value.ToString();
                                        }
                                        if (sides[0] == Item.F_Name)
                                        {
                                            sides[0] = Item.F_Value.ToString();
                                        }
                                        if (sides[0] == Item.D_Name)
                                        {
                                            sides[0] = Item.D_Value.ToString();
                                        }
                                    }
                                }
                                //Checking the right side
                                if (sides[1].Contains("\""))
                                {
                                    sides[1] = sides[1].Replace("\"", "");
                                }
                                else
                                {
                                    foreach (var Item in Variables)
                                    {
                                        if (sides[1] == Item.S_Name)
                                        {
                                            sides[1] = Item.S_Value;
                                        }
                                        if (sides[1] == Item.I_Name)
                                        {
                                            sides[1] = Item.I_Value.ToString();
                                        }
                                        if (sides[1] == Item.B_Name)
                                        {
                                            sides[1] = Item.B_Value.ToString();
                                        }
                                        if (sides[1] == Item.F_Name)
                                        {
                                            sides[1] = Item.F_Value.ToString();
                                        }
                                        if (sides[1] == Item.D_Name)
                                        {
                                            sides[1] = Item.D_Value.ToString();
                                        }
                                    }
                                }
                                if (sides[0] != sides[1])
                                {
                                    blank = true;
                                }
                                else
                                {
                                    Count++;
                                    Checker = true;
                                    blank = false;
                                }
                            }
                            if (temp.Contains(">"))
                            {
                                //Checking the left-side
                                string[] sides = temp.Split(">");
                                if (sides[0].Contains("\""))
                                {
                                    sides[0] = sides[0].Replace("\"", "");
                                }
                                else
                                {
                                    foreach (var Item in Variables)
                                    {
                                        if (sides[0] == Item.S_Name)
                                        {
                                            sides[0] = Item.S_Value;
                                        }
                                        if (sides[0] == Item.I_Name)
                                        {
                                            sides[0] = Item.I_Value.ToString();
                                        }
                                        if (sides[0] == Item.B_Name)
                                        {
                                            sides[0] = Item.B_Value.ToString();
                                        }
                                        if (sides[0] == Item.F_Name)
                                        {
                                            sides[0] = Item.F_Value.ToString();
                                        }
                                        if (sides[0] == Item.D_Name)
                                        {
                                            sides[0] = Item.D_Value.ToString();
                                        }
                                    }
                                }
                                //Checking the right side
                                if (sides[1].Contains("\""))
                                {
                                    sides[1] = sides[1].Replace("\"", "");
                                }
                                else
                                {
                                    foreach (var Item in Variables)
                                    {
                                        if (sides[1] == Item.S_Name)
                                        {
                                            sides[1] = Item.S_Value;
                                        }
                                        if (sides[1] == Item.I_Name)
                                        {
                                            sides[1] = Item.I_Value.ToString();
                                        }
                                        if (sides[1] == Item.B_Name)
                                        {
                                            sides[1] = Item.B_Value.ToString();
                                        }
                                        if (sides[1] == Item.F_Name)
                                        {
                                            sides[1] = Item.F_Value.ToString();
                                        }
                                        if (sides[1] == Item.D_Name)
                                        {
                                            sides[1] = Item.D_Value.ToString();
                                        }
                                    }
                                }
                                if (int.Parse(sides[0]) > int.Parse(sides[1]))
                                {
                                    blank = true;
                                }
                                else
                                {
                                    Count++;
                                    Checker = true;
                                    blank = false;
                                }
                            }
                            if (temp.Contains("<"))
                            {
                                //Checking the left-side
                                string[] sides = temp.Split("<");
                                if (sides[0].Contains("\""))
                                {
                                    sides[0] = sides[0].Replace("\"", "");
                                }
                                else
                                {
                                    foreach (var Item in Variables)
                                    {
                                        if (sides[0] == Item.S_Name)
                                        {
                                            sides[0] = Item.S_Value;
                                        }
                                        if (sides[0] == Item.I_Name)
                                        {
                                            sides[0] = Item.I_Value.ToString();
                                        }
                                        if (sides[0] == Item.B_Name)
                                        {
                                            sides[0] = Item.B_Value.ToString();
                                        }
                                        if (sides[0] == Item.F_Name)
                                        {
                                            sides[0] = Item.F_Value.ToString();
                                        }
                                        if (sides[0] == Item.D_Name)
                                        {
                                            sides[0] = Item.D_Value.ToString();
                                        }
                                    }
                                }
                                //Checking the right side
                                if (sides[1].Contains("\""))
                                {
                                    sides[1] = sides[1].Replace("\"", "");
                                }
                                else
                                {
                                    foreach (var Item in Variables)
                                    {
                                        if (sides[1] == Item.S_Name)
                                        {
                                            sides[1] = Item.S_Value;
                                        }
                                        if (sides[1] == Item.I_Name)
                                        {
                                            sides[1] = Item.I_Value.ToString();
                                        }
                                        if (sides[1] == Item.B_Name)
                                        {
                                            sides[1] = Item.B_Value.ToString();
                                        }
                                        if (sides[1] == Item.F_Name)
                                        {
                                            sides[1] = Item.F_Value.ToString();
                                        }
                                        if (sides[1] == Item.D_Name)
                                        {
                                            sides[1] = Item.D_Value.ToString();
                                        }
                                    }
                                }
                                if (int.Parse(sides[0]) < int.Parse(sides[1]))
                                {
                                    blank = true;
                                }
                                else
                                {
                                    Count++;
                                    Checker = true;
                                    blank = false;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            //BitFont.DrawBitFontString(ImprovedVBE.cover, "ArialCustomCharset16", System.Drawing.Color.White, e.Message, 10, 10);
                            Clipboard = e.Message;
                        }
                    }
                    else if (line.StartsWith("elseif"))
                    {
                        try
                        {
                            string temp = line.Replace("elseif(", "").Replace(")", "");
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
                                    foreach (var Item in Variables)
                                    {
                                        if (sides[0] == Item.S_Name)
                                        {
                                            sides[0] = Item.S_Value;
                                        }
                                        if (sides[0] == Item.I_Name)
                                        {
                                            sides[0] = Item.I_Value.ToString();
                                        }
                                        if (sides[0] == Item.B_Name)
                                        {
                                            sides[0] = Item.B_Value.ToString();
                                        }
                                        if (sides[0] == Item.F_Name)
                                        {
                                            sides[0] = Item.F_Value.ToString();
                                        }
                                        if (sides[0] == Item.D_Name)
                                        {
                                            sides[0] = Item.D_Value.ToString();
                                        }
                                    }
                                }
                                //Checking the right side
                                if (sides[1].Contains("\""))
                                {
                                    sides[1] = sides[1].Replace("\"", "");
                                }
                                else
                                {
                                    foreach (var Item in Variables)
                                    {
                                        if (sides[1] == Item.S_Name)
                                        {
                                            sides[1] = Item.S_Value;
                                        }
                                        if (sides[1] == Item.I_Name)
                                        {
                                            sides[1] = Item.I_Value.ToString();
                                        }
                                        if (sides[1] == Item.B_Name)
                                        {
                                            sides[1] = Item.B_Value.ToString();
                                        }
                                        if (sides[1] == Item.F_Name)
                                        {
                                            sides[1] = Item.F_Value.ToString();
                                        }
                                        if (sides[1] == Item.D_Name)
                                        {
                                            sides[1] = Item.D_Value.ToString();
                                        }
                                    }
                                }
                                if (sides[0] == sides[1])
                                {
                                    if(blank == false)
                                    {
                                        blank = true;
                                    }
                                    else
                                    {
                                        Count++;
                                        Checker = true;
                                    }
                                }
                                else
                                {
                                    Count++;
                                    Checker = true;
                                }
                            }
                            if (temp.Contains("!="))
                            {
                                //Checking the left-side
                                string[] sides = temp.Split("!=");
                                if (sides[0].Contains("\""))
                                {
                                    sides[0] = sides[0].Replace("\"", "");
                                }
                                else
                                {
                                    foreach (var Item in Variables)
                                    {
                                        if (sides[0] == Item.S_Name)
                                        {
                                            sides[0] = Item.S_Value;
                                        }
                                        if (sides[0] == Item.I_Name)
                                        {
                                            sides[0] = Item.I_Value.ToString();
                                        }
                                        if (sides[0] == Item.B_Name)
                                        {
                                            sides[0] = Item.B_Value.ToString();
                                        }
                                        if (sides[0] == Item.F_Name)
                                        {
                                            sides[0] = Item.F_Value.ToString();
                                        }
                                        if (sides[0] == Item.D_Name)
                                        {
                                            sides[0] = Item.D_Value.ToString();
                                        }
                                    }
                                }
                                //Checking the right side
                                if (sides[1].Contains("\""))
                                {
                                    sides[1] = sides[1].Replace("\"", "");
                                }
                                else
                                {
                                    foreach (var Item in Variables)
                                    {
                                        if (sides[1] == Item.S_Name)
                                        {
                                            sides[1] = Item.S_Value;
                                        }
                                        if (sides[1] == Item.I_Name)
                                        {
                                            sides[1] = Item.I_Value.ToString();
                                        }
                                        if (sides[1] == Item.B_Name)
                                        {
                                            sides[1] = Item.B_Value.ToString();
                                        }
                                        if (sides[1] == Item.F_Name)
                                        {
                                            sides[1] = Item.F_Value.ToString();
                                        }
                                        if (sides[1] == Item.D_Name)
                                        {
                                            sides[1] = Item.D_Value.ToString();
                                        }
                                    }
                                }
                                if (sides[0] != sides[1])
                                {
                                    if (blank == false)
                                    {
                                        blank = true;
                                    }
                                    else
                                    {
                                        Count++;
                                        Checker = true;
                                    }
                                }
                                else
                                {
                                    Count++;
                                    Checker = true;
                                }
                            }
                            if (temp.Contains(">"))
                            {
                                //Checking the left-side
                                string[] sides = temp.Split(">");
                                if (sides[0].Contains("\""))
                                {
                                    sides[0] = sides[0].Replace("\"", "");
                                }
                                else
                                {
                                    foreach (var Item in Variables)
                                    {
                                        if (sides[0] == Item.S_Name)
                                        {
                                            sides[0] = Item.S_Value;
                                        }
                                        if (sides[0] == Item.I_Name)
                                        {
                                            sides[0] = Item.I_Value.ToString();
                                        }
                                        if (sides[0] == Item.B_Name)
                                        {
                                            sides[0] = Item.B_Value.ToString();
                                        }
                                        if (sides[0] == Item.F_Name)
                                        {
                                            sides[0] = Item.F_Value.ToString();
                                        }
                                        if (sides[0] == Item.D_Name)
                                        {
                                            sides[0] = Item.D_Value.ToString();
                                        }
                                    }
                                }
                                //Checking the right side
                                if (sides[1].Contains("\""))
                                {
                                    sides[1] = sides[1].Replace("\"", "");
                                }
                                else
                                {
                                    foreach (var Item in Variables)
                                    {
                                        if (sides[1] == Item.S_Name)
                                        {
                                            sides[1] = Item.S_Value;
                                        }
                                        if (sides[1] == Item.I_Name)
                                        {
                                            sides[1] = Item.I_Value.ToString();
                                        }
                                        if (sides[1] == Item.B_Name)
                                        {
                                            sides[1] = Item.B_Value.ToString();
                                        }
                                        if (sides[1] == Item.F_Name)
                                        {
                                            sides[1] = Item.F_Value.ToString();
                                        }
                                        if (sides[1] == Item.D_Name)
                                        {
                                            sides[1] = Item.D_Value.ToString();
                                        }
                                    }
                                }
                                if (int.Parse(sides[0]) > int.Parse(sides[1]))
                                {
                                    if (blank == false)
                                    {
                                        blank = true;
                                    }
                                    else
                                    {
                                        Count++;
                                        Checker = true;
                                    }
                                }
                                else
                                {
                                    Count++;
                                    Checker = true;
                                }
                            }
                            if (temp.Contains("<"))
                            {
                                //Checking the left-side
                                string[] sides = temp.Split("<");
                                if (sides[0].Contains("\""))
                                {
                                    sides[0] = sides[0].Replace("\"", "");
                                }
                                else
                                {
                                    foreach (var Item in Variables)
                                    {
                                        if (sides[0] == Item.S_Name)
                                        {
                                            sides[0] = Item.S_Value;
                                        }
                                        if (sides[0] == Item.I_Name)
                                        {
                                            sides[0] = Item.I_Value.ToString();
                                        }
                                        if (sides[0] == Item.B_Name)
                                        {
                                            sides[0] = Item.B_Value.ToString();
                                        }
                                        if (sides[0] == Item.F_Name)
                                        {
                                            sides[0] = Item.F_Value.ToString();
                                        }
                                        if (sides[0] == Item.D_Name)
                                        {
                                            sides[0] = Item.D_Value.ToString();
                                        }
                                    }
                                }
                                //Checking the right side
                                if (sides[1].Contains("\""))
                                {
                                    sides[1] = sides[1].Replace("\"", "");
                                }
                                else
                                {
                                    foreach (var Item in Variables)
                                    {
                                        if (sides[1] == Item.S_Name)
                                        {
                                            sides[1] = Item.S_Value;
                                        }
                                        if (sides[1] == Item.I_Name)
                                        {
                                            sides[1] = Item.I_Value.ToString();
                                        }
                                        if (sides[1] == Item.B_Name)
                                        {
                                            sides[1] = Item.B_Value.ToString();
                                        }
                                        if (sides[1] == Item.F_Name)
                                        {
                                            sides[1] = Item.F_Value.ToString();
                                        }
                                        if (sides[1] == Item.D_Name)
                                        {
                                            sides[1] = Item.D_Value.ToString();
                                        }
                                    }
                                }
                                if (int.Parse(sides[0]) < int.Parse(sides[1]))
                                {
                                    if (blank == false)
                                    {
                                        blank = true;
                                    }
                                    else
                                    {
                                        Count++;
                                        Checker = true;
                                    }
                                }
                                else
                                {
                                    Count++;
                                    Checker = true;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            //BitFont.DrawBitFontString(ImprovedVBE.cover, "ArialCustomCharset16", System.Drawing.Color.White, e.Message, 10, 10);
                            Clipboard = e.Message;
                        }
                    }
                    else if (line.StartsWith("else"))
                    {
                        if (blank == false && WasIf == true)
                        {
                            WasIf = false;
                        }
                        else
                        {
                            Count++;
                            WasIf = false;
                            blank = false;
                            Checker = true;
                        }
                    }
                    #endregion Conditionals

                    #region Loops
                    if (line.StartsWith("for"))
                    {

                        looping = true;
                    }
                    if (line.StartsWith("{"))
                    {
                        if(looping == true)
                        {
                            Bracket++;
                        }
                    }
                    else if (line.Trim().StartsWith("}"))
                    {
                        if (looping == true)
                        {
                            Bracket--;
                        }
                    }
                    if(Bracket == 0 && Cycles >= MaxCycle)
                    {
                        looping = false;
                        Cycles = 0;
                    }
                    #endregion Loops

                    #region Allert window
                    if (line.StartsWith("MsgBox.New("))
                    {
                        string trimmed = line.Replace("MsgBox.New(", "").Replace(")", "");
                        string[] data = trimmed.Split(",");
                        bool found = false;
                        foreach(App a in TaskScheduler.Apps)
                        {
                            if(a.name == "Alert!")
                            {
                                found = true;
                            }
                        }
                        if(found == false)
                        {
                            TaskScheduler.Apps.Add(new MsgBox(999, int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3]), "Alert!", data[4].Replace("\"", ""), ImprovedVBE.ScaleImageStock(ImageViewer.Nr1, 56, 56)));
                        }
                    }
                    #endregion Allert window

                    #region Accessories, comments
                    if (line.StartsWith("//")) { }
                    #endregion Accessories, comments

                    #region System IO
                    if (line.StartsWith("File"))
                    {
                        if (line.Contains("Create"))
                        {
                            string temp = line.Replace("File.Create(", "").Replace(")", "");
                            string[] args = temp.Split(",");

                            if (!File.Exists(args[0].Replace("\"", "") + args[1].Replace("\"", "")))
                            {
                                try
                                {
                                    File.Create(args[0].Replace("\"", "") + args[1].Replace("\"", ""));
                                }
                                catch (Exception e)
                                {
                                    Clipboard += e.Message;
                                }
                            }
                        }
                        if (line.Contains("WriteAllText"))
                        {
                            string temp = input.Replace("File.WriteAllText(", "");
                            temp = temp.Remove(temp.Length - 3);

                            string[] args = temp.Split(",\"");

                            if (File.Exists(args[0].Replace("\"", "")))
                            {
                                try
                                {
                                    string CleanUp = args[1];
                                    
                                    Clipboard += CleanUp + "\n";
                                    
                                    //CleanUp = CleanUp.Remove(CleanUp.Length - 1);
                                    
                                    //Clipboard += CleanUp + "\n";
                                    
                                    CleanUp = CleanUp.Replace("\\\"", "\"");
                                    
                                    Clipboard += CleanUp;

                                    File.WriteAllText(args[0].Replace("\"", ""), CleanUp);
                                }
                                catch (Exception e)
                                {
                                    Clipboard += e.Message;
                                }
                            }
                        }
                    }
                    #endregion System IO
                }
                else
                {
                    if (line.StartsWith("{"))
                    {
                        if(Checker == false)
                        {
                            Count++;
                        }
                        else
                        {
                            Checker = false;
                        }
                    }
                    if (line.Trim().StartsWith("}"))
                    {
                        Count--;
                    }
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

        public string B_Name { get; set; }
        public bool B_Value { get; set; }
        public string F_Name { get; set; }
        public float F_Value { get; set; }

        public string D_Name { get; set; }
        public double D_Value { get; set; }

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
        public Variables(string name, bool value)
        {
            this.B_Name = name;
            this.B_Value = value;
        }
        public Variables(string name, float value)
        {
            this.F_Name = name;
            this.F_Value = value;
        }
        public Variables(string name, double value)
        {
            this.D_Name = name;
            this.D_Value = value;
        }
        public string GetString()
        {
            return S_Value;
        }
        public int GetInt()
        {
            return I_Value;
        }
        public bool GetBool()
        {
            return B_Value;
        }
        public float GetFloat()
        {
            return F_Value;
        }
        public double GetDouble()
        {
            return D_Value;
        }
    }
}
