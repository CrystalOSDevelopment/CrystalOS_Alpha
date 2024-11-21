using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using System;
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
            switch (GlobalValues.TaskBarType)
            {
                case "Classic":

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
                case "Nostalgia":

                    // Clear the canvas with a light gray background
                    Array.Fill(canvas.RawData, ImprovedVBE.colourToNumber(240, 240, 240));

                    // Draw a solid title bar (dark gray)
                    int[] PixelLine = new int[width];
                    int CurrentCol = ImprovedVBE.colourToNumber(250, 250, 250);
                    for (int Top = 0; Top < 20; Top++)
                    {
                        Array.Fill(PixelLine, CurrentCol);
                        Array.Copy(PixelLine, 0, canvas.RawData, Top * width, width);
                        if (Top > 10)
                        {
                            CurrentCol += ImprovedVBE.colourToNumber(5, 5, 5);
                        }
                        else
                        {
                            CurrentCol -= ImprovedVBE.colourToNumber(5, 5, 5);
                        }
                    }
                    ImprovedVBE.DrawLine(canvas, 0, 20, width, 20, ImprovedVBE.colourToNumber(80, 80, 80));

                    // Draw window control buttons on the title bar (basic circles)
                    ImprovedVBE.DrawFilledEllipse(canvas, width - 13, 10, 8, 8, ImprovedVBE.colourToNumber(255, 0, 0));
                    ImprovedVBE.DrawFilledEllipse(canvas, width - 34, 10, 8, 8, ImprovedVBE.colourToNumber(227, 162, 37));

                    // Draw window title text centered in the title bar
                    int textX = (width / 2) - (name.Length * 6) / 2; // Center text horizontally
                    BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.Black, name, textX, 0);

                    return (canvas, back_canvas, window); // Return updated bitmaps
                default:
                    return (canvas, back_canvas, window);
            }
        }
    }
}
