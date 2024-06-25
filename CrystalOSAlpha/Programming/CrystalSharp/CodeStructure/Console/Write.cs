using System.Collections.Generic;
using System.Text;
using System;

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
                    }
                }
                else
                {
                    // If it's not a variable, treat it as a literal string
                    result.Append(trimmedPart.Trim('"'));
                }
            }

            // Return the concatenated result
            return result.ToString();
        }
    }
}