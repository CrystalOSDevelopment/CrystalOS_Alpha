using Cosmos.Core;
using Cosmos.Core.Memory;
using Cosmos.HAL;
using Cosmos.System;
using CrystalOS_Alpha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kernel = CrystalOS_Alpha.Kernel;

namespace CrystalOSAlpha.Applications.Terminal
{
    class CommandLibrary
    {
        public static int offset = 0;
        public static string output = "";

        public static List<Value_Cases> memory = new List<Value_Cases>();
        public static string Execute(string input)
        {
            if (input.ToLower().StartsWith("echo "))
            {
                output = input.Remove(0, 5);
                offset = 1;
            }
            else if (input.ToLower().StartsWith("date"))
            {
                output = DateTime.Now.Year.ToString();
                output += ". ";
                output += DateTime.Now.Month.ToString();
                output += ". ";
                output += DateTime.Now.Day.ToString();
                output += ".";
                offset = 1;
            }
            else if (input.ToLower().StartsWith("time"))
            {
                output = DateTime.Now.Hour.ToString();
                output += ":";
                output += DateTime.Now.Minute.ToString();
                output += ":";
                output += DateTime.Now.Second.ToString();
                offset = 1;
            }
            else if (input.ToLower().StartsWith("version"))
            {
                output = "CrystalOS Alpha Edition\n";
                output += "Release date: 2023. 11. 21.\n\n";
                output += "About System:\n";
                output += "    Processor: " + CPU.GetCPUBrandString() + "\n";
                output += "    Available RAM: " + CPU.GetAmountOfRAM() + "MB\n";
                output += "    Used RAM: " + (CPU.GetEndOfKernel() + 1024) / 1048576 + "MB\n";
                offset = 3;
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
                        output = memory.First(d => d.Name == temp).StringValue;
                    }
                    else if (temp.StartsWith("int "))
                    {
                        temp = temp.Remove(0, 4).TrimStart();
                        output = memory.First(d => d.Name == temp).IntValue.ToString();
                    }
                }
                else if(temp.ToLower().StartsWith("clear"))
                {
                    memory.Clear();
                    output = "Memory cleared successfuly!";
                }
                offset = 1;
            }
            else if (input.ToLower().StartsWith("diskspace"))
            {
                output = "Free space: " + Kernel.fs.GetAvailableFreeSpace(@"0:\") / (1024 * 1024) + "MB";
                offset = 1;
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
                offset = 1;
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
