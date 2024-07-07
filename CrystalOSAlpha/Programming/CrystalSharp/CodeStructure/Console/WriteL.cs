using CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Console
{
    public class WriteL
    {
        public static string WriteLine(string input, List<Variables.Variable> vars)
        {
            // Remove the "Console.WriteLine(" part and the closing parenthesis and semicolon
            string temp = input.Replace("Console.WriteLine(", "").TrimEnd(';', ')').Trim();

            temp = temp.Replace("\\n", "\n");

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

            // Return the concatenated result
            return result.ToString().Insert(0, "\n");
        }
    }
}