using System.Collections.Generic;

namespace CrystalOSAlpha.Programming
{
    class BuildString
    {
        public static string Build(List<Variables> Vars, string Input)
        {
            string BuiltString = "";
            string[] Splitted = Input.Split("+");
            for(int i = 0; i < Splitted.Length; i++)
            {
                if (Splitted[i].Contains("\""))
                {
                    BuiltString += Splitted[i].Remove(Splitted[i].Length - 1).Remove(0, 1);
                }
                else
                {
                    bool Found = false;
                    for(int j = 0; j < Vars.Count && Found == false; j++)
                    {
                        if (Vars[j].S_Name == Splitted[i])
                        {
                            BuiltString += Vars[j].S_Value;
                            Found = true;
                        }
                        if (Vars[j].I_Name == Splitted[i])
                        {
                            BuiltString += Vars[j].I_Value;
                            Found = true;
                        }
                    }
                }
            }
            return BuiltString;
        }
        public static string ReplaceVarsToValues(List<Variables> Vars, string Input)
        {
            string result = "";
            string[] Splitted = Input.Split(new char[] {'+', '-', '*', '/'});
            List<char> Operands = new List<char>();
            for(int i = 0; i < Input.Length; i++)
            {
                if(new List<char> { '+', '-', '*', '/' }.Contains(Input[i]))
                {
                    Operands.Add(Input[i]);
                }
            }
            for(int i = 0; i < Splitted.Length; i++)
            {
                bool Found = false;
                for (int j = 0; j < Vars.Count && Found == false; j++)
                {
                    if (Vars[j].I_Name == Splitted[i])
                    {
                        result += Vars[j].I_Value;
                        Found = true;
                    }
                }
                if(Found == false)
                {
                    result += Splitted[i];
                }
                if(i < Splitted.Length - 1)
                {
                    result += Operands[i];
                }
            }
            return result;
        }
    }
}
