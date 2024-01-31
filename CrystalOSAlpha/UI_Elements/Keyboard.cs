using Cosmos.System;
using CrystalOS_Alpha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kernel = CrystalOS_Alpha.Kernel;

namespace CrystalOSAlpha.UI_Elements
{
    class Keyboard
    {
        public static string HandleKeyboard(string input, KeyEvent key)
        {
            string temp = input;
            if(Kernel.Is_KeyboardMouse == false)
            {
                switch (key.Key)
                {
                    case ConsoleKeyEx.Enter:
                        temp += "\n";
                        break;
                    case ConsoleKeyEx.Backspace:
                        if (temp.Length != 0)
                        {
                            temp = temp.Remove(temp.Length - 1);
                        }
                        break;
                    case ConsoleKeyEx.Tab:
                        temp += "    ";
                        break;
                    case ConsoleKeyEx.F1:
                        Kernel.Is_KeyboardMouse = true;
                        break;
                    default:
                        temp += Keyboard_HU(key);//key.KeyChar;
                        break;
                }
            }
            else
            {
                switch(key.Key)
                {
                    case ConsoleKeyEx.Num8:
                        MouseManager.Y -= 4;
                        break;
                    case ConsoleKeyEx.Num2:
                        MouseManager.Y += 4;
                        break;
                    case ConsoleKeyEx.Num4:
                        MouseManager.X -= 4;
                        break;
                    case ConsoleKeyEx.Num6:
                        MouseManager.X += 4;
                        break;
                    case ConsoleKeyEx.F1:
                        Kernel.Is_KeyboardMouse = false;
                        break;
                }
            }
            return temp;
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
                    case ConsoleKeyEx.D7:
                        return '=';
                    case ConsoleKeyEx.D6:
                        return '/';
                    case ConsoleKeyEx.D5:
                        return '%';
                    case ConsoleKeyEx.D4:
                        return '!';
                    case ConsoleKeyEx.D2:
                        return '\"';
                    case ConsoleKeyEx.D1:
                        return '\'';
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
