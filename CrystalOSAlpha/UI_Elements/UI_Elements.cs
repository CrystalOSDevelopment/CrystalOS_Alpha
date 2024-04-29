using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using System;
using System.Collections.Generic;
using static CrystalOSAlpha.UI_Elements.TextBox;

namespace CrystalOSAlpha.UI_Elements
{
    class UI_Elements
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string ID { get; set; }
        public string Sub_ID { get; set; }
        public string Content { get; set; }
        public string PlaceHolder { get; set; }
        public int Color { get; set; }
        public int BackgroundColor { get; set; }
        public string Font { get; set; }
        public Bitmap Image { get; set; }
        public bool Clicked = false;
        public ElementType EType { get; set; }
        public Options opt { get; set; }
        public KeyEvent Key { get; set; }

        /// <summary>
        /// Use this for labels
        /// </summary>
        /// <param name="X">X coordinate</param>
        /// <param name="Y">Y coordinate</param>
        /// <param name="Color">Color of text</param>
        /// <param name="Font">Font (registered fonts onnly!)</param>
        /// <param name="Content">Content of label</param>
        /// <param name="ID">ID of label</param>
        /// <param name="T">Type (this case: ElementType.Label)</param>
        /// <param name="BackgroundColor">The background color of label. By default it's 0 that means transparent</param>
        public UI_Elements(int X, int Y, int Color, string Content, string ID, ElementType T, string Font = "ArialCustomCharset16", int BackgroundColor = 0) 
        {
            this.X = X;
            this.Y = Y;
            this.Color = Color;
            this.Font = Font;
            this.Content = Content;
            this.ID = ID;
            this.BackgroundColor = BackgroundColor;
            this.EType = T;
        }

        /// <summary>
        /// Use this for buttons
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="Color"></param>
        /// <param name="BackgroundColor"></param>
        /// <param name="Content"></param>
        /// <param name="ID"></param>
        /// <param name="T"></param>
        /// <param name="Font"></param>
        public UI_Elements(int X, int Y, int Width, int Height, int Color, string Content, string ID, ElementType T, string Font = "ArialCustomCharset16")
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.Color = Color;
            this.BackgroundColor = BackgroundColor;
            this.Content = Content;
            this.ID = ID;
            this.EType = T;
            this.Font = Font;
        }

        /// <summary>
        /// Use this for TextBox
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinat</param>
        /// <param name="width">Width of TextBox</param>
        /// <param name="height">Height of TextBox</param>
        /// <param name="Color">Text color of TextBox</param>
        /// <param name="BackgroundColor">Background color of TextBox</param>
        /// <param name="iD">ID for reachout</param>
        /// <param name="content">Content of TextBox</param>
        /// <param name="placeHolder">Placeholder, when nothing is inside Content</param>
        /// <param name="T">In this case: ElementType.TextBox</param>
        /// <param name="Font">By default: ArialCustomCharset16</param>
        /// <param name="Opt">Adjusted to the left side by default</param>
        public UI_Elements(int x, int y, int width, int height, int Color, int BackgroundColor, string iD, string content, string placeHolder, ElementType T, string Font = "ArialCustomCharset16", Options Opt = Options.left)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.ID = iD;
            this.Content = content;
            this.PlaceHolder = placeHolder;
            this.Color = Color;
            this.BackgroundColor = BackgroundColor;
            this.Font = Font;
            this.EType = T;
            this.opt = Opt;
        }

        /// <summary>
        /// Use this for image rendering
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="Image"></param>
        /// <param name="ID"></param>
        /// <param name="T"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        public UI_Elements(int X, int Y, Bitmap Image, string ID, ElementType T, int Width = 0, int Height = 0)
        {
            Bitmap temp = Image;
            if(Image.Width != Width || Image.Height != Height)
            {
                if(Width == 0)
                {
                    temp = ImprovedVBE.ScaleImageStock(Image, Image.Width, (uint)Height);
                }
                else if(Height == 0)
                {
                    temp = ImprovedVBE.ScaleImageStock(Image, (uint)Width, Image.Height);
                }
                else
                {
                    temp = ImprovedVBE.ScaleImageStock(Image, (uint)Width, (uint)Height);
                }
            }
            this.X = X;
            this.Y = Y;
            this.Image = temp;
            this.ID = ID;
            this.EType = T;
        }
        public void Render(Bitmap Canvas)
        {
            switch(EType)
            {
                case ElementType.Label:
                    if(BackgroundColor == 0)
                    {
                        BitFont.DrawBitFontString(Canvas, Font, System.Drawing.Color.FromArgb(Color), Content, X, Y);
                    }
                    else
                    {

                    }
                    break;
                case ElementType.Button:
                    int offset = 0;
                    Bitmap canvas_Blank;

                    canvas_Blank = new Bitmap((uint)Width, (uint)Height, ColorDepth.ColorDepth32);
                    ImprovedVBE.DrawFilledRectangle(Canvas, ImprovedVBE.colourToNumber(36, 36, 36), X, Y, Width, Height, false);

                    if(Clicked == true)
                    {
                        ImprovedVBE.DrawFilledRectangle(Canvas, ComplimentaryColor.Generate(Color).ToArgb(), X + 2, Y + 2, Width - 4, Height - 4, false);
                        offset = BitFont.DrawBitFontString(canvas_Blank, "ArialCustomCharset16", System.Drawing.Color.FromArgb(Color), Content, Width - (Content.Length * 6) - 3, Height / 2 - 8);
                        BitFont.DrawBitFontString(Canvas, "ArialCustomCharset16", System.Drawing.Color.FromArgb(Color), Content, X + (Width / 2) - (offset / 2), Y + Height / 2 - 9);
                    }
                    else
                    {
                        ImprovedVBE.DrawFilledRectangle(Canvas, Color, X + 2, Y + 2, Width - 4, Height - 4, false);
                        offset = BitFont.DrawBitFontString(canvas_Blank, "ArialCustomCharset16", System.Drawing.Color.FromArgb(Color), Content, Width - (Content.Length * 6) - 3, Height / 2 - 8);
                        BitFont.DrawBitFontString(Canvas, "ArialCustomCharset16", ComplimentaryColor.Generate(Color), Content, X + (Width / 2) - (offset / 2), Y + Height / 2 - 9);
                    }
                    break;
                case ElementType.TextBox:
                    canvas = new Bitmap((uint)Width, (uint)Height, ColorDepth.ColorDepth32);
                    canvas_Blank = new Bitmap((uint)Width, (uint)Height, ColorDepth.ColorDepth32);
                    Array.Fill(canvas.RawData, ImprovedVBE.colourToNumber(36, 36, 36));

                    ImprovedVBE.DrawFilledRectangle(canvas, BackgroundColor, 2, 2, Width - 4, Height - 4, false);

                    if(Clicked == true && Key != null)
                    {
                        Content = Keyboard.HandleKeyboard(Content, Key);
                        Key = null;
                    }

                    string temp = Content;
                    switch (opt)
                    {
                        case Options.left:
                            if (Content == "")
                            {
                                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", System.Drawing.Color.Gray, PlaceHolder, (Width / 2) - (PlaceHolder.Length * 4), Height / 2 - 8);
                            }
                            else
                            {
                                offset = 0;
                                for (int i = Content.Length; i > 0; i--)
                                {
                                    offset += BitFont.DrawBitFontString(canvas_Blank, "ArialCustomCharset16", System.Drawing.Color.Black, Content[i].ToString(), 0, 0);
                                    if (offset > Width - 15)
                                    {
                                        temp = Content.Remove(0, i);
                                        break;
                                    }
                                }
                                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", System.Drawing.Color.FromArgb(Color), temp, 5, Height / 2 - 8);
                            }
                            break;
                        case Options.right:
                            if (Content == "")
                            {
                                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", System.Drawing.Color.Gray, PlaceHolder, (Width / 2) - (PlaceHolder.Length * 4), Height / 2 - 8);
                            }
                            else
                            {
                                if (Content.Length > 30)
                                {
                                    temp = Content.Remove(0, Content.Length - 30);
                                }
                                offset = BitFont.DrawBitFontString(canvas_Blank, "ArialCustomCharset16", System.Drawing.Color.White, Content, Width - (Content.Length * 6) - 3, Height / 2 - 8);
                                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", System.Drawing.Color.FromArgb(Color), temp, Width - offset - 3, Height / 2 - 8);
                            }
                            break;
                    }

                    ImprovedVBE.DrawImageAlpha(canvas, X, Y, Canvas);
                    break;
                case ElementType.PictureBox:
                    ImprovedVBE.DrawImage(Image, X, Y, Canvas);
                    break;
            }
        }
    }

    class IndexOfElement
    {
        public static int IndexOf(List<UI_Elements> Elements, string TargetID)
        {
            for(int i = 0; i < Elements.Count; i++)
            {
                if (Elements[i].ID == TargetID)
                {
                    return i;
                }
            }
            return -1;
        }
    }

    enum ElementType
    {
        Label,
        Button,
        TextBox,
        PictureBox,

        CheckBox,
        HorizontalScrollbar,
        VerticalScrollbar,
        Slider,
        Table,
    }
}
