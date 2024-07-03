using CrystalOSAlpha.Applications.Calculator;
using CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Variables;
using System.Collections.Generic;

namespace CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Math
{
    class MathOperations
    {
        //So as it turns out, GitHub's Copilot thing is a pile of garbage. Needs some rework as seen in Calculate(). Inside foreach loop.
        public static double Abs(string input, List<Variables.Variable> vars)
        {
            // Remove whitespace from the input
            string cleanInput = input.Replace(" ", "");

            // Replace variables with their values
            foreach (Variables.Variable var in vars)
            {
                switch (var.Type)
                {
                    case VariableType.Int:
                        cleanInput = cleanInput.Replace(var.ID, var.IntValue.ToString());
                        break;
                    case VariableType.Float:
                        cleanInput = cleanInput.Replace(var.ID, var.FloatValue.ToString());
                        break;
                    case VariableType.Double:
                        cleanInput = cleanInput.Replace(var.ID, var.DoubleValue.ToString());
                        break;
                    case VariableType.Char:
                        cleanInput = cleanInput.Replace(var.ID, var.CharValue.ToString());
                        break;
                    case VariableType.String:
                        cleanInput = cleanInput.Replace(var.ID, var.Value);
                        break;
                }
            }
            // Evaluate the mathematical expression
            double result = CalculatorA.Calculate(cleanInput);

            // Return the absolute value
            return System.Math.Abs(result);
        }
        public static double Sqrt(string input, List<Variables.Variable> vars)
        {
            // Remove whitespace from the input
            string cleanInput = input.Replace(" ", "");

            // Replace variables with their values
            foreach (Variables.Variable var in vars)
            {
                switch (var.Type)
                {
                    case VariableType.Int:
                        cleanInput = cleanInput.Replace(var.ID, var.IntValue.ToString());
                        break;
                    case VariableType.Float:
                        cleanInput = cleanInput.Replace(var.ID, var.FloatValue.ToString());
                        break;
                    case VariableType.Double:
                        cleanInput = cleanInput.Replace(var.ID, var.DoubleValue.ToString());
                        break;
                    case VariableType.Char:
                        cleanInput = cleanInput.Replace(var.ID, var.CharValue.ToString());
                        break;
                    case VariableType.String:
                        cleanInput = cleanInput.Replace(var.ID, var.Value);
                        break;
                }
            }
            // Evaluate the mathematical expression
            double result = CalculatorA.Calculate(cleanInput);

            // Return the square root
            return System.Math.Sqrt(result);
        }
        public static double Pow(string input, List<Variables.Variable> vars)
        {
            // Remove whitespace from the input
            string cleanInput = input.Replace(" ", "");

            // Replace variables with their values
            foreach (Variables.Variable var in vars)
            {
                switch (var.Type)
                {
                    case VariableType.Int:
                        cleanInput = cleanInput.Replace(var.ID, var.IntValue.ToString());
                        break;
                    case VariableType.Float:
                        cleanInput = cleanInput.Replace(var.ID, var.FloatValue.ToString());
                        break;
                    case VariableType.Double:
                        cleanInput = cleanInput.Replace(var.ID, var.DoubleValue.ToString());
                        break;
                    case VariableType.Char:
                        cleanInput = cleanInput.Replace(var.ID, var.CharValue.ToString());
                        break;
                    case VariableType.String:
                        cleanInput = cleanInput.Replace(var.ID, var.Value);
                        break;
                }
            }
            // Evaluate the mathematical expression
            double Left = CalculatorA.Calculate(cleanInput.Split(',')[0]);
            double Right = CalculatorA.Calculate(cleanInput.Split(',')[1]);

            // Return the power
            return System.Math.Pow(Left, Right);
        }
        public static double Sin(string input, List<Variables.Variable> vars)
        {
            // Remove whitespace from the input
            string cleanInput = input.Replace(" ", "");

            // Replace variables with their values
            foreach (Variables.Variable var in vars)
            {
                switch (var.Type)
                {
                    case VariableType.Int:
                        cleanInput = cleanInput.Replace(var.ID, var.IntValue.ToString());
                        break;
                    case VariableType.Float:
                        cleanInput = cleanInput.Replace(var.ID, var.FloatValue.ToString());
                        break;
                    case VariableType.Double:
                        cleanInput = cleanInput.Replace(var.ID, var.DoubleValue.ToString());
                        break;
                    case VariableType.Char:
                        cleanInput = cleanInput.Replace(var.ID, var.CharValue.ToString());
                        break;
                    case VariableType.String:
                        cleanInput = cleanInput.Replace(var.ID, var.Value);
                        break;
                }
            }
            // Evaluate the mathematical expression
            double result = CalculatorA.Calculate(cleanInput);

            // Return the sine value
            return System.Math.Sin(result);
        }
        public static double Cos(string input, List<Variables.Variable> vars)
        {
            // Remove whitespace from the input
            string cleanInput = input.Replace(" ", "");

            // Replace variables with their values
            foreach (Variables.Variable var in vars)
            {
                switch (var.Type)
                {
                    case VariableType.Int:
                        cleanInput = cleanInput.Replace(var.ID, var.IntValue.ToString());
                        break;
                    case VariableType.Float:
                        cleanInput = cleanInput.Replace(var.ID, var.FloatValue.ToString());
                        break;
                    case VariableType.Double:
                        cleanInput = cleanInput.Replace(var.ID, var.DoubleValue.ToString());
                        break;
                    case VariableType.Char:
                        cleanInput = cleanInput.Replace(var.ID, var.CharValue.ToString());
                        break;
                    case VariableType.String:
                        cleanInput = cleanInput.Replace(var.ID, var.Value);
                        break;
                }
            }
            // Evaluate the mathematical expression
            double result = CalculatorA.Calculate(cleanInput);

            // Return the cosine value
            return System.Math.Cos(result);
        }
        public static double Ceil(string input, List<Variables.Variable> vars)
        {
            // Remove whitespace from the input
            string cleanInput = input.Replace(" ", "");

            // Replace variables with their values
            foreach (Variables.Variable var in vars)
            {
                switch (var.Type)
                {
                    case VariableType.Int:
                        cleanInput = cleanInput.Replace(var.ID, var.IntValue.ToString());
                        break;
                    case VariableType.Float:
                        cleanInput = cleanInput.Replace(var.ID, var.FloatValue.ToString());
                        break;
                    case VariableType.Double:
                        cleanInput = cleanInput.Replace(var.ID, var.DoubleValue.ToString());
                        break;
                    case VariableType.Char:
                        cleanInput = cleanInput.Replace(var.ID, var.CharValue.ToString());
                        break;
                    case VariableType.String:
                        cleanInput = cleanInput.Replace(var.ID, var.Value);
                        break;
                }
            }
            // Evaluate the mathematical expression
            double result = CalculatorA.Calculate(cleanInput);

            // Return the ceiled value
            return System.Math.Ceiling(result);
        }
        public static double Floor(string input, List<Variables.Variable> vars)
        {
            // Remove whitespace from the input
            string cleanInput = input.Replace(" ", "");

            // Replace variables with their values
            foreach (Variables.Variable var in vars)
            {
                switch (var.Type)
                {
                    case VariableType.Int:
                        cleanInput = cleanInput.Replace(var.ID, var.IntValue.ToString());
                        break;
                    case VariableType.Float:
                        cleanInput = cleanInput.Replace(var.ID, var.FloatValue.ToString());
                        break;
                    case VariableType.Double:
                        cleanInput = cleanInput.Replace(var.ID, var.DoubleValue.ToString());
                        break;
                    case VariableType.Char:
                        cleanInput = cleanInput.Replace(var.ID, var.CharValue.ToString());
                        break;
                    case VariableType.String:
                        cleanInput = cleanInput.Replace(var.ID, var.Value);
                        break;
                }
            }
            // Evaluate the mathematical expression
            double result = CalculatorA.Calculate(cleanInput);

            // Return the floored value
            return System.Math.Floor(result);
        }
        public static double Round(string input, List<Variables.Variable> vars)
        {
            // Remove whitespace from the input
            string cleanInput = input.Replace(" ", "");

            // Replace variables with their values
            foreach (Variables.Variable var in vars)
            {
                switch (var.Type)
                {
                    case VariableType.Int:
                        cleanInput = cleanInput.Replace(var.ID, var.IntValue.ToString());
                        break;
                    case VariableType.Float:
                        cleanInput = cleanInput.Replace(var.ID, var.FloatValue.ToString());
                        break;
                    case VariableType.Double:
                        cleanInput = cleanInput.Replace(var.ID, var.DoubleValue.ToString());
                        break;
                    case VariableType.Char:
                        cleanInput = cleanInput.Replace(var.ID, var.CharValue.ToString());
                        break;
                    case VariableType.String:
                        cleanInput = cleanInput.Replace(var.ID, var.Value);
                        break;
                }
            }
            // Evaluate the mathematical expression
            double result = CalculatorA.Calculate(cleanInput);

            // Return the rounded value
            return System.Math.Round(result);
        }
        public static double Calculate(string input, List<Variables.Variable> vars)
        {
            // Remove whitespace from the input
            string cleanInput = input.Replace(" ", "");

            // Replace variables with their values
            foreach (Variables.Variable var in vars)
            {
                switch (var.Type)
                {
                    case VariableType.Int:
                        cleanInput = cleanInput.Replace(var.ID, var.IntValue.ToString());
                        break;
                    case VariableType.Float:
                        cleanInput = cleanInput.Replace(var.ID, var.FloatValue.ToString());
                        break;
                    case VariableType.Double:
                        cleanInput = cleanInput.Replace(var.ID, var.DoubleValue.ToString());
                        break;
                    case VariableType.Char:
                        cleanInput = cleanInput.Replace(var.ID, var.CharValue.ToString());
                        break;
                    case VariableType.String:
                        cleanInput = cleanInput.Replace(var.ID, var.Value);
                        break;
                }
            }

            // Check if the cleaned input is a valid number
            if (double.TryParse(cleanInput, out double result))
            {
                return result;
            }

            // Evaluate the mathematical expression if it's not a single number
            return CalculatorA.Calculate(cleanInput);
        }
    }
}
