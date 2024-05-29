using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Applications.Video_Player;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Icons;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CrystalOSAlpha.Applications.MediaCenter
{
    class MediaCenter : App
    {
        public MediaCenter(int X, int Y, int Z, int Width, int Height, string Name, Bitmap Icon)
        {
            this.x = X;
            this.y = Y;
            this.z = Z;
            this.width = Width;
            this.height = Height;
            this.name = Name;
            this.icon = Icon;
        }

        #region Window propeties
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
        #endregion Window propeties

        #region Extras
        public bool temp = true;
        public bool clicked = false;

        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);

        public Bitmap canvas;
        public Bitmap back_canvas;

        public List<Point> Stars = new List<Point>();
        #endregion Extras

        public void App()
        {
            if(once == true)
            {
                width = 862;
                height = 490;
                (canvas, back_canvas, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);

                //Design
                ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(51, 35, 63), 0, 22, width, height - 22);
                if(Stars.Count == 0)
                {
                    Random rnd = new Random();
                    for(int i = 0; i < 500; i++)
                    {
                        int XAxis = rnd.Next(0, width + 1);
                        int YAxis = rnd.Next(23, height - 22);
                        Stars.Add(new Point(XAxis, YAxis));
                    }
                }
                foreach(Point p in Stars)
                {
                    ImprovedVBE.DrawPixel(canvas, p.X, p.Y, ImprovedVBE.colourToNumber(255, 255, 255));
                }

                ImprovedVBE.DrawFilledEllipse(canvas, width / 2, height / 2 + 22, 145, 329, ImprovedVBE.colourToNumber(255, 0, 0));
                ImprovedVBE.DrawFilledEllipse(canvas, width / 2, height / 2 + 10, 115, 287, ImprovedVBE.colourToNumber(51, 35, 63));

                ImprovedVBE.DrawImageAlpha(Resources.CrystalMusic, 371, 266, canvas);
                ImprovedVBE.DrawImageAlpha(Resources.CrystalVideo, 685, 150, canvas);

                BitFont.DrawBitFontString(canvas, "VerdanaCustomCharset24", Color.White, "Crystal Music", 355, 387);
                BitFont.DrawBitFontString(canvas, "VerdanaCustomCharset24", Color.White, "Crystal Video", 657, 256);

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                once = false;
            }

            if(MouseManager.MouseState == MouseState.Left)
            {
                if(MouseManager.X > x + 371 && MouseManager.X < x + 371 + Resources.CrystalMusic.Width)
                {
                    if(MouseManager.Y > y + 266 && MouseManager.Y < y + 266 + Resources.CrystalMusic.Height + 25)
                    {
                        //Music Player goes here
                        throw new Exception("Failed to open Crystal Music:\nNot yet implemented!");
                    }
                }
                if (MouseManager.X > x + 685 && MouseManager.X < x + 685 + Resources.CrystalVideo.Width)
                {
                    if(MouseManager.Y > y + 150 && MouseManager.Y < y + 150 + Resources.CrystalVideo.Height + 25)
                    {
                        CrystalVideo cv = new CrystalVideo();
                        cv.x = 10;
                        cv.y = 100;
                        cv.width = 640;
                        cv.height = 379;
                        cv.z = 999;
                        cv.name = "CrystalVideo";
                        cv.minimised = false;
                        cv.once = true;
                        cv.icon = ImprovedVBE.ScaleImageStock(Resources.CrystalVideo, 56, 56);
                        TaskScheduler.Apps.Add(cv);
                        TaskScheduler.Apps.Remove(this);
                    }
                }
            }

            ImprovedVBE.DrawImage(window, x, y, ImprovedVBE.cover);
        }

        public void RightClick()
        {
            
        }
    }
}
