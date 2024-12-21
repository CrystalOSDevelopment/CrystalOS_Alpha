using Cosmos.System;
using CrystalOSAlpha.Applications.TaskManagerApp;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Icons;
using Kernel = CrystalOS_Alpha.Kernel;

namespace CrystalOSAlpha.System32
{
    class Keyboard
    {
        public static string HandleKeyboard(string input, KeyEvent key)
        {
            string temp = input;
            if (Kernel.Is_KeyboardMouse == false)
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
                    case ConsoleKeyEx.LWin:
                        foreach (var v in TaskScheduler.Apps)
                        {
                            if (v.name.Length > 2)
                            {
                                v.minimised = true;
                            }
                        }
                        break;
                    case ConsoleKeyEx.Escape:
                        if(KeyboardManager.ControlPressed == true)
                        {
                            //Hotkey for task manager
                            TaskScheduler.Apps.Add(new TaskManagerApp(100, 100, 999, 355, 300, "TaskManager", Resources.Celebration));
                        }
                        break;
                    default:
                        //TODO: Decide what keyboard layout to use
                        switch (GlobalValues.KeyboardLayout)
                        {
                            case KeyboardLayout.EN_US:
                                temp += key.KeyChar;
                                break;
                            case KeyboardLayout.HUngarian:
                                temp += Keyboard_HU(key);
                                break;
                        }
                        break;
                }
            }
            else
            {
                switch (key.Key)
                {
                    case ConsoleKeyEx.W:
                        MouseManager.Y -= 4;
                        break;
                    case ConsoleKeyEx.S:
                        MouseManager.Y += 4;
                        break;
                    case ConsoleKeyEx.A:
                        MouseManager.X -= 4;
                        break;
                    case ConsoleKeyEx.D:
                        MouseManager.X += 4;
                        break;
                    case ConsoleKeyEx.Q:
                        if (MouseManager.MouseState == MouseState.None)
                        {
                            MouseManager.MouseState = MouseState.Left;
                        }
                        else if(MouseManager.MouseState == MouseState.Left)
                        {
                            MouseManager.MouseState = MouseState.None;
                        }
                        break;
                    case ConsoleKeyEx.F1:
                        Kernel.Is_KeyboardMouse = false;
                        break;
                }
            }
            ImprovedVBE.RequestRedraw = true;       // After every key press, request a redraw
            return temp;
        }

        public static char Keyboard_HU(KeyEvent key)
        {
            if (KeyboardManager.ShiftPressed == true)
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
            if (KeyboardManager.AltPressed == true)
            {
                switch (key.Key)
                {
                    case ConsoleKeyEx.V:
                        return '@';
                    case ConsoleKeyEx.Comma:
                        return ';';
                    case ConsoleKeyEx.C:
                        return '&';
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
