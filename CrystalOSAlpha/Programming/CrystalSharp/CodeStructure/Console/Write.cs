using System.Collections.Generic;
using System.Text;
using System;
using CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Math;

namespace CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Console
{
    public class Write
    {
        public static string CWrite(string input, List<Variables.Variable> vars)
        {
            // Remove the "Console.WriteLine(" part and the closing parenthesis and semicolon
            string temp = input.Replace("Console.Write(", "").TrimEnd(';', ')').Trim();

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
                switch (variable)
                {
                    case null:
                        switch (trimmedPart.Contains("\""))
                        {
                            case true:
                                result.Append(trimmedPart.Trim('"'));
                                break;
                            case false:
                                switch (trimmedPart.Contains("["))
                                {
                                    case true:
                                        string[] Data = trimmedPart.Split('[');
                                        Data[1] = Data[1].TrimEnd(']').Trim();
                                        Variables.Variable FindIt = vars.Find(v => v.ID == Data[0]);
                                        switch (FindIt.Type)
                                        {
                                            case Variables.VariableType.Bitmap:
                                                bool Success = int.TryParse(Data[1].Replace(" ", "").Split(',')[0], out int X);
                                                bool Success2 = int.TryParse(Data[1].Replace(" ", "").Split(',')[1], out int Y);
                                                switch (Success && Success2)
                                                {
                                                    case true:
                                                        result.Append(ImprovedVBE.GetPixel(FindIt.BitmapValue, X, Y));
                                                        break;
                                                    case false:
                                                        int XAxis = (int)MathOperations.Calculate(Data[1].Replace(" ", "").Split(',')[0], vars);
                                                        int YAxis = (int)MathOperations.Calculate(Data[1].Replace(" ", "").Split(',')[1], vars);
                                                        result.Append(ImprovedVBE.GetPixel(FindIt.BitmapValue, XAxis, YAxis));
                                                        break;
                                                }
                                                break;
                                            case Variables.VariableType.List:
                                                switch(int.TryParse(Data[1], out int Index))
                                                {
                                                    case true:
                                                        result.Append(FindIt.Vars[Index].Value);
                                                        break;
                                                    case false:
                                                        int IndexValue = (int)MathOperations.Calculate(Data[1], vars);
                                                        result.Append(FindIt.Vars[IndexValue].Value);
                                                        break;
                                                }
                                                break;
                                        }
                                        break;
                                    case false:
                                        result.Append(trimmedPart);
                                        break;
                                }
                            break;
                        }
                        break;
                    default:
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
                        break;
                }
            }

            // Return the concatenated result
            return result.ToString();
        }
    }
}