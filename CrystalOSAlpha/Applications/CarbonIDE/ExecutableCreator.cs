using CrystalOSAlpha.Programming;
using System;
using System.Collections.Generic;
using System.IO;

namespace CrystalOSAlpha.Applications.CarbonIDE
{
    public class ExecutableCreator
    {
        public string Output = "";
        /*
         * Example MakeFile:
         * INCLUDE:
         * Main.wlf
         * Main2.wlf
         * Base.cs
         * 
         * SGN:Y
         * 
         * PBLSHR:Admin
         * 
         * STRTWNDW:Main
         */
        public string CreateExecutable(string WorkingDirector, string MakeFileContent)
        {
            string[] delimiters = new string[] { "DATE:", "VER:", "ICON:", "INCLUDE:", "SGN:", "PBLSHR:" , "STRTWNDW:" };
            List<string> Segments = CodeGenerator.ToList(MakeFileContent.Split(delimiters, StringSplitOptions.RemoveEmptyEntries));
            switch(Segments.Count)
            {
                case 7:
                    foreach(string FilesToInclude in Segments[3].Split('\n'))
                    {
                        if(FilesToInclude.Contains('.'))
                        {
                            switch (FilesToInclude.Split('.')[1])
                            {
                                case "wlf":
                                    if (Segments[^1].Trim() == FilesToInclude.Split('.')[0])
                                    {
                                        Output = Output.Insert(0, "-----||-----\n" + FilesToInclude + "\n" + File.ReadAllText(WorkingDirector + "\\Window_Layout\\" + FilesToInclude) + "\n");
                                    }
                                    else
                                    {
                                        Output += "-----||-----\n" + FilesToInclude + "\n" + File.ReadAllText(WorkingDirector + "\\Window_Layout\\" + FilesToInclude) + "\n";
                                    }
                                    break;
                                case "cs":
                                    Output += "-----||-----\n" + FilesToInclude + "\n" + File.ReadAllText(WorkingDirector + "\\Scripts\\" + FilesToInclude) + "\n";
                                    break;
                            }
                        }
                    }
                    break;
                case 4:
                    foreach (string FilesToInclude in Segments[0].Split('\n'))
                    {
                        if (FilesToInclude.Contains('.'))
                        {
                            switch (FilesToInclude.Split('.')[1])
                            {
                                case "wlf":
                                    if (Segments[^1] == FilesToInclude.Split('.')[0])
                                    {
                                        Output = Output.Insert(0, "-----||-----\n" + FilesToInclude + "\n" + File.ReadAllText(WorkingDirector + "\\Window_Layout\\" + FilesToInclude) + "\n");
                                    }
                                    else
                                    {
                                        Output += "-----||-----\n" + FilesToInclude + "\n" + File.ReadAllText(WorkingDirector + "\\Window_Layout\\" + FilesToInclude) + "\n";
                                    }
                                    break;
                                case "cs":
                                    Output += "-----||-----\n" + FilesToInclude + "\n" + File.ReadAllText(WorkingDirector + "\\Scripts\\" + FilesToInclude) + "\n";
                                    break;
                            }
                        }
                    }
                    break;
            }
            Output = Output.Remove(0, "-----||-----\n".Length);
            return Output;
        }
    }
}
