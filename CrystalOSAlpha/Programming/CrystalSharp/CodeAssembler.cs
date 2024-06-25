using System.Collections.Generic;

namespace CrystalOSAlpha.Programming.CrystalSharp
{
    class CodeAssembler
    {
        public static string Return = "";
        public static List<CodeSegments> AssembleCode(List<string> SourceFiles)
        {
            Return = "";
            List<CodeSegments> Codes = new List<CodeSegments>();
            foreach (string SourceFile in SourceFiles)
            {
                CodeSegments Code = new CodeSegments();
                Code.Segmenter(SourceFile);
                Codes.Add(Code);
                foreach(var s in Code.Segments)
                {
                    Return += Code.ClassName + ": \n" + s + "\n";
                }
            }
            
            return Codes;
        }
    }
    public class CodeSegments
    {
        public string ClassName = "";
        public List<string> Segments = new List<string>();
        public void Segmenter(string Input)
        {
            bool Started = false;
            string[] Splited = Input.Split('\n');
            string output = "";
            int Brackets = 0;
            for (int i = 0; i < Splited.Length; i++)
            {
                if (Splited[i].Contains("class"))
                {
                    ClassName = Splited[i].Split(' ')[1];
                }
                if (Started)
                {
                    output += Splited[i] + "\n";

                    if (Splited[i].Contains("{"))
                    {
                        Brackets++;
                    }
                    if (Splited[i].Contains("}"))
                    {
                        Brackets--;
                        if (Brackets == 0)
                        {
                            Segments.Add(output);
                            output = "";
                            Started = false;
                        }
                    }
                }
                else if (Splited[i].Contains("void") && Splited[i].Contains("(") && Splited[i].Contains(")"))
                {
                    Started = true;
                    Brackets = 0;  // Reset bracket count
                    output += Splited[i] + "\n";
                    if (Splited[i].Contains("{"))
                    {
                        Brackets++;
                    }
                }
            }
        }
    }
}
