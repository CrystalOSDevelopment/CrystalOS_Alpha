﻿using System;
using System.Collections.Generic;

namespace CrystalOSAlpha.Applications.Calculator
{
    class CalculatorA
    {
        public static double Calculate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return 0;
            }

            Queue<string> outputQueue = new Queue<string>();
            Stack<string> operatorStack = new Stack<string>();

            string[] tokens = Tokenize(input);

            foreach (string token in tokens)
            {
                if (IsNumber(token))
                {
                    outputQueue.Enqueue(token);
                }
                else if (IsOperator(token))
                {
                    while (operatorStack.Count > 0 && IsOperator(operatorStack.Peek()) && Precedence(token) <= Precedence(operatorStack.Peek()))
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }
                    operatorStack.Push(token);
                }
                else if (token == "(")
                {
                    operatorStack.Push(token);
                }
                else if (token == ")")
                {
                    while (operatorStack.Count > 0 && operatorStack.Peek() != "(")
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }
                    if (operatorStack.Count == 0)
                    {
                        throw new ArgumentException("Mismatched parentheses.");
                    }
                    operatorStack.Pop();
                }
            }

            while (operatorStack.Count > 0)
            {
                if (operatorStack.Peek() == "(" || operatorStack.Peek() == ")")
                {
                    throw new ArgumentException("Mismatched parentheses.");
                }
                outputQueue.Enqueue(operatorStack.Pop());
            }

            return EvaluateRPN(outputQueue);
        }

        static string[] Tokenize(string input)
        {
            input = input.Replace(" ", "");
            input = input.Replace("(-", "(0-");
            input = input.Replace(",-", ",0-");

            // Replace commas with dots
            input = input.Replace(",", ".");

            List<string> tokens = new List<string>();
            string current = "";

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (IsOperator(c.ToString()) || c == '(' || c == ')')
                {
                    if (!string.IsNullOrEmpty(current))
                    {
                        tokens.Add(current);
                        current = "";
                    }
                    if (c == '-' && (i == 0 || input[i - 1] == '(' || IsOperator(input[i - 1].ToString())))
                    {
                        current += c;
                    }
                    else
                    {
                        tokens.Add(c.ToString());
                    }
                }
                else
                {
                    current += c;
                }
            }

            if (!string.IsNullOrEmpty(current))
            {
                tokens.Add(current);
            }

            return tokens.ToArray();
        }

        static bool IsNumber(string token)
        {
            return double.TryParse(token, out _);
        }

        static bool IsOperator(string token)
        {
            return token == "+" || token == "-" || token == "*" || token == "/";
        }

        static int Precedence(string op)
        {
            if (op == "*" || op == "/")
            {
                return 2;
            }
            else if (op == "+" || op == "-")
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        static double EvaluateRPN(Queue<string> outputQueue)
        {
            Stack<double> stack = new Stack<double>();

            try
            {
                while (outputQueue.Count > 0)
                {
                    string token = outputQueue.Dequeue();

                    if (IsNumber(token))
                    {
                        stack.Push(double.Parse(token));
                    }
                    else if (IsOperator(token))
                    {
                        if (stack.Count < 2)
                        {
                            throw new ArgumentException("Invalid expression.");
                        }
                        double operand2 = stack.Pop();
                        double operand1 = stack.Pop();

                        if (token == "+")
                        {
                            stack.Push(operand1 + operand2);
                        }
                        else if (token == "-")
                        {
                            stack.Push(operand1 - operand2);
                        }
                        else if (token == "*")
                        {
                            stack.Push(operand1 * operand2);
                        }
                        else if (token == "/")
                        {
                            if (operand2 == 0)
                            {
                                // Division by zero, return 0
                                return 0;
                            }
                            stack.Push(operand1 / operand2);
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Invalid token: " + token);
                    }
                }

                if (stack.Count != 1)
                {
                    throw new ArgumentException("Invalid expression.");
                }

                return stack.Pop();
            }
            catch (DivideByZeroException)
            {
                // Handle division by zero
                return 0;
            }
            catch (Exception)
            {
                // Handle any other exception, treat it as invalid input
                return 0;
            }
        }
    }
}