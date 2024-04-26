using Cosmos.System.Graphics;
using System;

namespace CrystalOSAlpha.Graphics.Widgets
{
    public class Base
    {
        public static int width = 0;
        public static int height = 0;
        public static Bitmap canvas;
        public static Bitmap Widget_Back(int width, int height, int CurrentColor)
        {
            canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
            Array.Fill(canvas.RawData, 0);
            Base.width = width;
            Base.height = height;

            ImprovedVBE.DrawFilledEllipse(canvas, 10, 10, 10, 10, CurrentColor);
            ImprovedVBE.DrawFilledEllipse(canvas, width - 11, 10, 10, 10, CurrentColor);
            ImprovedVBE.DrawFilledEllipse(canvas, 10, height - 10, 10, 10, CurrentColor);
            ImprovedVBE.DrawFilledEllipse(canvas, width - 11, height - 10, 10, 10, CurrentColor);

            ImprovedVBE.DrawFilledRectangle(canvas, CurrentColor, 0, 10, width, height - 20, false);
            ImprovedVBE.DrawFilledRectangle(canvas, CurrentColor, 5, 0, width - 10, 15, false);
            ImprovedVBE.DrawFilledRectangle(canvas, CurrentColor, 5, height - 15, width - 10, 15, false);
            return canvas;
        }
        public static void Widget_Back(Bitmap back, int width, int height, int CurrentColor)
        {
            Base.width = (int)back.Width;
            Base.height = (int)back.Height;

            ImprovedVBE.DrawFilledEllipse(back, 10, 10, 10, 10, CurrentColor);
            ImprovedVBE.DrawFilledEllipse(back, width - 11, 10, 10, 10, CurrentColor);
            ImprovedVBE.DrawFilledEllipse(back, 10, height - 10, 10, 10, CurrentColor);
            ImprovedVBE.DrawFilledEllipse(back, width - 11, height - 10, 10, 10, CurrentColor);

            ImprovedVBE.DrawFilledRectangle(back, CurrentColor, 0, 10, width, height - 20, false);
            ImprovedVBE.DrawFilledRectangle(back, CurrentColor, 5, 0, width - 10, 15, false);
            ImprovedVBE.DrawFilledRectangle(back, CurrentColor, 5, height - 15, width - 10, 15, false);
        }
    }
}
