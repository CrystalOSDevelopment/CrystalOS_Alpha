using Cosmos.System;
using CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Console;
using CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Variables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrystalOSAlpha.Programming.CrystalSharp
{
    class Core
    {
        public int LineCounter = 2;
        public int VariableIndex = -99;
        public int BracketCounter = 0;
        public bool IsWaitingForReadLine = false;
        public bool isVariable = false;
        public bool AllowExecution = true;
        public string Cached = "";
        public string Output = "";
        public Variable VariableName;
        public List<Variable> Variables = new List<Variable>();

        public string Execute(List<CodeSegments> CodeSegments, string ProjectName)
        {
            var Main = CodeSegments.Find(d => d.ClassName == ProjectName);
            var Temp = "";
            if (Cached == "")
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
            if (LineCounter == Cached.Split('\n').Length)
            {
                LineCounter = -1;
            }
            else
            {
                if (AllowExecution)
                {
                    Temp = MethodReturner(Cached.Split('\n')[LineCounter]);
                }
                else
                {
                    BracketStepper(Cached.Split('\n')[LineCounter]);
                    if(BracketCounter == 0)
                    {
                        AllowExecution = true;
                    }
                }
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
            // IMPORTANT!!!! Always remember that semi-colons are present at the end of the line. They are not trimmed down.
            if (Line.Contains("string") || Line.Contains("int") || Line.Contains("bool") || Line.Contains("float") || Line.Contains("double") || Line.Contains("char"))
            {
                Line = SettingVariableValues(Line.Trim());
            }
            else
            {
                if(!Line.Contains("if"))
                {
                    var v = Variables.Find(d => d.ID == Line.Trim().Split(" = ")[0]);
                    if (v != null)
                    {
                        VariableName = v;
                        VariableIndex = Variables.IndexOf(v);
                        Line = Line.Split(" = ")[1];
                        isVariable = true;
                    }                    
                }
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
                            if (KeyboardManager.TryReadKey(out KeyEvent key))
                            {
                                if (key.Key == ConsoleKeyEx.Enter)
                                {
                                    IsWaitingForReadLine = false;
                                    if (isVariable)
                                    {
                                        switch (VariableName.Type)
                                        {
                                            case VariableType.String:
                                                Variables[VariableIndex].Value = Output;
                                                Output = "\n" + Variables[VariableIndex].ID + ":    " + Variables[VariableIndex].Value;
                                                break;
                                            case VariableType.Int:
                                                Variables[VariableIndex].IntValue = int.Parse(Output);
                                                break;
                                            case VariableType.Bool:
                                                bool.TryParse(Output, out bool ParsedBool);
                                                Variables[VariableIndex].BoolValue = ParsedBool;
                                                break;
                                            case VariableType.Float:
                                                Variables[VariableIndex].FloatValue = float.Parse(Output);
                                                break;
                                            case VariableType.Double:
                                                Variables[VariableIndex].DoubleValue = double.Parse(Output);
                                                break;
                                            case VariableType.Char:
                                                Variables[VariableIndex].CharValue = Output[0];
                                                break;
                                        }
                                        isVariable = false;
                                        VariableIndex = -99;
                                        VariableName = null;
                                    }
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
                case "Random":
                    break;
                case "Math":
                    switch (Parts[1].Split("(")[0])
                    {
                        case "Calculate":
                            // Add your math calculation logic here
                            break;
                    }
                    break;
                default:
                    if (isVariable)
                    {
                        Line = Line.Remove(Line.Length - 1);
                        switch (VariableName.Type)
                        {
                            case VariableType.String:
                                Variables[VariableIndex].Value = Line.Remove(Line.Length - 1).Remove(0, 1);
                                break;
                            case VariableType.Int:
                                Variables[VariableIndex].IntValue = int.Parse(Line);
                                break;
                            case VariableType.Bool:
                                if(bool.TryParse(Line, out bool ParsedBool))
                                {
                                    Variables[VariableIndex].BoolValue = ParsedBool;
                                }
                                else
                                {
                                    Output += "\n" + Line;
                                }
                                break;
                            case VariableType.Float:
                                Variables[VariableIndex].FloatValue = float.Parse(Line);
                                break;
                            case VariableType.Double:
                                Variables[VariableIndex].DoubleValue = double.Parse(Line);
                                break;
                            case VariableType.Char:
                                Variables[VariableIndex].CharValue = Line.Remove(Line.Length - 1).Remove(0, 1)[0];
                                break;
                        }
                        isVariable = false;
                        VariableIndex = -99;
                        VariableName = null;
                    }
                    else
                    {
                        if (Line.Contains("if"))
                        {
                            if (!EvaluateCondition(Line.Trim().Substring(3, Line.Trim().Length - 4)))
                            {
                                AllowExecution = false;
                            }
                        }
                        else
                        {
                            foreach (var variable in Variables)
                            {
                                if (variable.ID == Parts[0])
                                {
                                    switch (variable.Type)
                                    {
                                        case VariableType.String:
                                            variable.Value = Parts[1].Remove(Parts[1].Length - 2).Remove(0, 1);
                                            Output = "\n" + variable.Value;
                                            break;
                                        case VariableType.Int:
                                            variable.IntValue = int.Parse(Parts[1].Remove(Parts[1].Length - 1));
                                            Output = "\n" + variable.IntValue.ToString();
                                            break;
                                        case VariableType.Bool:
                                            bool.TryParse(Parts[1].Remove(Parts[1].Length - 1), out bool ParsedBool);
                                            variable.BoolValue = ParsedBool;
                                            Output = "\n" + variable.BoolValue.ToString();
                                            break;
                                        case VariableType.Float:
                                            variable.FloatValue = float.Parse(Line.Remove(Line.Length - 1).Remove(0, Line.IndexOf('.') + 1));
                                            Output = "\n" + variable.FloatValue.ToString();
                                            break;
                                        case VariableType.Double:
                                            variable.DoubleValue = double.Parse(Line.Remove(Line.Length - 1).Remove(0, Line.IndexOf('.') + 1));
                                            Output = "\n" + variable.DoubleValue.ToString();
                                            break;
                                        case VariableType.Char:
                                            variable.CharValue = char.Parse(Parts[1].Remove(Parts[1].Length - 2).Remove(0, 1));
                                            Output = "\n" + variable.CharValue.ToString();
                                            break;
                                    }
                                    isVariable = false;
                                    VariableIndex = -99;
                                    VariableName = null;
                                }
                            }
                        }
                    }
                    break;
            }
            return Output;
        }
        public void BracketStepper(string Input)
        {
            if(Input.Contains("{"))
            {
                BracketCounter++;
            }
            if (Input.Contains("}"))
            {
                BracketCounter--;
            }
        }

        private bool EvaluateCondition(string condition)
        {
            // Tokenize the condition to handle complex logical expressions
            var tokens = TokenizeCondition(condition);

            // Evaluate the tokenized condition
            var result = EvaluateTokens(tokens);

            return result;
        }

        // Tokenize the condition into parts that can be evaluated
        private List<string> TokenizeCondition(string condition)
        {
            var tokens = new List<string>();
            var operators = new HashSet<string> { "&&", "||", "==", "!=", "<=", ">=", "<", ">", "(", ")" };
            int i = 0;

            while (i < condition.Length)
            {
                if (char.IsWhiteSpace(condition[i]))
                {
                    i++;
                    continue;
                }

                // Handle operators and parentheses
                var op = operators.FirstOrDefault(o => condition.Substring(i).StartsWith(o));
                if (op != null)
                {
                    tokens.Add(op);
                    i += op.Length;
                    continue;
                }

                // Handle operands
                int start = i;
                while (i < condition.Length && !char.IsWhiteSpace(condition[i]) && !operators.Any(o => condition.Substring(i).StartsWith(o)))
                {
                    i++;
                }
                tokens.Add(condition.Substring(start, i - start));
            }
            return tokens;
        }

        // Evaluate the tokenized condition using a stack-based approach for logical operations
        private bool EvaluateTokens(List<string> tokens)
        {
            var values = new Stack<bool>();
            var operators = new Stack<string>();

            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];

                if (token == "&&" || token == "||")
                {
                    while (operators.Count > 0 && GetPrecedence(operators.Peek()) >= GetPrecedence(token))
                    {
                        var op = operators.Pop();
                        var right = values.Pop();
                        var left = values.Pop();
                        values.Push(ApplyOperator(left, right, op));
                    }
                    operators.Push(token);
                }
                else if (token == "(")
                {
                    operators.Push(token);
                }
                else if (token == ")")
                {
                    while (operators.Peek() != "(")
                    {
                        var op = operators.Pop();
                        var right = values.Pop();
                        var left = values.Pop();
                        values.Push(ApplyOperator(left, right, op));
                    }
                    operators.Pop(); // Remove the '('
                }
                else if (token == "!=")
                {
                    // Handle the "!=" operator
                    var leftOperand = tokens[i + 1]; // Next token is the left operand
                    var rightOperand = tokens[i + 2]; // Following token is the right operand
                    values.Push(EvaluateSimpleCondition(leftOperand, "!=", rightOperand));
                    i += 2; // Move to the next token after operands
                }
                else if (token == "==")
                {
                    // Handle the "==" operator
                    var leftOperand = tokens[i - 1]; // Previous token is the left operand
                    var rightOperand = tokens[i + 1]; // Following token is the right operand
                    values.Push(EvaluateSimpleCondition(leftOperand, "==", rightOperand));
                    i++; // Move to the next token after operands
                }
                else if (token == "<=" || token == ">=" || token == "<" || token == ">")
                {
                    // Handle comparison operators
                    var leftOperand = tokens[i - 1]; // Previous token is the left operand
                    var rightOperand = tokens[i + 1]; // Following token is the right operand
                    values.Push(EvaluateSimpleCondition(leftOperand, token, rightOperand));
                    i++; // Move to the next token after operands
                }
                else
                {
                    // Handle single boolean value or variable
                    var variable = Variables.Find(v => v.ID == token);
                    if (variable != null)
                    {
                        switch (variable.Type)
                        {
                            case VariableType.Bool:
                                values.Push(variable.BoolValue);
                                break;
                            case VariableType.Int:
                                values.Push(variable.IntValue != 0);
                                break;
                                // Handle other types as needed
                        }
                    }
                    else
                    {
                        // Handle literal boolean values
                        bool.TryParse(token, out bool parsedBool);
                        values.Push(parsedBool);
                    }
                }
            }

            while (operators.Count > 0)
            {
                var op = operators.Pop();
                var right = values.Pop();
                var left = values.Pop();
                values.Push(ApplyOperator(left, right, op));
            }

            return values.Pop();
        }

        // Get the precedence of the logical operators
        private int GetPrecedence(string op)
        {
            switch (op)
            {
                case "&&":
                    return 1;
                case "||":
                    return 0;
                default:
                    throw new Exception($"Unknown operator: {op}");
            }
        }

        // Apply the logical operator to the left and right boolean values
        private bool ApplyOperator(bool left, bool right, string op)
        {
            switch (op)
            {
                case "&&":
                    return left && right;
                case "||":
                    return left || right;
                default:
                    throw new Exception($"Unknown operator: {op}");
            }
        }

        // Evaluate simple conditions like "a < b", "a == b", etc.
        private bool EvaluateSimpleCondition(string leftOperand, string op, string rightOperand)
        {
            var leftVar = Variables.Find(v => v.ID == leftOperand);

            if (leftVar == null)
            {
                while (true)
                {
                    if(int.TryParse(leftOperand, out int ParsedInt))
                    {
                        leftVar = new Variable("LeftOperand", ParsedInt, VariableType.Int);
                        break;
                    }
                    if(bool.TryParse(leftOperand, out bool ParsedBool))
                    {
                        leftVar = new Variable("LeftOperand", ParsedBool, VariableType.Bool);
                        break;
                    }
                    if (float.TryParse(leftOperand, out float ParsedFloat))
                    {
                        leftVar = new Variable("LeftOperand", ParsedFloat, VariableType.Float);
                        break;
                    }
                    if (double.TryParse(leftOperand, out double ParsedDouble))
                    {
                        leftVar = new Variable("LeftOperand", ParsedDouble, VariableType.Double);
                        break;
                    }
                    if (char.TryParse(leftOperand, out char ParsedChar))
                    {
                        leftVar = new Variable("LeftOperand", ParsedChar, VariableType.Char);
                        break;
                    }
                    leftVar = new Variable("LeftOperand", leftOperand, VariableType.String);
                    break;
                }
            }

            switch (leftVar.Type)
            {
                case VariableType.Int:
                    int leftInt = leftVar.IntValue;
                    int.TryParse(rightOperand, out int ParsedInt);
                    int rightInt = ParsedInt;
                    switch (op)
                    {
                        case "<":
                            return leftInt < rightInt;
                        case ">":
                            return leftInt > rightInt;
                        case "<=":
                            return leftInt <= rightInt;
                        case ">=":
                            return leftInt >= rightInt;
                        case "==":
                            return leftInt == rightInt;
                        case "!=":
                            return leftInt != rightInt;
                        default:
                            throw new Exception($"Unknown operator: {op}");
                    }
                case VariableType.Bool:
                    bool leftBool = leftVar.BoolValue;
                    bool.TryParse(rightOperand, out bool ParsedBool);
                    bool rightBool = ParsedBool;
                    switch (op)
                    {
                        case "==":
                            return leftBool == rightBool;
                        case "!=":
                            return leftBool != rightBool;
                        default:
                            throw new Exception($"Unknown operator: {op}");
                    }
                // Add cases for other variable types as needed
                default:
                    throw new Exception($"Unsupported variable type: {leftVar.Type}");
            }
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
                case "int":
                    Variables.Add(new Variable(Parts[0].Split(' ')[1].Trim(), 0, VariableType.Int));
                    return ReturningValue.Replace("int ", "").Replace(" = ", ".");
                case "bool":
                    Variables.Add(new Variable(Parts[0].Split(' ')[1].Trim(), false, VariableType.Bool));
                    return ReturningValue.Replace("bool ", "").Replace(" = ", ".");
                case "float":
                    Variables.Add(new Variable(Parts[0].Split(' ')[1].Trim(), 0.0f, VariableType.Float));
                    return ReturningValue.Replace("float ", "").Replace(" = ", ".");
                case "double":
                    Variables.Add(new Variable(Parts[0].Split(' ')[1].Trim(), 0.0, VariableType.Double));
                    return ReturningValue.Replace("double ", "").Replace(" = ", ".");
                case "char":
                    Variables.Add(new Variable(Parts[0].Split(' ')[1].Trim(), ' ', VariableType.Char));
                    return ReturningValue.Replace("char ", "").Replace(" = ", ".");
            }
            return ReturningValue;
        }
    }
}
