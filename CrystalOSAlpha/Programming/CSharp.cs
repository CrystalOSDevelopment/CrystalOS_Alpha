using Cosmos.HAL.Audio;
using Cosmos.HAL.Drivers.Audio;
using Cosmos.System;
using Cosmos.System.Audio.IO;
using Cosmos.System.Audio;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Applications;
using CrystalOSAlpha.Applications.Terminal;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.Graphics.Widgets;
using CrystalOSAlpha.SystemApps;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TaskScheduler = CrystalOSAlpha.Graphics.TaskScheduler;
using CrystalOSAlpha.Applications.Calculator;
using System.Drawing;
using CrYstalOSAlpha.UI_Elements;

namespace CrystalOSAlpha.Programming
{
    class CSharp
    {
        public int Count = 0;
        public int CurrentColor = 0;
        public int WhileBracket = 0;
        public int StartPoint = 0;
        public int Bracket = 0;
        public int Bookmark = 0;
        public int Cycles = 0;
        public int MaxCycle = 0;
        public int CycleCounter = 0;

        public bool firstline = true;
        public bool WaitForResponse = false;
        public bool WasIf = false;
        public bool WasTrue = false;
        public bool Checker = false;
        public bool blank = false;
        public bool looping = false;
        public bool WhileLoop = false;
        public bool WasElse = false;
        public bool KeyOnly = false;
        public bool NeedUpdate = false;

        public string Returning_Value = null;
        public string name = null;
        public string format = "string";
        public string Clipboard = "";

        public Bitmap window;
        
        public ConsoleKeyEx key = ConsoleKeyEx.NoName;

        public List<bool> statements = new List<bool>() { true };
        public List<Variables> Variables = new List<Variables>();

        //For graphical use:
        #region UI_Elements
        public List<Button> Button = new List<Button>();
        public List<Slider> Slider = new List<Slider>();
        public List<CheckBox> CheckBox = new List<CheckBox>();
        public List<Dropdown> Dropdown = new List<Dropdown>();
        public List<Scrollbar> Scroll = new List<Scrollbar>();
        public List<TextBox> TextBox = new List<TextBox>();
        public List<label> Label = new List<label>();
        public List<Table> Tables = new List<Table>();
        public List<PictureBox> Picturebox = new List<PictureBox>();
        #endregion UI_Elements

        public string Executor(string input)
        {
            string output = "";
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

        public string Returning_methods(string input)
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

        public string Interpreting(string input, string directory)
        {
            string output = "";
            string[] functions = input.Split(';');
            foreach (string line in functions)
            {
                if (Count < 0)
                {
                    Count = 0;
                }
                if (Count == 0)
                {
                    #region variables
                    if (line.StartsWith("string"))
                    {
                        string temp = line.Replace("string", "");
                        string[] values = temp.Split("=");
                        if (values[1].Contains("\"") && !values[1].Contains("+"))
                        {
                            name = values[0];
                            Variables.RemoveAll(d => d.S_Name == name);
                            Variables.Add(new Programming.Variables(values[0], values[1].Replace("\"", "")));
                        }
                        else
                        {
                            if (!values[1].Contains("Console"))
                            {
                                try
                                {
                                    if (values[1].EndsWith(".Content"))
                                    {
                                        values[1] = values[1].Replace(".Content", "");
                                        foreach (var v in Button)
                                        {
                                            if (v.ID == values[1])
                                            {
                                                Variables.Add(new Programming.Variables(values[0], v.Text));
                                            }
                                        }
                                        foreach (var v in Label)
                                        {
                                            if (v.ID == values[1])
                                            {
                                                Variables.Add(new Programming.Variables(values[0], v.Text));
                                            }
                                        }
                                        foreach (var v in TextBox)
                                        {
                                            if (v.ID == values[1])
                                            {
                                                Variables.Add(new Programming.Variables(values[0], v.Text));
                                            }
                                        }
                                    }
                                    else if (values[1].Contains(".GetValue"))
                                    {
                                        foreach (var item in Tables)
                                        {
                                            if (values[1].StartsWith(item.ID))
                                            {
                                                string cleaned = values[1].Remove(0, item.ID.Length + 1);
                                                if (cleaned.StartsWith("GetValue("))
                                                {
                                                    cleaned = cleaned.Replace("GetValue(", "");
                                                    cleaned = cleaned.Remove(cleaned.Length - 1);
                                                    if (cleaned.Split(',').Length == 2)
                                                    {
                                                        Variables.Add(new Programming.Variables(values[0], item.GetValue(int.Parse(cleaned.Split(',')[0]), int.Parse(cleaned.Split(',')[1]))));
                                                    }
                                                    else
                                                    {
                                                        cleaned = Variables.Find(d => d.S_Name == cleaned).S_Value.Replace(" ", "");
                                                        if(cleaned.Split(',').Length >= 2)
                                                        {
                                                            if (cleaned.Split(',')[1].Length > 0)
                                                            {
                                                                Variables.Add(new Programming.Variables(values[0], item.GetValue(int.Parse(cleaned.Split(',')[0]) - 1, int.Parse(cleaned.Split(',')[1]) - 1)));
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (values[1].Contains(".GetActive"))
                                    {
                                        foreach (var item in Tables)
                                        {
                                            if (values[1].StartsWith(item.ID))
                                            {
                                                string cleaned = values[1].Remove(0, item.ID.Length + 1);
                                                if (cleaned.StartsWith("GetActive("))
                                                {
                                                    cleaned = cleaned.Replace("GetActive(", "");
                                                    cleaned = cleaned.Remove(cleaned.Length - 1);
                                                    foreach (var v in item.Cells)
                                                    {
                                                        if (v.Selected == true)
                                                        {
                                                            Variables.Add(new Programming.Variables(values[0], $"{v.X + 1},{v.Y + 1}"));
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (values[1].Contains("+"))
                                    {
                                        values[1] = BuildString.Build(Variables, values[1]);
                                        Variables.Add(new Programming.Variables(values[0], values[1]));
                                    }
                                }
                                catch(Exception e)
                                {

                                }
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
                    }
                    else if (line.StartsWith("int"))
                    {
                        string temp = line.Replace("int", "");
                        string[] values = temp.Split("=");
                        if (int.TryParse(values[1], out int s) == true)
                        {
                            name = values[0];
                            Variables.RemoveAll(d => d.I_Name == name);
                            Variables.Add(new Programming.Variables(values[0], s));
                        }
                        else if (values[1] == "DateTime.UtcNow.Second")
                        {
                            bool found = false;
                            foreach (var v in Variables)
                            {
                                if (v.I_Name == values[0])
                                {
                                    found = true;
                                    v.I_Value = DateTime.UtcNow.Second;
                                    break;
                                }
                            }
                            if (found == false)
                            {
                                Variables.Add(new Programming.Variables(values[0], DateTime.UtcNow.Second));
                            }
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
                        }
                        else if (!values[1].StartsWith("Console"))
                        {
                            bool gotit = false;
                            if (values[1].EndsWith(".X"))
                            {
                                values[1] = values[1].Replace(".X", "");
                                if(gotit == false)
                                {
                                    foreach(var v in Button)
                                    {
                                        if(v.ID == values[1])
                                        {
                                            Variables.Add(new Programming.Variables(values[0], v.X));
                                            gotit = true;
                                        }
                                    }
                                    foreach (var v in Slider)
                                    {
                                        if (v.ID == values[1])
                                        {
                                            Variables.Add(new Programming.Variables(values[0], v.X));
                                            gotit = true;
                                        }
                                    }
                                    foreach (var v in Label)
                                    {
                                        if (v.ID == values[1])
                                        {
                                            Variables.Add(new Programming.Variables(values[0], v.X));
                                            gotit = true;
                                        }
                                    }
                                }
                            }
                            else if (values[1].EndsWith(".Y"))
                            {
                                values[1] = values[1].Replace(".Y", "");
                                foreach (var v in Button)
                                {
                                    if (v.ID == values[1])
                                    {
                                        Variables.Add(new Programming.Variables(values[0], v.Y));
                                    }
                                }
                                foreach (var v in Slider)
                                {
                                    if (v.ID == values[1])
                                    {
                                        Variables.Add(new Programming.Variables(values[0], v.Y));
                                        gotit = true;
                                    }
                                }
                                foreach (var v in Label)
                                {
                                    if (v.ID == values[1])
                                    {
                                        Variables.Add(new Programming.Variables(values[0], v.Y));
                                        gotit = true;
                                    }
                                }
                            }
                            else if (values[1].EndsWith(".Width"))
                            {
                                values[1] = values[1].Replace(".Width", "");
                                foreach (var v in Button)
                                {
                                    if (v.ID == values[1])
                                    {
                                        Variables.Add(new Programming.Variables(values[0], v.Width));
                                    }
                                }
                                foreach (var v in Slider)
                                {
                                    if (v.ID == values[1])
                                    {
                                        Variables.Add(new Programming.Variables(values[0], v.Width));
                                        gotit = true;
                                    }
                                }
                            }
                            else if (values[1].EndsWith(".Height"))
                            {
                                values[1] = values[1].Replace(".Y", "");
                                foreach (var v in Button)
                                {
                                    if (v.ID == values[1])
                                    {
                                        Variables.Add(new Programming.Variables(values[0], v.Height));
                                    }
                                }
                            }
                            else if (values[1].EndsWith(".Value"))
                            {
                                values[1] = values[1].Replace(".Value", "");
                                foreach (var v in Slider)
                                {
                                    if (v.ID == values[1])
                                    {
                                        Variables.Add(new Programming.Variables(values[0], v.Value));
                                    }
                                }
                            }
                            else if (values[1].Contains("+") || values[1].Contains("-") || values[1].Contains("*") || values[1].Contains("/"))
                            {
                                try
                                {
                                    //TODO: If exists, replace value, if not, create var
                                    values[1] = BuildString.ReplaceVarsToValues(Variables, values[1]);
                                    if(Variables.Where(d => d.I_Name == values[0]).Count() != 0)
                                    {
                                        bool Found = false;
                                        for(int i = 0; i < Variables.Count() && Found == false; i++)
                                        {
                                            if (Variables[i].I_Name == values[0])
                                            {
                                                Variables[i].I_Value = (int)CalculatorA.Calculate(values[1]);
                                                Found = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Variables.Add(new Programming.Variables(values[0], ((int)CalculatorA.Calculate(values[1]))));
                                    }
                                }
                                catch(Exception e)
                                {
                                    
                                }
                            }
                        }
                        else
                        {
                            name = values[0];
                            Variables.RemoveAll(d => d.I_Name == name);
                            if (Returning_Value == null)
                            {
                                format = "int";
                                WaitForResponse = true;
                            }
                        }
                    }
                    else if (line.StartsWith("bool"))
                    {
                        string temp = line.Replace("bool", "");
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
                    else if (line.StartsWith("float"))
                    {
                        string temp = line.Replace("float", "");
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
                    else if (line.StartsWith("double"))
                    {
                        string temp = line.Replace("double", "");
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
                    else if (line.StartsWith("Point"))
                    {
                        string temp = line.Replace("Point", "");
                        string[] values = temp.Split("=");
                        string name = values[0];
                        string point = values[1].Replace("new(", "");
                        point = point.Remove(point.Length - 1);
                        string[] parts = point.Split(",");
                        Variables.Add(new Programming.Variables(name, new Point(int.Parse(parts[0]), int.Parse(parts[1]))));
                    }

                    //Variable operations
                    if (line.Contains("+="))
                    {
                        string[] Parts = line.Split("+=");
                        string varname = Parts[0];
                        bool changed = false;
                        for(int i = 0; i < Variables.Count && changed == false; i++)
                        {
                            if (Variables[i].S_Name == varname)
                            {
                                Variables[i].S_Value += Parts[1].Replace("\"", "");
                                changed = true;
                            }
                            else if (Variables[i].I_Name == varname)
                            {
                                if (int.TryParse(Parts[1], out int Parsed))
                                {
                                    Variables[i].I_Value += Parsed;
                                    changed = true;
                                }
                                else
                                {
                                    foreach (var c in Variables)
                                    {
                                        if (c.I_Name == Parts[1])
                                        {
                                            Variables[i].I_Value += c.I_Value;
                                            changed = true;
                                        }
                                    }
                                }
                            }
                            else if (Variables[i].P_Name + ".X" == varname)
                            {
                                Point p = new Point(Variables[i].P_Value.X, Variables[i].P_Value.Y);
                                if(int.TryParse(Parts[1], out int Parsed))
                                {
                                    p.X += Parsed;
                                }
                                Variables[i].P_Value = p;
                            }
                            else if (Variables[i].P_Name + ".Y" == varname)
                            {
                                Point p = new Point(Variables[i].P_Value.X, Variables[i].P_Value.Y);
                                if (int.TryParse(Parts[1], out int Parsed))
                                {
                                    p.Y += Parsed;
                                }
                                Variables[i].P_Value = p;
                            }
                        }

                    }
                    else if (line.Contains("-="))
                    {
                        string[] Parts = line.Split("-=");
                        foreach (var v in Variables)
                        {
                            if (v.I_Name == Parts[0])
                            {
                                if (int.TryParse(Parts[1], out int Parsed))
                                {
                                    v.I_Value -= Parsed;
                                }
                                else
                                {
                                    foreach (var c in Variables)
                                    {
                                        if (c.I_Name == Parts[1])
                                        {
                                            v.I_Value -= c.I_Value;
                                        }
                                    }
                                }
                            }
                            else if (v.P_Name + ".X" == Parts[0])
                            {
                                Point p = new Point(v.P_Value.X, v.P_Value.Y);
                                if (int.TryParse(Parts[1], out int Parsed))
                                {
                                    p.X -= Parsed;
                                }
                                v.P_Value = p;
                            }
                            else if (v.P_Name + ".Y" == Parts[0])
                            {
                                Point p = new Point(v.P_Value.X, v.P_Value.Y);
                                if (int.TryParse(Parts[1], out int Parsed))
                                {
                                    p.Y -= Parsed;
                                }
                                v.P_Value = p;
                            }
                        }
                    }
                    else if (line.Contains("="))
                    {
                        string[] Parts = line.Split("=");
                        foreach (var v in Variables)
                        {
                            if (v.I_Name == Parts[0])
                            {
                                if(int.TryParse(Parts[1], out int Parsed))
                                {
                                    v.I_Value = Parsed;
                                }
                                else
                                {
                                    foreach (var c in Variables)
                                    {
                                        if(c.I_Name == Parts[1])
                                        {
                                            v.I_Value = c.I_Value;
                                        }
                                        else if(c.P_Name == Parts[1])
                                        {
                                            if (Parts[0].Contains("X"))
                                            {
                                                v.I_Value = c.P_Value.X;
                                            }
                                            else
                                            {
                                                v.I_Value = c.P_Value.Y;
                                            }
                                        }
                                    }
                                }
                            }
                            else if(v.B_Name == Parts[0])
                            {
                                v.B_Value = bool.Parse(Parts[1]);
                            }
                            else if (v.P_Name + ".X" == Parts[0])
                            {
                                Point p = new Point(v.P_Value.X, v.P_Value.Y);
                                if (int.TryParse(Parts[1], out int Parsed))
                                {
                                    p.X = Parsed;
                                }
                                v.P_Value = p;
                            }
                            else if (v.P_Name + ".Y" == Parts[0])
                            {
                                Point p = new Point(v.P_Value.X, v.P_Value.Y);
                                if (int.TryParse(Parts[1], out int Parsed))
                                {
                                    p.Y = Parsed;
                                }
                                v.P_Value = p;
                            }
                        }
                    }

                    if (Returning_Value != null)
                    {
                        if(format == "string")
                        {
                            Variables.RemoveAll(d => d.S_Name == name);
                            Variables.Add(new Programming.Variables(name, Returning_Value));
                        }
                        else if(format == "int")
                        {
                            if(int.TryParse(Returning_Value, out int num))
                            {
                                Variables.RemoveAll(d => d.I_Name == name);
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
                            temp = temp.Remove(temp.Length - 1);
                            if (!temp.Contains("\""))
                            {
                                if (temp.Contains("+"))
                                {
                                    string[] container = temp.Split("+");
                                    foreach(string s in container)
                                    {
                                        foreach(var item in Variables)
                                        {
                                            if (item.S_Name == temp)
                                            {
                                                output += item.S_Value;
                                            }
                                            if (item.I_Name == temp)
                                            {
                                                output += item.I_Value.ToString();
                                            }
                                            if (item.B_Name == temp)
                                            {
                                                output += item.B_Value.ToString();
                                            }
                                            if (item.F_Name == temp)
                                            {
                                                output += item.F_Value.ToString();
                                            }
                                            if (item.D_Name == temp)
                                            {
                                                output += item.D_Value.ToString();
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
                                    output += temp.Replace("\"", "");
                                }
                            }
                            output += "\n";
                        }
                        else if (temp.StartsWith("Write("))
                        {
                            temp = temp.Replace("Write(", "");
                            temp = temp.Remove(temp.Length - 1);
                            if (!temp.Contains("\""))
                            {
                                if (temp.Contains("+"))
                                {
                                    string[] container = temp.Split("+");
                                    foreach (string s in container)
                                    {
                                        foreach (var item in Variables)
                                        {
                                            if (item.S_Name == temp)
                                            {
                                                output += item.S_Value;
                                            }
                                            if (item.I_Name == temp)
                                            {
                                                output += item.I_Value.ToString();
                                            }
                                            if (item.B_Name == temp)
                                            {
                                                output += item.B_Value.ToString();
                                            }
                                            if (item.F_Name == temp)
                                            {
                                                output += item.F_Value.ToString();
                                            }
                                            if (item.D_Name == temp)
                                            {
                                                output += item.D_Value.ToString();
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
                                            output += item.S_Value;
                                        }
                                        if (item.I_Name == temp)
                                        {
                                            output += item.I_Value.ToString();
                                        }
                                        if (item.B_Name == temp)
                                        {
                                            output += item.B_Value.ToString();
                                        }
                                        if (item.F_Name == temp)
                                        {
                                            output += item.F_Value.ToString();
                                        }
                                        if (item.D_Name == temp)
                                        {
                                            output += item.D_Value.ToString();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (temp.Contains("+"))
                                {
                                    foreach (string s in temp.Split('+'))
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
                        ConsoleKeyEx left = ConsoleKeyEx.NoName;
                        ConsoleKeyEx right = ConsoleKeyEx.E;
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
                                else if (Keys.StringToKey(sides[0]) != ConsoleKeyEx.NoName)
                                {
                                    left = Keys.StringToKey(sides[0]);
                                }
                                else
                                {
                                    if (bool.TryParse(sides[0], out bool s))
                                    {
                                        sides[0] = s.ToString();
                                    }
                                    else
                                    {
                                        bool Found = false;
                                        foreach (var Item in Variables)
                                        {
                                            if (sides[0] == Item.S_Name)
                                            {
                                                sides[0] = Item.S_Value;
                                                Found = true;
                                            }
                                            else if (sides[0] == Item.I_Name)
                                            {
                                                sides[0] = Item.I_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[0] == Item.B_Name)
                                            {
                                                sides[0] = Item.B_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[0] == Item.F_Name)
                                            {
                                                sides[0] = Item.F_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[0] == Item.D_Name)
                                            {
                                                sides[0] = Item.D_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[0] == Item.K_Name)
                                            {
                                                left = Item.K_Value;
                                                Found = true;
                                            }
                                        }
                                        if(Found == false)
                                        {
                                            foreach(var v in CheckBox)
                                            {
                                                if(v.ID == sides[0])
                                                {
                                                    sides[0] = v.Value.ToString();
                                                    Found = true;
                                                }
                                            }
                                        }
                                        if (Found == false)
                                        {
                                            foreach (var v in TextBox)
                                            {
                                                if (sides[0].Contains(v.ID))
                                                {
                                                    sides[0] = v.Text;
                                                }
                                            }
                                        }
                                    }
                                }
                                //Checking the right side
                                if (sides[1].Contains("\""))
                                {
                                    sides[1] = sides[1].Replace("\"", "");
                                }
                                else if (Keys.StringToKey(sides[1]) != ConsoleKeyEx.NoName)
                                {
                                    right = Keys.StringToKey(sides[1]);
                                }
                                else
                                {
                                    if (bool.TryParse(sides[1], out bool s))
                                    {
                                        sides[1] = s.ToString();
                                    }
                                    else
                                    {
                                        bool Found = false;
                                        foreach (var Item in Variables)
                                        {
                                            if (sides[1] == Item.S_Name)
                                            {
                                                sides[1] = Item.S_Value;
                                                Found = true;
                                            }
                                            else if (sides[1] == Item.I_Name)
                                            {
                                                sides[1] = Item.I_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[1] == Item.B_Name)
                                            {
                                                sides[1] = Item.B_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[1] == Item.F_Name)
                                            {
                                                sides[1] = Item.F_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[1] == Item.D_Name)
                                            {
                                                sides[1] = Item.D_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[1] == Item.K_Name)
                                            {
                                                left = Item.K_Value;
                                                Found = true;
                                            }
                                        }
                                        if (Found == false)
                                        {
                                            foreach (var v in CheckBox)
                                            {
                                                if (v.ID == sides[1])
                                                {
                                                    sides[1] = v.Value.ToString();
                                                }
                                            }
                                        }
                                        if (Found == false)
                                        {
                                            foreach (var v in TextBox)
                                            {
                                                if (sides[1].Contains(v.ID))
                                                {
                                                    sides[1] = v.Text;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (sides[0] == sides[1] || left == right)
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
                                    if (bool.TryParse(sides[0], out bool s))
                                    {
                                        sides[0] = s.ToString();
                                    }
                                    else
                                    {
                                        bool Found = false;
                                        foreach (var Item in Variables)
                                        {
                                            if (sides[0] == Item.S_Name)
                                            {
                                                sides[0] = Item.S_Value;
                                                Found = true;
                                            }
                                            else if (sides[0] == Item.I_Name)
                                            {
                                                sides[0] = Item.I_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[0] == Item.B_Name)
                                            {
                                                sides[0] = Item.B_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[0] == Item.F_Name)
                                            {
                                                sides[0] = Item.F_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[0] == Item.D_Name)
                                            {
                                                sides[0] = Item.D_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[0] == Item.K_Name)
                                            {
                                                left = Item.K_Value;
                                                Found = true;
                                            }
                                        }
                                        if (Found == false)
                                        {
                                            foreach (var v in CheckBox)
                                            {
                                                if (v.ID == sides[0])
                                                {
                                                    sides[0] = v.Value.ToString();
                                                    Found = true;
                                                }
                                            }
                                        }
                                        if (Found == false)
                                        {
                                            foreach (var v in TextBox)
                                            {
                                                if (sides[0].Contains(v.ID))
                                                {
                                                    sides[0] = v.Text;
                                                }
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
                                        bool Found = false;
                                        foreach (var Item in Variables)
                                        {
                                            if (sides[1] == Item.S_Name)
                                            {
                                                sides[1] = Item.S_Value;
                                                Found = true;
                                            }
                                            else if (sides[1] == Item.I_Name)
                                            {
                                                sides[1] = Item.I_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[1] == Item.B_Name)
                                            {
                                                sides[1] = Item.B_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[1] == Item.F_Name)
                                            {
                                                sides[1] = Item.F_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[1] == Item.D_Name)
                                            {
                                                sides[1] = Item.D_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[1] == Item.K_Name)
                                            {
                                                left = Item.K_Value;
                                                Found = true;
                                            }
                                        }
                                        if (Found == false)
                                        {
                                            foreach (var v in CheckBox)
                                            {
                                                if (v.ID == sides[1])
                                                {
                                                    sides[1] = v.Value.ToString();
                                                }
                                            }
                                        }
                                        if (Found == false)
                                        {
                                            foreach (var v in TextBox)
                                            {
                                                if (sides[1].Contains(v.ID))
                                                {
                                                    sides[1] = v.Text;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (int.TryParse(sides[0], out int l))
                                {
                                    if (int.TryParse(sides[1], out int r))
                                    {
                                        if (l > r)
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
                                    else
                                    {
                                        Count++;
                                        Checker = true;
                                        blank = false;
                                    }
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
                                    if (bool.TryParse(sides[0], out bool s))
                                    {
                                        sides[0] = s.ToString();
                                    }
                                    else
                                    {
                                        bool Found = false;
                                        foreach (var Item in Variables)
                                        {
                                            if (sides[0] == Item.S_Name)
                                            {
                                                sides[0] = Item.S_Value;
                                                Found = true;
                                            }
                                            else if (sides[0] == Item.I_Name)
                                            {
                                                sides[0] = Item.I_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[0] == Item.B_Name)
                                            {
                                                sides[0] = Item.B_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[0] == Item.F_Name)
                                            {
                                                sides[0] = Item.F_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[0] == Item.D_Name)
                                            {
                                                sides[0] = Item.D_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[0] == Item.K_Name)
                                            {
                                                left = Item.K_Value;
                                                Found = true;
                                            }
                                        }
                                        if (Found == false)
                                        {
                                            foreach (var v in CheckBox)
                                            {
                                                if (v.ID == sides[0])
                                                {
                                                    sides[0] = v.Value.ToString();
                                                    Found = true;
                                                }
                                            }
                                        }
                                        if (Found == false)
                                        {
                                            foreach (var v in TextBox)
                                            {
                                                if (sides[0].Contains(v.ID))
                                                {
                                                    sides[0] = v.Text;
                                                }
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
                                        bool Found = false;
                                        foreach (var Item in Variables)
                                        {
                                            if (sides[1] == Item.S_Name)
                                            {
                                                sides[1] = Item.S_Value;
                                                Found = true;
                                            }
                                            else if (sides[1] == Item.I_Name)
                                            {
                                                sides[1] = Item.I_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[1] == Item.B_Name)
                                            {
                                                sides[1] = Item.B_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[1] == Item.F_Name)
                                            {
                                                sides[1] = Item.F_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[1] == Item.D_Name)
                                            {
                                                sides[1] = Item.D_Value.ToString();
                                                Found = true;
                                            }
                                            else if (sides[1] == Item.K_Name)
                                            {
                                                left = Item.K_Value;
                                                Found = true;
                                            }
                                        }
                                        if (Found == false)
                                        {
                                            foreach (var v in CheckBox)
                                            {
                                                if (v.ID == sides[1])
                                                {
                                                    sides[1] = v.Value.ToString();
                                                }
                                            }
                                        }
                                        if (Found == false)
                                        {
                                            foreach (var v in TextBox)
                                            {
                                                if (sides[1].Contains(v.ID))
                                                {
                                                    sides[1] = v.Text;
                                                }
                                            }
                                        }
                                    }
                                }
                                if(int.TryParse(sides[0], out int l))
                                {
                                    if(int.TryParse(sides[1], out int r))
                                    {
                                        if (l < r)
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
                                    else
                                    {
                                        Count++;
                                        Checker = true;
                                        blank = false;
                                    }
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
                            Clipboard = e.Message;
                        }
                    }
                    else if (line.StartsWith("elseif"))
                    {
                        ConsoleKeyEx left = ConsoleKeyEx.NoName;
                        ConsoleKeyEx right = ConsoleKeyEx.E;
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
                                        if (sides[0] == Item.K_Name)
                                        {
                                            left = Item.K_Value;
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
                                        if (sides[1] == Item.K_Name)
                                        {
                                            right = Item.K_Value;
                                        }
                                    }
                                }
                                if (sides[0] == sides[1] || left == right)
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
                            WasElse = true;
                        }
                    }
                    #endregion Conditionals

                    #region Loops
                    if (line.StartsWith("for"))
                    {
                        looping = true;
                    }
                    if (line.StartsWith("while"))
                    {
                        string temp = line.Replace("while(", "");
                        temp = temp.Replace(")", "");

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
                                WhileLoop = true;
                                blank = true;
                            }
                            else
                            {
                                Count++;
                                Checker = true;
                                blank = false;
                                WhileLoop = false;
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
                    if (line.StartsWith("{") && WasIf == false)
                    {
                        if(looping == true)
                        {
                            Bracket++;
                        }
                        if (WhileLoop == true)
                        {
                            WhileBracket++;
                        }
                    }
                    else if (line.StartsWith("}") && WasIf == false)
                    {
                        if (looping == true && Bracket > 0)
                        {
                            Bracket--;
                        }
                        if (WhileLoop == true && WhileBracket > 0)
                        {
                            WhileBracket--;
                        }
                        WasElse = false;
                    }
                    if (Bracket == 0 && Cycles >= MaxCycle)
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
                        if (line.Contains("Copy"))
                        {
                            string temp = input.Replace("File.Copy(", "");
                            temp = temp.Remove(temp.Length - 2);
                            string[] args = temp.Split(",");

                            if (File.Exists(args[0].Replace("\"", "")))
                            {
                                try
                                {
                                    string s = File.ReadAllText(args[0].Replace("\"", ""));
                                    File.Create(args[1].Replace("\"", ""));
                                    File.WriteAllText(args[1].Replace("\"", ""), s);
                                }
                                catch (Exception e)
                                {
                                    Clipboard = e.Message;
                                }
                            }
                        }
                    }
                    #endregion System IO

                    #region Keyboard
                    if (line.StartsWith("ReadKey"))
                    {
                        string temp = line.Replace("ReadKey", "");
                        WaitForResponse = true;
                        KeyOnly = true;
                        if(key != ConsoleKeyEx.NoName)
                        {
                            Variables.Add(new Programming.Variables(temp, key));
                            key = ConsoleKeyEx.NoName;
                            WaitForResponse = false;
                            KeyOnly = false;
                        }
                    }
                    #endregion Keyboard

                    #region Graphical
                    if (line.StartsWith("Graphics."))
                    {
                        if (line.Contains("SetPixel"))
                        {
                            string cleaned = line.Replace("Graphics.SetPixel", "");
                            cleaned = cleaned.Remove(cleaned.Length - 1).Remove(0, 1);
                            string[] values = cleaned.Split(',');
                            window.RawData[window.Width * (int.Parse(values[1]) + 22) + int.Parse(values[0])] = ImprovedVBE.colourToNumber(int.Parse(values[2]), int.Parse(values[3]), int.Parse(values[4]));
                        }
                        else if (line.Contains("FilledRectangle"))
                        {
                            string cleaned = line.Replace("Graphics.FilledRectangle", "");
                            cleaned = cleaned.Remove(cleaned.Length - 1).Remove(0, 1);
                            string[] values = cleaned.Split(',');
                            ImprovedVBE.DrawFilledRectangle(window, ImprovedVBE.colourToNumber(int.Parse(values[4]), int.Parse(values[5]), int.Parse(values[6])), int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3]), false);
                        }
                        else if (line.Contains("FilledCircle"))
                        {
                            string cleaned = line.Replace("Graphics.FilledCircle", "");
                            cleaned = cleaned.Remove(cleaned.Length - 1).Remove(0, 1);
                            string[] values = cleaned.Split(',');
                            ImprovedVBE.DrawFilledEllipse(window, int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3]), ImprovedVBE.colourToNumber(int.Parse(values[4]), int.Parse(values[5]), int.Parse(values[6])));
                        }
                        else if (line.Contains("RGB"))
                        {
                            string cleaned = line.Replace("Graphics.RGB=", "");
                            string[] values = cleaned.Split(',');
                            CurrentColor = ImprovedVBE.colourToNumber(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]));
                        }
                    }
                    if (line.StartsWith("Process."))
                    {
                        if (line.Contains("Start"))
                        {
                            string cleaned = line.Replace("Process.Start(", "");
                            cleaned = cleaned.Remove(cleaned.Length - 1);
                            cleaned = cleaned.Replace("\"", "");

                            if (File.Exists(cleaned))
                            {
                                if (cleaned.EndsWith(".app"))
                                {
                                    //TaskScheduler.Apps.Add(new Window(100, 100, 999, 350, 200, 0, "Later", false, Resources.IDE, File.ReadAllText(cleaned)));
                                }
                                else if (cleaned.EndsWith(".cmd"))
                                {
                                    CSharp c = new CSharp();
                                    c.Executor(File.ReadAllText(cleaned));
                                }
                            }
                        }
                        else if (line.Contains("Terminate"))
                        {
                            string cleaned = line.Replace("Process.Terminate(", "");
                            cleaned = cleaned.Remove(cleaned.Length - 1);
                            Clipboard = "Terminate";
                        }
                    }
                    foreach(var item in Label)
                    {
                        string[] split = line.Split('=');
                        if (split[0].Split('.')[0] == item.ID)
                        {
                            if(split[0].Split('.')[1] == "Content")
                            {
                                if (!split[1].Contains("\""))
                                {
                                    foreach (var Item in Variables)
                                    {
                                        if (split[1] == Item.S_Name)
                                        {
                                            item.Text = Item.S_Value;
                                        }
                                        else if (split[1] == Item.I_Name)
                                        {
                                            item.Text = Item.I_Value.ToString();
                                        }
                                        else if (split[1] == Item.B_Name)
                                        {
                                            item.Text = Item.B_Value.ToString();
                                        }
                                        else if (split[1] == Item.F_Name)
                                        {
                                            item.Text = Item.F_Value.ToString();
                                        }
                                        else if (split[1] == Item.D_Name)
                                        {
                                            item.Text = Item.D_Value.ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    item.Text = split[1].Remove(split[1].Length - 1).Remove(0, 1);
                                }
                                NeedUpdate = true;
                            }
                            else if (split[0].Split('.')[1] == "Color")
                            {
                                string[] rgbValue = split[1].Split(',');
                                item.TextColor = ImprovedVBE.colourToNumber(int.Parse(rgbValue[0]), int.Parse(rgbValue[1]), int.Parse(rgbValue[2]));
                                NeedUpdate = true;
                            }
                            else if (split[0].Split('.')[1] == "X")
                            {
                                item.X = int.Parse(split[1]);
                                NeedUpdate = true;
                            }
                            else if (split[0].Split('.')[1] == "Y")
                            {
                                item.Y = int.Parse(split[1]);
                                NeedUpdate = true;
                            }
                        }
                    }
                    foreach (var item in Button)
                    {
                        string[] split = line.Split('=');
                        if (split[0].Split('.')[0] == item.ID)
                        {
                            if (split[0].Split('.')[1] == "Content")
                            {
                                item.Text = split[1].Remove(split[1].Length - 1).Remove(0, 1);
                            }
                            else if (split[0].Split('.')[1] == "Color")
                            {
                                string[] rgbValue = split[1].Split(',');
                                item.Color = ImprovedVBE.colourToNumber(int.Parse(rgbValue[0]), int.Parse(rgbValue[1]), int.Parse(rgbValue[2]));
                            }
                            else if (split[0].Split('.')[1] == "X")
                            {
                                item.X = int.Parse(split[1]);
                            }
                            else if (split[0].Split('.')[1] == "Y")
                            {
                                item.Y = int.Parse(split[1]);
                            }
                            else if (split[0].Split('.')[1] == "Width")
                            {
                                item.Width = int.Parse(split[1]);
                            }
                            else if (split[0].Split('.')[1] == "Height")
                            {
                                item.Height = int.Parse(split[1]);
                            }
                        }
                    }
                    foreach (var item in TextBox)
                    {
                        string[] split = line.Split('=');
                        if (split[0].Split('.')[0] == item.ID)
                        {
                            if (split[0].Split('.')[1] == "Content")
                            {
                                if (!split[1].Contains("\""))
                                {
                                    foreach (var Item in Variables)
                                    {
                                        if (split[1] == Item.S_Name)
                                        {
                                            item.Text = Item.S_Value;
                                        }
                                        else if (split[1] == Item.I_Name)
                                        {
                                            item.Text = Item.I_Value.ToString();
                                        }
                                        else if (split[1] == Item.B_Name)
                                        {
                                            item.Text = Item.B_Value.ToString();
                                        }
                                        else if (split[1] == Item.F_Name)
                                        {
                                            item.Text = Item.F_Value.ToString();
                                        }
                                        else if (split[1] == Item.D_Name)
                                        {
                                            item.Text = Item.D_Value.ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    item.Text = split[1].Remove(split[1].Length - 1).Remove(0, 1);
                                }
                            }
                            else if (split[0].Split('.')[1] == "X")
                            {
                                item.X = int.Parse(split[1]);
                            }
                            else if (split[0].Split('.')[1] == "Y")
                            {
                                item.Y = int.Parse(split[1]);
                            }
                        }
                    }
                    foreach (var item in Tables)
                    {
                        if (line.StartsWith(item.ID))
                        {
                            string cleaned = line.Remove(0, item.ID.Length + 1);
                            if (cleaned.StartsWith("SetValue("))
                            {
                                cleaned = cleaned.Replace("SetValue(", "");
                                cleaned = cleaned.Remove(cleaned.Length - 1);
                                if (cleaned.Split(',').Length == 3)
                                {
                                    item.SetValue(int.Parse(cleaned.Split(',')[1]) - 1, int.Parse(cleaned.Split(',')[0]) - 1, cleaned.Split(',')[2].Remove(cleaned.Split(',')[2].Length - 1).Remove(0, 1), false);
                                }
                                else
                                {
                                    string[] parts = cleaned.Split(",");
                                    parts[0] = Variables.Find(d => d.S_Name == parts[0]).S_Value.Replace(" ", "");
                                    if (parts[0].Split(',')[1].Length > 0)
                                    {
                                        item.SetValue(int.Parse(parts[0].Split(',')[1]) - 1, int.Parse(parts[0].Split(',')[0]) - 1, cleaned.Split(',')[1].Remove(cleaned.Split(',')[1].Length - 1).Remove(0, 1), false);
                                    }
                                }
                            }
                            else if (cleaned.StartsWith("ExtractToFile("))
                            {
                                cleaned = cleaned.Replace("ExtractToFile(", "");
                                if(cleaned.Length > 0)
                                {
                                    cleaned = cleaned.Remove(cleaned.Length - 1);
                                }
                                int X = 0;
                                int Y = 0;
                                string Content = "";//Cell data are separated by ';'
                                for (int Cells = 0; Cells < item.Width * item.Height; Cells++)
                                {
                                    Content += item.GetValue(X, Y) + ";";
                                    if (X < item.Width - 1)
                                    {
                                        X++;
                                    }
                                    else
                                    {
                                        Content += "\n";
                                        Y++;
                                        X = 0;
                                    }
                                }
                                if (cleaned.EndsWith(".spr\""))
                                {
                                    File.Create(cleaned.Replace("\"", ""));
                                    File.WriteAllText(cleaned.Replace("\"", ""), Content);
                                }
                            }
                            else if (cleaned.StartsWith("ReadFromFile("))//Not fully implemented
                            {
                                cleaned = cleaned.Replace("ReadFromFile(", "");
                                if (cleaned.Length > 0)
                                {
                                    cleaned = cleaned.Remove(cleaned.Length - 1);
                                }
                                int X = 0;
                                int Y = 0;
                                string[] cellLines = File.ReadAllText(cleaned.Replace("\"", "")).Split('\n');
                                string[] OneLine = cellLines[0].Split(';');
                                for (int Cells = 0; Cells < item.Width * item.Height; Cells++)
                                {
                                    OneLine = cellLines[Y].Split(';');
                                    item.SetValue(Y, X, OneLine[X], false);
                                    if (X < item.Width - 1)
                                    {
                                        X++;
                                    }
                                    else
                                    {
                                        Y++;
                                        X = 0;
                                    }
                                }
                            }
                        }
                    }
                    foreach (var item in Picturebox)
                    {
                        if (line.StartsWith(item.ID))
                        {
                            if (line.Contains(".MergeOnto("))
                            {
                                string temp = line.Replace(item.ID + ".MergeOnto(", "");
                                temp = temp.Remove(temp.Length - 1);
                                bool found = false;
                                for(int i = 0; i < Picturebox.Count && found == false; i++)
                                {
                                    if (Picturebox[i].ID == temp)
                                    {
                                        ImprovedVBE.DrawImageAlpha(item.image, 0, 0, Picturebox[i].image);
                                        found = true;
                                    }
                                }
                            }
                            else if (line.Contains(".SetPixel"))
                            {
                                string cleaned = line.Replace(item.ID + ".SetPixel(", "");
                                cleaned = cleaned.Remove(cleaned.Length - 1);
                                string[] values = cleaned.Split(',');
                                item.image.RawData[item.image.Width * (int.Parse(values[1])) + int.Parse(values[0])] = ImprovedVBE.colourToNumber(int.Parse(values[2]), int.Parse(values[3]), int.Parse(values[4]));
                            }
                            else if (line.Contains(".Clear"))
                            {
                                string cleaned = line.Replace(item.ID + ".Clear(", "");
                                cleaned = cleaned.Remove(cleaned.Length - 1);
                                string[] values = cleaned.Split(',');
                                Array.Fill(item.image.RawData, ImprovedVBE.colourToNumber(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2])));
                            }
                            else if (line.Contains(".FilledPollygon"))
                            {
                                string cleaned = line.Replace(item.ID + ".FilledPollygon(", "");
                                cleaned = cleaned.Remove(cleaned.Length - 1);
                                string[] values = cleaned.Split(',');
                                List<Point> points = new List<Point>();

                                foreach(string s in values)
                                {
                                    foreach(var v in Variables)
                                    {
                                        if(s == v.P_Name)
                                        {
                                            points.Add(v.P_Value);
                                        }
                                    }
                                }
                                if(points.Count != 0)
                                {
                                    ImprovedVBE.DrawFilledPollygon(item.image, points, ImprovedVBE.colourToNumber(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2])));
                                }
                            }
                            else if (line.Contains(".FilledRectangle"))
                            {
                                string cleaned = line.Replace(item.ID + ".FilledRectangle", "");
                                cleaned = cleaned.Remove(cleaned.Length - 1).Remove(0, 1);
                                string[] values = cleaned.Split(',');
                                ImprovedVBE.DrawFilledRectangle(item.image, ImprovedVBE.colourToNumber(int.Parse(values[4]), int.Parse(values[5]), int.Parse(values[6])), int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3]), false);
                            }
                            else if (line.Contains(".Line"))
                            {
                                string cleaned = line.Replace(item.ID + ".Line", "");
                                cleaned = cleaned.Remove(cleaned.Length - 1).Remove(0, 1);
                                string[] values = cleaned.Split(',');
                                ImprovedVBE.DrawLine(item.image, float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]), ImprovedVBE.colourToNumber(int.Parse(values[4]), int.Parse(values[5]), int.Parse(values[6])));
                            }
                            else if (line.Contains(".FilledCircle"))
                            {
                                string cleaned = line.Replace(item.ID + ".FilledCircle", "");
                                cleaned = cleaned.Remove(cleaned.Length - 1).Remove(0, 1);
                                string[] values = cleaned.Split(',');
                                if(int.TryParse(values[0], out int X) && int.TryParse(values[1], out int Y))
                                {
                                    ImprovedVBE.DrawFilledEllipse(item.image, X, Y, int.Parse(values[2]), int.Parse(values[3]), ImprovedVBE.colourToNumber(int.Parse(values[4]), int.Parse(values[5]), int.Parse(values[6])));
                                }
                                else
                                {
                                    int Xpos = 0;
                                    int Ypos = 0;
                                    foreach(var v in Variables)
                                    {
                                        if(v.P_Name == values[0])
                                        {
                                            Xpos = v.P_Value.X;
                                            Ypos = v.P_Value.Y;
                                            break;
                                        }
                                    }
                                    ImprovedVBE.DrawFilledEllipse(item.image, Xpos, Ypos, int.Parse(values[1]), int.Parse(values[2]), ImprovedVBE.colourToNumber(int.Parse(values[3]), int.Parse(values[4]), int.Parse(values[5])));
                                }
                            }
                        }
                    }
                    #endregion Graphical

                    #region Audio
                    //Requires PCSpeaker
                    if (line.StartsWith("PCSpeaker.Beep("))
                    {
                        string temp = line.Replace("PCSpeaker.Beep(", "");
                        temp = temp.Remove(temp.Length - 1);
                        string[] values = temp.Split(",");
                        PCSpeaker.Beep(uint.Parse(values[0]), uint.Parse(values[1]));
                    }
                    //Requires AC97
                    if (line.StartsWith("Audio"))
                    {
                        if (line.StartsWith("Audio.Play("))
                        {
                            string cleaned = line.Replace("Audio.Play(", "");
                            cleaned = cleaned.Replace(")", "").Replace("\"", "");
                            if (File.Exists(cleaned))
                            {
                                try
                                {
                                    var mixer = new AudioMixer();
                                    var audioStream = new MemoryAudioStream(new SampleFormat(AudioBitDepth.Bits16, 1, true), 48000, File.ReadAllBytes(cleaned));
                                    var driver = AC97.Initialize(4096);
                                    mixer.Streams.Add(audioStream);

                                    var audioManager = new AudioManager()
                                    {
                                        Stream = mixer,
                                        Output = driver
                                    };
                                    audioManager.Enable();
                                }
                                catch (Exception e)
                                {
                                    
                                }
                            }
                        }
                    }
                    #endregion Audio
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

        public string K_Name { get; set; }
        public ConsoleKeyEx K_Value { get; set; }

        public string P_Name { get; set; }
        public Point P_Value { get; set; }

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
        public Variables(string name, ConsoleKeyEx value)
        {
            this.K_Name = name;
            this.K_Value = value;
        }
        public Variables(string name, Point value)
        {
            this.P_Name = name;
            this.P_Value = value;
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
