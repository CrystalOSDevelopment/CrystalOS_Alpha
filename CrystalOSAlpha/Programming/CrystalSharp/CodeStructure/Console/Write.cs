namespace CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Console
{
    public class Write
    {
        public static string CWrite(string Input)
        {
            string Temp = Input.Replace("Console.Write(", "");
            Temp = Temp.Remove(Temp.Length - 2);
            Temp = Temp.Trim();
            Temp = Temp.Remove(Temp.Length - 1).Remove(0, 1);
            return Temp;
        }
    }
}