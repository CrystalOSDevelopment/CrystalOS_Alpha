using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOSAlpha.Applications.Minecraft
{
    public class Inventory
    {
        #region blocks
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.Icons.DIRT.bmp")] public static byte[] Dir;
        public static Bitmap Dirt = new Bitmap(Dir);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.Icons.DIAMOND.bmp")] public static byte[] Dia;
        public static Bitmap Diamond = new Bitmap(Dia);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.Icons.BRICK.bmp")] public static byte[] BRICKS;
        public static Bitmap Bricks = new Bitmap(BRICKS);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.Icons.BOOKSHELF.bmp")] public static byte[] BOOKSHELF;
        public static Bitmap Bookshelf = new Bitmap(BOOKSHELF);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.Icons.STONE.bmp")] public static byte[] STONE;
        public static Bitmap Stone = new Bitmap(STONE);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.Icons.WOOD.bmp")] public static byte[] WOOD;
        public static Bitmap Wood = new Bitmap(WOOD);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.Icons.LEAVES.bmp")] public static byte[] LEAVES;
        public static Bitmap Leaves = new Bitmap(LEAVES);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.Icons.AMETHYST.bmp")] public static byte[] AMETHYST;
        public static Bitmap Amethyst = new Bitmap(AMETHYST);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.Icons.EMERALD.bmp")] public static byte[] EMERALD;
        public static Bitmap Emerald = new Bitmap(EMERALD);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.Icons.GOLD.bmp")] public static byte[] GOLD;
        public static Bitmap Gold = new Bitmap(GOLD);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.Icons.IRON.bmp")] public static byte[] IRON;
        public static Bitmap Iron = new Bitmap(IRON);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.Icons.CACTUS.bmp")] public static byte[] CACTUS;
        public static Bitmap Cactus = new Bitmap(CACTUS);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.Icons.CHEST.bmp")] public static byte[] CHEST;
        public static Bitmap Chest = new Bitmap(CHEST);

        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.Icons.FURNACE.bmp")] public static byte[] FURNACE;
        public static Bitmap Furnace = new Bitmap(FURNACE);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.Icons.ICE.bmp")] public static byte[] ICE;
        public static Bitmap Ice = new Bitmap(ICE);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.Icons.LAVA.bmp")] public static byte[] LAVA;
        public static Bitmap Lava = new Bitmap(LAVA);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.Icons.MELON.bmp")] public static byte[] MELON;
        public static Bitmap Melon = new Bitmap(MELON);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.Icons.PLANKS.bmp")] public static byte[] PLANKS;
        public static Bitmap Planks = new Bitmap(PLANKS);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.Icons.SAND.bmp")] public static byte[] SAND;
        public static Bitmap Sand = new Bitmap(SAND);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.Icons.TNT.bmp")] public static byte[] TNT;
        public static Bitmap Tnt = new Bitmap(TNT);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.Icons.WATER.bmp")] public static byte[] WATER;
        public static Bitmap Water = new Bitmap(WATER);

        public List<Bitmap> BlockList = new List<Bitmap> { Dirt, Diamond, Bricks, Bookshelf, Stone, Wood, Leaves, Amethyst, Emerald, Gold, Iron, Cactus, Chest, Furnace, Ice, Lava, Melon, Planks, Sand, Tnt, Water };
        #endregion blocks

        //Player
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.Icons.PLAYER.bmp")] public static byte[] PLAYER;
        public static Bitmap Player = new Bitmap(PLAYER);
        //
        public static int Cursor = 0;
        public Bitmap Inv = new Bitmap(300, 300, ColorDepth.ColorDepth32);
        public int width = 300;
        public int height = 300;

        public Bitmap Render(int Width, int Height, int[] back, int health, int hunger, int level)
        {
            width = Width;
            height = Height;
            Inv = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);

            DrawFilledEllipse(10, 10, 10, 10, ImprovedVBE.colourToNumber(114, 114, 114));
            DrawFilledEllipse(width - 11, 10, 10, 10, ImprovedVBE.colourToNumber(114, 114, 114));
            DrawFilledEllipse(10, height - 10, 10, 10, ImprovedVBE.colourToNumber(114, 114, 114));
            DrawFilledEllipse(width - 11, height - 10, 10, 10, ImprovedVBE.colourToNumber(114, 114, 114));

            DrawFilledRectangle(ImprovedVBE.colourToNumber(114, 114, 114), 0, 10, width, height - 20);
            DrawFilledRectangle(ImprovedVBE.colourToNumber(114, 114, 114), 5, 0, width - 10, 15);
            DrawFilledRectangle(ImprovedVBE.colourToNumber(114, 114, 114), 5, height - 15, width - 10, 15);

            Apply_Transparency(back, ImprovedVBE.colourToNumber(114, 114, 114));

            //The Title.
            
            Graphics.Engine.BitFont.DrawBitFontString(Inv, "ArialCustomCharset16", Global_integers.c, "INVENTORY", width / 2 - 9 * 13 / 2, 5);
            Graphics.Engine.BitFont.DrawBitFontString(Inv, "ArialCustomCharset16", Global_integers.c, "PLAYER", width - 150, 30);
            Graphics.Engine.BitFont.DrawBitFontString(Inv, "ArialCustomCharset16", Global_integers.c, "Steve", width - 142, 50);

            DrawImageAlpha2(Player, width - 145, 65);

            Graphics.Engine.BitFont.DrawBitFontString(Inv, "ArialCustomCharset16", Global_integers.c, "Health: " + health, width - 185, 180);
            Graphics.Engine.BitFont.DrawBitFontString(Inv, "ArialCustomCharset16", Global_integers.c, "Food:  " + hunger, width - 185, 200);
            Graphics.Engine.BitFont.DrawBitFontString(Inv, "ArialCustomCharset16", Global_integers.c, "Level:  " + level, width - 185, 220);
            
            //The grid array using for loops

            int x_axis = 10;
            int y_axis = 30;
            for (int i = 0; i < 64; i++)
            {
                //Draw the backlighting
                if (Cursor == i)
                {
                    DrawFilledRectangle(ImprovedVBE.colourToNumber(184, 184, 184), x_axis, y_axis, 50, 50);
                }
                //Draw the icon
                if (i < BlockList.Count)
                {
                    DrawImageAlpha2(BlockList[i], x_axis + 10, y_axis + 10);
                }
                //Extend or draw caracter. Either way I need to extend the window size... Later...!!!
                if (x_axis > 410)
                {
                    y_axis += 60;
                    x_axis = 10;
                }
                else
                {
                    x_axis += 60;
                }
            }

            //TODO: Add the icons of the blocks and a cursor manager to manage the selected icon!

            return Inv;
        }

        public void DrawFilledEllipse(int xCenter, int yCenter, int yR, int xR, int color)
        {
            /*
            int r = (color & 0xff0000) >> 16;
            int g = (color & 0x00ff00) >> 8;
            int b = (color & 0x0000ff);

            float blendFactor = 0.5f;
            float inverseBlendFactor = 1 - blendFactor;
            */
            for (int y = -yR; y <= yR; y++)
            {
                for (int x = -xR; x <= xR; x++)
                {
                    if (x * x * yR * yR + y * y * xR * xR <= yR * yR * xR * xR)
                    {
                        /*
                        int r3 = (cover.RawData[(yCenter + y) * width + xCenter + x] & 0xff0000) >> 16;
                        int g3 = (cover.RawData[(yCenter + y) * width + xCenter + x] & 0x00ff00) >> 8;
                        int b3 = (cover.RawData[(yCenter + y) * width + xCenter + x] & 0x0000ff);
                        //Color c = Color.FromArgb(cover.RawData[j * width + i]);

                        int r2 = (int)(inverseBlendFactor * r3 + blendFactor * r);
                        int g2 = (int)(inverseBlendFactor * g3 + blendFactor * g);
                        int b2 = (int)(inverseBlendFactor * b3 + blendFactor * b);
                        */
                        if (xCenter + x > 0 && xCenter + x < width - 1 && yCenter + y > 0 && yCenter + y < height)
                        {
                            Inv.RawData[(yCenter + y) * width + xCenter + x] = color;
                        }

                        //DrawPixelfortext(xCenter + x, yCenter + y, GetGradientColor(x, 0, width, height));

                        //DrawPixelfortext(xCenter + x, yCenter + y, color);
                    }
                }
            }
        }

        public void DrawFilledRectangle(int color, int X, int Y, int Width, int Height)
        {
            /*
            int r = (color & 0xff0000) >> 16;
            int g = (color & 0x00ff00) >> 8;
            int b = (color & 0x0000ff);

            float blendFactor = 0.5f;
            float inverseBlendFactor = 1 - blendFactor;

            for (int j = Y; j < Y + Height; j++)
            {
                for (int i = X; i < X + Width; i++)
                {
                    int r3 = (cover.RawData[j * width + i] & 0xff0000) >> 16;
                    int g3 = (cover.RawData[j * width + i] & 0x00ff00) >> 8;
                    int b3 = (cover.RawData[j * width + i] & 0x0000ff);
                    //Color c = Color.FromArgb(cover.RawData[j * width + i]);

                    int r2 = (int)(inverseBlendFactor * r3 + blendFactor * r);
                    int g2 = (int)(inverseBlendFactor * g3 + blendFactor * g);
                    int b2 = (int)(inverseBlendFactor * b3 + blendFactor * b);

                    DrawPixelfortext(i, j, colourToNumber(r2, g2, b2));
                }
            }*/

            if (X < width && Y < height)
            {
                int[] line = new int[Width];
                if (X < 0)
                {
                    line = new int[Width + X];
                }
                else if (X + Width > width)
                {
                    line = new int[Width - (X + Width - width)];
                }
                Array.Fill(line, color);

                for (int i = Y; i < Y + Height; i++)
                {
                    Array.Copy(line, 0, Inv.RawData, i * width + X, line.Length);
                }
            }

        }
        public int GetGradientColor(int x, int y, int width, int height)
        {
            int r = (int)((double)x / width * 255);
            int g = (int)((double)y / height * 255);
            int b = 255;

            return ImprovedVBE.colourToNumber(r, g, b);
        }

        public void DrawImageAlpha2(Image image, int x, int y)
        {
            int counter = 0;
            for (int _y = y; _y < y + image.Height; _y++)
            {
                if (_y > 20)
                {
                    for (int _x = x; _x < x + image.Width; _x++)
                    {
                        if (_y < height)
                        {
                            if (_x < width && _x > 0)
                            {
                                if (image.RawData[counter] == 0)
                                {
                                    counter++;
                                }
                                else
                                {
                                    Inv.RawData[_y * width - (width - _x)] = image.RawData[counter];
                                    counter++;
                                }
                            }
                            else
                            {
                                counter++;
                            }
                        }
                        else
                        {
                            counter += (int)image.Width;
                        }
                    }
                }
                else
                {
                    counter += (int)image.Width;
                }
            }
        }

        public void Apply_Transparency(int[] input2, int color)
        {
            int r = (color & 0xff0000) >> 16;
            int g = (color & 0x00ff00) >> 8;
            int b = color & 0x0000ff;

            float blendFactor = 0.8f;
            float inverseBlendFactor = 1 - blendFactor;

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    if (Inv.RawData[j * width + i] != 0)
                    {
                        int r3 = (input2[(j + 41) * 800 + i + 20] & 0xff0000) >> 16;
                        int g3 = (input2[(j + 41) * 800 + i + 20] & 0x00ff00) >> 8;
                        int b3 = input2[(j + 41) * 800 + i + 20] & 0x0000ff;
                        //Color c = Color.FromArgb(cover.RawData[j * width + i]);

                        int r2 = (int)(inverseBlendFactor * r3 + blendFactor * r);
                        int g2 = (int)(inverseBlendFactor * g3 + blendFactor * g);
                        int b2 = (int)(inverseBlendFactor * b3 + blendFactor * b);

                        DrawPixelfortext(i, j, ImprovedVBE.colourToNumber(r2, g2, b2));
                    }
                }
            }
        }

        public void DrawPixelfortext(int x, int y, int color)
        {
            //16777215 white
            if (x > 0 && x < width && y > 0 && y < height)
            {
                Inv.RawData[y * width + x] = color;
            }
        }
    }
}
