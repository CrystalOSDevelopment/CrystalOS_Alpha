using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOS_Alpha;
using CrystalOSAlpha.Graphics.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Kernel = CrystalOS_Alpha.Kernel;

namespace CrystalOSAlpha.UI_Elements
{
    class Table
    {
        public int Width { get; set; } 
        public int Height { get; set; } 
        public int TableWidth { get; set; } 
        public int TableHeight { get; set; } 

        public List<Cell> Cells = new List<Cell>();
        public Table(int Width, int Height, int TWidth, int THeight)
        {
            this.Width = Width;
            this.Height = Height;
            this.TableWidth = TWidth;
            this.TableHeight = THeight;
        }
        public void Initialize()
        {
            int X = 0;
            int Y = 0;
            for(int i = 0; i < Width * Height; i++)
            {
                Cells.Add(new Cell(X, Y, "", false, false));
                if(X < Width - 1)
                {
                    X++;
                }
                else
                {
                    Y++;
                    X = 0;
                }
            }
        }
        public void SetValue(int X, int Y, string Value, bool writeprotected)
        {
            Cells.Find(d => d.X == Y && d.Y == X).Content = Value;
            Cells.Find(d => d.X == Y && d.Y == X).WriteProtected = writeprotected;
        }
        public string GetValue(int X, int Y)
        {
            return Cells.Find(d => d.X == Y && d.Y == X).Content;
        }
        public void Render(Bitmap OnTo, int X, int Y)
        {
            foreach(Cell c in Cells)
            {
                ImprovedVBE.DrawFilledRectangle(OnTo, ImprovedVBE.colourToNumber(69, 69, 69), X + c.X * TableWidth / Width - 5, Y + c.Y * 25 - 5, TableWidth / Width, 25, false);
                if(c.Selected == true)
                {
                    ImprovedVBE.DrawFilledRectangle(OnTo, ImprovedVBE.colourToNumber(255, 255, 255), X + c.X * TableWidth / Width + 2 - 5, Y + c.Y * 25 + 2 - 5, TableWidth / Width - 4, 22, false);
                    BitFont.DrawBitFontString(OnTo, "ArialCustomCharset16", Color.Black, c.Content, X + c.X * TableWidth / Width, Y + c.Y * 25);
                }
                else
                {
                    ImprovedVBE.DrawFilledRectangle(OnTo, ImprovedVBE.colourToNumber(50, 50, 50), X + c.X * TableWidth / Width + 2 - 5, Y + c.Y * 25 + 2 - 5, TableWidth / Width - 4, 22, false);
                    BitFont.DrawBitFontString(OnTo, "ArialCustomCharset16", Color.White, c.Content, X + c.X * TableWidth / Width, Y + c.Y * 25);
                }
            }
        }
        public void Select(int X, int Y)
        {
            int Top = (int)Math.Floor(Y / 25.0);
            int Left = (int)Math.Floor(X / ((decimal)TableWidth / Width));
            foreach(var v in Cells)
            {
                v.Selected = false;
            }
            Cells.Find(d => d.X == Left && d.Y == Top).Selected = true;
        }
    }
    class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Content { get; set; }
        public bool WriteProtected { get; set; }
        public bool Selected { get; set; }
        public Cell(int X, int Y, string Content, bool writeprotected, bool selected)
        {
            this.X = X;
            this.Y = Y;
            this.Content = Content;
            this.WriteProtected = writeprotected;
            this.Selected = selected;
        }
    }
}
