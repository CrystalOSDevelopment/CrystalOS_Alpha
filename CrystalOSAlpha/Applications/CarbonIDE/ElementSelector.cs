using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.SystemApps;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;

namespace CrystalOSAlpha.Applications.CarbonIDE
{
    class ElementSelector : App
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
        public string Path { get; set; }
        public string namedProject { get; set; }
        public bool minimised { get; set; }
        public bool movable { get; set; }
        public Bitmap icon { get; set; }

        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);

        public Bitmap canvas;
        public Bitmap back_canvas;
        public Bitmap window { get; set; }

        public bool initial = true;
        public bool once { get; set; }
        public bool temp { get; set; }
        public bool clicked = false;
        #endregion Essential

        #region Variables
        public int DestinationID = -1;
        public int Selected = 0;

        public Bitmap ListDisplay;

        public List<UIElementHandler> handlers = new List<UIElementHandler>();
        public List<UIElementHandler> Elements = new List<UIElementHandler>();
        #endregion Variables

        public ElementSelector(int X, int Y, int Width, int Height, int IDDestination, List<UIElementHandler> ElementsToList, Bitmap Icon)
        {
            this.x = X;
            this.y = Y;
            this.width = Width;
            this.height = Height;
            this.DestinationID = IDDestination;
            this.name = "Element Selector";
            this.icon = Icon;
            handlers = ElementsToList;
            z = 999;
        }

        public void App()
        {
            z = 999;
            if (once == true)
            {
                canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                window = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                ListDisplay = new Bitmap((uint)(width - 30), (uint)(height - 90), ColorDepth.ColorDepth32);

                Elements.Clear();
                Elements.Add(new Button(width - 35, 16, 20, (int)ListDisplay.Height / 2, "/\\", 1, "Up"));
                Elements.Add(new Button(width - 35, 15 + (int)ListDisplay.Height / 2, 20, (int)ListDisplay.Height / 2, "\\/", 1, "Down"));

                Elements.Add(new Button(221, 467, 90, 30, "Select", 1, "Approve"));
                Elements.Add(new Button(316, 467, 90, 30, "Cnacel", 1, "Cancel"));

                (canvas, back_canvas, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);

                once = false;
                temp = true;
            }

            if (MouseManager.MouseState == MouseState.Left && TaskScheduler.Apps[^1] == this && clicked == false)
            {
                temp = true;
            }
            else if (MouseManager.MouseState == MouseState.None && clicked)
            {
                temp = true;
                clicked = false;
            }

            if (temp == true)
            {
                temp = false;

                Array.Fill(ListDisplay.RawData, ImprovedVBE.colourToNumber(60, 60, 60));
                ImprovedVBE.DrawFilledRectangle(ListDisplay, ImprovedVBE.colourToNumber(36, 36, 36), 2, 2, (int)ListDisplay.Width, (int)ListDisplay.Height);

                int Top = 3;
                int itemHeight = 28; // The height of each item including margin
                int itemsPerPage = ((int)ListDisplay.Height - 4) / itemHeight; // Number of items that can fully fit on the page
                int totalHeight = itemsPerPage * itemHeight;

                int extraSpace = (int)ListDisplay.Height - totalHeight;
                if (extraSpace > 0)
                {
                    itemHeight += extraSpace / itemsPerPage; // Distribute the extra space evenly among all items
                    Top = 3; // Reset Top after adjusting item height
                }

                int firstVisibleItem = Math.Max(0, Selected - itemsPerPage + 1);

                int Counter = 0;
                foreach (var v in handlers)
                {
                    if (Counter >= firstVisibleItem && Counter < firstVisibleItem + itemsPerPage)
                    {
                        string Assembled = v.ID;
                        switch (v.TypeOfElement)
                        {
                            case TypeOfElement.Button:
                                Assembled += " (Button)";
                                break;
                            case TypeOfElement.Label:
                                Assembled += " (Label)";
                                break;
                            case TypeOfElement.TextBox:
                                Assembled += " (TextBox)";
                                break;
                            case TypeOfElement.Slider:
                                Assembled += " (Slider)";
                                break;
                            case TypeOfElement.CheckBox:
                                Assembled += " (CheckBox)";
                                break;
                            case TypeOfElement.PictureBox:
                                Assembled += " (PictureBox)";
                                break;
                        }

                        if (Counter == Selected)
                        {
                            ImprovedVBE.DrawFilledRectangle(ListDisplay, ImprovedVBE.colourToNumber(80, 80, 80), 3, Top - 3, (int)ListDisplay.Width - 6, 25);
                        }
                        BitFont.DrawBitFontString(ListDisplay, "ArialCustomCharset16", System.Drawing.Color.White, Assembled, 3, Top);
                        Top += itemHeight;
                    }
                    Counter++;
                }

                foreach (var Element in Elements)
                {
                    if(MouseManager.MouseState == MouseState.Left && clicked == false || MouseManager.MouseState == MouseState.None)
                    {
                        Element.CheckClick(x, y);
                        if (Element.Clicked)
                        {
                            switch (Element.ID)
                            {
                                case "Up":
                                    if(Selected > 0)
                                    {
                                        Selected--;
                                        temp = true;
                                    }
                                    break;
                                case "Down":
                                    if(Selected < handlers.Count - 1)
                                    {
                                        Selected++;
                                        temp = true;
                                    }
                                    break;
                                case "Approve":
                                    WindowMessenger.Send(new WindowMessage(Selected.ToString(), name, DestinationID.ToString()));
                                    TaskScheduler.Apps.Remove(this);
                                    break;
                                case "Cancel":
                                    TaskScheduler.Apps.Remove(this);
                                    break;
                            }
                        }
                    }
                }

                ImprovedVBE.DrawImage(ListDisplay, 15, 37, window);

                foreach(var Element in Elements)
                {
                    Element.Render(window);
                }

                if (MouseManager.MouseState == MouseState.Left)
                {
                    clicked = true;
                }
            }

            ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
        }

        public void RightClick()
        {
            
        }
    }
}
