using Cosmos.System;
using CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Console;
using CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Variables;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace CrystalOSAlpha.Programming.CrystalSharp
{
    class Core
    {
        public int LineCounter = 2;
        public bool IsWaitingForReadLine = false;
        public string Cached = "";
        public string Output = "";
        public string ActualLine = "";
        public List<Variable> Variables = new List<Variable>();
        public string Execute(List<CodeSegments> CodeSegments, string ProjectName)
        {
            var Main = CodeSegments.Find(d => d.ClassName == ProjectName);
            var Temp = "";
            if(Cached == "")
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
            if(LineCounter == Cached.Split('\n').Length)
            {
                LineCounter = -1;
            }
            else
            {
                Temp = MethodReturner(Cached.Split('\n')[LineCounter]);
                if (!IsWaitingForReadLine)
                {
                    LineCounter++;
                    Output = "";
                }
            }
            return Temp;
        }
        public string MethodReturner(string Line)
        {
            if (Line.Contains("string") || Line.Contains("int") || Line.Contains("bool") || Line.Contains("float") || Line.Contains("double") || Line.Contains("char"))
            {
                Line = SettingVariableValues(Line.Trim());
            }
            string[] Parts = Line.Trim().Split('.');
            switch (Parts[0])
            {
                case "Console":
                    switch (Parts[1].Split("(")[0])
                    {
                        case "WriteLine":
                            Output = WriteL.WriteLine(Line);
                            break;
                        case "Write":
                            Output = Write.CWrite(Line);
                            break;
                        case "ReadLine":
                            IsWaitingForReadLine = true;
                            if(KeyboardManager.TryReadKey(out KeyEvent key))
                            {
                                if(key.Key == ConsoleKeyEx.Enter)
                                {
                                    IsWaitingForReadLine = false;
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
                case "Math":
                    switch (Parts[1].Split("(")[0])
                    {
                        case "Calculate":
                            
                            break;
                    }
                    break;
                default:
                    foreach(var variable in Variables)
                    {
                        if(variable.ID == Parts[0])
                        {
                            Output = Parts[0];
                            switch (variable.Type)
                            {
                                case VariableType.String:
                                    variable.Value = Parts[1];
                                    Output = variable.Value;
                                    break;
                                case VariableType.Int:
                                    Output = variable.IntValue.ToString();
                                    break;
                                case VariableType.Bool:
                                    Output = variable.BoolValue.ToString();
                                    break;
                                case VariableType.Float:
                                    Output = variable.FloatValue.ToString();
                                    break;
                                case VariableType.Double:
                                    Output = variable.DoubleValue.ToString();
                                    break;
                                case VariableType.Char:
                                    Output = variable.CharValue.ToString();
                                    break;
                            }
                        }
                    }
                    break;
            }
            return Output;
        }
        public string SettingVariableValues(string Input)
        {
            string ReturningValue = Input;
            string[] Parts = ReturningValue.Split('=');
            switch (Parts[0].Split(' ')[0])
            {
                case "string":
                    Variables.Add(new Variable(Parts[0].Split(' ')[1].Trim(), "", VariableType.String));
                    return ReturningValue.Replace("string ", "").Replace(" = ", ".");
            }
            return ReturningValue;
        }
    }
}
