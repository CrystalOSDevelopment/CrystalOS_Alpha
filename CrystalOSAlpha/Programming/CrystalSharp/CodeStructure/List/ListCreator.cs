using CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Variables;
using System.Collections.Generic;

namespace CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.List
{
    public class ListCreator
    {
        public static List<Variable> ListGenerator(string Input, List<Variable> ExistingList)
        {
            List<Variable> NewList = ExistingList;
            string Temp = Input.Remove(Input.Length - 2).Remove(0, 1);
            //Inout -> "string 1","string 2", "string 3" , "string 4"
            foreach (string s in SplitQuotedString(Temp))
            {
                switch(s.Contains("\""))
                {
                    case true:
                        NewList.Add(new Variable("", s.Trim().Remove(s.Trim().Length - 1).Remove(0, 1), VariableType.String));
                        break;
                    case false:
                        int.TryParse(s.Trim(), out int i);
                        NewList.Add(new Variable("", i, VariableType.Int));
                        break;
                }
            }
            return NewList;
        }
        public static List<string> SplitQuotedString(string input)
        {
            List<string> result = new List<string>();
            bool insideQuotes = false;
            char quoteChar = '\"';
            List<char> currentSegment = new List<char>();

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                if (c == quoteChar)
                {
                    insideQuotes = !insideQuotes;
                    currentSegment.Add(c);
                }
                else if (c == ',' && !insideQuotes)
                {
                    if (currentSegment.Count > 0)
                    {
                        result.Add(new string(currentSegment.ToArray()).Trim());
                        currentSegment.Clear();
                    }
                }
                else
                {
                    currentSegment.Add(c);
                }
            }

            // Add the last segment if there's any
            if (currentSegment.Count > 0)
            {
                result.Add(new string(currentSegment.ToArray()).Trim());
            }

            return result;
        }
    }
}