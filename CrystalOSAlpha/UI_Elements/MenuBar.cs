using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.SystemApps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Kernel = CrystalOS_Alpha.Kernel;

namespace CrystalOSAlpha.UI_Elements
{
    class MenuBar
    {
        public List<Submenu> SubmenuList = new List<Submenu>();
        public List<Coordinates> Coord = new List<Coordinates>();
        public List<string> SubmenuNames = new List<string>();
        public int XOff = 5;
        public bool Clicked = false;
        public bool NeedUpdate = false;
        public MenuBar(List<string> SubmenuNames, List<Submenu> SubmenuList)
        {
            this.SubmenuList = SubmenuList;
            this.SubmenuNames = SubmenuNames;
        }
        public bool Render(Bitmap Canvas, int AppID)
        {
            ImprovedVBE.DrawFilledRectangle(Canvas, ImprovedVBE.colourToNumber(255, 255, 255), 0, 22, (int)Canvas.Width, 30, false);
            XOff = 5;
            foreach(string s in SubmenuNames)
            {
                Coord.Add(new Coordinates(XOff, 30));
                int temp = BitFont.DrawBitFontString(Canvas, "ArialCustomCharset16", Color.Black, s, XOff, 30) + 15;
                if (MouseManager.X > XOff && MouseManager.X < XOff + temp)
                {
                    if(MouseManager.Y > 22 && MouseManager.Y < 52)
                    {
                        ImprovedVBE.DrawFilledRectangle(Canvas, Color.LightBlue.ToArgb(), XOff, 22, temp, 30, false);
                    }
                }
                XOff += BitFont.DrawBitFontString(Canvas, "ArialCustomCharset16", Color.Black, s, XOff, 30) + 15;
            }
            foreach(var v in SubmenuList)
            {
                if(MouseManager.MouseState == MouseState.Left)
                {
                    for(int i = 0; i < Coord.Count; i++)
                    {
                        if(i < Coord.Count - 1)
                        {
                            if(MouseManager.X > Coord[i].X && MouseManager.X < Coord[i + 1].X)
                            {
                                if(MouseManager.Y > 22 && MouseManager.Y < 52)
                                {
                                    if(Clicked == false)
                                    {
                                        if (SubmenuList[i].Clicked == true)
                                        {
                                            SubmenuList[i].Clicked = false;
                                        }
                                        else
                                        {
                                            foreach (var t in SubmenuList)
                                            {
                                                t.Clicked = false;
                                            }
                                            SubmenuList[i].Clicked = true;
                                        }
                                        Clicked = true;
                                        NeedUpdate = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if(MouseManager.X > Coord[i].X && MouseManager.X < Coord[i].X + XOff + 15)
                            {
                                if (MouseManager.Y > 22 && MouseManager.Y < 52)
                                {
                                    if (Clicked == false)
                                    {
                                        if (SubmenuList[i].Clicked == true)
                                        {
                                            SubmenuList[i].Clicked = false;
                                        }
                                        else
                                        {
                                            foreach (var t in SubmenuList)
                                            {
                                                t.Clicked = false;
                                            }
                                            SubmenuList[i].Clicked = true;
                                        }
                                        Clicked = true;
                                        NeedUpdate = true;
                                    }
                                }
                            }
                        }
                    }
                }
                if(v.Clicked == true)
                {
                    v.Render(Canvas, 3, 55, AppID);
                }
            }
            Coord.Clear();
            if(MouseManager.MouseState == MouseState.None)
            {
                Clicked = false;
                NeedUpdate = false;
            }
            return NeedUpdate;
        }
    }
    class Submenu
    {
        public string ID { get; set; }
        public List<Items> items = new List<Items>();
        public bool Clicked = false;
        public Submenu(string ID, List<Items> items)
        {
            this.ID = ID;
            this.items = items;
        }
        public bool sent = false;
        public void Render(Bitmap Canvas, int X, int Y, int AppID)
        {
            if(Clicked)
            {
                ImprovedVBE.DrawFilledRectangle(Canvas, ImprovedVBE.colourToNumber(255, 255, 255), X, Y, 200, 100, false);
                int offsteY = 3;
                for(int i = 0; i < items.Count; i++)
                {
                    if(MouseManager.Y > Y + offsteY && MouseManager.Y < Y + offsteY + 20)
                    {
                        if(MouseManager.X > X && MouseManager.X < X + 200)
                        {
                            if (MouseManager.MouseState == MouseState.Left)
                            {
                                if(sent == false)
                                {
                                    WindowMessenger.Send(new WindowMessage(items[i].Code, "Submenu", AppID.ToString()));
                                    sent = true;
                                }
                                //Kernel.Clipboard = items[i].Code;
                            }
                            ImprovedVBE.DrawFilledRectangle(Canvas, Color.LightBlue.ToArgb(), X + 1, Y + offsteY - 2, 198, 20, false);
                        }
                    }
                    if(MouseManager.MouseState == MouseState.None)
                    {
                        sent = false;
                    }
                    BitFont.DrawBitFontString(Canvas, "ArialCustomCharset16", Color.Black, items[i].Name, X + 3, Y + offsteY);
                    offsteY += 20;
                }
            }
        }
    }
    class Items
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Items(string name, string code)
        {
            Name = name;
            Code = code;
        }
    }
    class Coordinates
    {
        public int X { get; set;}
        public int Y { get; set;}
        public Coordinates(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }
}
