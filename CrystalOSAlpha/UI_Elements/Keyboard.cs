using Cosmos.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOSAlpha.UI_Elements
{
    class Keyboard
    {
        public static string HandleKeyboard(string input, KeyEvent key)
        {
            switch (key.Key)
            {
                case ConsoleKeyEx.Enter:
                    input += "\n";
                    break;
                case ConsoleKeyEx.Backspace:
                    if (input.Length != 0)
                    {
                        input = input.Remove(input.Length - 1);
                    }
                    break;
                default:
                    input += key.KeyChar;
                    break;
            }
            return input;
        }
    }
}
