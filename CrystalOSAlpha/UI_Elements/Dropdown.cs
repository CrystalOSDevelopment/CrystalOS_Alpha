using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace CrystalOSAlpha.UI_Elements
{
    class Dropdown
    {
        public bool Clicked { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string ID { get; set; }
        public Bitmap canv;
        public Bitmap Drop;
        public Bitmap Draw(Bitmap canvas, List<values> v)
        {
            canv = new Bitmap((uint)Width, (uint)Height, ColorDepth.ColorDepth32);
            Array.Fill(canv.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
            ImprovedVBE.DrawFilledRectangle(canv, ImprovedVBE.colourToNumber(60, 60, 60), 2, 2, Width - 4, Height - 4, false);
            Drop = new Bitmap((uint)Width, 100, ColorDepth.ColorDepth32);
            Array.Fill(Drop.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
            ImprovedVBE.DrawFilledRectangle(Drop, ImprovedVBE.colourToNumber(60, 60, 60), 2, 2, Width - 4, 100 - 4, false);

            Button.Button_render(canv, (int)(canv.Width - 30), 0, 30, (int)canv.Height, ImprovedVBE.colourToNumber(60, 60, 60), "exp");

            string temp = "";
            int t = 0;
            a:
            if (t < v.Count)
            {
                if(v[t].ID == ID && v[t].Highlighted == true)
                {
                    temp = v[t].content;
                }
                else
                {
                    t++;
                    goto a;
                }
            }
            if (temp.Length > 7)
            {
                temp = temp.Remove(7);
            }

            if(Clicked == true)
            {
                //ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(60, 60, 60), X, Y + Height, Width, 100, false);
                int top = 3;
                foreach(var d in v)
                {
                    if (d.ID == ID)
                    {
                        if (d.Highlighted == true)
                        {
                            ImprovedVBE.DrawFilledRectangle(Drop, ImprovedVBE.colourToNumber(90, 90, 90), 2, top, Width - 4, 20, false);
                        }
                        BitFont.DrawBitFontString(Drop, "ArialCustomCharset16", Color.White, d.content, 3, top);
                        top += 20;
                    }
                }
                ImprovedVBE.DrawImageAlpha(Drop, X, Y + Height, canvas);
            }

            BitFont.DrawBitFontString(canv, "ArialCustomCharset16", System.Drawing.Color.White, temp, 5, Height / 2 - 8);
            ImprovedVBE.DrawImageAlpha(canv, X, Y, canvas);
            return canvas;
        }
        public void Render(int x, int y)
        {
            if(Clicked == true)
            {
                ImprovedVBE.DrawImageAlpha(Drop, x + X, y + Y + Height, ImprovedVBE.cover);
            }
        }
        public bool CheckActivity(int x, int y)
        {
            if(MouseManager.MouseState == MouseState.Left)
            {
                /*
                if(MouseManager.X > X && MouseManager.X < X + Width)
                {
                    if (MouseManager.Y > Y && MouseManager.Y < Y + Height)
                    {
                        return true;
                    }
                }
                */
                if (MouseManager.X > x + X + (canv.Width - 30) && MouseManager.X < x + X + canv.Width)
                {
                    if (MouseManager.Y > y + Y && MouseManager.Y < y + Y + Height)
                    {
                        if(Clicked == true)
                        {
                            Clicked = false;
                        }
                        else
                        {
                            Clicked = true;
                        }
                        return true;
                    }
                }
            }
            return false;
        }
    }

    class values
    {
        public bool Highlighted { get; set; }
        public string content { get; set; }
        public string ID { get; set; }
        public values (bool highlighted, string content, string iD)
        {
            this.Highlighted = highlighted;
            this.content = content;
            ID = iD;
        }
    }
}
