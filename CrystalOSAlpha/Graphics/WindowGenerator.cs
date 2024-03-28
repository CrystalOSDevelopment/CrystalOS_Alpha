using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using System.Drawing;

namespace CrystalOSAlpha.Graphics
{
    class WindowGenerator
    {
        public static (Bitmap, Bitmap, Bitmap) Generate(int x, int y, int width, int height, int CurrentColor, string name)
        {
            Bitmap canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
            Bitmap back_canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
            Bitmap window = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);

            #region corners
            ImprovedVBE.DrawFilledEllipse(canvas, 10, 10, 10, 10, CurrentColor);
            ImprovedVBE.DrawFilledEllipse(canvas, width - 11, 10, 10, 10, CurrentColor);
            ImprovedVBE.DrawFilledEllipse(canvas, 10, height - 10, 10, 10, CurrentColor);
            ImprovedVBE.DrawFilledEllipse(canvas, width - 11, height - 10, 10, 10, CurrentColor);

            ImprovedVBE.DrawFilledRectangle(canvas, CurrentColor, 0, 10, width, height - 20, false);
            ImprovedVBE.DrawFilledRectangle(canvas, CurrentColor, 5, 0, width - 10, 15, false);
            ImprovedVBE.DrawFilledRectangle(canvas, CurrentColor, 5, height - 15, width - 10, 15, false);
            #endregion corners

            canvas = ImprovedVBE.EnableTransparency(canvas, x, y, canvas);

            ImprovedVBE.DrawGradientLeftToRight(canvas);

            ImprovedVBE.DrawFilledEllipse(canvas, width - 13, 10, 8, 8, ImprovedVBE.colourToNumber(255, 0, 0));
            ImprovedVBE.DrawFilledEllipse(canvas, width - 34, 10, 8, 8, ImprovedVBE.colourToNumber(227, 162, 37));

            BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, name, 2, 2);
            return (canvas, back_canvas, window);
        }
    }
}
