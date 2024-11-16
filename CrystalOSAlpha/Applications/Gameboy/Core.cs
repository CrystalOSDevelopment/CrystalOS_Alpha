using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.UI_Elements;
using ProjectDMG;
using System.Collections.Generic;
using System.Drawing;

namespace CrystalOSAlpha.Applications.Gameboy
{
    class Core : App
    {
        public Core(int X, int Y, int Z, int Width, int Height, string Name, Bitmap Icon)
        {
            this.x = X;
            this.y = Y;
            this.z = Z;
            this.width = Width;
            this.height = Height;
            this.name = Name;
            this.icon = Icon;
        }

        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int desk_ID { get; set; }
        public int AppID { get; set; }
        public string name {get ; set; }
        public bool minimised { get; set; }
        public bool movable { get; set; }
        public Bitmap icon { get; set; }

        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);

        public bool once { get; set; }
        public bool Power_on = true;
        public bool initial = true;
        public bool clicked = false;

        public Bitmap canvas;
        public Bitmap window { get; set; }

        public List<Button> Buttons = new List<Button>();

        ProjectDMG.ProjectDMG gameboy = new ProjectDMG.ProjectDMG();
        public void App()
        {
            if(initial == true)
            {
                Buttons.Add(new Button(3, 23, 50, 23, "Game1", 1));
                Buttons.Add(new Button(56, 23, 50, 23, "Game2", 1));
                Buttons.Add(new Button(109, 23, 50, 23, "Game3", 1));
                Buttons.Add(new Button(162, 23, 50, 23, "Game4", 1));
                initial = false;
            }
            if(once == true)
            {
                width = 162 * 3 - 4;
                height = 165 * 3 - 39 + 25;
                Bitmap back = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                window = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                (canvas, back, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);

                foreach (var button in Buttons)
                {
                    if (button.Clicked == true)
                    {
                        int Col = button.Color;
                        button.Color = Color.White.ToArgb();
                        button.Render(canvas);
                        button.Color = Col;
                        if (button.Text == "Game1")
                        {
                            ProjectDMG.ProjectDMG.gamenum = 0;
                            gameboy.POWER_ON();
                        }
                        else if (button.Text == "Game2")
                        {
                            ProjectDMG.ProjectDMG.gamenum = 1;
                            gameboy.POWER_ON();
                            
                        }
                        else if (button.Text == "Game3")
                        {
                            ProjectDMG.ProjectDMG.gamenum = 2;
                            gameboy.POWER_ON();
                        }
                        else
                        {
                            ProjectDMG.ProjectDMG.gamenum = 3;
                            gameboy.POWER_ON();
                        }
                    }
                    else
                    {
                        button.Render(canvas);
                    }
                }
                once = false;
            }
            if(Power_on == true)
            {
                gameboy.POWER_ON();
                Power_on = false;
            }

            PPU.x = x + 1;
            PPU.y = y + 22 + 26;
            if (ImprovedVBE.RequestRedraw == true)
            {
                switch (GlobalValues.TaskBarType)
                {
                    case "Classic":
                        ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
                        break;
                    case "Nostalgia":
                        ImprovedVBE.DrawImage(window, x, y, ImprovedVBE.cover);
                        break;
                }
            }
            //ImprovedVBE.DrawImageAlpha(canvas, x, y, ImprovedVBE.cover);

            foreach (var button in Buttons)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if (MouseManager.X > x + button.X && MouseManager.X < x + button.X + button.Width)
                    {
                        if (MouseManager.Y > y + button.Y && MouseManager.Y < y + button.Y + button.Height)
                        {
                            if (button.Clicked == false)
                            {
                                button.Clicked = true;
                                once = true;
                                clicked = true;
                            }
                        }
                    }
                }
                else
                {
                    if(button.Clicked == true)
                    {
                        button.Clicked = false;
                        once = true;
                    }
                }
            }

            gameboy.EXECUTE();
        }

        public void RightClick()
        {

        }
    }
}
