using CrystalOSAlpha.Applications.Calculator;
using System;
using System.Collections.Generic;

namespace CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Math
{
    class MathOperations
    {
        public static double Abs(string input, List<Variables.Variable> vars)
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
            return System.Math.Pow(Left, Right);
        }
        public static double Sin(string input, List<Variables.Variable> vars)
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
            return CalculatorA.Calculate(cleanInput);
        }
    }
}
