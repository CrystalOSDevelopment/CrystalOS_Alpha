using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace CrystalOSAlpha.UI_Elements
{
    class Table : UIElementHandler
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
        //Since this is basically useless in this case, it will function as a feedback bool to show if the given selected cell is write protected
        public bool Clicked { get; set; }
        public string Text { get; set; }
        public int Color { get; set; }
        public TypeOfElement TypeOfElement { get; set; }
        public int Pos { get; set; }
        public int Value { get; set; }
        public float Sensitivity { get; set; }
        public int LockedPos { get; set; }
        //This can be used for the x coordinate(Column) of the selected cell
        public int MinVal { get; set; }
        //This can be used for the y coordinate(Row) of the selected cell
        public int MaxVal { get; set; }

        public Bitmap Canvas;
        public List<Cell> Cells = new List<Cell>();
        public int CellWidth = 0;
        public int CellHeight = 0;
        //TODO: Remove this below
        public Table(int Width, int Height, int TWidth, int THeight)
        {
            this.Width = Width;
            this.Height = Height;
            this.TableWidth = TWidth;
            this.TableHeight = THeight;
        }
        /// <summary>
        /// Creates a table
        /// </summary>
        /// <param name="X">Position of the table on the x-axis</param>
        /// <param name="Y">Position of the table on the y-axis</param>
        /// <param name="Width">Width of the table</param>
        /// <param name="Height">Height of the table</param>
        /// <param name="TWidth">Amount of columns in the table</param>
        /// <param name="THeight">Amount of rows in the table</param>
        /// <param name="ID">ID of the table</param>
        public Table(int X, int Y, int Width, int Height, int TWidth, int THeight, string ID)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.TableWidth = TWidth;
            this.TableHeight = THeight;
            this.ID = ID;
            this.TypeOfElement = TypeOfElement.Table;

            CellWidth = Width / TableWidth;
            CellHeight = Height / TableHeight;
            Canvas = new Bitmap((uint)((uint)CellWidth * TWidth + 3), (uint)((uint)CellHeight * THeight + 3), ColorDepth.ColorDepth32);
            int XVal = 0;
            int YVal = 0;
            for (int i = 0; i < TableWidth * TableHeight; i++)
            {
                Cells.Add(new Cell(XVal, YVal, "", false, false));
                if (XVal < TableWidth - 1)
                {
                    XVal++;
                }
                else
                {
                    YVal++;
                    XVal = 0;
                }
            }
        }
        public void Render(Bitmap Window)
        {
            Array.Fill(Canvas.RawData, ImprovedVBE.colourToNumber(32, 32, 32));
            Bitmap cell = new Bitmap((uint)CellWidth - 4, (uint)CellHeight - 4, ColorDepth.ColorDepth32);
            foreach (Cell c in Cells)
            {
                if(c.Selected == true)
                {
                    Array.Fill(cell.RawData, ImprovedVBE.colourToNumber(200, 200, 200));
                    BitFont.DrawBitFontString(cell, "ArialCustomCharset16", System.Drawing.Color.Black, c.Content, 3, CellHeight / 2 - 12);
                    Text = c.X + "," + c.Y;
                }
                else
                {
                    Array.Fill(cell.RawData, ImprovedVBE.colourToNumber(69, 69, 69));
                    BitFont.DrawBitFontString(cell, "ArialCustomCharset16", System.Drawing.Color.White, c.Content, 3, CellHeight / 2 - 12);
                }
                ImprovedVBE.DrawImage(cell, c.X * CellWidth + 3, c.Y * CellHeight + 3, Canvas);
            }
            ImprovedVBE.DrawImageAlpha(Canvas, X, Y, Window);
        }
        public void SetValue(int X, int Y, string Value, bool writeprotected)
        {
            Cells.Find(d => d.X == X && d.Y == Y).Content = Value;
            Cells.Find(d => d.X == X && d.Y == Y).WriteProtected = writeprotected;
        }
        public string GetValue(int X, int Y)
        {
            return Cells.Find(d => d.X == X && d.Y == Y).Content;
        }
        public bool CheckClick(int X, int Y)
        {
            int Left = (int)MouseManager.X - X - this.X;
            int Top = (int)MouseManager.Y - Y - this.Y;
            foreach(var v in Cells)
            {
                v.Selected = false;
            }

            if(Top >= 0 && Top <= Height && Left >= 0 && Left <= Width)
            {
                int Row = Top / CellHeight;
                int Column = Left / CellWidth;
                Cells.Find(d => d.X == Column && d.Y == Row).Selected = true;
                Clicked = Cells.Find(d => d.X == Column && d.Y == Row).WriteProtected;
                MinVal = Column;
                MaxVal = Row;
                return true;
            }
            Clicked = false;
            MinVal = -1;
            MaxVal = -1;
            return false;
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
