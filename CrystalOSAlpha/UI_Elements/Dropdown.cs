using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using System;
using System.Collections.Generic;

namespace CrystalOSAlpha.UI_Elements
{
    class Dropdown : UIElementHandler
    {
        public bool Clicked { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string ID { get; set; }
        public string Text { get; set; }
        public int Color { get; set; }
        public TypeOfElement TypeOfElement { get; set; }
        public int Pos { get; set; }
        public int Value { get; set; }
        public float Sensitivity { get; set; }
        public int LockedPos { get; set; }
        public int MinVal { get; set; }
        public int MaxVal { get; set; }

        public Bitmap canv;
        public Bitmap Drop;
        public List<values> v;
        public bool DisableClick = false;
        public void Render(Bitmap canvas)
        {
            TypeOfElement = TypeOfElement.DropDown;
            canv = new Bitmap((uint)Width, (uint)Height, ColorDepth.ColorDepth32);
            Array.Fill(canv.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
            ImprovedVBE.DrawFilledRectangle(canv, ImprovedVBE.colourToNumber(60, 60, 60), 2, 2, Width - 4, Height - 4, false);
            Drop = new Bitmap((uint)Width, 100, ColorDepth.ColorDepth32);
            Array.Fill(Drop.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
            ImprovedVBE.DrawFilledRectangle(Drop, ImprovedVBE.colourToNumber(60, 60, 60), 2, 2, Width - 4, 100 - 4, false);

            Button btn = new Button((int)(canv.Width - 30), 0, 30, (int)canv.Height, "exp", ImprovedVBE.colourToNumber(60, 60, 60));
            btn.Render(canv);

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
            Text = temp;
            if (temp.Length > 7 && Width - 20 < temp.Length * 8)
            {
                temp = temp.Remove(7);
            }

            if(Clicked == true)
            {
                int top = 3;
                foreach(var d in v)
                {
                    if (d.ID == ID)
                    {
                        if (d.Highlighted == true)
                        {
                            ImprovedVBE.DrawFilledRectangle(Drop, ImprovedVBE.colourToNumber(90, 90, 90), 2, top, Width - 4, 20, false);
                        }
                        BitFont.DrawBitFontString(Drop, "ArialCustomCharset16", System.Drawing.Color.White, d.content, 3, top);
                        top += 20;
                    }
                }
                ImprovedVBE.DrawImageAlpha(Drop, X, Y + Height, canvas);
            }

            BitFont.DrawBitFontString(canv, "ArialCustomCharset16", System.Drawing.Color.White, temp, 5, Height / 2 - 8);
            ImprovedVBE.DrawImageAlpha(canv, X, Y, canvas);
        }
        public bool CheckClick(int x, int y)
        {
            if(Clicked == true)
            {
                if(MouseManager.X > x + X && MouseManager.X < x + X + Width)
                {
                    if(MouseManager.Y > y + Y + Height && MouseManager.Y < y + Y + Height + Drop.Height)
                    {
                        foreach(var vals in v)
                        {
                            vals.Highlighted = false;
                        }
                        int Index = (int)Math.Floor(((double)MouseManager.Y - (double)y - (double)Y - (double)Height) / 20);
                        if(Index >= 0 && Index < v.Count)
                        {
                            v[Index].Highlighted = true;
                        }
                        else
                        {
                            v[0].Highlighted = true;
                        }
                        if(MouseManager.MouseState == MouseState.Left)
                        {
                            Clicked = false;
                        }
                    }
                }
            }
            if (MouseManager.MouseState == MouseState.Left)
            {
                if (MouseManager.X > x + X + (canv.Width - 30) && MouseManager.X < x + X + canv.Width)
                {
                    if (MouseManager.Y > y + Y && MouseManager.Y < y + Y + Height)
                    {
                        if(DisableClick == false)
                        {
                            if (Clicked == true)
                            {
                                Clicked = false;
                            }
                            else
                            {
                                Clicked = true;
                            }
                            DisableClick = true;
                        }
                        return true;
                    }
                }
            }
            else
            {
                DisableClick = false;
            }
            return false;
        }

        public Dropdown(int x, int y, int width, int height, string iD, List<values> V)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            ID = iD;
            v = V;
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
