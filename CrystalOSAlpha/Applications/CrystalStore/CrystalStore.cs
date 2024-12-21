using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.TaskBar;
using CrystalOSAlpha.Graphics.Widgets;
using CrystalOSAlpha.System32;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.IO;
using Kernel = CrystalOS_Alpha.Kernel;

namespace CrystalOSAlpha.Applications.CrystalStore
{
    class CrystalStore : App
    {
        #region Properties
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
        #endregion Properties

        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);
        public int AnimationLeft = 0;
        public int SelectedCardIndex = 0;

        public Bitmap canvas;
        public Bitmap RoundedWorkspace;
        public Bitmap RoundedWorkspaceBackUp;

        public bool initial = true;
        public bool clicked = false;
        public bool Rerender = true;
        public bool AnimationEnabled = false;

        public List<UIElementHandler> UIElements = new List<UIElementHandler>();
        public List<Cards> Cards = new List<Cards>();
        public List<Cards> CardsBackBuffer = new List<Cards>();

        public List<Cards> Popular = new List<Cards>();//Later I might refer to this as all apps.

        public void App()
        {
            if(Kernel.IsNetSupport == false)
            {
                throw new Exception("Failed to establish an Ethernet\nconnection!");
            }
            if (initial)
            {
                UIElements.Add(new Button(8, 65, 45, 35, "Home", ImprovedVBE.colourToNumber(60, 60, 60), "Home"));
                UIElements.Add(new Button(8, 122, 45, 35, "Apps", ImprovedVBE.colourToNumber(60, 60, 60), "Apps"));
                UIElements.Add(new Button(8, 179, 45, 35, "Games", ImprovedVBE.colourToNumber(60, 60, 60), "Games"));
                UIElements.Add(new Button(8, 528, 45, 35, "About", ImprovedVBE.colourToNumber(60, 60, 60), "About"));

                UIElements.Add(new TextBox(78, 37, 525, 25, ImprovedVBE.colourToNumber(60, 60, 60), "", "Search apps, games, themes etc.", TextBox.Options.left, "SearchBox"));
                UIElements.Add(new Button(533, 15, 70, 25, "Search", ImprovedVBE.colourToNumber(60, 60, 60), "Search"));

                UIElements.Add(new Button(67, 122, 25, 154, "<", ImprovedVBE.colourToNumber(60, 60, 60), "Left"));
                UIElements.Add(new Button(769, 122, 25, 154, ">", ImprovedVBE.colourToNumber(60, 60, 60), "Right"));

                //Request data and store it on the disk
                File.WriteAllText("0:\\Store.txt", Kernel.getContent("", "CrystalStore/AppList.txt"));
                string[] lines = File.ReadAllText("0:\\Store.txt").Split("---==---", StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < lines.Length; i++)
                {
                    if (i < lines.Length)
                    {
                        string AppName = "";
                        string Description = "";
                        string[] Line = lines[i].Replace("\r", "").Split("\n");
                        for (int j = 0; j < Line.Length; j++)
                        {
                            Line[j] = Line[j].Trim();
                            if (Line[j].Contains("AppPath:"))
                            {
                                AppName = Line[j].Remove(0, Line[j].IndexOf(":") + 1).Trim();
                            }
                            else if (Line[j].Contains("Description:"))
                            {
                                Description = Line[j].Remove(0, Line[j].IndexOf(":") + 1).Trim();
                            }
                        }
                        if (AppName != "")
                        {
                            Cards.Add(new Cards(18 + i * (500 + 25), 26, 500, 250, AppName.Remove(0, AppName.LastIndexOf("/") + 1), Description, "Image", "AppID", "Category", "Developer"));
                        }
                    }
                    else
                    {
                        Cards.Add(new Cards(18 + i * (500 + 25), 26, 500, 250, "Title", "Description", "Image", "AppID", "Category", "Developer"));
                    }
                }

                int Count = 0;
                int Top = 334;
                int Left = 34;
                for (int i = 0; i < 9; i++)
                {
                    if(Count % 3 == 0 && Count != 0)
                    {
                        Top += 110;
                        Left = 34;
                    }
                    Popular.Add(new Cards(Left, Top, 213, 90, "Title", "Description", "Image", "AppID", "Category", "Developer"));
                    Left += 232;
                    Count++;
                }
                initial = false;
            }
            if (once)
            {
                Bitmap back = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                (canvas, back, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);
                ImprovedVBE.DrawImageAlpha(ImprovedVBE.ScaleImageStock(new Bitmap(TaskManager.Elephant), 38, 38), 13, 30, canvas);

                RoundedWorkspace = Base.Widget_Back(745, 540, ImprovedVBE.colourToNumber(36, 36, 36));
                RoundedWorkspaceBackUp = Base.Widget_Back(745, 540, ImprovedVBE.colourToNumber(36, 36, 36));

                once = false;
            }

            KeyEvent key = null;
            if (MouseManager.MouseState == MouseState.Left && clicked == false || KeyboardManager.TryReadKey(out key) || UIElements.FindAll(d => d.TypeOfElement == TypeOfElement.Button && d.Clicked).Count != 0 || Rerender)
            {
                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                Array.Copy(RoundedWorkspaceBackUp.RawData, RoundedWorkspace.RawData, RoundedWorkspaceBackUp.RawData.Length);
                foreach (UIElementHandler UIElement in UIElements)
                {
                    switch (UIElement.TypeOfElement)
                    {
                        case TypeOfElement.Button:
                            UIElement.CheckClick(x, y);
                            if (UIElement.Clicked && clicked == false)
                            {
                                switch (UIElement.ID)
                                {
                                    case "Home":
                                        break;
                                    case "Apps":
                                        break;
                                    case "Games":
                                        break;
                                    case "About":
                                        break;
                                    case "Search":
                                        break;
                                    case "Left":
                                        if(AnimationEnabled == false && Cards[0].X != 18)
                                        {
                                            AnimationEnabled = true;
                                            AnimationLeft = Cards[0].Width + 25;
                                        }
                                        break;
                                    case "Right":
                                        if (AnimationEnabled == false && Cards[^1].X > 200)
                                        {
                                            AnimationEnabled = true;
                                            AnimationLeft = -1 * (Cards[0].Width + 25);
                                        }
                                        break;
                                }
                                clicked = true;
                            }
                            break;
                        case TypeOfElement.TextBox:
                            if (UIElement.CheckClick(x, y))
                            {
                                foreach (UIElementHandler UI in UIElements)
                                {
                                    if (UI.TypeOfElement == TypeOfElement.TextBox)
                                    {
                                        UI.Clicked = false;
                                    }
                                }
                                UIElement.Clicked = true;
                            }
                            if (UIElement.Clicked == true)
                            {
                                if (key != null)
                                {
                                    UIElement.Text = Keyboard.HandleKeyboard(UIElement.Text, key);
                                }
                            }
                            break;
                        case TypeOfElement.CheckBox:
                            if (clicked == false)
                            {
                                if (UIElement.CheckClick(x, y))
                                {
                                    foreach (UIElementHandler UI in UIElements)
                                    {
                                        if (UI.TypeOfElement != TypeOfElement.CheckBox)
                                        {
                                            UI.Clicked = false;
                                        }
                                    }
                                    clicked = true;
                                }
                            }
                            break;
                        case TypeOfElement.Slider:
                            UIElement.CheckClick(x, y);
                            break;
                        case TypeOfElement.Polygon:
                            UIElement.MinVal = width;
                            UIElement.MaxVal = height;
                            break;
                        //No rendering here, beacuse layer order
                    }
                }
                foreach (Cards card in Cards)
                {
                    card.Generate(RoundedWorkspace);
                }
                foreach (Cards card in Popular)
                {
                    card.Generate(RoundedWorkspace);
                }

                int JumpRight = 50;
                //Drawing selected card with points
                for (int i = 0; i < Cards.Count; i++)
                {
                    if (Cards[i].X > 0 && Cards[i].X < 200)
                    {
                        ImprovedVBE.DrawFilledEllipse(RoundedWorkspace, JumpRight, Cards[i].Y + Cards[i].Height + 30, 12, 12, ImprovedVBE.colourToNumber(255, 255, 255));
                        JumpRight += 30;
                    }
                    else
                    {
                        ImprovedVBE.DrawFilledEllipse(RoundedWorkspace, JumpRight, Cards[i].Y + Cards[i].Height + 30, 8, 8, ImprovedVBE.colourToNumber(200, 200, 200));
                        JumpRight += 30;
                    }
                }
                ImprovedVBE.DrawImageAlpha(RoundedWorkspace, 61, 70, window);

                //UIElements are rendered here, after the workspace is rendered
                foreach (var UIElement in UIElements)
                {
                    UIElement.Render(window);
                }
                Rerender = false;
            }
            if (MouseManager.MouseState == MouseState.None && clicked == true)
            {
                clicked = false;
            }

            if (AnimationEnabled)
            {
                if(AnimationLeft > 0)
                {
                    if(AnimationLeft > 50)
                    {
                        AnimationLeft -= 50;
                        foreach (Cards card in Cards)
                        {
                            card.X += 50;
                        }
                    }
                    else
                    {
                        foreach (Cards card in Cards)
                        {
                            card.X += AnimationLeft;
                        }
                        AnimationLeft = 0;
                    }
                }
                else if(AnimationLeft < 0)
                {
                    if (AnimationLeft < 50)
                    {
                        AnimationLeft += 50;
                        foreach (Cards card in Cards)
                        {
                            card.X -= 50;
                        }
                    }
                    else
                    {
                        foreach (Cards card in Cards)
                        {
                            card.X -= AnimationLeft;
                        }
                        AnimationLeft = 0;
                    }
                }
                else
                {
                    AnimationEnabled = false;
                }
                Rerender = true;
            }
        }

        public void RightClick()
        {
            
        }
    }
}
