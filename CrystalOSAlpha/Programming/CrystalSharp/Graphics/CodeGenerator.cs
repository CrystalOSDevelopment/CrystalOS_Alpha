using System.Runtime.CompilerServices;
using System.Text;

namespace CrystalOSAlpha.Programming.CrystalSharp.Graphics
{
    public class CodeGenerator
    {
        public static string GenerateBase()
        {
            string code = "Main\n" +
                            "{\n" +
                                "this.x = 30;\n" +
                                "this.y = 40;\n" +
                                "this.width = 800;\n" +
                                "this.height = 600;\n" +
                                "this.title = \"Main Window\";\n" +
                                "this.color = 60, 60, 60;\n" +
                                "this.AlwaysOnTop = true;\n" +
                            "}";
            
            return code;
        }

        public static string ModifyCode(string code, string Propety, string Value)
        {
            string[] lines = code.Split('\n');

            // Find the property and replace it with the new value
            bool Found = false;
            for(int i = 0; i < lines.Length && Found == false; i++)
            {
                string Export = lines[i].Trim();
                if (Export.ToLower().Contains(Propety.Split('.')[1].ToLower()))
                {
                    lines[i] = "this." + Propety.Split('.')[1].ToLower() + " = " + Value + ";";
                    Found = true;
                }
            }

            // Rebuild the code
            string Output = "";
            foreach (string line in lines)
            {
                Output += line + "\n";
            }
            Output = Output.Trim('\n');
            return Output;
        }
    }
}