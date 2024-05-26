using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Applications;
using CrystalOSAlpha.Applications.CarbonIDE;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.SystemApps;
using CrystalOSAlpha.UI_Elements;
using System.Drawing;
using System;
using System.Collections.Generic;
using System.IO;

namespace CrystalOSAlpha.System32.Installer
{
    class CopyFiles : App
    {
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
        public Bitmap canvas;
        public Bitmap back_canvas;

        public bool initial = true;
        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);
        public List<UIElementHandler> Elements = new List<UIElementHandler>();

        public int iterator = 0;
        public string Currently_Created = "";

        public CopyFiles(int X, int Y, int Z, int Width, int Height, string Title, Bitmap Icon)
        {
            x = X;
            y = Y;
            z = Z;
            width = Width;
            height = Height;
            name = Title;
            icon = Icon;
        }

        public void App()
        {
            if (initial == true)
            {
                Elements.Add(new Progressbar(40, 367, 620, 45, ImprovedVBE.colourToNumber(100, 255, 10), 0, "CreationProgress"));
                Elements.Add(new Progressbar(40, 527, 620, 45, ImprovedVBE.colourToNumber(100, 255, 10), 0, "OverallPorgress"));
                once = true;
                initial = false;
            }
            if (once == true)
            {
                (canvas, back_canvas, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);

                BitFont.DrawBitFontString(canvas, "VerdanaCustomCharset32", Color.White, "Copying files", 30, 52);
                BitFont.DrawBitFontString(canvas, "VerdanaCustomCharset24", Color.White, "The setup now formats your drive and starts copying files.", 40, 142);

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                once = false;
            }

            Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);

            //Iteration process
            if (iterator % 2 == 0)
            {
                switch (iterator)
                {
                    case 0:
                        Currently_Created = "0:\\User\\" + GlobalValues.Username + "\\Favorites";
                        Directory.CreateDirectory("0:\\User\\" + GlobalValues.Username + "\\Favorites");
                        Elements.Find(d => d.ID == "CreationProgress").Value = 50;
                        break;
                    case 2:
                        Currently_Created = "0:\\User\\" + GlobalValues.Username + "\\Documents";
                        Directory.CreateDirectory("0:\\User\\" + GlobalValues.Username + "\\Documents");
                        Elements.Find(d => d.ID == "CreationProgress").Value = 50;
                        break;
                    case 4:
                        Currently_Created = "0:\\User\\" + GlobalValues.Username + "\\Pictures";
                        Directory.CreateDirectory("0:\\User\\" + GlobalValues.Username + "\\Pictures");
                        Elements.Find(d => d.ID == "CreationProgress").Value = 50;
                        break;
                    case 6:
                        Currently_Created = "0:\\User\\" + GlobalValues.Username + "\\Films";
                        Directory.CreateDirectory("0:\\User\\" + GlobalValues.Username + "\\Films");
                        Elements.Find(d => d.ID == "CreationProgress").Value = 50;
                        break;
                    case 8:
                        Currently_Created = "0:\\User\\" + GlobalValues.Username + "\\Wastebasket";
                        Directory.CreateDirectory("0:\\User\\" + GlobalValues.Username + "\\Wastebasket");
                        Elements.Find(d => d.ID == "CreationProgress").Value = 50;
                        break;
                    case 10:
                        Currently_Created = "0:\\User\\" + GlobalValues.Username + "\\SecurityArea";
                        Directory.CreateDirectory("0:\\User\\" + GlobalValues.Username + "\\SecurityArea");
                        Elements.Find(d => d.ID == "CreationProgress").Value = 50;
                        break;
                    case 12:
                        Currently_Created = "0:\\System";
                        Directory.CreateDirectory("0:\\System");
                        Elements.Find(d => d.ID == "CreationProgress").Value = 50;
                        break;
                    case 14:
                        Currently_Created = "0:\\System\\FrequentApps.sys";
                        File.Create("0:\\System\\FrequentApps.sys");
                        Elements.Find(d => d.ID == "CreationProgress").Value = 50;
                        break;
                    case 16:
                        Currently_Created = "0:\\System\\Layout.sys";
                        File.Create("0:\\System\\Layout.sys");
                        Elements.Find(d => d.ID == "CreationProgress").Value = 50;
                        break;
                    case 18:
                        Currently_Created = "0:\\Programs";
                        Directory.CreateDirectory("0:\\Programs");
                        Elements.Find(d => d.ID == "CreationProgress").Value = 50;
                        break;
                    case 20:
                        Currently_Created = "0:\\User\\Source";
                        Directory.CreateDirectory("0:\\User\\Source");
                        Elements.Find(d => d.ID == "CreationProgress").Value = 50;
                        break;
                }
            }
            else
            {
                switch (iterator)
                {
                    case 15:
                        File.WriteAllText("0:\\System\\FrequentApps.sys", "Settings\nGameboy\nMinecraft\nFileSystem");
                        break;
                    case 17:
                        string Layout =
                            "WindowR=" + GlobalValues.R +
                            "\nWindowG=" + GlobalValues.G +
                            "\nWindowB=" + GlobalValues.B +
                            "\nTaskbarR=" + GlobalValues.TaskBarR +
                            "\nTaskbarG=" + GlobalValues.TaskBarG +
                            "\nTaskbarB=" + GlobalValues.TaskBarB +
                            "\nTaskbarType=" + GlobalValues.TaskBarType +
                            "\nUsername=" + GlobalValues.Username +
                            "\nIconR=" + GlobalValues.IconR +
                            "\nIconG=" + GlobalValues.IconG +
                            "\nIconB=" + GlobalValues.IconB +
                            "\nIconwidth=" + GlobalValues.IconWidth +
                            "\nIconheight=" + GlobalValues.IconHeight +
                            "\nStartcolor=" + GlobalValues.StartColor.ToArgb() +
                            "\nEndcolor=" + GlobalValues.EndColor.ToArgb() +
                            "\nBakground=" + GlobalValues.Background_type +
                            "\nBackgroundcolor=" + GlobalValues.Background_color +
                            "\nTransparency=" + GlobalValues.LevelOfTransparency;
                        switch (GlobalValues.KeyboardLayout)
                        {
                            case KeyboardLayout.EN_US:
                                Layout += "\nKeyboard=EN_US";
                                break;
                            case KeyboardLayout.HUngarian:
                                Layout += "\nKeyboard=Hungarian";
                                break;
                        }
                        File.WriteAllText("0:\\System\\Layout.sys", Layout);
                        break;
                }
                Elements.Find(d => d.ID == "CreationProgress").Value = 100;
                Elements.Find(d => d.ID == "OverallPorgress").Value += 10;
            }
            iterator++;

            foreach (var Element in Elements)
            {
                if (TaskScheduler.Apps.FindAll(d => d.name == "Error!").Count == 0)
                {
                    if (Element.Clicked == true && Element.TypeOfElement == TypeOfElement.TextBox)
                    {
                        KeyEvent Key;
                        if (KeyboardManager.TryReadKey(out Key))
                        {
                            Element.Text = Keyboard.HandleKeyboard(Element.Text, Key);
                            if (Element.Text.Length > 4)
                            {
                                Element.Text = Element.Text.Remove(4);
                            }
                        }
                    }
                    Element.Render(window);
                    if (MouseManager.MouseState == MouseState.Left)
                    {
                        if (MouseManager.X > x + Element.X && MouseManager.X < x + Element.X + Element.Width)
                        {
                            if (MouseManager.Y > y + Element.Y && MouseManager.Y < y + Element.Y + Element.Height)
                            {
                                switch (Element.TypeOfElement)
                                {
                                    case TypeOfElement.TextBox:
                                        foreach (var e in Elements)
                                        {
                                            e.Clicked = false;
                                        }
                                        Element.Clicked = true;
                                        break;
                                    case TypeOfElement.Button:
                                        int Col = Element.Color;
                                        Element.Color = ImprovedVBE.colourToNumber(255, 255, 255);
                                        Element.Render(window);
                                        Element.Clicked = true;
                                        Element.Color = Col;
                                        if (Element.ID == "Next")
                                        {
                                            TaskScheduler.Apps.Remove(this);
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    else if (Element.TypeOfElement != TypeOfElement.TextBox)
                    {
                        Element.Clicked = false;
                    }
                }
            }

            BitFont.DrawBitFontString(window, "VerdanaCustomCharset24", Color.White, "File/Directory name: " + Currently_Created, 40, 307);

            BitFont.DrawBitFontString(window, "VerdanaCustomCharset24", Color.White, "Overall progress: " + Elements.Find(d => d.ID == "OverallPorgress").Value, 40, 467);

            ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
            if(iterator > 21)
            {
                TaskScheduler.Apps.Remove(this);
            }
        }

        public void RightClick()
        {
            
        }
    }
}
