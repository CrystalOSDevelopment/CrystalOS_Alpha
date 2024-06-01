using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Applications;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.SystemApps;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CrystalOSAlpha.System32.Installer
{
    class LicenceKey : App
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

        public LicenceKey(int X, int Y, int Z, int Width, int Height, string Title, Bitmap Icon)
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
            if(initial == true)
            {
                Elements.Add(new TextBox(40, 257, 150, 45, ImprovedVBE.colourToNumber(60, 60, 60), "", "XXXX", TextBox.Options.left, "First"));
                Elements.Add(new TextBox(240, 257, 150, 45, ImprovedVBE.colourToNumber(60, 60, 60), "", "XXXX", TextBox.Options.left, "Second"));
                Elements.Add(new TextBox(440, 257, 150, 45, ImprovedVBE.colourToNumber(60, 60, 60), "", "XXXX", TextBox.Options.left, "Third"));
                Elements.Add(new TextBox(640, 257, 150, 45, ImprovedVBE.colourToNumber(60, 60, 60), "", "XXXX", TextBox.Options.left, "Fourth"));

                Elements.Add(new Button(width - 215, height - 98, 192, 58, "Next", 1, "Next"));
                Elements[0].Clicked = true;
                once = true;
                initial = false;
            }
            if(once == true)
            {
                (canvas, back_canvas, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);

                BitFont.DrawBitFontString(canvas, "VerdanaCustomCharset32", Color.White, "Licence key", 30, 52);
                BitFont.DrawBitFontString(canvas, "VerdanaCustomCharset24", Color.White, "Before proceeding with the installation, please enter your license key.", 40, 142);
                BitFont.DrawBitFontString(canvas, "VerdanaCustomCharset24", Color.White, "(You can obtain one, by asking a friend or by reaching out to me)", 40, 212);

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                once = false;
            }
            foreach(var Element in Elements)
            {
                if(TaskScheduler.Apps.FindAll(d => d.name == "Error!").Count == 0)
                {
                    if(Element.Clicked == true && Element.TypeOfElement == TypeOfElement.TextBox)
                    {
                        KeyEvent Key;
                        if(KeyboardManager.TryReadKey(out Key))
                        {
                            Element.Text = Keyboard.HandleKeyboard(Element.Text, Key);
                            if(Element.Text.Length > 4)
                            {
                                Element.Text = Element.Text.Remove(4);
                            }
                        }
                    }
                    Element.Render(window);
                    if(MouseManager.MouseState == MouseState.Left)
                    {
                        if(MouseManager.X > x + Element.X && MouseManager.X < x + Element.X + Element.Width)
                        {
                            if(MouseManager.Y > y + Element.Y && MouseManager.Y < y + Element.Y + Element.Height)
                            {
                                switch(Element.TypeOfElement)
                                {
                                    case TypeOfElement.TextBox:
                                        foreach(var e in Elements)
                                        {
                                            e.Clicked = false;
                                        }
                                        Element.Clicked = true;
                                        break;
                                    case TypeOfElement.Button:
                                        if(GlobalValues.Clicked == false)
                                        {
                                            int Col = Element.Color;
                                            Element.Color = ImprovedVBE.colourToNumber(255, 255, 255);
                                            Element.Render(window);
                                            Element.Clicked = true;
                                            Element.Color = Col;
                                            if(Element.ID == "Next")
                                            {
                                                if(ValidateLicenseKey(Elements.Find(d => d.ID == "First").Text + "-" + 
                                                Elements.Find(d => d.ID == "Second").Text + "-" + 
                                                Elements.Find(d => d.ID == "Third").Text + "-" + 
                                                Elements.Find(d => d.ID == "Fourth").Text) == true)
                                                {
                                                    TaskScheduler.Apps.Remove(this);
                                                }
                                                else
                                                {
                                                    TaskScheduler.Apps.Add(new MsgBox(999, ImprovedVBE.width / 2 - 225, ImprovedVBE.height / 2 - 100, 450, 200, "Error!", "The key you provided is not a valid key!\nDebug: " + Elements.Find(d => d.ID == "First").Text + "-" +
                                                    Elements.Find(d => d.ID == "Second").Text + "-" +
                                                    Elements.Find(d => d.ID == "Third").Text + "-" +
                                                    Elements.Find(d => d.ID == "Fourth").Text, icon));
                                                }
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    else if(Element.TypeOfElement != TypeOfElement.TextBox)
                    {
                        Element.Clicked = false;
                        GlobalValues.Clicked = false;
                    }
                }
            }
            ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
        }
        #region License Key
        static string GenerateChecksum(string part1, string part2, string part3)
        {
            int sum = 0;
            string combined = part1 + part2 + part3;
            for (int i = 0; i < combined.Length; i++)
            {
                sum += (int)combined[i];
            }
            char[] part = new char[4];
            for (int i = 0; i < 4; i++)
            {
                part[i] = (char)((sum >> (i * 4)) & 0xF); // Create a checksum part based on bitwise operations
                if (part[i] < 10)
                    part[i] += '0'; // Convert to '0'-'9'
                else
                    part[i] += (char)('A' - 10); // Convert to 'A'-'F'
            }
            return new string(part);
        }
        static bool ValidateLicenseKey(string key)
        {
            if (key.Length != 19 || key[4] != '-' || key[9] != '-' || key[14] != '-')
                return false;

            string part1 = key.Substring(0, 4);
            string part2 = key.Substring(5, 4);
            string part3 = key.Substring(10, 4);
            string part4 = key.Substring(15, 4);

            if (!ValidatePart1(part1))
                return false;
            if (!ValidatePart2(part2))
                return false;
            if (!ValidatePart3(part3))
                return false;

            string expectedChecksum = GenerateChecksum(part1, part2, part3);
            return expectedChecksum == part4;
        }

        static bool ValidatePart1(string part)
        {
            if (part.Length != 4)
                return false;
            int sum = 0;
            for (int i = 0; i < part.Length; i++)
            {
                if (!IsAlphaNumeric(part[i]))
                    return false;
                sum += (int)part[i];
            }
            return sum % 69 == 0;
        }

        static bool ValidatePart2(string part)
        {
            if (part.Length != 4)
                return false;
            bool hasDigit = false;
            for (int i = 0; i < part.Length; i++)
            {
                if (!IsAlphaNumeric(part[i]))
                    return false;
                if (part[i] >= '0' && part[i] <= '9')
                    hasDigit = true;
            }
            return hasDigit;
        }

        static bool ValidatePart3(string part)
        {
            if (part.Length != 4)
                return false;
            if (part[0] != 'A')
                return false;
            for (int i = 1; i < part.Length; i++)
            {
                if (!IsAlphaNumeric(part[i]))
                    return false;
            }
            return true;
        }

        static bool IsAlphaNumeric(char c)
        {
            return (c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9');
        }
        #endregion 

        public void RightClick()
        {
            
        }
    }
}