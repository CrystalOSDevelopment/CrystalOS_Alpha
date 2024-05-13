using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace CrystalOSAlpha.Applications.Video_Player
{
    class CrystalVideo : App
    {
        #region Window porpeties
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public int desk_ID { get; set; }

        public int AppID { get; set; }
        public string name { get; set; }
        public bool minimised { get; set; }
        public bool movable { get; set; }
        public bool once { get; set; }
        public Bitmap icon { get; set; }
        public Bitmap window { get; set; }
        #endregion Window porpeties

        #region Extras
        public bool temp = true;
        public bool clicked = false;
        public bool AllowPlay = true;

        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);
        public int RenderX = 0;
        public int RenderY = 0;
        public int FrameCount = 0;
        public int i = 0;
        public int ChunkCount = 0;

        public Bitmap previousFrame = null;
        public Bitmap canvas;
        public Bitmap back_canvas;

        public List<Button_prop> Buttons = new List<Button_prop>();
        #endregion Extras

        public void App()
        {
            if(once == true)
            {
                width = 650;
                height = 570;
                (canvas, back_canvas, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);

                //Design 
                ImprovedVBE.DrawFilledPollygon(canvas, new List<Point>
                {
                    new Point(468, 569),
                    new Point(469, 536),
                    new Point(474, 528),
                    new Point(481, 523),
                    new Point(495, 517),
                    new Point(512, 513),
                    new Point(551, 510),
                    new Point(650, 510),
                    new Point(649, 569),
                }, ImprovedVBE.colourToNumber(161, 161, 161));

                ImprovedVBE.DrawFilledPollygon(canvas, new List<Point>
                {
                    new Point(2, 510),
                    new Point(412, 510),
                    new Point(439, 512),
                    new Point(452, 515),
                    new Point(464, 520),
                    new Point(474, 526),
                    new Point(485, 534),
                    new Point(504, 552),
                    new Point(521, 569),
                    new Point(1, 569)
                }, ImprovedVBE.colourToNumber(60, 114, 255));

                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, "Currently Playing: SampleVideo", 14, 513);
                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, "Framerate: 25", 14, 530);
                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, "Duration: 0:01:00", 215, 530);

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);

                if(Buttons.Count == 0)
                {
                    Buttons.Add(new Button_prop(514, 491, 70, 50, "Reset", 1, "Reset"));
                    Buttons.Add(new Button_prop(584, 491, 70, 50, "Play/\nPause", 1, "PP"));
                }

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
                                switch (button.ID)
                                {
                                    case "Reset":
                                        i = 0;
                                        RenderX = 0;
                                        RenderY = 0;
                                        FrameCount = 0;
                                        ChunkCount = 0;
                                        previousFrame = null;
                                        AllowPlay = true;
                                        break;
                                    case "PP":
                                        if (AllowPlay == true)
                                        {
                                            AllowPlay = false;
                                        }
                                        else
                                        {
                                            AllowPlay = true;
                                        }
                                        break;
                                }
                                button.Clicked = true;
                                temp = true;
                                clicked = true;
                            }
                        }
                    }
                }
            }

            if (temp == true)
            {
                foreach (var button in Buttons)
                {
                    if (button.Clicked == true)
                    {
                        //Button.Button_render(window, button.X, button.Y, button.Width, button.Height, Color.White.ToArgb(), button.Text);
                        ImprovedVBE.DrawFilledEllipse(window, button.X + button.Width / 2, button.Y + button.Height / 2, 25, 25, ImprovedVBE.colourToNumber(255, 255, 255));
                        ImprovedVBE.DrawFilledEllipse(window, button.X + button.Width / 2, button.Y + button.Height / 2, 22, 22, ImprovedVBE.colourToNumber(1, 1, 1));
                        if(button.ID == "PP")
                        {
                            if(AllowPlay == true)
                            {
                                ImprovedVBE.DrawFilledRectangle(window, ImprovedVBE.colourToNumber(255, 255, 255), button.X + button.Width / 2 - 5, button.Y + button.Height / 2 - 8, 4, 16);
                                ImprovedVBE.DrawFilledRectangle(window, ImprovedVBE.colourToNumber(255, 255, 255), button.X + button.Width / 2 + 5, button.Y + button.Height / 2 - 8, 4, 16);
                            }
                            else
                            {
                                ImprovedVBE.DrawFilledPollygon(window, new List<Point>
                                {
                                    new Point(button.X + button.Width / 2 - 8, button.Y + button.Height / 2 - 8),
                                    new Point(button.X + button.Width / 2 - 9, button.Y + button.Height / 2 + 8),
                                    new Point(button.X + button.Width / 2 + 16, button.Y + button.Height / 2)
                                }, ImprovedVBE.colourToNumber(255, 255, 255));
                            }
                        }
                        else
                        {
                            BitFont.DrawBitFontString(window, "ArialCustomCharset16", Color.White, "Reset", button.X + 15, button.Y + 17);
                        }
                    }
                    else
                    {
                        //Button.Button_render(window, button.X, button.Y, button.Width, button.Height, button.Color, button.Text);
                        ImprovedVBE.DrawFilledEllipse(window, button.X + button.Width / 2, button.Y + button.Height / 2, 25, 25, ImprovedVBE.colourToNumber(255, 255, 255));
                        if (button.ID == "PP")
                        {
                            if (AllowPlay == true)
                            {
                                ImprovedVBE.DrawFilledRectangle(window, 1, button.X + button.Width / 2 - 5, button.Y + button.Height / 2 - 8, 4, 16);
                                ImprovedVBE.DrawFilledRectangle(window, 1, button.X + button.Width / 2 + 5, button.Y + button.Height / 2 - 8, 4, 16);
                            }
                            else
                            {
                                ImprovedVBE.DrawFilledPollygon(window, new List<Point>
                                {
                                    new Point(button.X + button.Width / 2 - 8, button.Y + button.Height / 2 - 8),
                                    new Point(button.X + button.Width / 2 - 9, button.Y + button.Height / 2 + 8),
                                    new Point(button.X + button.Width / 2 + 16, button.Y + button.Height / 2)
                                }, 1);
                            }
                        }
                        else
                        {
                            BitFont.DrawBitFontString(window, "ArialCustomCharset16", Color.Black, "Reset", button.X + 15, button.Y + 17);
                        }
                    }
                    if (MouseManager.MouseState == MouseState.None)
                    {
                        button.Clicked = false;
                    }
                }

                if(i < Resources.TotalBytes.Length && AllowPlay == true)
                {
                    while (true)
                    {
                        if (RenderY != 0 && RenderX == 0)
                        {
                            RenderX += 64;
                        }
                        if (FrameCount % 30 == 0 && i < Resources.TotalBytes.Length - 640 * 480 * 3)
                        {
                            byte[] data = new byte[640 * 480 * 3];
                            Array.Copy(Resources.TotalBytes, i, data, 0, data.Length);
                            previousFrame = ReconstructMain(data, 640, 480);
                            i += data.Length;
                            FrameCount++;
                        }
                        else
                        {
                            if (Resources.TotalBytes[i] == 0 && Resources.TotalBytes[i + 1] == 0 && Resources.TotalBytes[i + 2] == 0 && Resources.TotalBytes[i + 3] == 0)
                            {
                                i += 4;
                                if (RenderX < previousFrame.Width)
                                {
                                    RenderX += 64;
                                }
                                else
                                {
                                    RenderY += 32;
                                    RenderX = 0;
                                }
                                ChunkCount++;
                            }
                            else
                            {
                                byte[] data = new byte[64 * 32 * 3];
                                Array.Copy(Resources.TotalBytes, i, data, 0, data.Length);
                                Bitmap neu = ReconstructMain(data, 64, 32);
                                ImprovedVBE.DrawImageAlpha(neu, RenderX, RenderY, previousFrame);
                                if (RenderX < previousFrame.Width)
                                {
                                    RenderX += 64;
                                }
                                else
                                {
                                    RenderY += 32;
                                    RenderX = 0;
                                }
                                i += 64 * 32 * 3;
                                ChunkCount++;
                            }
                        }
                        if (ChunkCount >= 15 * 10)
                        {
                            ChunkCount = 0;
                            FrameCount++;
                            RenderY = 0;
                            RenderX = 0;
                            break;
                        }
                    }
                    ImprovedVBE.DrawImage(previousFrame, 5, 27, window);
                }
                else
                {
                    AllowPlay = false;
                }
                temp = false;
            }
            ImprovedVBE.DrawImage(window, x, y, ImprovedVBE.cover);
            if(AllowPlay == true)
            {
                Thread.Sleep(40);
                temp = true;
            }
            if(MouseManager.MouseState == MouseState.None && clicked == true)
            {
                clicked = false;
            }
        }

        public void RightClick()
        {
            
        }

        public Bitmap ReconstructMain(byte[] aData, int SizeX, int SizeY)
        {
            int Counter = 0;

            Bitmap FinishedFrame = new Bitmap((uint)SizeX, (uint)SizeY, ColorDepth.ColorDepth32);

            for (int Y = 0; Y < FinishedFrame.Height; Y++)
            {
                for (int X = 0; X < FinishedFrame.Width; X++)
                {
                    ImprovedVBE.DrawPixel(FinishedFrame, X, Y, ImprovedVBE.colourToNumber(aData[Counter], aData[Counter + 1], aData[Counter + 2]));
                    Counter += 3;
                }
            }
            return FinishedFrame;
        }
    }
}
