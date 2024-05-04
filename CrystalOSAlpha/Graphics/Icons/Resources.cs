using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;

namespace CrystalOSAlpha.Graphics.Icons
{
    class Resources
    {
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Icons.Settings.bmp")] public static byte[] Sett;
        public static Bitmap Settings = new Bitmap(Sett);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Icons.Calculator.bmp")] public static byte[] Calc;
        public static Bitmap Calculator = new Bitmap(Calc);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Icons.Notepad.bmp")] public static byte[] Note;
        public static Bitmap Notepad = new Bitmap(Note);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Icons.Gameboy.bmp")] public static byte[] Game;
        public static Bitmap Gameboy = new Bitmap(Game);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Icons.Folder.bmp")] public static byte[] Fold;
        public static Bitmap Folder = new Bitmap(Fold);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Icons.File.bmp")] public static byte[] Fil;
        public static Bitmap File = new Bitmap(Fil);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Icons.Explorer.bmp")] public static byte[] Expl;
        public static Bitmap Explorer = new Bitmap(Expl);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Icons.Terminal.bmp")] public static byte[] Term;
        public static Bitmap Terminal = new Bitmap(Term);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Icons.WebBrowser.bmp")] public static byte[] WEB;
        public static Bitmap Web = new Bitmap(WEB);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Icons.IDE.bmp")] public static byte[] ide;
        public static Bitmap IDE = new Bitmap(ide);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Icons.Clock.bmp")] public static byte[] clock;
        public static Bitmap Clock = new Bitmap(clock);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Icons.Patterngenerator.bmp")] public static byte[] ptg;
        public static Bitmap PTG = new Bitmap(ptg);

        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Icons.celebrate.bmp")] public static byte[] Celeb;
        public static Bitmap Celebration = new Bitmap(Celeb);
    }
}
