using Cosmos.System;
using CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Console;
using CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.FileOperations;
using CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Math;
using CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Variables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrystalOSAlpha.Programming.CrystalSharp
{
    class Core
    {
        public int LineCounter = 2;
        public int VariableIndex = -99;
        public int BracketCounter = 0;
        public int EndOfElse = -99;
        public bool IsWaitingForReadLine = false;
        public bool isVariable = false;
        public bool AllowExecution = true;
        public bool SkipElse = false;
        public string Cached = "";
        public string Output = "";
        public string Operator = "";
        public Variable VariableName;
        public List<Variable> Variables = new List<Variable>();

        public string Execute(List<CodeSegments> CodeSegments, string ProjectName)
        {
            try
            {
                var Main = CodeSegments.Find(d => d.ClassName == ProjectName);
                var Temp = "";
                if (Cached == "")
                {
                    foreach (var Segment in Main.Segments)
                    {
                        if (Segment.Split("\n")[0].Contains("Main()"))
                        {
                            Cached = Segment;
                            break;
                        }
                    }
                }
                if (LineCounter >= Cached.Split('\n').Length)
                {
                    LineCounter = -1;
                }
                else
                {
                    if (AllowExecution)
                    {
                        Temp = MethodReturner(Cached.Split('\n')[LineCounter]);
                    }
                    else
                    {
                        BracketStepper(Cached.Split('\n')[LineCounter]);
                        if (BracketCounter == 0)
                        {
                            //AllowExecution = true;
                            if (Cached.Split('\n')[LineCounter].Contains("if") || Cached.Split('\n')[LineCounter].Contains("else if") || Cached.Split('\n')[LineCounter].Contains("else"))
                            {
                                Temp = MethodReturner(Cached.Split('\n')[LineCounter]);
                            }
                            if(EndOfElse > 0 && LineCounter == EndOfElse)
                            {
                                AllowExecution = true;
                                EndOfElse = -99;
                            }
                        }
                    }
                    if (!IsWaitingForReadLine)
                    {
                        LineCounter++;
                        Output = "";
                    }
                }
                return Temp;
            }
            catch (Exception ex)
            {
                LogError("Error in Execute method: " + ex.Message);
                return "Error: " + ex.Message;
            }
        }

        public string MethodReturner(string Line)
        {
            try
            {
                // IMPORTANT!!!! Always remember that semi-colons are present at the end of the line. They are not trimmed down.
                if (Line.Contains("string") || Line.Contains("int") || Line.Contains("bool") || Line.Contains("float") || Line.Contains("double") || Line.Contains("char"))
                {
                    Line = SettingVariableValues(Line.Trim());
                }
                else
                {
                    if (!Line.Contains("if") && !Line.Contains("else if") && !Line.Contains("else") && Line.Contains('='))
                    {
                        if (Line.Contains("+="))
                        {
                            var v = Variables.Find(d => d.ID == Line.Trim().Split(" += ")[0]);
                            if (v != null)
                            {
                                VariableName = v;
                                VariableIndex = Variables.IndexOf(v);
                                Line = Line.Split(" += ")[1];
                                Operator = "+=";
                                isVariable = true;
                            }
                        }
                        else if (Line.Contains("-="))
                        {
                            var v = Variables.Find(d => d.ID == Line.Trim().Split(" -= ")[0]);
                            if (v != null)
                            {
                                VariableName = v;
                                VariableIndex = Variables.IndexOf(v);
                                Line = Line.Split(" -= ")[1];
                                Operator = "-=";
                                isVariable = true;
                            }
                        }
                        else if (Line.Contains("*="))
                        {
                            var v = Variables.Find(d => d.ID == Line.Trim().Split(" *= ")[0]);
                            if (v != null)
                            {
                                VariableName = v;
                                VariableIndex = Variables.IndexOf(v);
                                Line = Line.Split(" *= ")[1];
                                Operator = "*=";
                                isVariable = true;
                            }
                        }
                        else if (Line.Contains("/="))
                        {
                            var v = Variables.Find(d => d.ID == Line.Trim().Split(" /= ")[0]);
                            if (v != null)
                            {
                                VariableName = v;
                                VariableIndex = Variables.IndexOf(v);
                                Line = Line.Split(" /= ")[1];
                                Operator = "/=";
                                isVariable = true;
                            }
                        }
                        else
                        {
                            var v = Variables.Find(d => d.ID == Line.Trim().Split(" = ")[0]);
                            if (v != null)
                            {
                                VariableName = v;
                                VariableIndex = Variables.IndexOf(v);
                                Line = Line.Split(" = ")[1];
                                isVariable = true;
                            }
                        }
                    }
                }

                string[] Parts = Line.Trim().Split('.');
                switch (Parts[0])
                {
                    case "Console":
                        switch (Parts[1].Split("(")[0])
                        {
                            case "WriteLine":
                                Output = WriteL.WriteLine(Line, Variables);
                                break;
                            case "Write":
                                Output = Write.CWrite(Line, Variables);
                                break;
                            case "ReadLine":
                                IsWaitingForReadLine = true;
                                if (KeyboardManager.TryReadKey(out KeyEvent key))
                                {
                                    if (key.Key == ConsoleKeyEx.Enter)
                                    {
                                        IsWaitingForReadLine = false;
                                        if (isVariable)
                                        {
                                            switch (Operator)
                                            {
                                                case "+=":
                                                    switch (VariableName.Type)
                                                    {
                                                        case VariableType.String:
                                                            Variables[VariableIndex].Value += Output;
                                                            break;
                                                        case VariableType.Int:
                                                            Variables[VariableIndex].IntValue += int.Parse(Output);
                                                            break;
                                                        case VariableType.Float:
                                                            Variables[VariableIndex].FloatValue += float.Parse(Output);
                                                            break;
                                                        case VariableType.Double:
                                                            Variables[VariableIndex].DoubleValue += double.Parse(Output);
                                                            break;
                                                        case VariableType.Char:
                                                            Variables[VariableIndex].CharValue += Output[0];
                                                            break;
                                                    }
                                                    isVariable = false;
                                                    VariableIndex = -99;
                                                    VariableName = null;
                                                    break;
                                                case "-=":
                                                    switch (VariableName.Type)
                                                    {
                                                        case VariableType.String:
                                                            throw new Exception("Cannot subtract a string.");
                                                        case VariableType.Int:
                                                            Variables[VariableIndex].IntValue -= int.Parse(Output);
                                                            break;
                                                        case VariableType.Float:
                                                            Variables[VariableIndex].FloatValue -= float.Parse(Output);
                                                            break;
                                                        case VariableType.Double:
                                                            Variables[VariableIndex].DoubleValue -= double.Parse(Output);
                                                            break;
                                                        case VariableType.Char:
                                                            Variables[VariableIndex].CharValue -= Output[0];
                                                            break;
                                                    }
                                                    isVariable = false;
                                                    VariableIndex = -99;
                                                    VariableName = null;
                                                    break;
                                                case "*=":
                                                    switch (VariableName.Type)
                                                    {
                                                        case VariableType.String:
                                                            string temp = "";
                                                            if(int.TryParse(Output, out int Parsed))
                                                            {
                                                                for (int i = 0; i < Parsed; i++)
                                                                {
                                                                    temp += Variables[VariableIndex].Value;
                                                                }
                                                            }
                                                            Variables[VariableIndex].Value = temp;
                                                            break;
                                                        case VariableType.Int:
                                                            Variables[VariableIndex].IntValue *= int.Parse(Output);
                                                            break;
                                                        case VariableType.Float:
                                                            Variables[VariableIndex].FloatValue *= float.Parse(Output);
                                                            break;
                                                        case VariableType.Double:
                                                            Variables[VariableIndex].DoubleValue *= double.Parse(Output);
                                                            break;
                                                        case VariableType.Char:
                                                            Variables[VariableIndex].CharValue *= Output[0];
                                                            break;
                                                    }
                                                    isVariable = false;
                                                    VariableIndex = -99;
                                                    VariableName = null;
                                                    break;
                                                case "/=":
                                                    switch (VariableName.Type)
                                                    {
                                                        case VariableType.String:
                                                            throw new Exception("Cannot divide a string.");
                                                        case VariableType.Int:
                                                            Variables[VariableIndex].IntValue /= int.Parse(Output);
                                                            break;
                                                        case VariableType.Float:
                                                            Variables[VariableIndex].FloatValue /= float.Parse(Output);
                                                            break;
                                                        case VariableType.Double:
                                                            Variables[VariableIndex].DoubleValue /= double.Parse(Output);
                                                            break;
                                                        case VariableType.Char:
                                                            Variables[VariableIndex].CharValue /= Output[0];
                                                            break;
                                                    }
                                                    isVariable = false;
                                                    VariableIndex = -99;
                                                    VariableName = null;
                                                    break;
                                                default:
                                                    switch (VariableName.Type)
                                                    {
                                                        case VariableType.String:
                                                            Variables[VariableIndex].Value = Output;
                                                            break;
                                                        case VariableType.Int:
                                                            Variables[VariableIndex].IntValue = int.Parse(Output);
                                                            break;
                                                        case VariableType.Bool:
                                                            bool.TryParse(Output, out bool ParsedBool);
                                                            Variables[VariableIndex].BoolValue = ParsedBool;
                                                            break;
                                                        case VariableType.Float:
                                                            Variables[VariableIndex].FloatValue = float.Parse(Output);
                                                            break;
                                                        case VariableType.Double:
                                                            Variables[VariableIndex].DoubleValue = double.Parse(Output);
                                                            break;
                                                        case VariableType.Char:
                                                            Variables[VariableIndex].CharValue = Output[0];
                                                            break;
                                                    }
                                                    isVariable = false;
                                                    VariableIndex = -99;
                                                    VariableName = null;
                                                    break;
                                            }
                                            Operator = "";
                                        }
                                    }
                                    else
                                    {
                                        ReadL.ReadLine(key, ref Output);
                                    }
                                }
                                break;
                            case "Clear":
                                return null;
                        }
                        break;
                    case "Random":
                        switch (Parts[1].Split("(")[0])
                        {
                            case "Next":
                                if (isVariable)
                                {
                                    switch (VariableName.Type)
                                    {
                                        case VariableType.String:
                                            Variables[VariableIndex].Value = RandomOperations.Next(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables).ToString();
                                            break;
                                        case VariableType.Int:
                                            Variables[VariableIndex].IntValue = RandomOperations.Next(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                        case VariableType.Bool:
                                            switch(RandomOperations.Next(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables))
                                            {
                                                case 0:
                                                    Variables[VariableIndex].BoolValue = false;
                                                    break;
                                                case 1:
                                                    Variables[VariableIndex].BoolValue = true;
                                                    break;
                                                default:
                                                    throw new Exception("Random.Next() returned a value that is not 0 or 1.");
                                            }
                                            break;
                                        case VariableType.Float:
                                            Variables[VariableIndex].FloatValue = RandomOperations.Next(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                        case VariableType.Double:
                                            Variables[VariableIndex].DoubleValue = RandomOperations.Next(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                        case VariableType.Char:
                                            Variables[VariableIndex].CharValue = (char)RandomOperations.Next(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                    }
                                    isVariable = false;
                                    VariableIndex = -99;
                                    VariableName = null;
                                }
                                break;
                        }
                        break;
                    case "Math":
                        switch (Parts[1].Split("(")[0])
                        {
                            case "Abs":
                                if (isVariable)
                                {
                                    switch (VariableName.Type)
                                    {
                                        case VariableType.String:
                                            Variables[VariableIndex].Value = MathOperations.Abs(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables).ToString();
                                            break;
                                        case VariableType.Int:
                                            Variables[VariableIndex].IntValue = (int)MathOperations.Abs(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                        case VariableType.Bool:
                                            throw new Exception("Cannot assign an number to a boolean.");
                                        case VariableType.Float:
                                            Variables[VariableIndex].FloatValue = (float)MathOperations.Abs(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                        case VariableType.Double:
                                            Variables[VariableIndex].DoubleValue = MathOperations.Abs(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                    }
                                }
                                break;
                            case "Sqrt":
                                if (isVariable)
                                {
                                    switch (VariableName.Type)
                                    {
                                        case VariableType.String:
                                            Variables[VariableIndex].Value = MathOperations.Sqrt(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables).ToString();
                                            break;
                                        case VariableType.Int:
                                            Variables[VariableIndex].IntValue = (int)MathOperations.Sqrt(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                        case VariableType.Bool:
                                            throw new Exception("Cannot assign an number to a boolean.");
                                        case VariableType.Float:
                                            Variables[VariableIndex].FloatValue = (float)MathOperations.Sqrt(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                        case VariableType.Double:
                                            Variables[VariableIndex].DoubleValue = MathOperations.Sqrt(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                    }
                                }
                                break;
                            case "Pow":
                                if (isVariable)
                                {
                                    switch (VariableName.Type)
                                    {
                                        case VariableType.String:
                                            Variables[VariableIndex].Value = MathOperations.Pow(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables).ToString();
                                            break;
                                        case VariableType.Int:
                                            Variables[VariableIndex].IntValue = (int)MathOperations.Pow(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                        case VariableType.Bool:
                                            throw new Exception("Cannot assign an number to a boolean.");
                                        case VariableType.Float:
                                            Variables[VariableIndex].FloatValue = (float)MathOperations.Pow(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                        case VariableType.Double:
                                            Variables[VariableIndex].DoubleValue = MathOperations.Pow(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                    }
                                }
                                break;
                            case "Sin":
                                if (isVariable)
                                {
                                    switch (VariableName.Type)
                                    {
                                        case VariableType.String:
                                            Variables[VariableIndex].Value = MathOperations.Sin(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables).ToString();
                                            break;
                                        case VariableType.Int:
                                            Variables[VariableIndex].IntValue = (int)MathOperations.Sin(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                        case VariableType.Bool:
                                            throw new Exception("Cannot assign an number to a boolean.");
                                        case VariableType.Float:
                                            Variables[VariableIndex].FloatValue = (float)MathOperations.Sin(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                        case VariableType.Double:
                                            Variables[VariableIndex].DoubleValue = MathOperations.Sin(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                    }
                                }
                                break;
                            case "Cos":
                                if (isVariable)
                                {
                                    switch (VariableName.Type)
                                    {
                                        case VariableType.String:
                                            Variables[VariableIndex].Value = MathOperations.Cos(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables).ToString();
                                            break;
                                        case VariableType.Int:
                                            Variables[VariableIndex].IntValue = (int)MathOperations.Cos(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                        case VariableType.Bool:
                                            throw new Exception("Cannot assign an number to a boolean.");
                                        case VariableType.Float:
                                            Variables[VariableIndex].FloatValue = (float)MathOperations.Cos(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                        case VariableType.Double:
                                            Variables[VariableIndex].DoubleValue = MathOperations.Cos(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                    }
                                }
                                break;
                            case "Ceil":
                                if (isVariable)
                                {
                                    switch (VariableName.Type)
                                    {
                                        case VariableType.String:
                                            Variables[VariableIndex].Value = MathOperations.Ceil(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables).ToString();
                                            break;
                                        case VariableType.Int:
                                            Variables[VariableIndex].IntValue = (int)MathOperations.Ceil(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                        case VariableType.Bool:
                                            throw new Exception("Cannot assign an number to a boolean.");
                                        case VariableType.Float:
                                            Variables[VariableIndex].FloatValue = (float)MathOperations.Ceil(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                        case VariableType.Double:
                                            Variables[VariableIndex].DoubleValue = MathOperations.Ceil(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                    }
                                }
                                break;
                            case "Floor":
                                if (isVariable)
                                {
                                    switch (VariableName.Type)
                                    {
                                        case VariableType.String:
                                            Variables[VariableIndex].Value = MathOperations.Floor(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables).ToString();
                                            break;
                                        case VariableType.Int:
                                            Variables[VariableIndex].IntValue = (int)MathOperations.Floor(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                        case VariableType.Bool:
                                            throw new Exception("Cannot assign an number to a boolean.");
                                        case VariableType.Float:
                                            Variables[VariableIndex].FloatValue = (float)MathOperations.Floor(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                        case VariableType.Double:
                                            Variables[VariableIndex].DoubleValue = MathOperations.Floor(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                    }
                                }
                                break;
                            case "Round":
                                if (isVariable)
                                {
                                    switch (VariableName.Type)
                                    {
                                        case VariableType.String:
                                            Variables[VariableIndex].Value = MathOperations.Round(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables).ToString();
                                            break;
                                        case VariableType.Int:
                                            Variables[VariableIndex].IntValue = (int)MathOperations.Round(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                        case VariableType.Bool:
                                            throw new Exception("Cannot assign an number to a boolean.");
                                        case VariableType.Float:
                                            Variables[VariableIndex].FloatValue = (float)MathOperations.Round(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                        case VariableType.Double:
                                            Variables[VariableIndex].DoubleValue = MathOperations.Round(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                    }
                                }
                                break;
                            case "Calculate":
                                if (isVariable)
                                {
                                    switch (VariableName.Type)
                                    {
                                        case VariableType.String:
                                            Variables[VariableIndex].Value = MathOperations.Calculate(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables).ToString();
                                            break;
                                        case VariableType.Int:
                                            Variables[VariableIndex].IntValue = (int)MathOperations.Calculate(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                        case VariableType.Bool:
                                            throw new Exception("Cannot assign an number to a boolean.");
                                        case VariableType.Float:
                                            Variables[VariableIndex].FloatValue = (float)MathOperations.Calculate(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                        case VariableType.Double:
                                            Variables[VariableIndex].DoubleValue = MathOperations.Calculate(Parts[1].Split("(")[1].Remove(Parts[1].Split("(")[1].Length - 2), Variables);
                                            break;
                                    }
                                }
                                break;
                        }
                        break;
                    case "File":
                        switch (Parts[1].Split("(")[0])
                        {
                            case "ReadAllText":
                                //TODO: Do it like it's in console class
                                if (isVariable)
                                {
                                    switch (VariableName.Type)
                                    {
                                        case VariableType.String:
                                            Variables[VariableIndex].Value = FileOperations.ReadAllText(Line.Trim().Remove(Line.Trim().Length - 2).Remove(0, "File.ReadAllText(".Length + 1));
                                            break;
                                        case VariableType.Int:
                                            throw new Exception("Cannot assign a string to an integer.");
                                        case VariableType.Bool:
                                            throw new Exception("Cannot assign a string to a boolean.");
                                        case VariableType.Float:
                                            throw new Exception("Cannot assign a string to a float.");
                                        case VariableType.Double:
                                            throw new Exception("Cannot assign a string to a double.");
                                        case VariableType.Char:
                                            throw new Exception("Cannot assign a string to a char.");
                                    }
                                    isVariable = false;
                                    VariableIndex = -99;
                                    VariableName = null;
                                }
                                break;
                            case "WriteAllText":
                                FileOperations.WriteAllText(Line.Trim().Remove(Line.Trim().Length - 2).Remove(0, "File.ReadAllText(".Length + 1), Variables);
                                break;
                        }
                        break;
                    default:
                        if (isVariable)
                        {
                            switch (Operator)
                            {
                                case "+=":
                                    Line = Line.Remove(Line.Length - 1);
                                    switch (VariableName.Type)
                                    {
                                        case VariableType.String:
                                            Variables[VariableIndex].Value += Line.Remove(Line.Length - 1).Remove(0, 1);
                                            break;
                                        case VariableType.Int:
                                            Variables[VariableIndex].IntValue += int.Parse(Line);
                                            break;
                                        case VariableType.Bool:
                                            throw new Exception("Cannot add to a boolean.");
                                        case VariableType.Float:
                                            Variables[VariableIndex].FloatValue += float.Parse(Line.Remove(Line.Length - 1).Remove(0, Line.IndexOf('.') + 1));
                                            break;
                                        case VariableType.Double:
                                            Variables[VariableIndex].DoubleValue += double.Parse(Line.Remove(Line.Length - 1).Remove(0, Line.IndexOf('.') + 1));
                                            break;
                                        case VariableType.Char:
                                            Variables[VariableIndex].CharValue += Line.Remove(Line.Length - 1).Remove(0, 1)[0];
                                            break;
                                    }
                                    isVariable = false;
                                    VariableIndex = -99;
                                    VariableName = null;
                                    break;
                                case "-=":
                                    Line = Line.Remove(Line.Length - 1);
                                    switch (VariableName.Type)
                                    {
                                        case VariableType.String:
                                            throw new Exception("Cannot subtract a string.");
                                        case VariableType.Int:
                                            Variables[VariableIndex].IntValue -= int.Parse(Line);
                                            break;
                                        case VariableType.Bool:
                                            throw new Exception("Cannot subtract a boolean.");
                                        case VariableType.Float:
                                            Variables[VariableIndex].FloatValue -= float.Parse(Line.Remove(Line.Length - 1).Remove(0, Line.IndexOf('.') + 1));
                                            break;
                                        case VariableType.Double:
                                            Variables[VariableIndex].DoubleValue -= double.Parse(Line.Remove(Line.Length - 1).Remove(0, Line.IndexOf('.') + 1));
                                            break;
                                        case VariableType.Char:
                                            Variables[VariableIndex].CharValue -= Line.Remove(Line.Length - 1).Remove(0, 1)[0];
                                            break;
                                    }
                                    isVariable = false;
                                    VariableIndex = -99;
                                    VariableName = null;
                                    break;
                                case "*=":
                                    Line = Line.Remove(Line.Length - 1);
                                    switch (VariableName.Type)
                                    {
                                        case VariableType.String:
                                            string temp = "";
                                            if (int.TryParse(Line, out int Parsed))
                                            {
                                                for (int i = 0; i < Parsed; i++)
                                                {
                                                    temp += Variables[VariableIndex].Value;
                                                }
                                            }
                                            Variables[VariableIndex].Value = temp;
                                            break;
                                        case VariableType.Int:
                                            Variables[VariableIndex].IntValue *= int.Parse(Line);
                                            break;
                                        case VariableType.Float:
                                            Variables[VariableIndex].FloatValue *= float.Parse(Line);
                                            break;
                                        case VariableType.Double:
                                            Variables[VariableIndex].DoubleValue *= double.Parse(Line);
                                            break;
                                        case VariableType.Char:
                                            Variables[VariableIndex].CharValue *= Line[0];
                                            break;
                                    }
                                    isVariable = false;
                                    VariableIndex = -99;
                                    VariableName = null;
                                    break;
                                case "/=":
                                    Line = Line.Remove(Line.Length - 1);
                                    switch (VariableName.Type)
                                    {
                                        case VariableType.String:
                                            throw new Exception("Cannot divide a string.");
                                        case VariableType.Int:
                                            Variables[VariableIndex].IntValue /= int.Parse(Line);
                                            break;
                                        case VariableType.Float:
                                            Variables[VariableIndex].FloatValue /= float.Parse(Line);
                                            break;
                                        case VariableType.Double:
                                            Variables[VariableIndex].DoubleValue /= double.Parse(Line);
                                            break;
                                        case VariableType.Char:
                                            Variables[VariableIndex].CharValue /= Line[0];
                                            break;
                                    }
                                    isVariable = false;
                                    VariableIndex = -99;
                                    VariableName = null;
                                    break;
                                default:
                                    Line = Line.Remove(Line.Length - 1);
                                    switch (VariableName.Type)
                                    {
                                        case VariableType.String:
                                            Variables[VariableIndex].Value = Line.Remove(Line.Length - 1).Remove(0, 1);
                                            break;
                                        case VariableType.Int:
                                            Variables[VariableIndex].IntValue = int.Parse(Line);
                                            break;
                                        case VariableType.Bool:
                                            if (bool.TryParse(Line, out bool ParsedBool))
                                            {
                                                Variables[VariableIndex].BoolValue = ParsedBool;
                                            }
                                            else
                                            {
                                                Output += "\n" + Line;
                                            }
                                            break;
                                        case VariableType.Float:
                                            Variables[VariableIndex].FloatValue = float.Parse(Line.Remove(Line.Length - 1).Remove(0, Line.IndexOf('.') + 1));
                                            break;
                                        case VariableType.Double:
                                            Variables[VariableIndex].DoubleValue = double.Parse(Line.Remove(Line.Length - 1).Remove(0, Line.IndexOf('.') + 1));
                                            break;
                                        case VariableType.Char:
                                            Variables[VariableIndex].CharValue = Line.Remove(Line.Length - 1).Remove(0, 1)[0];
                                            break;
                                    }
                                    isVariable = false;
                                    VariableIndex = -99;
                                    VariableName = null;
                                    Operator = "";
                                    break;
                            }
                        }
                        else
                        {
                            if (Line.Contains("else if"))//Traceback problem. Symptom: skips it completely, and not gets anything from the inside of this if block.
                            {
                                if (!AllowExecution && EvaluateCondition(Line.Trim().Substring(8, Line.Trim().Length - 9)))
                                {
                                    AllowExecution = true; // If else if-condition is true and previous if-condition was false, allow execution
                                }
                                else
                                {
                                    AllowExecution = false; // If else if-condition is false or previous if-condition was true, disallow execution
                                }
                            }
                            else if (Line.Contains("if"))
                            {
                                AllowExecution = true;
                                if (!EvaluateCondition(Line.Trim().Substring(3, Line.Trim().Length - 4)))
                                {
                                    AllowExecution = false; // If if-condition is false, disallow execution until else or next if/else if
                                }
                            }
                            else if (Line.Contains("else"))
                            {
                                if (!AllowExecution)
                                {
                                    AllowExecution = true; // If all preceding conditions were false, allow execution of else block
                                }
                                else
                                {
                                    AllowExecution = false; // If preceding conditions were true, disallow execution of else block
                                }
                                int Opening = 0;
                                string[] lines = Cached.Split('\n');
                                for (int i = LineCounter + 1; i < lines.Length; i++)
                                {
                                    if (lines[i].Contains("{"))
                                    {
                                        Opening++;
                                    }
                                    else if (lines[i].Contains("}"))
                                    {
                                        Opening--;
                                    }
                                    if(Opening == 0)
                                    {
                                        EndOfElse = i;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                foreach (var variable in Variables)
                                {
                                    if (variable.ID == Parts[0])
                                    {
                                        switch (variable.Type)
                                        {
                                            case VariableType.String:
                                                variable.Value = Parts[1].Remove(Parts[1].Length - 2).Remove(0, 1);
                                                break;
                                            case VariableType.Int:
                                                variable.IntValue = int.Parse(Parts[1].Remove(Parts[1].Length - 1));
                                                break;
                                            case VariableType.Bool:
                                                bool.TryParse(Parts[1].Remove(Parts[1].Length - 1), out bool ParsedBool);
                                                variable.BoolValue = ParsedBool;
                                                break;
                                            case VariableType.Float:
                                                variable.FloatValue = float.Parse(Line.Remove(Line.Length - 1).Remove(0, Line.IndexOf('.') + 1));
                                                break;
                                            case VariableType.Double:
                                                variable.DoubleValue = double.Parse(Line.Remove(Line.Length - 1).Remove(0, Line.IndexOf('.') + 1));
                                                break;
                                            case VariableType.Char:
                                                variable.CharValue = Line.Remove(Line.Length - 1).Remove(0, 1)[2];
                                                break;
                                        }
                                        isVariable = false;
                                        VariableIndex = -99;
                                        VariableName = null;
                                        Operator = "";
                                    }
                                }
                            }
                        }
                        break;
                }
                return Output;
            }
            catch (Exception ex)
            {
                LogError("Error in MethodReturner: " + ex.Message);
                return "Error: " + ex.Message;
            }
        }


        public void BracketStepper(string Input)
        {
            try
            {
                if (Input.Contains("{"))
                {
                    BracketCounter++;
                }
                if (Input.Contains("}"))
                {
                    BracketCounter--;
                }
            }
            catch (Exception ex)
            {
                LogError("Error in BracketStepper: " + ex.Message);
            }
        }

        public bool EvaluateCondition(string condition)
        {
            try
            {
                var tokens = TokenizeCondition(condition);
                return EvaluateTokens(tokens);
            }
            catch (Exception ex)
            {
                LogError("Error in EvaluateCondition: " + ex.Message);
                return false;
            }
        }

        private List<string> TokenizeCondition(string condition)
        {
            LogError("Tokenizing condition: " + condition);
            try
            {
                var tokens = new List<string>();
                var operators = new[] { "&&", "||", "==", "!=", "<=", ">=", "<", ">", "(", ")" };
                int i = 0;
                while (i < condition.Length)
                {
                    if (char.IsWhiteSpace(condition[i]))
                    {
                        i++;
                    }
                    else if (i < condition.Length - 1 && operators.Contains(condition.Substring(i, 2)))
                    {
                        tokens.Add(condition.Substring(i, 2));
                        i += 2;
                    }
                    else if (operators.Contains(condition[i].ToString()))
                    {
                        tokens.Add(condition[i].ToString());
                        i++;
                    }
                    else
                    {
                        var token = "";
                        while (i < condition.Length && !char.IsWhiteSpace(condition[i]) && !operators.Contains(condition[i].ToString()))
                        {
                            token += condition[i];
                            i++;
                        }
                        tokens.Add(token);
                    }
                }
                return tokens;
            }
            catch (Exception ex)
            {
                LogError("Error in TokenizeCondition: " + ex.Message);
                return new List<string>();
            }
        }

        private bool EvaluateTokens(List<string> tokens)
        {
            var values = new Stack<bool>();
            var operators = new Stack<string>();
            int i = 0;
            try
            {

                LogError("Starting EvaluateTokens");

                while (i < tokens.Count)
                {
                    var token = tokens[i];
                    LogError($"Token[{i}]: {token}");

                    if (bool.TryParse(token, out bool boolValue))
                    {
                        values.Push(boolValue);
                        LogError($"Pushed boolean value: {boolValue}");
                    }
                    else if (token == "(")
                    {
                        operators.Push(token);
                        LogError($"Pushed operator: {token}");
                    }
                    else if (token == ")")
                    {
                        while (operators.Peek() != "(")
                        {
                            values.Push(EvaluateOperator(operators.Pop(), values.Pop(), values.Pop()));
                        }
                        operators.Pop(); // Remove the "("
                        LogError($"Popped operator: (");
                    }
                    else if (token == "&&" || token == "||")
                    {
                        while (operators.Count > 0 && HasPrecedence(token, operators.Peek()))
                        {
                            values.Push(EvaluateOperator(operators.Pop(), values.Pop(), values.Pop()));
                        }
                        operators.Push(token);
                        LogError($"Pushed operator: {token}");
                    }
                    else
                    {
                        if (i + 2 < tokens.Count)
                        {
                            values.Push(EvaluateComparison(token, tokens[i + 1], tokens[i + 2]));
                            LogError($"Evaluated comparison: {token} {tokens[i + 1]} {tokens[i + 2]}");
                            i += 2;
                        }
                    }
                    i++;
                }

                while (operators.Count > 0)
                {
                    values.Push(EvaluateOperator(operators.Pop(), values.Pop(), values.Pop()));
                }

                LogError("EvaluateTokens completed successfully");

                return values.Pop();
            }
            catch (Exception ex)
            {
                LogError("Error in EvaluateTokens: " + ex.Message);
                var v = Variables.Find(d => d.ID == tokens[i - 1]);
                if(v != null)
                {
                    return v.BoolValue;
                }
                return false;
            }
        }

        private bool EvaluateComparison(string left, string op, string right)
        {
            try
            {
                LogError($"Evaluating comparison: {left} {op} {right}");
                var leftValue = GetVariableValue(left);
                var rightValue = GetVariableValue(right);
                LogError($"Evaluated values: {leftValue} {op} {rightValue}");

                switch (op)
                {
                    case "==":
                        return leftValue == rightValue;
                    case "!=":
                        return leftValue != rightValue;
                    case "<=":
                        return int.Parse(leftValue) <= int.Parse(rightValue);
                    case ">=":
                        return int.Parse(leftValue) >= int.Parse(rightValue);
                    case "<":
                        return int.Parse(leftValue) < int.Parse(rightValue);
                    case ">":
                        return int.Parse(leftValue) > int.Parse(rightValue);
                    default:
                        LogError($"Unknown operator in comparison: {op}");
                        return false;
                }
            }
            catch (Exception ex)
            {
                LogError($"Error in EvaluateComparison: {ex.Message}");
                return false;
            }
        }

        private bool EvaluateOperator(string op, bool right, bool left)
        {
            try
            {
                return op switch
                {
                    "&&" => left && right,
                    "||" => left || right,
                    _ => false,
                };
            }
            catch (Exception ex)
            {
                LogError("Error in EvaluateOperator: " + ex.Message);
                return false;
            }
        }

        private bool HasPrecedence(string op1, string op2)
        {
            if (op2 == "(" || op2 == ")")
            {
                return false;
            }
            if ((op1 == "&&" || op1 == "||") && (op2 == "&&" || op2 == "||"))
            {
                return true;
            }
            return false;
        }

        private string GetVariableValue(string token)
        {
            var variable = Variables.FirstOrDefault(v => v.ID == token);
            if (variable != null)
            {
                switch (variable.Type)
                {
                    case VariableType.String:
                        return variable.Value;
                    case VariableType.Int:
                        return variable.IntValue.ToString();
                    case VariableType.Bool:
                        return variable.BoolValue.ToString();
                    case VariableType.Float:
                        return variable.FloatValue.ToString();
                    case VariableType.Double:
                        return variable.DoubleValue.ToString();
                    case VariableType.Char:
                        return variable.CharValue.ToString();
                    default:
                        LogError($"Unknown variable type for: {token}");
                        return token;
                }
            }
            LogError($"Variable not found: {token}");
            return token;
        }

        public string SettingVariableValues(string Input)
        {
            try
            {
                string ReturningValue = Input;
                string[] Parts = ReturningValue.Split('=');
                switch (Parts[0].Split(' ')[0])
                {
                    case "string":
                        Variables.Add(new Variable(Parts[0].Split(' ')[1].Trim(), "", VariableType.String));
                        return ReturningValue.Replace("string ", "").Replace(" = ", ".");
                    case "int":
                        Variables.Add(new Variable(Parts[0].Split(' ')[1].Trim(), 0, VariableType.Int));
                        return ReturningValue.Replace("int ", "").Replace(" = ", ".");
                    case "bool":
                        Variables.Add(new Variable(Parts[0].Split(' ')[1].Trim(), false, VariableType.Bool));
                        return ReturningValue.Replace("bool ", "").Replace(" = ", ".");
                    case "float":
                        Variables.Add(new Variable(Parts[0].Split(' ')[1].Trim(), 0.0f, VariableType.Float));
                        return ReturningValue.Replace("float ", "").Replace(" = ", ".");
                    case "double":
                        Variables.Add(new Variable(Parts[0].Split(' ')[1].Trim(), 0.0, VariableType.Double));
                        return ReturningValue.Replace("double ", "").Replace(" = ", ".");
                    case "char":
                        Variables.Add(new Variable(Parts[0].Split(' ')[1].Trim(), ' ', VariableType.Char));
                        return ReturningValue.Replace("char ", "").Replace(" = ", ".");
                }
                return ReturningValue;
            }
            catch (Exception ex)
            {
                LogError("Error in SettingVariableValues: " + ex.Message);
                return "Error: " + ex.Message;
            }
        }

        private void LogError(string message)
        {
            // Implement your logging mechanism here.
            // For now, we'll just output the message to the console.
            //Output += "\n" + message;
        }
    }
}