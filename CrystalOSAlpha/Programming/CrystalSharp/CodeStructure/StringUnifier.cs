using CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Variables;
using System.Collections.Generic;

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
    }
}
