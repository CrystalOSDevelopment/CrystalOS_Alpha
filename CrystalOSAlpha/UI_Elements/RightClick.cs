using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOSAlpha.UI_Elements
{
    class RightClick
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Color { get; set; }
        public int Selected = 0;
        public string ID { get; set; }

        public bool Found = false;
        
        public Color TextColor = System.Drawing.Color.White;
        
        public List<string> TextLines = new List<string>();
        public RightClick(int X, int Y, int Width, int Height, List<string> MenuItemNames, string iD)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.TextLines = MenuItemNames;
            ID = iD;
        }
        public void ProcessNRender()
        {
            Height = TextLines.Count * 25 + 10;
            int Top = 5;
            //Graphical appearance
            Bitmap Canvas = new Bitmap((uint)Width, (uint)Height, ColorDepth.ColorDepth32);
            Array.Fill(Canvas.RawData, 1);
            ImprovedVBE.DrawFilledRectangle(Canvas, ImprovedVBE.colourToNumber(Global_integers.R, Global_integers.G, Global_integers.B), 2, 2, Width - 4, Height - 4, false);
            //Render MenuItems
            for(int i = 0; i < TextLines.Count; i++)
            {
                //Check if the cursor is pointing to Item
                if(MouseManager.X > X && MouseManager.X < X + Width)
                {
                    if(MouseManager.Y > Y + Top && MouseManager.Y < Y + Top + 25)
                    {
                        ImprovedVBE.DrawFilledRectangle(Canvas, ImprovedVBE.colourToNumber(0, 0, 255), 2, Top, Width - 4, 25, false);
                        Selected = i;
                        Found = true;
                    }
                }
                if (TextLines[i].EndsWith(":Extensive:"))
                {
                    string temp = TextLines[i].Replace(":Extensive:", "->");
                    BitFont.DrawBitFontString(Canvas, "ArialCustomCharset16", TextColor, temp, 3, Top);
                }
                else
                {
                    BitFont.DrawBitFontString(Canvas, "ArialCustomCharset16", TextColor, TextLines[i], 3, Top);
                }
                Top += 25;
            }
            if(Found == false)
            {
                Selected = -99;
            }
            Found = false;
            ImprovedVBE.DrawImage(Canvas, X, Y, ImprovedVBE.cover);
        }
    }
}
