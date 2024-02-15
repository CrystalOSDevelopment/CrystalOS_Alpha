using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
namespace CrystalOSAlpha.Applications.Artistic_Stuff.ColorView
{
    class ColorPalette : App
    {
        public int x {get; set;}
        public int y { get; set;}
        public int z { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public int desk_ID {get; set;}

        public string name {get; set;}

        public bool minimised { get; set; }
        public bool movable { get; set; }
        public Bitmap icon { get; set; }

        public int x_1 = 0;
        public int y_1 = 0;

        public Bitmap canvas;
        public Bitmap back_canvas;
        public bool once = true;
        public Bitmap window;
        public int CurrentColor = ImprovedVBE.colourToNumber(Global_integers.R, Global_integers.G, Global_integers.B);

        public string Content = "";
        public List<Button_prop> Buttons = new List<Button_prop>();
        public List<Slider> Sliders = new List<Slider>();
        public List<CheckBox> CheckBox = new List<CheckBox>();

        public bool initial = true;
        public bool clicked = false;

        public bool temp = false;

        public int Red = 255;
        public int Green = 255;
        public int Blue = 255;

        public Bitmap Canvas = new Bitmap(256, 256, ColorDepth.ColorDepth32);

        public void App()
        {
            if (initial == true)
            {
                initial = false;

                Sliders.Add(new Slider(320, 177 - 22, 127, 127, "Blue"));//Blue

                Sliders.Add(new Slider(320, 138 - 22, 127, 127, "Red"));//Green

                Sliders.Add(new Slider(320, 100 - 22, 127, 68, "Green"));//Red

                //CheckBox.Add(new UI_Elements.CheckBox(320, 20, 20, 20, false));

                //CheckBox.Add(new UI_Elements.CheckBox(38, 20, 20, 20, true));
                
                #region Render
                //for (int x = 0; x < Canvas.Width; x++)
                //{
                //    for (int y = 0; y < Canvas.Height; y++)
                //    {
                //        double hue = (double)x / (Canvas.Width - 1) * 360;
                //        double saturation = 1 - (double)y / (Canvas.Height - 1); // Reverse the saturation
                //        double lightness = 0.5; // You can adjust this value

                //        // Convert HSL to RGB
                //        Color color = HSLtoRGB(hue, saturation, lightness);

                //        Canvas.RawData[y * Canvas.Width + x] = ImprovedVBE.colourToNumber(color.R, color.G, color.B);
                //    }
                //}

                //for (int y = 0; y < Canvas.Height; y++)
                //{
                //    for (int x = 1; x < Canvas.Width; x++)
                //    {
                //        Color previousColor = Color.FromArgb(Canvas.RawData[y * Canvas.Width + x - 1]);
                //        Color currentColor = Color.FromArgb(Canvas.RawData[y * Canvas.Width + x]);

                //        int blendFactor = 20; // Adjust the blending factor based on your preference

                //        int blendedR = (previousColor.R * (blendFactor - 1) + currentColor.R) / blendFactor;
                //        int blendedG = (previousColor.G * (blendFactor - 1) + currentColor.G) / blendFactor;
                //        int blendedB = (previousColor.B * (blendFactor - 1) + currentColor.B) / blendFactor;

                //        Color blendedColor = Color.FromArgb(blendedR, blendedG, blendedB);

                //        // Update the pixel color in the bitmap with blended color
                //        Canvas.RawData[y * Canvas.Width + x] = blendedColor.ToArgb();
                //    }
                //}
                ///*
                //int R = 0;
                //int G = 0;
                //int B = 0;
                //for(int y = 0; y < Canvas.RawData.Length; y++)
                //{
                //    Red = 255;
                //    Green = 0;
                //    Blue = 0;

                //    double intensity = 1.0 - (double)y / Canvas.Height;

                //    for (int x = 0; x < Canvas.Width; x++)
                //    {
                //        if (Red > 0)
                //        {
                //            Red -= 1;
                //            Green++;
                //        }
                //        else if (Green > 0)
                //        {
                //            Green -= 1;
                //            Blue++;
                //        }
                //        else if(Blue > 0)
                //        {
                //            Blue -= 1;
                //            Red++;
                //        }

                //        int r = (int)(Red * intensity);
                //        int g = (int)(Green * intensity);
                //        int b = (int)(Blue * intensity);

                //        Canvas.RawData[y * Canvas.Width + x] = ImprovedVBE.colourToNumber(r, g, b);
                //    }
                //}
                //*/
                #endregion Render
            }
            if (once == true)
            {
                canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32); //new int[width * height];
                back_canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32); //new int[width * height];
                window = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);

                #region corners
                DrawFilledEllipse(10, 10, 10, 10, CurrentColor);
                DrawFilledEllipse(width - 11, 10, 10, 10, CurrentColor);
                DrawFilledEllipse(10, height - 10, 10, 10, CurrentColor);
                DrawFilledEllipse(width - 11, height - 10, 10, 10, CurrentColor);

                DrawFilledRectangle(CurrentColor, 0, 10, width, height - 20);
                DrawFilledRectangle(CurrentColor, 5, 0, width - 10, 15);
                DrawFilledRectangle(CurrentColor, 5, height - 15, width - 10, 15);
                #endregion corners

                //Canvas = GenerateColorPalette(256, 256);

                canvas = ImprovedVBE.DrawImageAlpha2(canvas, x, y, canvas);

                DrawGradientLeftToRight();

                DrawFilledEllipse(width - 13, 10, 8, 8, ImprovedVBE.colourToNumber(255, 0, 0));

                DrawFilledEllipse(width - 34, 10, 8, 8, ImprovedVBE.colourToNumber(227, 162, 37));

                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, name, 2, 2);

                //Button.Button_render(canvas, 10, 70, 100, 25, 1, "Click");

                foreach (var button in Buttons)
                {
                    if (button.Clicked == true)
                    {
                        Button.Button_render(canvas, button.X, button.Y, button.Width, button.Height, Color.White.ToArgb(), button.Text);
                        
                        button.Clicked = false;
                    }
                    else
                    {
                        Button.Button_render(canvas, button.X, button.Y, button.Width, button.Height, button.Color, button.Text);
                    }
                    if (MouseManager.MouseState == MouseState.None)
                    {
                        button.Clicked = false;
                    }
                }

                //Sliders[0].Render(canvas);

                ImprovedVBE.DrawImageAlpha(Canvas, x, y, canvas);

                //window.RawData = canvas.RawData;
                back_canvas = canvas;
                temp = true;
                once = false;
            }

            foreach (var button in Buttons)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if (MouseManager.X > x + button.X && MouseManager.X < x + button.X + button.Width)
                    {
                        if (MouseManager.Y > y + button.Y && MouseManager.Y < y + button.Y + button.Height)
                        {
                            if (clicked == false)
                            {
                                button.Clicked = true;
                                once = true;
                                clicked = true;
                            }
                        }
                    }
                }
            }

            int c = 0;
            foreach(var slid in Sliders)
            {
                int val = slid.Value;
                if (slid.CheckForClick(x, y))
                {
                    slid.Clicked = true;
                }
                if(slid.Clicked == true)
                {
                    slid.UpdateValue(x);
                    if(val != slid.Value)
                    {
                        if (c == 0)
                        {
                            Blue = slid.Value * 2;
                        }
                        if (c == 1)
                        {
                            Green = slid.Value * 2;
                        }
                        if (c == 2)
                        {
                            Red = slid.Value * 2;
                        }
                        temp = true;
                    }
                    if(MouseManager.MouseState == MouseState.None)
                    {
                        slid.Clicked = false;
                    }
                }
                if (CheckBox[0].Value == false)
                {
                    if (c == 0)
                    {
                        Blue = slid.Value * 2;
                    }
                    if (c == 1)
                    {
                        Green = slid.Value * 2;
                    }
                    if (c == 2)
                    {
                        Red = slid.Value * 2;
                    }
                }
                c++;
            }

            foreach (var slid in CheckBox)
            {
                bool val = slid.Value;
                if (slid.CheckForClick(x, y))
                {
                    slid.Clicked = true;
                }
                if (slid.Clicked == true && MouseManager.MouseState == MouseState.None)
                {
                    if(slid.Value == false)
                    {
                        slid.Value = true;
                    }
                    else
                    {
                        slid.Value = false;
                    }
                    temp = true;
                    slid.Clicked = false;
                }
            }

            if (temp == true)
            {
                Bitmap selected_col = new Bitmap(95, 95, ColorDepth.ColorDepth32);
                Array.Fill(selected_col.RawData, 1);
                ImprovedVBE.DrawFilledRectangle(selected_col, ImprovedVBE.colourToNumber(Red, Green, Blue), 2, 2, (int)(selected_col.Width - 4), (int)(selected_col.Height - 4), false);
                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                Sliders[0].Render(window);
                Sliders[1].Render(window);
                Sliders[2].Render(window);

                CheckBox[0].Render(window);
                //CheckBox[1].Render(window);

                //Text
                BitFont.DrawBitFontString(window, "ArialCustomCharset16", Color.White, "Color spectrum:", 63, 42);
                BitFont.DrawBitFontString(window, "ArialCustomCharset16", Color.White, "RGB input:", 345, 42);
                BitFont.DrawBitFontString(window, "ArialCustomCharset16", Color.White, "Color preview:", 511, 42);

                Canvas = GenerateColorPalette(256, 256);
                ImprovedVBE.DrawImageAlpha(Canvas, 38, 76, window);
                ImprovedVBE.DrawImageAlpha(selected_col, 511, 76, window);
                temp = false;
            }

            if (MouseManager.MouseState == MouseState.None && clicked == true)
            {
                once = true;
                clicked = false;
            }

            if(MouseManager.MouseState == MouseState.Left)
            {
                if(MouseManager.X > x + 38 && MouseManager.X < x + 38 + Canvas.Width)
                {
                    if(MouseManager.Y > y + 76 && MouseManager.Y < y + 76 + Canvas.Height)
                    {
                        int x_coord = (int)(MouseManager.X - x - 38);
                        int y_coord = (int)(MouseManager.Y - y - 76);
                        if(x_coord > 0 && x_coord < Canvas.Width && y_coord > 0 && y_coord < Canvas.Height)
                        {
                            int color = Canvas.RawData[y_coord * Canvas.Width + x_coord];
                            Red = Color.FromArgb(color).R;
                            Green = Color.FromArgb(color).G;
                            Blue = Color.FromArgb(color).B;
                            temp = true;
                        }
                    }
                }
            }

            ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
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
                            canvas.RawData[(yCenter + y) * width + xCenter + x] = color;
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
                    Array.Copy(line, 0, canvas.RawData, i * width + X, line.Length);
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
        public void DrawGradientLeftToRight()
        {
            int gradientColorStart = GetGradientColor(0, 0, width, height);
            int gradientColorEnd = GetGradientColor(width, 0, width, height);

            int rStart = Color.FromArgb(gradientColorStart).R;
            int gStart = Color.FromArgb(gradientColorStart).G;
            int bStart = Color.FromArgb(gradientColorStart).B;

            int rEnd = Color.FromArgb(gradientColorEnd).R;
            int gEnd = Color.FromArgb(gradientColorEnd).G;
            int bEnd = Color.FromArgb(gradientColorEnd).B;

            for (int i = 0; i < canvas.RawData.Length; i++)
            {
                if (x_1 == width - 1)
                {
                    x_1 = 0;
                }
                else
                {
                    x_1++;
                }
                int r = (int)((double)x_1 / width * (rEnd - rStart)) + rStart;
                int g = (int)((double)x_1 / width * (gEnd - gStart)) + gStart;
                int b = (int)((double)x_1 / width * (bEnd - bStart)) + bStart;
                if (canvas.RawData[i] != 0)
                {
                    canvas.RawData[i] = ImprovedVBE.colourToNumber(r, g, b);
                }
                if (i / width > 20)
                {
                    break;
                }
            }
            x_1 = 0;
        }

        private Color HSLtoRGB(double hue, double saturation, double lightness)
        {
            double chroma = (1 - Math.Abs(2 * lightness - 1)) * saturation;
            double huePrime = hue / 60;
            double x = chroma * (1 - Math.Abs(huePrime % 2 - 1));

            double r, g, b;

            if (huePrime >= 0 && huePrime < 1)
            {
                r = chroma;
                g = x;
                b = 0;
            }
            else if (huePrime >= 1 && huePrime < 2)
            {
                r = x;
                g = chroma;
                b = 0;
            }
            else if (huePrime >= 2 && huePrime < 3)
            {
                r = 0;
                g = chroma;
                b = x;
            }
            else if (huePrime >= 3 && huePrime < 4)
            {
                r = 0;
                g = x;
                b = chroma;
            }
            else if (huePrime >= 4 && huePrime < 5)
            {
                r = x;
                g = 0;
                b = chroma;
            }
            else
            {
                r = chroma;
                g = 0;
                b = x;
            }

            double m = lightness - chroma / 2;

            int red = (int)((r + m) * 255);
            int green = (int)((g + m) * 255);
            int blue = (int)((b + m) * 255);

            return Color.FromArgb(red, green, blue);
        }
        private Color BlendColors(Color color1, Color color2)
        {
            double ratio = 0.5; // You can adjust this value for blending strength

            int red = (int)(color1.R * ratio + color2.R * (1 - ratio));
            int green = (int)(color1.G * ratio + color2.G * (1 - ratio));
            int blue = (int)(color1.B * ratio + color2.B * (1 - ratio));

            return Color.FromArgb(red, green, blue);
        }
        public Bitmap GenerateColorPalette(int width, int height)
        {
            Bitmap palette = new Bitmap(256, 256, ColorDepth.ColorDepth32);

            for (int y = 0; y < palette.Height; y++)
            {
                for (int x = 0; x < palette.Width; x++)
                {
                    // Create a color based on the current pixel position
                    //Color pixelColor = Color.FromArgb(x, y, (x + y) % 256);
                    int r = (int)(Sliders[2].Value * 2 * x / (palette.Width - 1));
                    int g = (int)(Sliders[1].Value * 2 * y / (palette.Height - 1));
                    int b = Sliders[0].Value * 2; // You can set a fixed value for blue or adjust it as needed

                    // Set the pixel color in the palette
                    palette.RawData[y * palette.Width + x] = ImprovedVBE.colourToNumber(r, g, b);
                }
            }

            return palette;
        }
    }
}