using CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Variables;
using System;
using System.Collections.Generic;

namespace CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Conditional
{
    class CheckConditional
    {
        public bool IsTrue(string condition, List<Variable> Vars)
        {
            string[] AllowingCases = condition.Split("||");
            bool[] Cases = new bool[AllowingCases.Length];
            Array.Fill(Cases, false);
            int Index = 0;
            foreach (string Case in AllowingCases)
            {
                string[] CaseParts = Case.Split("&&");
                bool[] CasePartsBool = new bool[CaseParts.Length];
                Array.Fill(CasePartsBool, false);
                foreach (string Part in CaseParts)
                {
                    
                }
                Index++;
            }
            return false;
        }
    }
}
