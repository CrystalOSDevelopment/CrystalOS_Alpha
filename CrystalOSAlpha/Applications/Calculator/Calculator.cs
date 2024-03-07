using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CrystalOSAlpha.Applications.Calculator
{
    class Calculator : App
    {
        public bool movable { get; set; }

        public int desk_ID { get; set; }
        public int AppID { get; set; }
        public string name { get; set; }

        public bool minimised { get; set; }
        public int z { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        public int x_1 = 0;
        public int y_1 = 0;
        public int width { get; set; }
        public int height { get; set; }

        public Bitmap icon { get; set; }
        public Bitmap canvas;
        public Bitmap back_canvas;
        public bool once = true;
        public Bitmap window;
        public int CurrentColor = ImprovedVBE.colourToNumber(Global_integers.R, Global_integers.G, Global_integers.B);

        public string Content = "";
        public List<Button_prop> Buttons = new List<Button_prop>();

        public bool initial = true;
        public bool clicked = false;
        public void App()
        {
            if(initial == true)
            {
                Buttons.Add(new Button_prop(5, 75, 40, 40, "C", 1));
                Buttons.Add(new Button_prop(50, 75, 40, 40, "Del", 1));
                Buttons.Add(new Button_prop(95, 75, 40, 40, "+", 1));
                Buttons.Add(new Button_prop(140, 75, 40, 40, "-", 1));

                Buttons.Add(new Button_prop(5, 120, 40, 40, "7", 1));
                Buttons.Add(new Button_prop(50, 120, 40, 40, "8", 1));
                Buttons.Add(new Button_prop(95, 120, 40, 40, "9", 1));
                Buttons.Add(new Button_prop(140, 120, 40, 40, "*", 1));

                Buttons.Add(new Button_prop(5, 165, 40, 40, "4", 1));
                Buttons.Add(new Button_prop(50, 165, 40, 40, "5", 1));
                Buttons.Add(new Button_prop(95, 165, 40, 40, "6", 1));
                Buttons.Add(new Button_prop(140, 165, 40, 40, "/", 1));

                Buttons.Add(new Button_prop(5, 210, 40, 40, "1", 1));
                Buttons.Add(new Button_prop(50, 210, 40, 40, "2", 1));
                Buttons.Add(new Button_prop(95, 210, 40, 40, "3", 1));
                Buttons.Add(new Button_prop(140, 210, 40, 85, "Enter", 1));

                Buttons.Add(new Button_prop(5, 255, 40, 40, "0", 1));
                Buttons.Add(new Button_prop(50, 255, 40, 40, ",", 1));
                Buttons.Add(new Button_prop(95, 255, 40, 40, ")", 1));

                Buttons.Add(new Button_prop(5, 300, 40, 40, "(", 1));

                width = 185;

                initial = false;
            }
            if(once == true)
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

                canvas = ImprovedVBE.EnableTransparency(canvas, x, y, canvas);

                DrawGradientLeftToRight();

                DrawFilledEllipse(width - 13, 10, 8, 8, ImprovedVBE.colourToNumber(255, 0, 0));

                DrawFilledEllipse(width - 34, 10, 8, 8, ImprovedVBE.colourToNumber(227, 162, 37));

                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, name, 2, 2);

                //Button.Button_render(canvas, 10, 70, 100, 25, 1, "Click");

                foreach(var button in Buttons)
                {
                    if(button.Clicked == true)
                    {
                        Button.Button_render(canvas, button.X, button.Y, button.Width, button.Height, Color.White.ToArgb(), button.Text);
                        if(button.Text == "Enter")
                        {
                            Content = CalculatorA.Calculate(Content).ToString();
                        }
                        else if(button.Text == "C")
                        {
                            Content = "";
                        }
                        else if(button.Text == "Del")
                        {
                            if(Content.Length != 0)
                            {
                                Content = Content.Remove(Content.Length - 1);
                            }
                        }
                        else
                        {
                            Content += button.Text;
                        }
                        button.Clicked = false;
                    }
                    else
                    {
                        Button.Button_render(canvas, button.X, button.Y, button.Width, button.Height, button.Color, button.Text);
                    }
                    if(MouseManager.MouseState == MouseState.None)
                    {
                        button.Clicked = false;
                    }
                }

                TextBox.Box(canvas, 5, 25, (int)(canvas.Width - 10), 40, ImprovedVBE.colourToNumber(60, 60, 60), Content, "Sample text", TextBox.Options.right);

                window.RawData = canvas.RawData;
                back_canvas = canvas;
                once = false;
            }

            foreach (var button in Buttons)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if(MouseManager.X > x + button.X && MouseManager.X < x + button.X + button.Width)
                    {
                        if (MouseManager.Y > y + button.Y && MouseManager.Y < y + button.Y + button.Height)
                        {
                            if(clicked == false)
                            {
                                button.Clicked = true;
                                once = true;
                                clicked = true;
                            }
                        }
                    }
                }
            }

            if (MouseManager.MouseState == MouseState.None && clicked == true)
            {
                once = true;
                clicked = false;
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

                        //DrawPixel(xCenter + x, yCenter + y, GetGradientColor(x, 0, width, height));

                        //DrawPixel(xCenter + x, yCenter + y, color);
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

                    DrawPixel(i, j, colourToNumber(r2, g2, b2));
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

        public bool EnableTransparency(Image image, int x, int y)
        {
            int counter = 0;
            if (y < 14 || y > height || x < -16 || x > width)
            {
                return false;
            }
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
                                    canvas.RawData[_y * width - (width - _x)] = image.RawData[counter];
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
                            return false;
                        }
                    }
                }
                else
                {
                    counter += (int)image.Width;
                }
            }
            return true;
        }
    }
}
