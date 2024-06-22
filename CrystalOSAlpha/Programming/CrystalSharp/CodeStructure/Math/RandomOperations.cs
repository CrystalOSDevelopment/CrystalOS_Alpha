using CrystalOSAlpha.Applications.Calculator;
using CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Variables;
using System;
using System.Collections.Generic;

namespace CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Math
{
    class RandomOperations
    {
        public static int Next(string input, List<Variable> vars)
        {
            // Remove whitespace from the input
            string cleanInput = input.Replace(" ", "");

            // Replace variables with their values
            foreach (Variables.Variable var in vars)
            {
                string varValue = var.Type switch
                {
                    Variables.VariableType.Int => var.IntValue.ToString(),
                    Variables.VariableType.Float => var.FloatValue.ToString(),
                    Variables.VariableType.Double => var.DoubleValue.ToString(),
                    Variables.VariableType.String => var.Value,
                    Variables.VariableType.Char => var.CharValue.ToString(),
                    Variables.VariableType.Bool => var.BoolValue.ToString().ToLower(),
                    _ => throw new ArgumentException($"Unsupported variable type: {var.Type}")
                };

                cleanInput = cleanInput.Replace(var.ID, varValue);
            }
            // Evaluate the mathematical expression
            double Left = CalculatorA.Calculate(cleanInput.Split(',')[0]);
            double Right = CalculatorA.Calculate(cleanInput.Split(',')[1]);

            // Return the power
            Random random = new Random();
            return random.Next((int)Left, (int)Right);
        }

    }
}
