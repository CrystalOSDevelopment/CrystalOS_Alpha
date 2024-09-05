using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.System32;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using TaskScheduler = CrystalOSAlpha.Graphics.TaskScheduler;
using Kernel = CrystalOS_Alpha.Kernel;

namespace CrystalOSAlpha.Applications.WebscapeNavigator
{
    class Webscape : App
    {
        #region important
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public int desk_ID { get; set; }
        public int AppID { get; set; }

        public string name { get; set; }

        public bool minimised { get; set; }
        public bool movable { get; set; }
        
        public Bitmap icon { get; set; }
        #endregion important

        public int Reg_Y = 0;
        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);

        public bool initial = true;
        public bool clicked = false;
        public bool once { get; set; }
        public bool temp = true;

        public string content = "";
        public string source = "";

        public Bitmap canvas;
        public Bitmap window { get; set; }
        public Bitmap Container;
        
        public List<UIElementHandler> Scroll = new List<UIElementHandler>();
        public List<UIElementHandler> Buttons = new List<UIElementHandler>();
        public List<UIElementHandler> TextBoxes = new List<UIElementHandler>();

        public void App()
        {
            if (initial == true)
            {
                Buttons.Add(new Button(5, 27, 50, 20, "Go", 1));

                Scroll.Add(new VerticalScrollbar(width - 22, 52, 20, height - 60, 20, 0, 500, ""));

                TextBoxes.Add(new TextBox(60, 27, width - 84, 20, ImprovedVBE.colourToNumber(60, 60, 60), "httpforever.com", "Url:", TextBox.Options.left, "URL"));

                initial = false;
            }
            if (once == true)
            {
                Bitmap back = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                (canvas, back, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);

                Container = new Bitmap((uint)(width - 29), (uint)(height - 60), ColorDepth.ColorDepth32);
                Array.Fill(Container.RawData, ImprovedVBE.colourToNumber(255, 255, 255));

                foreach (var button in Buttons)
                {
                    if (button.Clicked == true)
                    {
                        int Col = button.Color;
                        button.Color = Color.White.ToArgb();
                        button.Render(canvas);
                        button.Color = Col;

                        switch (button.Text)
                        {
                            case "Go":
                                content = Kernel.getContent(TextBoxes[0].Text);//"<html>\n  <head></head>\n  <body>\n\n    <h1>Connection error.</h1>\n    <p>the page that you were searching was not found.<br>plese try to reload the page</p>\n    \n  </body>\n</html>";
                                File.WriteAllText("0:\\index.txt", content);
                                temp = true;
                                break;
                        }
                    }
                    else
                    {
                        button.Render(canvas);
                    }
                    if (MouseManager.MouseState == MouseState.None)
                    {
                        button.Clicked = false;
                    }
                }

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                
                //Array.Copy(Container.RawData, 0, Webrendering.Render(Container, content).RawData, 0, Container.RawData.Length);
                once = false;
                temp = true;
            }

            foreach (var button in Buttons)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if (MouseManager.X > x + button.X && MouseManager.X < x + button.X + button.Width)
                    {
                        if (MouseManager.Y > y + button.Y && MouseManager.Y < y + button.Y + button.Height)
                        {
                            if (clicked == false)
                            {
                                button.Clicked = true;
                                once = true;
                                clicked = true;
                            }
                        }
                    }
                }
                if (clicked == true && MouseManager.MouseState == MouseState.None)
                {
                    once = true;
                    button.Clicked = false;
                    clicked = false;
                }
            }

            foreach (var vscroll in Scroll)
            {
                if (vscroll.CheckClick((int)MouseManager.X - x, (int)MouseManager.Y - y))
                {
                    temp = true;
                }
            }

            foreach (var Box in TextBoxes)
            {
                if (Box.CheckClick(x + Box.X, y + Box.Y) == true && clicked == false)
                {
                    foreach (var box2 in TextBoxes)
                    {
                        box2.Clicked = false;
                    }
                    clicked = true;
                    Box.Clicked = true;
                }
            }

            if (TaskScheduler.counter == TaskScheduler.Apps.Count - 1)
            {
                KeyEvent key;
                if (KeyboardManager.TryReadKey(out key))
                {
                    if (key.Key == ConsoleKeyEx.Enter)
                    {
                        Buttons[0].Clicked = true;
                        clicked = true;
                        once = true;
                    }
                    else
                    {
                        TextBoxes[0].Text = Keyboard.HandleKeyboard(TextBoxes[0].Text, key);
                    }

                    Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                    temp = true;
                }
            }

            if (temp == true)
            {
                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                foreach (var vscroll in Scroll)
                {
                    vscroll.X = width - 22;
                    vscroll.Height = height - 60;
                    vscroll.Render(window);
                }
                foreach (var Box in TextBoxes)
                {
                    Box.Width = width - 84;
                    Box.Render(window);
                }
                ImprovedVBE.DrawImageAlpha(Container, 5, 52, window);

                temp = false;
            }
            
            ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
        }

        public void RightClick()
        {

        }
    }
}
