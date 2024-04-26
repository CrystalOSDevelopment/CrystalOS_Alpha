using System.Text;

namespace CrystalOSAlpha.Programming
{
    class WhitespaceRemover
    {
        public static string Remover(string input)
        {
            StringBuilder output = new StringBuilder();
            bool inQuotes = false;

            foreach (char c in input)
            {
                if (c == '\"')
                {
                    inQuotes = !inQuotes;
                }

                if (!char.IsWhiteSpace(c) || inQuotes)
                {
                    output.Append(c);
                }
            }
            return output.ToString();
        }
    }
}
