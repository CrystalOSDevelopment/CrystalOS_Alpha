namespace CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Console
{
    public class WriteL
    {
        public static string WriteLine(string Input)
        {
            string Temp = Input.Replace("Console.WriteLine(", "");
            Temp = Temp.Remove(Temp.Length - 2);
            Temp = Temp.Trim();
            Temp = Temp.Remove(Temp.Length - 1).Remove(0, 1);
            Temp = Temp.Insert(0, "\n");
            return Temp;
        }
    }
}