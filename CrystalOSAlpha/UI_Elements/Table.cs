using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CrystalOSAlpha.UI_Elements
{
    class Table
    {
        public int Width { get; set; } 
        public int Height { get; set; } 
        public int TableWidth { get; set; } 
        public int TableHeight { get; set; }
        public int VisibleWidth { get; set; }
        public int VisibleHeight { get; set; }
        public int X { get; set; } 
        public int Y { get; set; } 
        public string ID { get; set; }
        public int XOffset = 0;
        public int YOffset = 0;
        public bool Clicked = false;

        public Bitmap Canvas;

        public List<Cell> Cells = new List<Cell>();
        public Table(int Width, int Height, int TWidth, int THeight)
        {
            this.Width = Width;
            this.Height = Height;
            this.TableWidth = TWidth;
            this.TableHeight = THeight;
        }
        public Table(int X, int Y, int Width, int Height, int TWidth, int THeight, string ID, int VisibleX, int VisibleY)
        {
            this.Width = Width;
            this.Height = Height;
            this.TableWidth = TWidth;
            this.TableHeight = THeight;
            this.X = X;
            this.Y = Y;
            this.ID = ID;
            this.VisibleWidth = VisibleX;
            this.VisibleHeight = VisibleY;
        }
        public void Initialize()
        {
            Canvas = new Bitmap((uint)VisibleWidth, (uint)VisibleHeight, ColorDepth.ColorDepth32);
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
        public void Resize()
        {
            Canvas = new Bitmap((uint)VisibleWidth, (uint)VisibleHeight, ColorDepth.ColorDepth32);
        }
        public void SetValue(int X, int Y, string Value, bool writeprotected)
        {
            Cells.Find(d => d.X == Y && d.Y == X).Content = Value;
            Cells.Find(d => d.X == Y && d.Y == X).WriteProtected = writeprotected;
        }
        public string GetValue(int X, int Y)
        {
            return Cells.Find(d => d.X == X && d.Y == Y).Content;
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
        public void Render(Bitmap OnTo)
        {
            Y -= 44;
            for(int i = 0; i < VisibleHeight; i++)
            {
                Array.Copy(OnTo.RawData, (Y + i) * OnTo.Width + X, Canvas.RawData, i * Canvas.Width, VisibleWidth);
            }
            foreach (Cell c in Cells)
            {
                if(X + c.X * TableWidth / Width - 5 - XOffset < Canvas.Width && Y + c.Y * TableHeight / Height - 5 < Canvas.Height - YOffset)
                {
                    ImprovedVBE.DrawFilledRectangle(Canvas, ImprovedVBE.colourToNumber(69, 69, 69), X + c.X * TableWidth / Width - 5 - XOffset, Y + c.Y * TableHeight / Height - 5 - YOffset, TableWidth / Width, TableHeight / Height, false);
                    if (c.Selected == true)
                    {
                        ImprovedVBE.DrawFilledRectangle(Canvas, ImprovedVBE.colourToNumber(255, 255, 255), X + c.X * TableWidth / Width + 2 - 5 - XOffset, Y + c.Y * TableHeight / Height + 2 - 5 - YOffset, TableWidth / Width - 4, TableHeight / Height - 3, false);
                        BitFont.DrawBitFontString(Canvas, "ArialCustomCharset16", Color.Black, c.Content, X + c.X * TableWidth / Width - XOffset, Y + c.Y * TableHeight / Height - YOffset);
                    }
                    else
                    {
                        ImprovedVBE.DrawFilledRectangle(Canvas, ImprovedVBE.colourToNumber(50, 50, 50), X + c.X * TableWidth / Width + 2 - 5 - XOffset, Y + c.Y * TableHeight / Height + 2 - 5 - YOffset, TableWidth / Width - 4, TableHeight / Height - 3, false);
                        BitFont.DrawBitFontString(Canvas, "ArialCustomCharset16", Color.White, c.Content, X + c.X * TableWidth / Width - XOffset, Y + c.Y * TableHeight / Height - YOffset);
                    }
                }
            }
            ImprovedVBE.DrawImageAlpha(Canvas, X, Y, OnTo);
            Y += 44;
        }
        public bool Select2(int X, int Y)
        {
            int Top = (int)Math.Floor((Y - 22) / (decimal)(TableHeight / Height));
            int Left = (int)Math.Floor(X / ((decimal)TableWidth / Width));
            
            var p = Cells.Find(d => d.X == Left && d.Y == Top);
            
            if(p.Selected == false)
            {
                foreach(var v in Cells)
                {
                    v.Selected = false;
                }
                p.Selected = true;
                return true;
            }
            else
            {
                foreach (var v in Cells)
                {
                    v.Selected = false;
                }
                return true;
            }
        }
        public void Select(int X, int Y)
        {
            int Top = (int)Math.Floor(Y / 25.0);
            int Left = (int)Math.Floor(X / ((decimal)TableWidth / Width));
            foreach (var v in Cells)
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
