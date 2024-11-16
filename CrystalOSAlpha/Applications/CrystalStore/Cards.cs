using Cosmos.System.Graphics;
using CrystalOS_Alpha.Graphics.Widgets;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Widgets;
using System.Drawing;

namespace CrystalOSAlpha.Applications.CrystalStore
{
    public class Cards
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Bitmap Received { get; set; }
        public string Image { get; set; }
        public string AppID { get; set; }
        public string Category { get; set; }
        public string Developer { get; set; }
        public Bitmap FinishedOutput = null;
        
        public string BufferedDescription = "";

        public Cards(int X, int Y, int Width, int Height, string Title, string Description, string Image, string AppID, string Category, string Developer)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.Title = Title;
            this.Description = Description;
            this.Image = Image;
            this.AppID = AppID;
            this.Category = Category;
            this.Developer = Developer;
        }
        public void Generate(Bitmap Canvas, int XOffset = 0, int YOffset = 0)
        {
            if(FinishedOutput == null)
            {
                FinishedOutput = Base.Widget_Back(Width, Height, ImprovedVBE.colourToNumber(100, 100, 100));
                BitFont.DrawBitFontString(FinishedOutput, "VerdanaCustomCharset32", Color.White, Title, 10, 10);
                if(BufferedDescription.Length > 0)
                {
                    if(Height > 100)
                    {
                        BitFont.DrawBitFontString(FinishedOutput, "ArialCustomCharset16", Color.White, BufferedDescription, 10, Height - 30);
                    }
                    else
                    {
                        BitFont.DrawBitFontString(FinishedOutput, "ArialCustomCharset16", Color.White, BufferedDescription, 10, Height - 10);
                    }
                }
                else
                {
                    BufferedDescription = ChuckNorrisFacts.LineBreak(Description, 40);
                    if(Height > 100)
                    {
                        BitFont.DrawBitFontString(FinishedOutput, "ArialCustomCharset16", Color.White, BufferedDescription, 10, Height - 30 - (10 * BufferedDescription.Split("\n").Length - 1));
                    }
                    else
                    {
                        BitFont.DrawBitFontString(FinishedOutput, "ArialCustomCharset16", Color.White, BufferedDescription, 10, Height - 10 - (10 * BufferedDescription.Split("\n").Length - 1));
                    }
                }
            }
            ImprovedVBE.DrawImageAlpha(FinishedOutput, X - XOffset, Y - YOffset, Canvas);
        }
    }
}
