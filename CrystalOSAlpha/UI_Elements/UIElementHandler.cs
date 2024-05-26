using Cosmos.System.Graphics;

namespace CrystalOSAlpha.UI_Elements
{
    public interface UIElementHandler
    {
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
        public string Text { get; set; }
        public string ID { get; set; }
        public TypeOfElement TypeOfElement { get; set; }
        public void Render(Bitmap Canvas);
        public bool CheckClick(int X, int Y);
    }
    public enum TypeOfElement
    {
        Button,
        CheckBox,
        DropDown,
        HorizontalScrollbar,
        PictureBox,
        Slider,
        Table,
        TextBox,
        VerticalScrollbar,
        Label,
        ProgressBar
    }
}