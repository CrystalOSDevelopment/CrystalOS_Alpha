using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CrystalOSAlpha.Applications.Calculator
{
    class Calculator : App
    {
        public Calculator(int X, int Y, int Z, int Width, int Height, string Name, Bitmap Icon)
        {
            this.x = X;
            this.y = Y;
            this.z = Z;
            this.width = Width;
            this.height = Height;
            this.name = Name;
            this.icon = Icon;
        }
        #region Window porpeties
        public int z { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int desk_ID { get; set; }
        public int AppID { get; set; }
        public string name { get; set; }
        public bool movable { get; set; }
        public bool minimised { get; set; }
        public Bitmap icon { get; set; }
        #endregion Window porpeties

        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);
        
        public string Content = "";
        
        public bool initial = true;
        public bool clicked = false;
        public bool temp = false;
        public bool once { get; set; }

        public Bitmap canvas;
        public Bitmap back_canvas;
        public Bitmap window { get; set; }

        public List<UIElementHandler> UIElements = new List<UIElementHandler>();

        public void App()
        {
            if(initial == true)
            {
                UIElements.Add(new Button(5, 75, 40, 40, "C", 1, "C"));
                UIElements.Add(new Button(50, 75, 40, 40, "Del", 1, "Del"));
                UIElements.Add(new Button(95, 75, 40, 40, "+", 1, "+"));
                UIElements.Add(new Button(140, 75, 40, 40, "-", 1, "-"));

                UIElements.Add(new Button(5, 120, 40, 40, "7", 1, "7"));
                UIElements.Add(new Button(50, 120, 40, 40, "8", 1, "8"));
                UIElements.Add(new Button(95, 120, 40, 40, "9", 1, "9"));
                UIElements.Add(new Button(140, 120, 40, 40, "*", 1, "*"));

                UIElements.Add(new Button(5, 165, 40, 40, "4", 1, "4"));
                UIElements.Add(new Button(50, 165, 40, 40, "5", 1, "5"));
                UIElements.Add(new Button(95, 165, 40, 40, "6", 1, "6"));
                UIElements.Add(new Button(140, 165, 40, 40, "/", 1, "/"));

                UIElements.Add(new Button(5, 210, 40, 40, "1", 1, "1"));
                UIElements.Add(new Button(50, 210, 40, 40, "2", 1, "2"));
                UIElements.Add(new Button(95, 210, 40, 40, "3", 1, "3"));
                UIElements.Add(new Button(140, 210, 40, 85, "Enter", 1, "Enter"));

                UIElements.Add(new Button(5, 255, 40, 40, "0", 1, "0"));
                UIElements.Add(new Button(50, 255, 40, 40, ",", 1, ","));
                UIElements.Add(new Button(95, 255, 40, 40, "(", 1, "("));

                UIElements.Add(new Button(5, 300, 40, 40, ")", 1, ")"));

                width = 185;

                UIElements.Add(new TextBox(5, 25, (int)(window.Width - 10), 40, ImprovedVBE.colourToNumber(60, 60, 60), Content, "0.", TextBox.Options.right, "TextBox1"));

                initial = false;
            }
            if(once == true)
            {
                (canvas, back_canvas, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);
                once = false;
                temp = true;
            }

            foreach (var element in UIElements)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if(MouseManager.X > x + element.X && MouseManager.X < x + element.X + element.Width)
                    {
                        if (MouseManager.Y > y + element.Y && MouseManager.Y < y + element.Y + element.Height)
                        {
                            if(clicked == false)
                            {
                                element.Clicked = true;
                                temp = true;
                                clicked = true;
                            }
                        }
                    }
                }
            }

            if(temp == true)
            {
                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                int ButtonWidth = (width - 25) / 4;
                int ButtonHeight = (height - 123) / 5;
                int i = 0;
                int Line = 0;
                foreach (var element in UIElements)
                {
                    switch (element.TypeOfElement)
                    {
                        case TypeOfElement.Button:
                            element.Width = ButtonWidth;
                            if (i == 0)
                            {
                                element.X = 5 + ButtonWidth * i;
                            }
                            else
                            {
                                element.X = (ButtonWidth + 5) * i + 5;
                            }

                            element.Height = ButtonHeight;
                            element.Y = 98 + (ButtonHeight + 5) * Line;
                            if (i == 3)
                            {
                                i = 0;
                                Line++;
                            }
                            else
                            {
                                i++;
                            }

                            if (element.Clicked == true)
                            {
                                int Col = element.Color;
                                element.Color = Color.White.ToArgb();
                                element.Render(window);
                                element.Color = Col;
                                if (element.Text == "Enter")
                                {
                                    Content = CalculatorA.Calculate(Content).ToString();
                                }
                                else if (element.Text == "C")
                                {
                                    Content = "";
                                }
                                else if (element.Text == "Del")
                                {
                                    if (Content.Length != 0)
                                    {
                                        Content = Content.Remove(Content.Length - 1);
                                    }
                                }
                                else
                                {
                                    Content += element.Text;
                                }
                                element.Clicked = false;
                            }
                            else
                            {
                                element.Render(window);
                            }
                            if (MouseManager.MouseState == MouseState.None)
                            {
                                element.Clicked = false;
                            }
                            break;

                        case TypeOfElement.TextBox:
                            element.Text = Content;
                            element.Width = (int)(window.Width - 10);
                            element.Render(window);
                            break;
                    }
                }

                temp = false;
            }

            if (MouseManager.MouseState == MouseState.None && clicked == true)
            {
                temp = true;
                clicked = false;
            }
        }
        public void RightClick()
        {

        }
    }
}
