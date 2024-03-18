using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.UI_Elements;
using ProjectDMG;
using System.Collections.Generic;
using System.Drawing;

namespace CrystalOSAlpha.Applications.Gameboy
{
    class Core : App
    {
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

        public int CurrentColor = ImprovedVBE.colourToNumber(Global_integers.R, Global_integers.G, Global_integers.B);

        public bool once = true;
        public bool Power_on = true;
        public bool initial = true;
        public bool clicked = false;

        public Bitmap canvas;

        public List<Button_prop> Buttons = new List<Button_prop>();

        ProjectDMG.ProjectDMG gameboy = new ProjectDMG.ProjectDMG();
        public void App()
        {
            if(initial == true)
            {
                Buttons.Add(new Button_prop(3, 23, 50, 23, "Game1", 1));
                Buttons.Add(new Button_prop(56, 23, 50, 23, "Game2", 1));
                Buttons.Add(new Button_prop(109, 23, 50, 23, "Game3", 1));
                Buttons.Add(new Button_prop(162, 23, 50, 23, "Game4", 1));
                initial = false;
            }
            if(once == true)
            {
                canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);

                #region corners
                ImprovedVBE.DrawFilledEllipse(canvas, 10, 10, 10, 10, CurrentColor);
                ImprovedVBE.DrawFilledEllipse(canvas, width - 11, 10, 10, 10, CurrentColor);
                ImprovedVBE.DrawFilledEllipse(canvas, 10, height - 10, 10, 10, CurrentColor);
                ImprovedVBE.DrawFilledEllipse(canvas, width - 11, height - 10, 10, 10, CurrentColor);

                ImprovedVBE.DrawFilledRectangle(canvas, CurrentColor, 0, 10, width, height - 20, false);
                ImprovedVBE.DrawFilledRectangle(canvas, CurrentColor, 5, 0, width - 10, 15, false);
                ImprovedVBE.DrawFilledRectangle(canvas, CurrentColor, 5, height - 15, width - 10, 15, false);
                #endregion corners

                canvas = ImprovedVBE.EnableTransparency(canvas, x, y, canvas);

                ImprovedVBE.DrawGradientLeftToRight(canvas);

                ImprovedVBE.DrawFilledEllipse(canvas, width - 13, 10, 8, 8, ImprovedVBE.colourToNumber(255, 0, 0));

                ImprovedVBE.DrawFilledEllipse(canvas, width - 34, 10, 8, 8, ImprovedVBE.colourToNumber(227, 162, 37));

                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, name, 2, 2);

                foreach (var button in Buttons)
                {
                    if (button.Clicked == true)
                    {
                        Button.Button_render(canvas, button.X, button.Y, button.Width, button.Height, Color.White.ToArgb(), button.Text);
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
                        button.Clicked = false;
                    }
                    else
                    {
                        Button.Button_render(canvas, button.X, button.Y, button.Width, button.Height, button.Color, button.Text);
                    }
                    if (MouseManager.MouseState == MouseState.None)
                    {
                        button.Clicked = false;
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
            ImprovedVBE.DrawImageAlpha(canvas, x, y, ImprovedVBE.cover);

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
            }

            gameboy.EXECUTE();

        }
    }
}
