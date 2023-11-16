using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOSAlpha.Graphics.Icons
{
    class Resources
    {
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Icons.Settings.bmp")] public static byte[] Sett;
        public static Bitmap Settings = new Bitmap(Sett);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Icons.Calculator.bmp")] public static byte[] Calc;
        public static Bitmap Calculator = new Bitmap(Calc);
    }
}
