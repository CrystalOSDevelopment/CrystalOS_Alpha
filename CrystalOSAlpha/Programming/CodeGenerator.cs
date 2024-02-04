using CrystalOS_Alpha;
using CrystalOSAlpha.SystemApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOSAlpha.Programming
{
    class CodeGenerator
    {
        public static string Generate(string input, string newComponent)
        {
            string Modified_Code = input;
            if(Modified_Code == "")
            {
                Modified_Code = "#Define Window_Main\n" +
                                "{\n" +
                                "    this.Title = \"New window\";\n" +
                                "    this.Width = 500;\n" +
                                "    this.Height = 300;\n" +
                                "    this.Titlebar = true;\n" +
                                "}";
            }
            if (newComponent != "")
            {
                List<string> Code = Separate(Modified_Code);
                string temp = "";
                //Kernel.Clipboard = Code[0];
                string[] block = Code[0].Split('\n');
                if (newComponent.StartsWith("Button"))
                {
                    for(int i = 0; i < block.Length; i++)
                    {
                        if(i < block.Length - 1)
                        {
                            temp += block[i] + "\n";
                        }
                        else
                        {
                            temp += "    " + newComponent + "\n";
                            temp += block[i] + "\n";
                        }
                    }
                }
                //Kernel.Clipboard += "\n" + temp;
                Modified_Code = temp;
            }
            return Modified_Code;
        }
        public static List<string> Separate(string In)
        {
            List<string> parts = new List<string>();

            string current = "";
            int openedRemainingBrackets = 0;
            bool Started = false;
            for (int i = 0; i < In.Length; i++)
            {
                current += In[i];
                if (In[i] == '{')
                {
                    openedRemainingBrackets++;
                    Started = true;
                }
                else if (In[i] == '}')
                {
                    openedRemainingBrackets--;
                }
                if (i != 0 && openedRemainingBrackets == 0 && Started == true)
                {
                    parts.Add(current);
                    Started = false;
                    current = "";
                }
            }
            return parts;
        }
    }
}
