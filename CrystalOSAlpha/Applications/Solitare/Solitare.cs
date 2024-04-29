using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;

namespace CrystalOSAlpha.Applications.Solitare
{
    class Solitare : App
    {
        #region core_values
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int desk_ID { get; set; }
        public int AppID { get; set; }
        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);
        public string name { get; set; }
        public bool minimised { get; set; }
        public bool movable { get; set; }
        public bool once { get; set; }
        public bool temp = true;
        public bool Clicked = false;
        public Bitmap icon { get; set; }
        public Bitmap window { get; set; }
        public Bitmap canvas;
        #endregion core_values

        public List<UI_Elements.UI_Elements> Elements = new List<UI_Elements.UI_Elements>();

        public void App()
        {
            if(Elements.Count == 0)
            {
                //Label
                Elements.Add(new UI_Elements.UI_Elements(100, 100, 2, "This is a test message", "Demo_text", ElementType.Label, "ArialCustomCharset16"));
                //Button
                Elements.Add(new UI_Elements.UI_Elements(10, 32, 200, 25, 2, "Demo Button", "Demo1", ElementType.Button));
                //TextBox
                Elements.Add(new UI_Elements.UI_Elements(10, 130, 200, 25, ImprovedVBE.colourToNumber(255, 255, 255), ImprovedVBE.colourToNumber(60, 60, 60), "TextBox1", "", "PlaceHolder", ElementType.TextBox));
                //PictureBox
                Elements.Add(new UI_Elements.UI_Elements(10, 150, Resources.Celebration, "TestImage", ElementType.PictureBox, 80, 80));
            }

            if(once == true)
            {
                Bitmap back = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                (canvas, back, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);
                once = false;
                temp = true;
            }

            if(MouseManager.MouseState == MouseState.Left && Clicked == false)
            {
                temp = true;
                Clicked = true;
            }
            else if(MouseManager.MouseState == MouseState.None && Clicked == true)
            {
                temp = true;
                Clicked = false;
            }

            if(KeyboardManager.TryReadKey(out KeyEvent Key))
            {
                temp = true;
            }

            if (temp == true)
            {
                temp = false;
                //Clear the canvas
                Array.Copy(canvas.RawData, window.RawData, canvas.RawData.Length);
                foreach(var v in Elements)
                {
                    switch (v.EType)
                    {
                        case ElementType.Label:
                            break;
                        case ElementType.Button:
                            if(MouseManager.MouseState == MouseState.Left)
                            {
                                if(MouseManager.X > x + v.X && MouseManager.X < x + v.X + v.Width)
                                {
                                    if(MouseManager.Y > y + v.Y && MouseManager.Y < y + v.Y + v.Height)
                                    {
                                        if(v.Clicked == false)
                                        {
                                            switch (v.ID)
                                            {
                                                case "Demo1":
                                                    int Index = IndexOfElement.IndexOf(Elements, "Demo_text");
                                                    if(Index != -1)
                                                    {
                                                        if(Elements[Index].Content != "This is a modified text!")
                                                        {
                                                            Elements[Index].Content = "This is a modified text!";
                                                        }
                                                        else
                                                        {
                                                            Elements[Index].Content = "This is a test message";
                                                        }
                                                    }
                                                    temp = true;
                                                    break;
                                            }
                                        }
                                        v.Clicked = true;
                                    }
                                }
                            }
                            else if(MouseManager.MouseState == MouseState.None)
                            {
                                v.Clicked = false;
                            }
                            break;
                        case ElementType.TextBox:
                            v.Key = Key;
                            if (MouseManager.MouseState == MouseState.Left)
                            {
                                foreach (var t in Elements)
                                {
                                    if (t.Clicked == true)
                                    {
                                        t.Clicked = false;
                                    }
                                }
                                if (MouseManager.X > x + v.X && MouseManager.X < x + v.X + v.Width)
                                {
                                    if (MouseManager.Y > y + v.Y && MouseManager.Y < y + v.Y + v.Height)
                                    {
                                        v.Clicked = true;
                                    }
                                }

                            }
                            break;
                        case ElementType.PictureBox:
                            break;
                    }
                    v.Render(window);
                }
            }

            ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
        }

        public void RightClick()
        {

        }
    }
}
