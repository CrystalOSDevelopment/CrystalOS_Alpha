using Cosmos.System.Graphics;
using System.Collections.Generic;
using System.Drawing;

namespace CrystalOSAlpha.UI_Elements.Shapes
{
    public class Circle : UIElementHandler
    {
        public Circle(int Color, List<Point> Points, bool Visible, bool Filled, string ID)
        {
            this.Color = Color;
            this.ID = ID;
            this.Points = Points;
            this.Visible = Visible;
            this.TypeOfElement = TypeOfElement.Circle;
            this.Filled = Filled;
            for (int i = 0; i < this.Points.Count; i++)
            {
                this.Points[i] = new Point(this.Points[i].X, this.Points[i].Y + 22);
            }
        }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Color { get; set; }
        public int Pos { get; set; }
        public int Value { get; set; }
        public float Sensitivity { get; set; }
        public int LockedPos { get; set; }
        public int MinVal { get; set; }
        public int MaxVal { get; set; }
        public bool Clicked { get; set; }
        public bool Visible = true;
        public bool Filled = true;
        public string Text { get; set; }
        public string ID { get; set; }
        public List<Point> Points { get; set; }
        public TypeOfElement TypeOfElement { get; set; }

        public void Render(Bitmap Canvas)
        {
            if (Visible)
            {
                if (Filled)
                {
                    ImprovedVBE.DrawFilledEllipse(Canvas, Points[0].X, Points[0].Y, Points[1].X - Points[0].X, Points[1].X - Points[0].X, Color);
                }
                else
                {
                    ImprovedVBE.DrawCircle(Canvas, Points[0].X, Points[0].Y, Points[1].X - Points[0].X, Color);
                }
            }
        }
        public bool CheckClick(int X, int Y)
        {
            return false;
        }

        public string GetValue(int X, int Y)
        {
            return "";
        }

        public void SetValue(int X, int Y, string Value, bool writeprotected)
        {

        }
    }
}
