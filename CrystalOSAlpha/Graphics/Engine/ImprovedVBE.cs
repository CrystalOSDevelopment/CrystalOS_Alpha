using Cosmos.Core.IOGroup;
using Cosmos.HAL.Drivers;
using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOS_Alpha;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.TaskBar;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Image = Cosmos.System.Graphics.Image;
using Kernel = CrystalOS_Alpha.Kernel;

namespace CrystalOSAlpha
{
    public class ImprovedVBE
    {
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Engine.Wallp1.bmp")] public static byte[] VBECanvas;
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Engine.Arial_16.btf")] public static byte[] Arial;

        //These are black images matching the size exacly the resolution
        public static Bitmap cover = new Bitmap(VBECanvas);//The base canvas
        public static Bitmap data = new Bitmap(VBECanvas);//To reset the base canvas

        public static Bitmap Temp = new Bitmap(VBECanvas);//To reset the base canvas

        public static int width = 1920;
        public static int height = 1080;
        public static bool Res = true;

        public static void display(VBECanvas c)
        {
            c.DrawImage(cover, 0, 0);
            //clear(c, Global_Integers.c);
            //cover.RawData = data.RawData;
            clear(0);
            if(Res == true)
            {
                try
                {
                    data = ScaleImageStock(Temp, (uint)width, (uint)height);
                    cover = ScaleImageStock(Temp, (uint)width, (uint)height);

                    MouseManager.ScreenWidth = (uint)width;
                    MouseManager.ScreenHeight = (uint)height;

                    TaskManager.Top = ImprovedVBE.height;
                    TaskManager.Left = ImprovedVBE.width / 2 + 60;

                    Kernel.disable = true;

                    //width = 1920;
                    //height = 1080;

                    c.Clear(Color.Black);
                }
                catch
                {

                }

                Res = false;
            }
        }

        public static void clear(int col)
        {
            data.RawData.CopyTo(cover.RawData, col);
        }
        public static void DrawPixelfortext(Bitmap Canvas, int x, int y, int color)
        {
            //16777215 white
            /*
            if (x > 0 && x < width && y > 0 && y < height)
            {
                cover.RawData[y * width + x] = color;
            }
            */
            if (x > 0 && x < Canvas.Width && y > 0 && y < Canvas.Height)
            {
                Canvas.RawData[y * Canvas.Width + x] = color;
            }
        }

        public static void DrawFilledRectangle(Bitmap Canvas, int color, int X, int Y, int Width, int Height, bool transparent)
        {
            if (transparent == true)
            {
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

                        DrawPixelfortext(Canvas, i, j, colourToNumber(r2, g2, b2));
                    }
                }
            }
            else
            {
                if (X <= width)
                {
                    int[] line = new int[Width];
                    if (X < 0)
                    {
                        line = new int[Width + X];
                    }
                    else if (X + Width > Canvas.Width)
                    {
                        line = new int[Width - (X + Width - Canvas.Width)];
                    }
                    Array.Fill(line, color);

                    for (int i = Y; i < Y + Height; i++)
                    {
                        if(i < height)
                        {
                            Array.Copy(line, 0, Canvas.RawData, (i * Canvas.Width) + X, line.Length);
                        }
                    }
                }
            }
        }

        //Slower but can be improved with Array.Copy();
        public static void DrawImage(Image image, int x, int y)
        {
            int counter = 0;
            int prewy = y;
            for (int _y = y; _y < y + image.Height; _y++)
            {
                for (int _x = x; _x < x + image.Width; _x++)
                {
                    if ((_y * width) - (width - _x) < width * height)
                    {
                        if (_x > width || _x < 0)
                        {
                            //cover.RawData[((_y * width) - (width - _x))] = 0;
                            counter++;
                        }
                        else
                        {
                            cover.RawData[((_y * width) - (width - _x))] = image.RawData[counter];
                            counter++;
                        }
                    }
                }
                prewy++;
            }
        }
        public static void DrawImageArray(int Width, int Height, int[] RawData, int x, int y)
        {
            int counter = 0;
            int scan_line = 0;
            for (int _y = y; _y < y + Height; _y++)
            {
                int[] line = new int[Width];

                Array.Copy(RawData, scan_line * Width, line, 0, Width);

                if (line[0] != 0 || line[^1] != 0)
                {
                    line.CopyTo(cover.RawData, (_y - 1) * width + x);
                    //TODO: copy just a specific amount of length
                    counter += (int)Width;
                }
                else
                {
                    for (int _x = x; _x < x + Width; _x++)
                    {
                        if (_y <= height - 1)
                        {
                            if (_x <= width)
                            {
                                if (RawData[counter] == 0)
                                {
                                    counter++;
                                }
                                else
                                {
                                    cover.RawData[((_y * width) - (width - _x))] = RawData[counter];
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
                            counter += (int)Width;
                        }
                    }
                }
                scan_line++;
            }
        }

        public static Bitmap DrawImageAlpha2(Bitmap image, int x, int y, Bitmap output)
        {
            int r = Global_integers.R;
            int g = Global_integers.G;
            int b = Global_integers.B;

            float blendFactor = 0.85f;
            float inverseBlendFactor = 1 - blendFactor;

            int counter = 0;
            for (int _y = y; _y < y + image.Height; _y++)
            {
                for (int _x = x; _x < x + image.Width; _x++)
                {
                    if (_y <= height - 1)
                    {
                        if (_x <= width)
                        {
                            if (image.RawData[counter] == 0)
                            {
                                //output.RawData[counter] = cover.RawData[_y * width + _x];
                                counter++;
                            }
                            else
                            {
                                int r3 = (cover.RawData[_y * width + _x] & 0xff0000) >> 16;
                                int g3 = (cover.RawData[_y * width + _x] & 0x00ff00) >> 8;
                                int b3 = (cover.RawData[_y * width + _x] & 0x0000ff);
                                //Color c = Color.FromArgb(cover.RawData[j * width + i]);

                                int r2 = (int)(inverseBlendFactor * r3 + blendFactor * r);
                                int g2 = (int)(inverseBlendFactor * g3 + blendFactor * g);
                                int b2 = (int)(inverseBlendFactor * b3 + blendFactor * b);

                                //DrawPixelfortext(image, _x, _y, colourToNumber(r2, g2, b2));
                                output.RawData[counter] = colourToNumber(r2, g2, b2);
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
            if(y + output.Height < height)
            {
                output = Blur(output, 5);
            }
            return output;
        }

        public static Bitmap DrawImageAlpha2(Bitmap image, int x, int y, Bitmap output, int R, int G, int B)
        {
            int r = R;
            int g = G;
            int b = B;

            float blendFactor = 0.85f;
            float inverseBlendFactor = 1 - blendFactor;

            int counter = 0;
            for (int _y = y; _y < y + image.Height; _y++)
            {
                for (int _x = x; _x < x + image.Width; _x++)
                {
                    if (_y <= height - 1)
                    {
                        if (_x <= width)
                        {
                            if (image.RawData[counter] == 0)
                            {
                                //output.RawData[counter] = cover.RawData[_y * width + _x];
                                counter++;
                            }
                            else
                            {
                                int r3 = (cover.RawData[_y * width + _x] & 0xff0000) >> 16;
                                int g3 = (cover.RawData[_y * width + _x] & 0x00ff00) >> 8;
                                int b3 = (cover.RawData[_y * width + _x] & 0x0000ff);
                                //Color c = Color.FromArgb(cover.RawData[j * width + i]);

                                int r2 = (int)(inverseBlendFactor * r3 + blendFactor * r);
                                int g2 = (int)(inverseBlendFactor * g3 + blendFactor * g);
                                int b2 = (int)(inverseBlendFactor * b3 + blendFactor * b);

                                DrawPixelfortext(image, _x, _y, colourToNumber(r2, g2, b2));
                                output.RawData[counter] = colourToNumber(r2, g2, b2);
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
            output = Blur(output, 5);
            return output;
        }

        //Don't use this!
        public static Bitmap DrawImageAlpha(Bitmap image, int x, int y, Bitmap into)
        {
            int[] line = new int[image.Width];
            if (x < into.Width)
            {
                if (x < 0)
                {
                    line = new int[image.Width + x];
                    x = 0;
                }
                else if (x + image.Width > into.Width)
                {
                    line = new int[image.Width - (x + image.Width - into.Width)];
                }

                int counter = 0;
                int scan_line = 0;
                for (int _y = y; _y < y + image.Height; _y++)
                {
                    Array.Copy(image.RawData, scan_line * image.Width, line, 0, line.Length);

                    if (line[0] != 0 && line[^1] != 0)
                    {
                        if (_y < into.Height)
                        {
                            line.CopyTo(into.RawData, _y * into.Width + x);
                            counter += (int)image.Width;
                        }
                        else
                        {
                            counter += (int)image.Width;
                        }
                    }
                    else
                    {
                        for (int _x = x; _x < x + image.Width; _x++)
                        {
                            if (_y < into.Height)
                            {
                                if (_x <= into.Width)
                                {
                                    if (image.RawData[counter] == 0)
                                    {
                                        counter++;
                                    }
                                    else
                                    {
                                        into.RawData[_y * into.Width + _x] = image.RawData[counter];
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
                    scan_line++;
                }
            }
            return into;
        }

        public static void DrawImageAlpha3(Image image, int x, int y)
        {
            int counter = 0;
            for (int _y = y; _y < y + image.Height; _y++)
            {
                for (int _x = x; _x < x + image.Width; _x++)
                {
                    if (_y <= height - 1)
                    {
                        if (_x <= width)
                        {
                            if (image.RawData[counter] == 0)
                            {
                                counter++;
                            }
                            else
                            {
                                cover.RawData[((_y * width) - (width - _x))] = image.RawData[counter];
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
        }

        public static Bitmap ScaleImageStock(Bitmap image, uint newWidth, uint newHeight)
        {
            Bitmap pixels = image;

            int w1 = (int)image.Width;
            int h1 = (int)image.Height;
            Bitmap temp = new Bitmap(newWidth, newHeight, ColorDepth.ColorDepth32);
            int xRatio = (int)((w1 << 16) / newWidth) + 1;
            int yRatio = (int)((h1 << 16) / newHeight) + 1;
            int x2, y2;

            for (int i = 0; i < newHeight; i++)
            {
                for (int j = 0; j < newWidth; j++)
                {
                    x2 = (j * xRatio) >> 16;
                    y2 = (i * yRatio) >> 16;
                    temp.RawData[(i * newWidth) + j] = pixels.RawData[(y2 * w1) + x2];
                }
            }

            return temp;
        }

        public static Bitmap Blur(Bitmap image, int blurAmount)
        {
            for(int x = blurAmount; x < image.Width - blurAmount; x++)
            {
                for (int y = blurAmount; y < image.Height; y++)
                {
                    try
                    {
                        if((y + blurAmount) < height)
                        {
                            Color prevX = Color.FromArgb(image.RawData[y * image.Width + (x - 1)]);
                            Color nextX = Color.FromArgb(image.RawData[y * image.Width + (x + 1)]);
                            Color prevY = Color.FromArgb(image.RawData[(y - blurAmount) * image.Width + x]);
                            Color nextY = Color.FromArgb(image.RawData[(y + blurAmount) * image.Width + x]);

                            int avgR = (int)((prevX.R + nextX.R + prevY.R + nextY.R) / 4);
                            int avgG = (int)((prevX.G + nextX.G + prevY.G + nextY.G) / 4);
                            int avgB = (int)((prevX.B + nextX.B + prevY.B + nextY.B) / 4);

                            image.RawData[y * image.Width + x] = colourToNumber(avgR, avgG, avgB);
                        }
                    }
                    catch
                    {

                    }
                }
            }
            return image;
        }

        //static string ASC16Base64 = "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AAB4zMzM2MzGxsbMAAAAAAAA/sbGwMDAwMDAwAAAAAAAAAAA/mxsbGxsbGwAAAAAAAAA/sZgMBgwYMb+AAAAAAAAAAAAftjY2NjYcAAAAAAAAAAAZmZmZmZ8YGDAAAAAAAAAAHbcGBgYGBgYAAAAAAAAAH4YPGZmZjwYfgAAAAAAAAA4bMbG/sbGbDgAAAAAAAA4bMbGxmxsbGzuAAAAAAAAHjAYDD5mZmZmPAAAAAAAAAAAAH7b29t+AAAAAAAAAAAAAwZ+29vzfmDAAAAAAAAAHDBgYHxgYGAwHAAAAAAAAAB8xsbGxsbGxsYAAAAAAAAAAP4AAP4AAP4AAAAAAAAAAAAYGH4YGAAA/wAAAAAAAAAwGAwGDBgwAH4AAAAAAAAADBgwYDAYDAB+AAAAAAAADhsbGBgYGBgYGBgYGBgYGBgYGBgYGNjY2HAAAAAAAAAAABgYAH4AGBgAAAAAAAAAAAAAdtwAdtwAAAAAAAAAOGxsOAAAAAAAAAAAAAAAAAAAAAAAABgYAAAAAAAAAAAAAAAAAAAAGAAAAAAAAAAADwwMDAwM7GxsPBwAAAAAANhsbGxsbAAAAAAAAAAAAABw2DBgyPgAAAAAAAAAAAAAAAAAfHx8fHx8fAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==";
        static string ASC16Base64 = "AAAAAAAAAAAAAAAACAAIAAgACAAIAAgAAAAIAAAAAAAAAAAAAAAAAAAAAAAKAAoACgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAKAAoAPgAUABQAPgAoACgAAAAAAAAAAAAAAAAAAAAAABwAKgAoABwACgAKACoAHAAIAAAAAAAAAAAAAAAAAAAAGIAlACUAGgACwAUgBSAIwAAAAAAAAAAAAAAAAAAAAAAMABIAEgAMABQAIwAiAB0AAAAAAAAAAAAAAAAAAAAAACAAIAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACAAQACAAIAAgACAAIAAgABAACAAAAAAAAAAAAAAAAAAgABAACAAIAAgACAAIAAgAEAAgAAAAAAAAAAAAAAAAABAAOAAQACgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAgACAA+AAgACAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABAAEAAQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA4AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAQAAAAAAAAAAAAAAAAAAAAAAAIAAgAEAAQABAAEAAgACAAAAAAAAAAAAAAAAAAAAAAABwAIgAiACIAIgAiACIAHAAAAAAAAAAAAAAAAAAAAAAACAAYACgACAAIAAgACAAIAAAAAAAAAAAAAAAAAAAAAAAcACIAAgACAAQACAAQAD4AAAAAAAAAAAAAAAAAAAAAABwAIgACAAwAAgACACIAHAAAAAAAAAAAAAAAAAAAAAAABAAMABQAFAAkAD4ABAAEAAAAAAAAAAAAAAAAAAAAAAAeABAAIAA8AAIAAgAiABwAAAAAAAAAAAAAAAAAAAAAABwAIgAgADwAIgAiACIAHAAAAAAAAAAAAAAAAAAAAAAAPgAEAAQACAAIABAAEAAQAAAAAAAAAAAAAAAAAAAAAAAcACIAIgAcACIAIgAiABwAAAAAAAAAAAAAAAAAAAAAABwAIgAiACIAHgACACIAHAAAAAAAAAAAAAAAAAAAAAAAAAAAACAAAAAAAAAAAAAgAAAAAAAAAAAAAAAAAAAAAAAAAAAAIAAAAAAAAAAAACAAIAAgAAAAAAAAAAAAAAAAAAAAAAACABwAIAAcAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAPgAAAD4AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAIAAcAAIAHAAgAAAAAAAAAAAAAAAAAAAAAAAAABwAIgACAAQACAAIAAAACAAAAAAAAAAAAAAAAAAAAAAAB8AYIBNQJNAokCiQKaAmwBAQD+AAAAAAAAAAAAAAAAAEAAoACgAKABEAHwAggCCAAAAAAAAAAAAAAAAAAAAAAD4AIQAhAD8AIQAhACEAPgAAAAAAAAAAAAAAAAAAAAAADgARACAAIAAgACAAEQAOAAAAAAAAAAAAAAAAAAAAAAA8ACIAIQAhACEAIQAiADwAAAAAAAAAAAAAAAAAAAAAAD4AIAAgAD4AIAAgACAAPgAAAAAAAAAAAAAAAAAAAAAAPgAgACAAPAAgACAAIAAgAAAAAAAAAAAAAAAAAAAAAAAOABEAIIAgACOAIIARAA4AAAAAAAAAAAAAAAAAAAAAACEAIQAhAD8AIQAhACEAIQAAAAAAAAAAAAAAAAAAAAAAIAAgACAAIAAgACAAIAAgAAAAAAAAAAAAAAAAAAAAAAAEAAQABAAEAAQAJAAkABgAAAAAAAAAAAAAAAAAAAAAACEAIgAkACwANAAiACIAIQAAAAAAAAAAAAAAAAAAAAAAIAAgACAAIAAgACAAIAA+AAAAAAAAAAAAAAAAAAAAAAAggDGAMYAqgCqAKoAkgCSAAAAAAAAAAAAAAAAAAAAAACEAMQApACkAJQAlACMAIQAAAAAAAAAAAAAAAAAAAAAADgARACCAIIAggCCAEQAOAAAAAAAAAAAAAAAAAAAAAAA8ACIAIgAiADwAIAAgACAAAAAAAAAAAAAAAAAAAAAAAA4AEQAggCCAIIAmgBEADoAAAAAAAAAAAAAAAAAAAAAAPgAhACEAPgAkACIAIgAhAAAAAAAAAAAAAAAAAAAAAAAeACEAIAAYAAYAAQAhAB4AAAAAAAAAAAAAAAAAAAAAAD4ACAAIAAgACAAIAAgACAAAAAAAAAAAAAAAAAAAAAAAIQAhACEAIQAhACEAIQAeAAAAAAAAAAAAAAAAAAAAAAAggCCAEQARAAoACgAEAAQAAAAAAAAAAAAAAAAAAAAAAEIQRRAlICUgKKAooBBAEEAAAAAAAAAAAAAAAAAAAAAAIQASABIADAAMABIAEgAhAAAAAAAAAAAAAAAAAAAAAAAggBEAEQAKAAQABAAEAAQAAAAAAAAAAAAAAAAAAAAAAB8AAgAEAAQACAAIABAAPwAAAAAAAAAAAAAAAAAAAAAAMAAgACAAIAAgACAAIAAgACAAMAAAAAAAAAAAAAAAAAAgACAAEAAQABAAEAAIAAgAAAAAAAAAAAAAAAAAAAAAADAAEAAQABAAEAAQABAAEAAQADAAAAAAAAAAAAAAAAAACAAUABQAIgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA/AAAAAAAAAAAAAAAAACAAEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABwAIgAeACIAJgAaAAAAAAAAAAAAAAAAAAAAAAAgACAALAAyACIAIgAyACwAAAAAAAAAAAAAAAAAAAAAAAAAAAAcACIAIAAgACIAHAAAAAAAAAAAAAAAAAAAAAAAAgACABoAJgAiACIAJgAaAAAAAAAAAAAAAAAAAAAAAAAAAAAAHAAiAD4AIAAiABwAAAAAAAAAAAAAAAAAAAAAAAgAEAA4ABAAEAAQABAAEAAAAAAAAAAAAAAAAAAAAAAAAAAAABoAJgAiACIAJgAaAAIAPAAAAAAAAAAAAAAAAAAgACAALAAyACIAIgAiACIAAAAAAAAAAAAAAAAAAAAAACAAAAAgACAAIAAgACAAIAAAAAAAAAAAAAAAAAAAAAAAIAAAACAAIAAgACAAIAAgACAAQAAAAAAAAAAAAAAAAAAgACAAJAAoADAAKAAoACQAAAAAAAAAAAAAAAAAAAAAACAAIAAgACAAIAAgACAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAC8ANIAkgCSAJIAkgAAAAAAAAAAAAAAAAAAAAAAAAAAAPAAiACIAIgAiACIAAAAAAAAAAAAAAAAAAAAAAAAAAAAcACIAIgAiACIAHAAAAAAAAAAAAAAAAAAAAAAAAAAAACwAMgAiACIAMgAsACAAIAAAAAAAAAAAAAAAAAAAAAAAGgAmACIAIgAmABoAAgACAAAAAAAAAAAAAAAAAAAAAAAoADAAIAAgACAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAABwAIgAYAAQAIgAcAAAAAAAAAAAAAAAAAAAAAAAgACAAcAAgACAAIAAgADAAAAAAAAAAAAAAAAAAAAAAAAAAAAAiACIAIgAiACYAGgAAAAAAAAAAAAAAAAAAAAAAAAAAACIAIgAUABQACAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAIiAlIBVAFUAIgAiAAAAAAAAAAAAAAAAAAAAAAAAAAAAiABQACAAIABQAIgAAAAAAAAAAAAAAAAAAAAAAAAAAACIAIgAUABQACAAIAAgAEAAAAAAAAAAAAAAAAAAAAAAAPgAEAAgACAAQAD4AAAAAAAAAAAAAAAAAAAAAAAgAEAAQABAAIAAQABAAEAAQAAgAAAAAAAAAAAAAAAAAIAAgACAAIAAgACAAIAAgACAAIAAAAAAAAAAAAAAAAAAQAAgACAAIAAQACAAIAAgACAAQAAAAAAAAAAAAAAAAAAAAAAAAADoALAAAAAAAAAAAAAAA=";
        static MemoryStream ASC16FontMS = new MemoryStream(Convert.FromBase64String(ASC16Base64));

        public static void _DrawACSIIString(string s, int x, int y, int color)
        {
            string[] lines = s.Split('\n');
            for (int l = 0; l < lines.Length; l++)
            {
                for (int c = 0; c < lines[l].Length; c++)
                {
                    int offset = (Encoding.ASCII.GetBytes(lines[l][c].ToString())[0] & 0xFF) * 16;
                    ASC16FontMS.Seek(offset, SeekOrigin.Begin);
                    byte[] fontbuf = new byte[16];
                    ASC16FontMS.Read(fontbuf, 0, fontbuf.Length);

                    for (int i = 0; i < 16; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if ((fontbuf[i] & (0x80 >> j)) != 0)
                            {
                                if (x + c * 8 > width)
                                {

                                }
                                else
                                {
                                    //DrawPixelfortext((int)((x + j) + (c * 8)), (int)(y + i + (l * 16)), color);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void ScaleImage(Image image, int x, int y)
        {
            int counter = 0;
            int counter2 = 1;
            int prewy = y;
            for (int _y = y; _y < y + image.Height * 3; _y++)
            {
                for (int _x = x; _x < x + image.Width * 3; _x++)
                {
                    if ((_y * width) - (width - _x) < width * height)
                    {
                        if (_x > width)
                        {
                            counter++;
                        }
                        else
                        {
                            if (counter2 % 3 == 0)
                            {
                                cover.RawData[((_y * width) - (width - _x))] = image.RawData[counter];
                                counter++;
                                counter2++;
                            }
                            else
                            {
                                cover.RawData[((_y * width) - (width - _x))] = image.RawData[counter];
                                counter2++;
                            }
                        }
                    }
                }
                counter -= (int)image.Width;
                _y += 1;
                for (int _x = x; _x < x + image.Width * 3; _x++)
                {
                    if ((_y * width) - (width - _x) < width * height)
                    {
                        if (_x > width)
                        {
                            counter++;
                        }
                        else
                        {
                            if (counter2 % 3 == 0)
                            {
                                cover.RawData[((_y * width) - (width - _x))] = image.RawData[counter];
                                counter++;
                                counter2++;
                            }
                            else
                            {
                                cover.RawData[((_y * width) - (width - _x))] = image.RawData[counter];
                                counter2++;
                            }
                        }
                    }
                }
                counter -= (int)image.Width;
                _y += 1;
                for (int _x = x; _x < x + image.Width * 3; _x++)
                {
                    if ((_y * width) - (width - _x) < width * height)
                    {
                        if (_x > width)
                        {
                            counter++;
                        }
                        else
                        {
                            if (counter2 % 3 == 0)
                            {
                                cover.RawData[((_y * width) - (width - _x))] = image.RawData[counter];
                                counter++;
                                counter2++;
                            }
                            else
                            {
                                cover.RawData[((_y * width) - (width - _x))] = image.RawData[counter];
                                counter2++;
                            }
                        }
                    }
                }
                counter2 = 1;
                prewy++;
            }
        }

        public static int colourToNumber(int r, int g, int b)
        {
            return (r << 16) + (g << 8) + (b);
        }

        public static void line(float x1, float y1, float x2, float y2, int color)
        {
            float dx = x2 - x1;
            float dy = y2 - y1;

            float length = (float)Math.Sqrt(dx * dx + dy * dy);

            float angle = (float)Math.Atan2(dy, dx);

            for (float i = 0; i < length; i++)
            {
                //ImprovedVBE.DrawPixelfortext((int)(x1 + Math.Cos(angle) * i), (int)(y1 + Math.Sin(angle) * i), color);
            }
        }

        public static MemoryStream memoryStream = new MemoryStream(Arial);
        public static int Size = 16;
        public static string charset = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";

        public static void Draw_string(string s, int x, int y, int color)
        {
            foreach (char c in s)
            {
                if (c == '\n')
                {
                    y += 12;
                }
                else if (c == ' ')
                {
                    x += 10;
                }
                else
                {
                    int aIndex = charset.IndexOf(c);
                    int SizePerFont = Size * (Size / 8);
                    byte[] buffer = new byte[SizePerFont];
                    memoryStream.Seek(SizePerFont * aIndex, SeekOrigin.Begin);
                    memoryStream.Read(buffer, 0, buffer.Length);

                    for (int h = 0; h < Size; h++)
                    {
                        for (int aw = 0; aw < Size / 8; aw++)
                        {
                            for (int ww = 0; ww < 8; ww++)
                            {
                                if ((buffer[(h * (Size / 8)) + aw] & (0x80 >> ww)) != 0)
                                {
                                    //DrawPixelfortext((aw * 8) + ww + x, h + y, color);
                                }
                            }
                        }
                    }
                    if (c == 'W' || c == 'w')
                    {
                        x += 13;
                    }
                    else
                    {
                        x += 9;
                    }
                }
            }
        }

        public static void DrawFilledEllipse(int xCenter, int yCenter, int yR, int xR, int color)
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
                    if ((x * x * yR * yR) + (y * y * xR * xR) <= yR * yR * xR * xR)
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
                            cover.RawData[(yCenter + y) * width + xCenter + x] = color;
                        }

                        //DrawPixelfortext(xCenter + x, yCenter + y, GetGradientColor(x, 0, width, height));

                        //DrawPixelfortext(xCenter + x, yCenter + y, color);
                    }
                }
            }
        }
    }
}
