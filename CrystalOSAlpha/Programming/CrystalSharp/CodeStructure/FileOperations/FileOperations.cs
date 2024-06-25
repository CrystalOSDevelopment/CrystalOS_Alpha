using CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Variables;
using System.Collections.Generic;

namespace CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.FileOperations
{
    public class FileOperations
    {
        public static string ReadAllText(string path)
        {
            if (!System.IO.File.Exists(path.Replace("\"", "")))
            {
                throw new System.IO.FileNotFoundException("File not found", path);
            }
            return System.IO.File.ReadAllText(path.Replace("\"", ""));
        }

        public static string WriteAllText(string path, List<Variable> variables)
        {
            //Separating the input parameters
            int FirstIndex = path.IndexOf(",");
            //File path
            string FileName = path.Substring(0, FirstIndex).Trim();
            if(FileName.Contains("\""))
            {
                FileName = FileName.Substring(1, FileName.Length - 2);
            }
            else
            {
                FileName = StringUnifier.GetFileName(FileName, variables);
            }

            //File content
            string Content = path.Substring(FirstIndex + 1).Trim();

            if(!System.IO.File.Exists(FileName))
            {
                System.IO.File.Create(FileName);
            }
            System.IO.File.WriteAllText(FileName, StringUnifier.GetFileName(Content, variables));
            return FileName + "\n" + StringUnifier.GetFileName(Content, variables);
        }
    }
}
