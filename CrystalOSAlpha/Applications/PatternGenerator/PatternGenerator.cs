using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.UI_Elements;
using System.Drawing;
using System;
using System.Collections.Generic;
using Cosmos.System;
using CrystalOSAlpha.Graphics.TaskBar;
using CrystalOSAlpha.System32;

namespace CrystalOSAlpha.Applications.PatternGenerator
{
    class PatternGenerator : App
    {
        public PatternGenerator(int X, int Y, int Z, int Width, int Height, string Name, Bitmap Icon)
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
        public bool once { get; set; }
        public Bitmap icon { get; set; }
        public Bitmap window { get; set; }
        #endregion Window porpeties

        #region Extras
        public bool temp = false;
        public bool clicked = false;

        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);
        public int SelectedColor = ImprovedVBE.colourToNumber(50, 50, 50);

        public Bitmap canvas;
        public Bitmap ColorPreview;
        public Bitmap Container = new Bitmap(240, 240, ColorDepth.ColorDepth32);

        public List<int> Colors = new List<int>();
        public List<TextBox> TextBoxes = new List<TextBox>();
        public List<Button> Buttons = new List<Button>();
        #endregion Extras

        public void App()
        {
            if (once == true)
            {
                if(width != 375)
                {
                    width = 375;
                }
                if(height != 307)
                {
                    height = 307;
                }
                Bitmap back = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                (canvas, back, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);

                ColorPreview = new Bitmap(120, 114, ColorDepth.ColorDepth32);
                Array.Fill(ColorPreview.RawData, 0);
                ImprovedVBE.DrawFilledRectangle(ColorPreview, SelectedColor, 2, 2, (int)ColorPreview.Width - 4, (int)ColorPreview.Height - 4);

                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, "Coloring grid", 7, 35);

                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, "Red", 251, 61);
                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, "Green", 251, 98);
                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, "Blue", 251, 135);

                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, "Current color", 251, 164);

                //Coloring grid initialisation
                Array.Fill(Container.RawData, 0);

                for(int i = 0; i < 16 * 16; i++)
                {
                    Colors.Add(ImprovedVBE.colourToNumber(255, 255, 255));
                }

                for(int i = 0; i < 16; i++)
                {
                    ImprovedVBE.DrawLine(Container, i * 15, 1, i * 15, Container.Height, 0);
                }

                for (int i = 0; i < 16; i++)
                {
                    ImprovedVBE.DrawLine(Container, 0, i * 15, Container.Width, i * 15, 0);
                }

                if(TextBoxes.Count == 0)
                {
                    TextBoxes.Add(new TextBox(291, 56, 80, 25, ImprovedVBE.colourToNumber(60, 60, 60), "50", "Red", TextBox.Options.left, "Red"));
                    TextBoxes.Add(new TextBox(291, 93, 80, 25, ImprovedVBE.colourToNumber(60, 60, 60), "50", "Green", TextBox.Options.left, "Green"));
                    TextBoxes.Add(new TextBox(291, 130, 80, 25, ImprovedVBE.colourToNumber(60, 60, 60), "50", "Blue", TextBox.Options.left, "Blue"));

                    Buttons.Add(new Button(216, 2, 155, 25, "Create wallpaper", 1, "Create_Wallpaper"));
                    Buttons.Add(new Button(117, 2, 95, 25, "Fill", 1, "Fill"));
                }
                else
                {
                    if (int.TryParse(TextBoxes[0].Text, out int Red) && int.TryParse(TextBoxes[1].Text, out int Green) && int.TryParse(TextBoxes[2].Text, out int Blue))
                    {
                        SelectedColor = ImprovedVBE.colourToNumber(Red, Green, Blue);
                    }
                }

                temp = true;
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

            if (MouseManager.MouseState == MouseState.Left)
            {
                if (MouseManager.X > x + 7 && MouseManager.X < x + 7 + Container.Width)
                {
                    if(MouseManager.Y > y + 56 && MouseManager.Y < y + 56 + Container.Height)
                    {
                        // Calculate the grid width for a 16x16 grid
                        int gridWidth = 16; // 16 columns

                        // Calculate relative position from the mouse
                        int ExtractX = (int)(MouseManager.X - x - 7); // Correct the offset
                        int ExtractY = (int)(MouseManager.Y - y - 56);

                        // Determine the row and column in the 16x16 grid
                        int column = ExtractX / 15; // Adjusted for 16-pixel cells
                        int row = ExtractY / 15;

                        // Calculate the 1D index from 2D coordinates
                        int index = row * gridWidth + column;

                        // Ensure the calculated index is within bounds
                        if (column >= 0 && column < gridWidth && row >= 0 && row < gridWidth)
                        {
                            if (index >= 0 && index < Colors.Count) // Double-check array bounds
                            {
                                if (Colors[index] != SelectedColor)
                                {
                                    Colors[index] = SelectedColor; // Assign the color
                                    temp = true;
                                }
                            }
                        }
                    }
                }
            }

            if (MouseManager.MouseState == MouseState.Right)
            {
                if (MouseManager.X > x + 7 && MouseManager.X < x + 7 + Container.Width)
                {
                    if (MouseManager.Y > y + 56 && MouseManager.Y < y + 56 + Container.Height)
                    {
                        // Calculate the grid width for a 16x16 grid
                        int gridWidth = 16; // 16 columns

                        // Calculate relative position from the mouse
                        int ExtractX = (int)(MouseManager.X - x - 7); // Correct the offset
                        int ExtractY = (int)(MouseManager.Y - y - 56);

                        // Determine the row and column in the 16x16 grid
                        int column = ExtractX / 15; // Adjusted for 16-pixel cells
                        int row = ExtractY / 15;

                        // Calculate the 1D index from 2D coordinates
                        int index = row * gridWidth + column;

                        // Ensure the calculated index is within bounds
                        if (column >= 0 && column < gridWidth && row >= 0 && row < gridWidth)
                        {
                            if (index >= 0 && index < Colors.Count) // Double-check array bounds
                            {
                                SelectedColor = Colors[index];
                                temp = true;
                            }
                        }
                    }
                }
            }

            if (MouseManager.MouseState == MouseState.None)
            {
                clicked = false;
            }

            if (TaskScheduler.Apps[^1] == this)
            {
                KeyEvent key;
                if (KeyboardManager.TryReadKey(out key))
                {
                    if (key.Key == ConsoleKeyEx.Enter)
                    {

                    }
                    else
                    {
                        foreach (var box in TextBoxes)
                        {
                            if (box.Clicked == true)
                            {
                                box.Text = Keyboard.HandleKeyboard(box.Text, key);
                                temp = true;
                            }
                        }
                    }
                    if (int.TryParse(TextBoxes[0].Text, out int Red) && int.TryParse(TextBoxes[1].Text, out int Green) && int.TryParse(TextBoxes[2].Text, out int Blue))
                    {
                        SelectedColor = ImprovedVBE.colourToNumber(Red, Green, Blue);
                        ImprovedVBE.DrawFilledRectangle(ColorPreview, SelectedColor, 2, 2, (int)ColorPreview.Width - 4, (int)ColorPreview.Height - 4);
                    }
                    temp = true;
                }
            }

            if (temp == true)
            {
                Array.Copy(canvas.RawData, window.RawData, canvas.RawData.Length);

                int XIndex = 0;
                int YIndex = 0;
                foreach(var v in Colors)
                {
                    ImprovedVBE.DrawFilledRectangle(Container, v, XIndex, YIndex, 14, 14);
                    if(XIndex + 15 < Container.Width)
                    {
                        XIndex += 15;
                    }
                    else
                    {
                        XIndex = 0;
                        YIndex += 15;
                    }
                }

                foreach (var v in TextBoxes)
                {
                    v.Render(window);
                }

                foreach (var button in Buttons)
                {
                    if (button.Clicked == true)
                    {
                        int Col = button.Color;
                        button.Color = Color.White.ToArgb();
                        button.Render(window);
                        button.Color = Col;

                        switch (button.ID)
                        {
                            case "Create_Wallpaper":
                                // Iterate over each pixel in the bitmap to render the tiled pattern
                                for (int y = 0; y < ImprovedVBE.height; y += 16)
                                {
                                    for (int x = 0; x < ImprovedVBE.width; x += 16)
                                    {
                                        for (int i = 0; i < 256; i++)
                                        {
                                            int pixelX = x + (i % 16);
                                            int pixelY = y + (i / 16);
                                            if (pixelX < ImprovedVBE.width && pixelY <  ImprovedVBE.height)
                                            {
                                                ImprovedVBE.DrawPixel(ImprovedVBE.data, pixelX, pixelY, Colors[i]);
                                            }
                                        }
                                    }
                                }
                                TaskManager.update = true;
                                TaskManager.resize = true;
                                TaskManager.Back_Buffer = null;
                                SideNav.Get_Back = true;
                                break;
                            case "Fill":
                                for(int i = 0; i < Colors.Count; i++)
                                {
                                    Colors[i] = SelectedColor;
                                }
                                break;
                        }
                    }
                    else
                    {
                        button.Render(window);
                    }
                }

                ImprovedVBE.DrawImage(Container, 7, 56, window);
                ImprovedVBE.DrawImage(ColorPreview, 251, 182, window);
                temp = false;
            }
        }

        public void RightClick()
        {
            
        }
    }
}
