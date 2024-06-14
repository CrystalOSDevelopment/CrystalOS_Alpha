using Cosmos.System;
using CrystalOSAlpha.System32;

namespace CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Console
{
    public class ReadL
    {
        public static void ReadLine(KeyEvent Key, ref string Input)
        {
            Input = Keyboard.HandleKeyboard(Input, Key);
        }
    }
}
