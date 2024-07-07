using CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Math;
using CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Variables;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrystalOSAlpha.Programming.CrystalSharp.CodeStructure
{
    public class StringUnifier
    {
        public static string GetFileName(string path, List<Variable> variables)
        {
            string FileName = "";
            string VarName = "";
            bool IsInQuotes = false;
            for (int i = 0; i < path.Length; i++)
            {
                switch (path[i])
                {
                    case '"':
                        IsInQuotes = !IsInQuotes;
                        break;
                    default:
                        if (IsInQuotes)
                        {
                            FileName += path[i];
                        }
                        else
                        {
                            switch (path[i])
                            {
                                case ' ':
                                    if (VarName != "")
                                    {
                                        var Var = variables.Find(d => d.ID == VarName);
                                        if (Var != null)
                                        {
                                            switch (Var.Type)
                                            {
                                                case VariableType.String:
                                                    FileName += Var.Value;
                                                    break;
                                                case VariableType.Int:
                                                    FileName += Var.IntValue.ToString();
                                                    break;
                                                case VariableType.Double:
                                                    FileName += Var.DoubleValue.ToString();
                                                    break;
                                                case VariableType.Char:
                                                    FileName += Var.CharValue.ToString();
                                                    break;
                                                case VariableType.Bool:
                                                    FileName += Var.BoolValue.ToString();
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            return "Variable not found: " + VarName;
                                        }
                                        VarName = "";
                                    }
                                    break;
                                case '+':
                                    if (VarName != "")
                                    {
                                        var Var2 = variables.Find(d => d.ID == VarName);
                                        if (Var2 != null)
                                        {
                                            switch (Var2.Type)
                                            {
                                                case VariableType.String:
                                                    FileName += Var2.Value;
                                                    break;
                                                case VariableType.Int:
                                                    FileName += Var2.IntValue.ToString();
                                                    break;
                                                case VariableType.Double:
                                                    FileName += Var2.DoubleValue.ToString();
                                                    break;
                                                case VariableType.Char:
                                                    FileName += Var2.CharValue.ToString();
                                                    break;
                                                case VariableType.Bool:
                                                    FileName += Var2.BoolValue.ToString();
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            return "Variable not found: " + VarName;
                                        }
                                        VarName = "";
                                    }
                                    break;
                                default:
                                    VarName += path[i];
                                    break;
                            }
                        }
                        break;
                }
            }
            if (VarName != "")
            {
                var Var3 = variables.Find(d => d.ID == VarName);
                if (Var3 != null)
                {
                    switch (Var3.Type)
                    {
                        case VariableType.String:
                            FileName += Var3.Value;
                            break;
                        case VariableType.Int:
                            FileName += Var3.IntValue.ToString();
                            break;
                        case VariableType.Double:
                            FileName += Var3.DoubleValue.ToString();
                            break;
                        case VariableType.Char:
                            FileName += Var3.CharValue.ToString();
                            break;
                        case VariableType.Bool:
                            FileName += Var3.BoolValue.ToString();
                            break;
                    }
                }
                else
                {
                    return "Variable not found: " + VarName;
                }
                VarName = "";
            }
            return FileName;
        }

        public static string StringAssembler(string temp, List<Variable> vars)
        {
            // Split the input string by the concatenation operator (+)
            string[] parts = temp.Split(new[] { '+' }, StringSplitOptions.None);

            // Create a StringBuilder to efficiently concatenate strings
            StringBuilder result = new StringBuilder();

            // Iterate through each part, trim it and append the corresponding value or the part itself
            foreach (string part in parts)
            {
                string trimmedPart = part.Trim();

                // Check if the part is a variable by looking it up in the vars list
                Variables.Variable variable = vars.Find(v => v.ID == trimmedPart);
                if (variable != null)
                {
                    switch (variable.Type)
                    {
                        case Variables.VariableType.Int:
                            result.Append(variable.IntValue);
                            break;
                        case Variables.VariableType.Float:
                            result.Append(variable.FloatValue);
                            break;
                        case Variables.VariableType.Double:
                            result.Append(variable.DoubleValue);
                            break;
                        case Variables.VariableType.String:
                            result.Append(variable.Value);
                            break;
                        case Variables.VariableType.Char:
                            result.Append(variable.CharValue);
                            break;
                        case Variables.VariableType.Bool:
                            result.Append(variable.BoolValue);
                            break;
                        case Variables.VariableType.ConsoleKeyEx:
                            result.Append(Keys.KeyToString(variable.ConsoleKeyEx).Split('.')[1]);
                            break;
                    }
                }
                else
                {
                    if (trimmedPart.Contains("\""))
                    {
                        result.Append(trimmedPart.Trim('"'));
                    }
                    else
                    {
                        if (trimmedPart.Contains("["))
                        {
                            string[] Data = trimmedPart.Split('[');
                            Data[1] = Data[1].TrimEnd(']').Trim();
                            Variables.Variable FindIt = vars.Find(v => v.ID == Data[0]);
                            switch (FindIt.Type)
                            {
                                case Variables.VariableType.Bitmap:
                                    if (int.TryParse(Data[1].Replace(" ", "").Split(',')[0], out int X) && int.TryParse(Data[1].Replace(" ", "").Split(',')[1], out int Y))
                                    {
                                        result.Append(ImprovedVBE.GetPixel(FindIt.BitmapValue, X, Y));
                                    }
                                    else
                                    {
                                        int XAxis = (int)MathOperations.Calculate(Data[1].Replace(" ", "").Split(',')[0], vars);
                                        int YAxis = (int)MathOperations.Calculate(Data[1].Replace(" ", "").Split(',')[1], vars);
                                        result.Append(ImprovedVBE.GetPixel(FindIt.BitmapValue, XAxis, YAxis));
                                    }
                                    break;
                                case Variables.VariableType.List:
                                    if (int.TryParse(Data[1], out int Index))
                                    {
                                        result.Append(FindIt.Vars[Index].Value);
                                    }
                                    else
                                    {
                                        int IndexValue = (int)MathOperations.Calculate(Data[1], vars);
                                        result.Append(FindIt.Vars[IndexValue].Value);
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            result.Append(trimmedPart);
                        }
                    }
                }
            }

            return result.ToString();
        }
    }
}
