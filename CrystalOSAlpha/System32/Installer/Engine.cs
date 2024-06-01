using Cosmos.System;
using CrystalOS_Alpha;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.UI_Elements;
using System.Collections.Generic;
using System.Drawing;
using Kernel = CrystalOS_Alpha.Kernel;

namespace CrystalOSAlpha.System32.Installer
{
    class Engine
    {
        public bool GenerateBase = true;
        public bool First = true;
        public bool Done = false;
        public int[] Pattern = new int[16 * 16]
        {
            13055, 7895160, 7895160, 7895160, 7895160, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055,
            7895160, 13055, 13055, 13055, 13055, 7895160, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055,
            7895160, 13055, 13055, 13055, 13055, 7895160, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055,
            7895160, 13055, 13055, 13055, 13055, 7895160, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055,
            7895160, 13055, 13055, 13055, 13055, 7895160, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055,
            13055, 7895160, 7895160, 7895160, 7895160, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055,
            13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055,
            13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055,
            13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055,
            13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055,
            13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055,
            13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055,
            13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055,
            13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055,
            13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055,
            13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055, 13055
        };
        public List<UIElementHandler> Elements = new List<UIElementHandler>();

        public void Main()
        {
            switch (ImprovedVBE.width)
            {
                case 1920:
                    if (First == true)
                    {
                        double WidthOfProgressbar = (double)ImprovedVBE.width * 0.15;
                        Elements.Add(new Progressbar(ImprovedVBE.width - 280 - (int)WidthOfProgressbar, ImprovedVBE.height - 53, (int)WidthOfProgressbar, 35, ImprovedVBE.colourToNumber(100, 255, 10), 0, "Progress"));
                        First = false;
                    }
                    if (GenerateBase == true)
                    {
                        //Apply the pattern
                        for (int y = 0; y < ImprovedVBE.height; y += 16)
                        {
                            for (int x = 0; x < ImprovedVBE.width; x += 16)
                            {
                                for (int i = 0; i < 256; i++)
                                {
                                    int pixelX = x + (i % 16);
                                    int pixelY = y + (i / 16);
                                    if (pixelX < ImprovedVBE.width && pixelY < ImprovedVBE.height)
                                    {
                                        ImprovedVBE.DrawPixel(ImprovedVBE.data, pixelX, pixelY, Pattern[i]);
                                    }
                                }
                            }
                        }
                        //Create the bottom dock that has a progressbar on it
                        ImprovedVBE.DrawFilledRectangle(ImprovedVBE.data, ImprovedVBE.colourToNumber(105, 105, 105), 260, ImprovedVBE.height - 70, ImprovedVBE.width - 260 * 2, 70);
                        BitFont.DrawBitFontString(ImprovedVBE.data, "VerdanaCustomCharset24", Color.White, "Welcome to CrystalOS Alpha!", 280, 1035);
                        BitFont.DrawBitFontString(ImprovedVBE.data, "ArialCustomCharset16", Color.White, "Progress:", Elements.Find(d => d.ID == "Progress").X - 70, 1035);
                        //Renders to the screen
                        ImprovedVBE.Display(Kernel.vbe);
                        GenerateBase = false;
                    }

                    foreach (var Elements in Elements)
                    {
                        Elements.Render(ImprovedVBE.cover);
                    }

                    if (TaskScheduler.Apps.Count == 0)
                    {
                        GlobalValues.Clicked = true;
                        if (Elements.Find(d => d.ID == "Progress").Value == 0)
                        {
                            TaskScheduler.Apps.Add(new Greeting(260, 50, 999, ImprovedVBE.width - 520, ImprovedVBE.height - 237, "Welcome to CrystalOS Alpha!", Resources.Celebration));
                            Elements.Find(d => d.ID == "Progress").Value += 20;
                        }
                        else if (Elements.Find(d => d.ID == "Progress").Value == 20)
                        {
                            TaskScheduler.Apps.Add(new LicenceKey(260, 50, 999, ImprovedVBE.width - 520, ImprovedVBE.height - 237, "Registration", Resources.Celebration));
                            Elements.Find(d => d.ID == "Progress").Value += 20;
                        }
                        else if (Elements.Find(d => d.ID == "Progress").Value == 40)
                        {
                            TaskScheduler.Apps.Add(new Layout(260, 50, 999, ImprovedVBE.width - 520, ImprovedVBE.height - 237, "Customization", Resources.Celebration));
                            Elements.Find(d => d.ID == "Progress").Value += 20;
                        }
                        else if (Elements.Find(d => d.ID == "Progress").Value == 60)
                        {
                            if (Kernel.IsDiskSupport == true)
                            {
                                TaskScheduler.Apps.Add(new CopyFiles(260, 50, 999, ImprovedVBE.width - 520, ImprovedVBE.height - 237, "Copying files", Resources.Celebration));
                                Elements.Find(d => d.ID == "Progress").Value += 20;
                            }
                            else
                            {
                                //Exit screen that states everything is saved
                                TaskScheduler.Apps.Add(new Finale(260, 50, 999, ImprovedVBE.width - 520, ImprovedVBE.height - 237, "Finished!", Resources.Celebration));
                                Elements.Find(d => d.ID == "Progress").Value += 40;
                            }
                        }
                        else if (Elements.Find(d => d.ID == "Progress").Value == 80)
                        {
                            TaskScheduler.Apps.Add(new Finale(260, 50, 999, ImprovedVBE.width - 520, ImprovedVBE.height - 237, "Finished!", Resources.Celebration));
                            Elements.Find(d => d.ID == "Progress").Value += 20;
                        }
                        else
                        {
                            //In case no reboot is needed
                            if (TaskScheduler.Apps.Count == 0)
                            {
                                Done = true;
                            }
                        }
                    }

                    if (Elements.Find(d => d.ID == "Progress").Value != 40)
                    {
                        KeyEvent key;
                        if (KeyboardManager.TryReadKey(out key))
                        {
                            Keyboard.HandleKeyboard("", key);
                        }
                    }
                    break;

                case 1024:
                    if(First == true)
                    {
                        double WidthOfProgressbar = (double)ImprovedVBE.width * 0.15;
                        Elements.Add(new Progressbar(ImprovedVBE.width - 120 - (int)WidthOfProgressbar, ImprovedVBE.height - 53, (int)WidthOfProgressbar, 35, ImprovedVBE.colourToNumber(100, 255, 10), 0, "Progress"));
                        First = false;
                    }
                    if(GenerateBase == true)
                    {
                        //Apply the pattern
                        for (int y = 0; y < ImprovedVBE.height; y += 16)
                        {
                            for (int x = 0; x < ImprovedVBE.width; x += 16)
                            {
                                for (int i = 0; i < 256; i++)
                                {
                                    int pixelX = x + (i % 16);
                                    int pixelY = y + (i / 16);
                                    if (pixelX < ImprovedVBE.width && pixelY < ImprovedVBE.height)
                                    {
                                        ImprovedVBE.DrawPixel(ImprovedVBE.data, pixelX, pixelY, Pattern[i]);
                                    }
                                }
                            }
                        }
                        //Create the bottom dock that has a progressbar on it
                        ImprovedVBE.DrawFilledRectangle(ImprovedVBE.data, ImprovedVBE.colourToNumber(105, 105, 105), 50, ImprovedVBE.height - 70, ImprovedVBE.width - 100, 70);
                        BitFont.DrawBitFontString(ImprovedVBE.data, "VerdanaCustomCharset24", Color.White, "Welcome to CrystalOS Alpha!", 70, ImprovedVBE.height - 45);
                        BitFont.DrawBitFontString(ImprovedVBE.data, "ArialCustomCharset16", Color.White, "Progress:", Elements.Find(d => d.ID == "Progress").X - 70, ImprovedVBE.height - 45);
                        //Renders to the screen
                        ImprovedVBE.Display(Kernel.vbe);
                        GenerateBase = false;
                    }

                    foreach(var Elements in Elements)
                    {
                        Elements.Render(ImprovedVBE.cover);
                    }

                    if (TaskScheduler.Apps.Count == 0)
                    {
                        GlobalValues.Clicked = true;
                        if (Elements.Find(d => d.ID == "Progress").Value == 0)
                        {
                            TaskScheduler.Apps.Add(new Greeting(50, 50, 999, ImprovedVBE.width - 100, ImprovedVBE.height - 150, "Welcome to CrystalOS Alpha!", Resources.Celebration));
                            Elements.Find(d => d.ID == "Progress").Value += 20;
                        }
                        else if (Elements.Find(d => d.ID == "Progress").Value == 20)
                        {
                            TaskScheduler.Apps.Add(new LicenceKey(50, 50, 999, ImprovedVBE.width - 100, ImprovedVBE.height - 150, "Registration", Resources.Celebration));
                            Elements.Find(d => d.ID == "Progress").Value += 20;
                        }
                        else if (Elements.Find(d => d.ID == "Progress").Value == 40)
                        {
                            TaskScheduler.Apps.Add(new Layout(50, 50, 999, ImprovedVBE.width - 100, ImprovedVBE.height - 150, "Customization", Resources.Celebration));
                            Elements.Find(d => d.ID == "Progress").Value += 20;
                        }
                        else if(Elements.Find(d => d.ID == "Progress").Value == 60)
                        {
                            if(Kernel.IsDiskSupport == true)
                            {
                                TaskScheduler.Apps.Add(new CopyFiles(50, 50, 999, ImprovedVBE.width - 100, ImprovedVBE.height - 150, "Copying files", Resources.Celebration));
                                Elements.Find(d => d.ID == "Progress").Value += 20;
                            }
                            else
                            {
                                //Exit screen that states everything is saved
                                TaskScheduler.Apps.Add(new Finale(50, 50, 999, ImprovedVBE.width - 100, ImprovedVBE.height - 150, "Finished!", Resources.Celebration));
                                Elements.Find(d => d.ID == "Progress").Value += 40;
                            }
                        }
                        else if(Elements.Find(d => d.ID == "Progress").Value == 80)
                        {
                            TaskScheduler.Apps.Add(new Finale(50, 50, 999, ImprovedVBE.width - 100, ImprovedVBE.height - 150, "Finished!", Resources.Celebration));
                            Elements.Find(d => d.ID == "Progress").Value += 20;
                        }
                        else
                        {
                            //In case no reboot is needed
                            if(TaskScheduler.Apps.Count == 0)
                            {
                                Done = true;
                            }
                        }
                    }

                    if (Elements.Find(d => d.ID == "Progress").Value != 40)
                    {
                        KeyEvent key;
                        if (KeyboardManager.TryReadKey(out key))
                        {
                            Keyboard.HandleKeyboard("", key);
                        }
                    }
                    break;
            }
        }
    }
}
