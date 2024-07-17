using System.Collections.Generic;

namespace CrystalOSAlpha.Programming.CrystalSharp.Graphics
{
    public class CodeChunk
    {
        public static List<string> Chunkification(string code)
        {
            List<string> chunks = new List<string>();
            string[] lines = code.Split('\n');
            string chunk = "";
            foreach (string line in lines)
            {
                if (line == "-----||-----")
                {
                    chunks.Add(chunk.Trim('\n'));
                    chunk = "";
                }
                else
                {
                    chunk += line + "\n";
                }
            }
            if(chunk != "")
            {
                chunks.Add(chunk.Trim('\n'));
            }
            return chunks;
        }
    }

    public class Separated
    {
        public string Header { get; set; }
        public string Content { get; set; }
        public Separated(string header, string content)
        {
            Header = header;
            Content = content;
        }
    }
}
