using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CrystalOSAlpha.Applications.Calculator
{
    class Calculator : App
    {
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

        public int CurrentColor = ImprovedVBE.colourToNumber(Global_integers.R, Global_integers.G, Global_integers.B);
        
        public string Content = "";
        
        public bool initial = true;
        public bool clicked = false;
        public bool once = true;

        public Bitmap canvas;
        public Bitmap back_canvas;
        public Bitmap window;

        public List<Button_prop> Buttons = new List<Button_prop>();

        public void App()
        {
            if(initial == true)
            {
                Buttons.Add(new Button_prop(5, 75, 40, 40, "C", 1));
                Buttons.Add(new Button_prop(50, 75, 40, 40, "Del", 1));
                Buttons.Add(new Button_prop(95, 75, 40, 40, "+", 1));
                Buttons.Add(new Button_prop(140, 75, 40, 40, "-", 1));

                Buttons.Add(new Button_prop(5, 120, 40, 40, "7", 1));
                Buttons.Add(new Button_prop(50, 120, 40, 40, "8", 1));
                Buttons.Add(new Button_prop(95, 120, 40, 40, "9", 1));
                Buttons.Add(new Button_prop(140, 120, 40, 40, "*", 1));

                Buttons.Add(new Button_prop(5, 165, 40, 40, "4", 1));
                Buttons.Add(new Button_prop(50, 165, 40, 40, "5", 1));
                Buttons.Add(new Button_prop(95, 165, 40, 40, "6", 1));
                Buttons.Add(new Button_prop(140, 165, 40, 40, "/", 1));

                Buttons.Add(new Button_prop(5, 210, 40, 40, "1", 1));
                Buttons.Add(new Button_prop(50, 210, 40, 40, "2", 1));
                Buttons.Add(new Button_prop(95, 210, 40, 40, "3", 1));
                Buttons.Add(new Button_prop(140, 210, 40, 85, "Enter", 1));

                Buttons.Add(new Button_prop(5, 255, 40, 40, "0", 1));
                Buttons.Add(new Button_prop(50, 255, 40, 40, ",", 1));
                Buttons.Add(new Button_prop(95, 255, 40, 40, ")", 1));

                Buttons.Add(new Button_prop(5, 300, 40, 40, "(", 1));

                width = 185;

                initial = false;
            }
            if(once == true)
            {
                (canvas, back_canvas, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);

                foreach(var button in Buttons)
                {
                    if(button.Clicked == true)
                    {
                        Button.Button_render(canvas, button.X, button.Y, button.Width, button.Height, Color.White.ToArgb(), button.Text);
                        if(button.Text == "Enter")
                        {
                            Content = CalculatorA.Calculate(Content).ToString();
                        }
                        else if(button.Text == "C")
                        {
                            Content = "";
                        }
                        else if(button.Text == "Del")
                        {
                            if(Content.Length != 0)
                            {
                                Content = Content.Remove(Content.Length - 1);
                            }
                        }
                        else
                        {
                            Content += button.Text;
                        }
                        button.Clicked = false;
                    }
                    else
                    {
                        Button.Button_render(canvas, button.X, button.Y, button.Width, button.Height, button.Color, button.Text);
                    }
                    if(MouseManager.MouseState == MouseState.None)
                    {
                        button.Clicked = false;
                    }
                }

                TextBox.Box(canvas, 5, 25, (int)(canvas.Width - 10), 40, ImprovedVBE.colourToNumber(60, 60, 60), Content, "Sample text", TextBox.Options.right);

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                once = false;
            }

            foreach (var button in Buttons)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if(MouseManager.X > x + button.X && MouseManager.X < x + button.X + button.Width)
                    {
                        if (MouseManager.Y > y + button.Y && MouseManager.Y < y + button.Y + button.Height)
                        {
                            if(clicked == false)
                            {
                                button.Clicked = true;
                                once = true;
                                clicked = true;
                            }
                        }
                    }
                }
            }

            if (MouseManager.MouseState == MouseState.None && clicked == true)
            {
                once = true;
                clicked = false;
            }

            ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
        }
    }
}
