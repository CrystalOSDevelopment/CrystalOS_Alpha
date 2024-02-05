﻿using CrystalOS_Alpha;
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
                                "    this.RGB = 255, 255, 255;\n" +
                                "}";
            }
            if (newComponent != "")
            {
                List<string> Code = Separate(Modified_Code);
                string temp = "";
                //Kernel.Clipboard = Code[0];
                string[] block = Code[0].Split('\n');
                if (newComponent.StartsWith("this."))
                {
                    if (newComponent.Trim().StartsWith("this.Width"))
                    {
                        //Kernel.Clipboard = "It comes here just fine";
                        for (int i = 0; i < block.Length; i++)
                        {
                            if (block[i].Trim().StartsWith("this.Width"))
                            {
                                string[] clean = newComponent.Split("=");
                                if (int.TryParse(clean[1].Trim().Replace(";", ""), out int t))
                                {
                                    if(t >= 50)
                                    {
                                        block[i] = "    this.Width = " + clean[1].Trim();
                                    }
                                }
                            }
                        }
                    }
                    else if (newComponent.StartsWith("this.Height"))
                    {
                        for (int i = 0; i < block.Length; i++)
                        {
                            if (block[i].Trim().StartsWith("this.Height"))
                            {
                                string[] clean = newComponent.Split("=");
                                if (int.TryParse(clean[1].Trim().Replace(";", ""), out int t))
                                {
                                    if (t >= 50)
                                    {
                                        block[i] = "    this.Height = " + clean[1].Trim();
                                    }
                                }
                            }
                        }
                    }
                    else if (newComponent.StartsWith("this.RGB"))
                    {
                        for (int i = 0; i < block.Length; i++)
                        {
                            if (block[i].Trim().StartsWith("this.RGB"))
                            {
                                string[] clean = newComponent.Split("=");
                                //Kernel.Clipboard = clean[1];
                                string[] miert = clean[1].Split(", ");
                                if (miert.Length == 3)
                                {
                                    if (int.TryParse(miert[^1].Replace(";", ""), out int num))
                                    {
                                        block[i] = "    this.RGB = " + clean[1].Trim();
                                    }
                                }
                                //block[i] = "    this.RGB = 60, 60, 60;";
                            }
                        }
                    }
                    else if (newComponent.StartsWith("this.Titlebar"))
                    {
                        for (int i = 0; i < block.Length; i++)
                        {
                            if (block[i].Trim().StartsWith("this.Titlebar"))
                            {
                                string[] clean = newComponent.Split("=");
                                block[i] = "    this.Titlebar = " + clean[1].Trim();
                            }
                        }
                    }
                }
                else if (newComponent.StartsWith("Button"))
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
                if(temp.Length == 0)
                {
                    for (int i = 0; i < block.Length; i++)
                    {
                        if (i < block.Length - 1)
                        {
                            temp += block[i] + "\n";
                        }
                        else
                        {
                            temp += block[i];
                        }
                    }
                }
                //Kernel.Clipboard = "\n" + temp;
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
