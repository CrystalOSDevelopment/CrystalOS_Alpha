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
                case ConsoleKeyEx.Tab:
                    input += "    ";
                    break;
                default:
                    input += Keyboard_HU(key);
                    break;
            }
            return input;
        }

        public static char Keyboard_HU(KeyEvent key)
        {
            if(KeyboardManager.ShiftPressed == true)
            {
                switch (key.Key)
                {
                    case ConsoleKeyEx.Backquote:
                        return '§';
                    case ConsoleKeyEx.D0:
                        return 'Ö';
                    case ConsoleKeyEx.D9:
                        return ')';
                    case ConsoleKeyEx.D8:
                        return '(';
                    case ConsoleKeyEx.D4:
                        return '!';
                    case ConsoleKeyEx.D1:
                        return '\'';
                    case ConsoleKeyEx.D2:
                        return '\"';
                    case ConsoleKeyEx.Z:
                        return 'Y';
                    case ConsoleKeyEx.Y:
                        return 'Z';
                    default:
                        return key.KeyChar;

                }
            }
            if(KeyboardManager.AltPressed == true)
            {
                switch (key.Key)
                {
                    case ConsoleKeyEx.V:
                        return '@';
                }
            }
            switch (key.Key)
            {
                case ConsoleKeyEx.Backquote:
                    return '0';
                case ConsoleKeyEx.D0:
                    return 'ö';
                case ConsoleKeyEx.Z:
                    return 'y';
                case ConsoleKeyEx.Y:
                    return 'z';
                default:
                    return key.KeyChar;

            }
        }
    }
}
