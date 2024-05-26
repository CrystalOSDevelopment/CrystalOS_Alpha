using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Applications;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using TaskScheduler = CrystalOSAlpha.Graphics.TaskScheduler;

namespace CrystalOSAlpha.SystemApps
{
    class MsgBox : App
    {
        #region Essential
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public int desk_ID {get; set; }
        public int AppID { get; set; }
        public string name {get; set; }

        public bool minimised { get; set; }
        public bool movable { get; set; }
        public Bitmap icon { get; set; }
        #endregion Essential

        public List<Button> Buttons = new List<Button>();
        public bool initial = true;
        public bool once { get; set; }
        public bool clicked = false;

        public Bitmap canvas;
        public Bitmap back_canvas;
        public Bitmap window { get; set; }
        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);

        public string message = "";

        public int x_1 = 0;

        public MsgBox(int z_index, int x, int y, int width, int height, string title, string message, Bitmap Icon)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.message = message;
            this.name = title;
            this.icon = Icon;
            this.z = z_index;
        }

        public void App()
        {
            if (initial == true)
            {
                Buttons.Add(new Button(width / 2 - 35, height - 45, 70, 20, "OK", 1));

                initial = false;
            }
            if (once == true)
            {
                (canvas, back_canvas, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);

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
                            case "OK":
                                TaskScheduler.Apps.Remove(this);
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

                ImprovedVBE.DrawImageAlpha(Resources.Celebration, 20, (int)(height / 2 - Resources.Celebration.Height / 2), canvas);

                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, message, (int)(Resources.Celebration.Width + 60), (int)(height / 2 - Resources.Celebration.Height / 2 + 10));

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
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

            ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
        }

        public void RightClick()
        {

        }
    }
}
