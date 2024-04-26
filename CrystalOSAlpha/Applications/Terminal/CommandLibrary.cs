using Cosmos.Core;
using Cosmos.System;
using CrystalOSAlpha.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Kernel = CrystalOS_Alpha.Kernel;

namespace CrystalOSAlpha.Applications.Terminal
{
    class CommandLibrary
    {
        public static string output = "";

        public static List<Value_Cases> memory = new List<Value_Cases>();
        public static string Execute(string input)
        {
            if (input.ToLower().StartsWith("echo "))
            {
                output = input.Remove(0, 5);
            }
            else if (input.ToLower().StartsWith("help"))
            {
                output =
                    "Echo {text} - Prints out the text to the console\n" +
                    "Clear - Clears the screen\n" +
                    "Help - Show the full help list\n" +
                    "Date - Prints out the current date to the console(YYYY.MM.DD)\n" +
                    "Time - Prints out the current time to the console(HH.MM.SS)\n" +
                    "Version - Displays information about computer(Ex.: Neofetch in Linux)\n" +
                    "Mem - For details, check out the source code\n" +
                    "Diskspace - Shows how much diskspace is available\n" +
                    "Reboot/rbt - Reboot the machine\n" +
                    "Shutdown/std - Turn off the computer\n";
            }
            else if (input.ToLower().StartsWith("date"))
            {
                output = DateTime.Now.Year.ToString();
                output += ". ";
                output += DateTime.Now.Month.ToString();
                output += ". ";
                output += DateTime.Now.Day.ToString();
                output += ".";
            }
            else if (input.ToLower().StartsWith("time"))
            {
                output = DateTime.Now.Hour.ToString();
                output += ":";
                output += DateTime.Now.Minute.ToString();
                output += ":";
                output += DateTime.Now.Second.ToString();
            }
            else if (input.ToLower().StartsWith("version"))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("                                                            \n");
                sb.Append("                             .                              \n");
                sb.Append("                          ./((/,                            \n");
                sb.Append("                        ,///(((((/.                         \n");
                sb.Append("                     *((/////(((((((*                       \n");
                sb.Append("                   ,///////////(((((((/.                    \n");
                sb.Append("                .****////////////(((((((/*                  CrystalOS Alpha Edition \n");
                sb.Append("             .,..,**///////////////((((((((/.               Release date: 2024. 04. 25. \n");
                sb.Append("           ,*////,..*////////////////((#%%#(/,              \n");
                sb.Append("        .*//////////,.,//////////////(//((*..,,,,.          About System: \n");
                sb.Append("      ,****/((((((((/*. .*/////////////*..,******,,.            Processor: " + CPU.GetCPUBrandString() + " \n");
                sb.Append("   .,******///((((/,,(###*.,////////*,.,**********,,,,.         Available RAM: " + CPU.GetAmountOfRAM() + "MB \n");
                sb.Append("    ,****///////*.*########(..////*..*//////********,,          Used RAM: " + (CPU.GetEndOfKernel() + 1024) / 1048576 + "MB \n");
                sb.Append("      .*/////*,,*((###########*...*/(((////////****.            Resolution: " + ImprovedVBE.width.ToString() + "x" + ImprovedVBE.height.ToString() + "x32 \n");
                sb.Append("         ,*,.,////(((((#########(.,/((((((//////*                   Video driver: VESA BIOS Extension (VBE) \n");
                sb.Append("           .*/////////(((((########*.*(((((((/,             \n");
                sb.Append("              ,*/////////////(((#####/,,/((*.                   Username: " + GlobalValues.Username + " \n");
                sb.Append("                .*///////////((/(//(((((*                   \n");
                sb.Append("                   ,*/////////((#((///,                     \n");
                sb.Append("                     .*****/****///*.                       \n");
                sb.Append("                        ,********,                          \n");
                sb.Append("                          .,***.                            \n");
                sb.Append("                                                            \n");
                sb.Append("                                                            ");

                //output =
                //    "                                                            \n" +
                //    "                             .                              \n" +
                //    "                          ./((/,                            \n" +
                //    "                        ,///(((((/.                         \n" +
                //    "                     *((/////(((((((*                       \n" +
                //    "                   ,///////////(((((((/.                    \n" +
                //    "                .****////////////(((((((/*                  CrystalOS Alpha Edition \n" +
                //    "             .,..,**///////////////((((((((/.               Release date: 2024. 04. 25. \n" +
                //    "           ,*////,..*////////////////((#%%#(/,              \n" +
                //    "        .*//////////,.,//////////////(//((*..,,,,.          About System: \n" +
                //    "      ,****/((((((((/*. .*/////////////*..,******,,.            Processor: " + CPU.GetCPUBrandString()  + " \n" +
                //    "   .,******///((((/,,(###*.,////////*,.,**********,,,,.         Available RAM: " + CPU.GetAmountOfRAM() + "MB \n" +
                //    "    ,****///////*.*########(..////*..*//////********,,          Used RAM: " + (CPU.GetEndOfKernel() + 1024) / 1048576 + "MB \n" +
                //    "      .*/////*,,*((###########*...*/(((////////****.            Resolution: " + ImprovedVBE.width.ToString() + "x" + ImprovedVBE.height.ToString() + "x32 \n" +
                //    "         ,*,.,////(((((#########(.,/((((((//////*                   Video dirver: VESA BIOS Extension (VBE)\n" +
                //    "           .*/////////(((((########*.*(((((((/,             \n" +
                //    "              ,*/////////////(((#####/,,/((*.                   Username: " + GlobalValues.Username + " \n" +
                //    "                .*///////////((/(//(((((*                   \n" +
                //    "                   ,*/////////((#((///,                     \n" +
                //    "                     .*****/****///*.                       \n" +
                //    "                        ,********,                          \n" +
                //    "                          .,***.                            \n" +
                //    "                                                            \n" +
                //    "                                                            ";

                output = sb.ToString();
                if (VMTools.IsVMWare == true)
                {
                    output += "\nPowered by: VMWare\n" +
                        "                   (((((((((((((((((((((((((((((((((((      \n" +
                        "                 (((((((((((((((((((((((((((((((((((((((    \n" +
                        "                 (((((((((((((((((((((((((((((((((((((((,   \n" +
                        "                 (((((((((                     /((((((((,   \n" +
                        "                 ((((((((                       .(((((((,   \n" +
                        "      .**********************************,      .(((((((,   \n" +
                        "     **************************************,    .(((((((,   \n" +
                        "    /**************************************/    .(((((((,   \n" +
                        "    /********.   ######((          ********/    .(((((((,   \n" +
                        "    /*******     ((((((((           *******/    .(((((((,   \n" +
                        "    /*******     ((((((((           *******/    .(((((((,   \n" +
                        "    /*******     ((((((((           *******/    .(((((((,   \n" +
                        "    /*******     (((((((((.         *******/   (((((((((,   \n" +
                        "    /*******     (((((((((((((((((((((((((((((((((((((((,   \n" +
                        "    /*******     /((((((((((((((((((((((((((((((((((((((    \n" +
                        "    /*******       ,(((((((((((((((((((((((((((((((((/      \n" +
                        "    /*******                        ***/((((                \n" +
                        "    /*********                    ,********/                \n" +
                        "    /**************************************/                \n" +
                        "     **************************************.                \n" +
                        "       *********************************/                   \n" +
                        "                                                           ";
                }
            }
            else if (input.ToLower().StartsWith("mem "))
            {
                string temp = input.Remove(0, 4).TrimStart();
                if(temp.ToLower().StartsWith("string"))
                {
                    temp = temp.Remove(0, 6).TrimStart();
                    string[] cont = temp.Split(" : ");
                    memory.Add(new Value_Cases(cont[0], cont[1].Replace("\"", "")));
                    output = "Value successfuly saved!";
                }
                else if (temp.ToLower().StartsWith("int"))
                {
                    temp = temp.Remove(0, 3).TrimStart();
                    string[] cont = temp.Split(" : ");
                    memory.Add(new Value_Cases(cont[0], int.Parse(cont[1])));
                    output = "Value successfuly saved!";
                }
                else if (temp.ToLower().StartsWith("get"))
                {
                    temp = temp.Remove(0, 3).TrimStart();
                    if (temp.StartsWith("string "))
                    {
                        temp = temp.Remove(0, 7).TrimStart();
                        output = memory.FindAll(d => d.Name == temp)[0].StringValue;
                    }
                    else if (temp.StartsWith("int "))
                    {
                        temp = temp.Remove(0, 4).TrimStart();
                        output = memory.FindAll(d => d.Name == temp)[0].IntValue.ToString();
                    }
                }
                else if(temp.ToLower().StartsWith("clear"))
                {
                    memory.Clear();
                    output = "Memory cleared successfuly!";
                }
            }
            else if (input.ToLower().StartsWith("diskspace"))
            {
                output = "Free space: " + Kernel.fs.GetAvailableFreeSpace(@"0:\") / (1024 * 1024) + "MB";
            }
            else if (input.ToLower().StartsWith("reboot") || input.ToLower().StartsWith("rbt"))
            {
                Cosmos.System.Power.Reboot();
            }
            else if (input.ToLower().StartsWith("shutdown") || input.ToLower().StartsWith("std"))
            {
                Cosmos.System.Power.Shutdown();
            }
            else
            {
                output = "Cannot recognize command.";
            }
            return output;
        }
    }
    class Value_Cases
    {
        public string Name { get; set; }
        public string StringValue { get; set; }
        public int IntValue { get; set; }
        public void StringSet(string Name, string value)
        {
            this.Name = Name;
            this.StringValue = value;
        }
        public string StringGet(string Name, string value)
        {
            return this.StringValue;
        }
        public Value_Cases(string Name, string StringValue)
        {
            this.StringSet(Name, StringValue);
        }
        public Value_Cases(string Name, int IntValue)
        {
            this.Name = Name;
            this.IntValue = IntValue;
        }
    }
}
