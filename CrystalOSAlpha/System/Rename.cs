using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Applications;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CrystalOSAlpha.SystemApps
{
    class Rename : App
    {
        #region Essential
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
        public Bitmap window { get; set; }
        #endregion Essential

        #region UI_Elements
        public List<Button_prop> Buttons = new List<Button_prop>();
        public List<TextBox> TextBoxes = new List<TextBox>();
        #endregion UI_Elements

        #region Extra
        public Rename(int X, int Y, int Width, int Height, string Title, string Content, Bitmap Icon)
        {
            x = X;
            y = Y;
            z = 999;
            width = Width;
            height = Height;
            name = Title;
            this.Content = Content;
            icon = Icon;
        }

        public bool once { get; set; }
        public bool initial = true;
        public bool temp = true;
        public bool clicked = false;
        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);
        public string Content = "";
        public Bitmap canvas;
        public Bitmap back_canvas;
        #endregion Extra

        public void App()
        {
            if(initial == true)
            {
                Buttons.Add(new Button_prop(230, 147, 75, 25, "Ok", 1, "Ok"));
                Buttons.Add(new Button_prop(316, 147, 75, 25, "Abort", 1, "Abort"));

                TextBoxes.Add(new TextBox(50, 100, 300, 25, ImprovedVBE.colourToNumber(60, 60, 60), "", "Ex.: TestFile.txt", TextBox.Options.left, "NewName"));
                initial = false;
            }
            if(once == true)
            {
                (canvas, back_canvas, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);
                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, Content, 9, 71);
                once = false;
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
                                temp = true;
                                clicked = true;
                            }
                        }
                    }
                }
                else if (MouseManager.MouseState == MouseState.None)
                {
                    if (button.Clicked == true)
                    {
                        button.Clicked = false;
                        temp = true;
                    }

                }
            }

            foreach (var Box in TextBoxes)
            {
                if (Box.Clciked(x + Box.X, y + Box.Y) == true && clicked == false)
                {
                    foreach (var box2 in TextBoxes)
                    {
                        box2.Selected = false;
                    }
                    clicked = true;
                    Box.Selected = true;
                }
            }

            if (TaskScheduler.Apps[^1] == this)
            {
                KeyEvent key;
                if (KeyboardManager.TryReadKey(out key))
                {
                    if(key.Key == ConsoleKeyEx.Enter)
                    {
                        WindowMessenger.Send(new WindowMessage(TextBoxes[0].Text, name, "FileSystem"));
                        TaskScheduler.Apps.Remove(this);
                    }
                    else
                    {
                        foreach (var box in TextBoxes)
                        {
                            if (box.Selected == true)
                            {
                                box.Text = Keyboard.HandleKeyboard(box.Text, key);
                            }
                        }
                    }
                    temp = true;
                }
            }

            if (temp == true)
            {
                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                foreach (var button in Buttons)
                {
                    if (button.Clicked == true)
                    {
                        Button.Button_render(window, button.X, button.Y, button.Width, button.Height, Color.White.ToArgb(), button.Text);

                        switch (button.ID)
                        {

                        }
                    }
                    else
                    {
                        Button.Button_render(window, button.X, button.Y, button.Width, button.Height, button.Color, button.Text);
                    }
                }

                foreach (var Box in TextBoxes)
                {
                    Box.Box(window, Box.X, Box.Y);
                }
                temp = false;
            }
            ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);

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
