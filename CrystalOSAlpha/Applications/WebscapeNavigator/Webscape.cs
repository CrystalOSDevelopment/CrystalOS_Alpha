using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.System32;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using TaskScheduler = CrystalOSAlpha.Graphics.TaskScheduler;
using System.Net.Sockets;
using System.Text;
using CrystalOS_Alpha;
using Kernel = CrystalOS_Alpha.Kernel;

namespace CrystalOSAlpha.Applications.WebscapeNavigator
{
    class Webscape : App
    {
        #region important
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
        #endregion important

        public int Reg_Y = 0;
        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);
        public int KernelCycle = 0;
        public int Left = 0;
        public int Top = 0;

        public bool initial = true;
        public bool clicked = false;
        public bool once { get; set; }
        public bool temp = true;
        public bool WaitToFetch = false;

        public string content = "";
        public string source = "";

        public Bitmap canvas;
        public Bitmap window { get; set; }
        public Bitmap Container;
        
        public List<UIElementHandler> Scroll = new List<UIElementHandler>();
        public List<UIElementHandler> Buttons = new List<UIElementHandler>();
        public List<UIElementHandler> TextBoxes = new List<UIElementHandler>();

        public byte[] Parts = new byte[0];

        public List<Tab> Tabs = new List<Tab>();

        public void App()
        {
            if (Kernel.IsNetSupport == false)
            {
                throw new Exception("Failed to establish an Ethernet\nconnection!");
            }
            if (initial == true)
            {
                Buttons.Add(new Button(450, 45, 85, 30, "Go", 1));
                Buttons.Add(new Button(550, 45, 85, 30, "Bookmark", 1));

                Scroll.Add(new VerticalScrollbar(width - 22, 118, 20, height - 146, 20, 0, 10000, ""));

                TextBoxes.Add(new TextBox(10, 45, 430, 30, ImprovedVBE.colourToNumber(60, 60, 60), "google.com", "Url:", TextBox.Options.left, "URL"));

                Tabs.Add(new Tab(10, 98, 100, 20, "Google", "google.com", true));
                Tabs.Add(new Tab(115, 98, 100, 20, "ChatGPT", "chatgpt.com", false));
                Tabs.Add(new Tab(220, 98, 100, 20, "Milk", "milk.com", false));

                initial = false;
            }
            if (once == true)
            {
                once = false;
                temp = true;

                for(int i = 0; i < 1; i++)//A cheap and dirty way of rerendering
                {
                    Bitmap back = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                    (canvas, back, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);

                    Container = new Bitmap(688, 360, ColorDepth.ColorDepth32);
                    Array.Fill(Container.RawData, ImprovedVBE.colourToNumber(255, 255, 255));

                    foreach (var button in Buttons)
                    {
                        if (button.Clicked == true)
                        {
                            switch (button.Text)
                            {
                                case "Go":
                                    Tabs.Find(d => d.IsActive == true).TriggerLoading = false;
                                    Scroll[0].Value = 0;
                                    if ((TextBoxes[0].Text.Contains("discord.com") || TextBoxes[0].Text.Contains("google")) && TextBoxes.Count != 2)
                                    {
                                        height += 35;
                                        TextBoxes.Add(new TextBox(10, height - 35, (int)Container.Width, 30, ImprovedVBE.colourToNumber(60, 60, 60), "", "Type your message here!", TextBox.Options.left, "MSG"));//2
                                        i = -1;
                                    }
                                    WaitToFetch = true;
                                    break;
                            }
                        }
                        button.Render(canvas);
                        if (MouseManager.MouseState == MouseState.None)
                        {
                            button.Clicked = false;
                        }
                    }

                }

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
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
                if (button.Clicked == true && MouseManager.MouseState == MouseState.None)
                {
                    once = true;
                    button.Clicked = false;
                    clicked = false;
                }
            }

            foreach (var vscroll in Scroll)
            {
                if (vscroll.CheckClick((int)MouseManager.X - x, (int)MouseManager.Y - y))
                {
                    temp = true;
                }
            }

            foreach (var UIElement in TextBoxes)
            {
                if (UIElement.CheckClick(x, y))
                {
                    foreach (UIElementHandler UI in TextBoxes)
                    {
                        if (UI.TypeOfElement == TypeOfElement.TextBox)
                        {
                            UI.Clicked = false;
                        }
                    }
                    UIElement.Clicked = true;
                }
            }

            Tabs.Find(d => d.IsActive == true).Load(TextBoxes[0].Text);

            if (TaskScheduler.counter == TaskScheduler.Apps.Count - 1)
            {
                KeyEvent key;
                if (KeyboardManager.TryReadKey(out key))
                {
                    if (key.Key == ConsoleKeyEx.Enter)
                    {
                        if(TextBoxes.Count == 1)
                        {
                            Buttons[0].Clicked = true;
                            clicked = true;
                            once = true;
                        }
                        if (TextBoxes.Count != 1)
                        {
                            try
                            {
                                Tabs.Find(d => d.IsActive == true).SendKeys(TextBoxes[1].Text);
                            }
                            catch { }
                            TextBoxes[1].Text = "";
                        }
                    }
                    else
                    {
                        try
                        {
                            TextBoxes.Find(d => d.Clicked == true).Text = Keyboard.HandleKeyboard(TextBoxes.Find(d => d.Clicked == true).Text, key);
                        }
                        catch { }
                    }

                    Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                    temp = true;
                }
                if(MouseManager.MouseState == MouseState.Left && clicked == false)
                {
                    Top = (int)(MouseManager.Y - y - 118) * 2 + Scroll[0].Value;
                    Left = (int)(MouseManager.X - x - 10) * 2;
                    if (Top - Scroll[0].Value >= 0 && Top - Scroll[0].Value < Container.Height * 2 && Left >= 0 && Left < Container.Width * 2)
                    {
                        //Tabs.Find(d => d.IsActive == true).SendClick(Left, Top);
                        WaitToFetch = true;
                    }
                    
                    //Change tabs using this code
                    if (MouseManager.Y > y + 98 && MouseManager.Y < y + 118)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (MouseManager.X > x + 10 + (i * 105) && MouseManager.X < x + 110 + (i * 105))
                            {
                                Tabs.ForEach(tab => tab.IsActive = false);
                                Tabs[i].IsActive = true;
                                Tabs[i].RequestRerender = true;
                                TextBoxes[0].Text = Tabs[i].URL;
                                once = true;
                                break;
                            }
                        }
                    }
                    clicked = true;
                }
                else if(MouseManager.ScrollDelta != 0)
                {
                    int Top = (int)(MouseManager.Y - y - 118) * 2 + Scroll[0].Value;
                    int Left = (int)(MouseManager.X - x - 10) * 2;

                    string serverIp = GlobalValues.TCPIP;
                    int serverPort = 1312;
                    try
                    {
                        using (TcpClient client = new TcpClient())
                        {
                            /**Connect to server **/
                            client.Connect(serverIp, serverPort);
                            NetworkStream stream = client.GetStream();

                            /** Send data **/
                            string messageToSend = "BrowserScroll:" + MouseManager.ScrollDelta + "," + Left + "," + Top;
                            byte[] dataToSend = Encoding.ASCII.GetBytes(messageToSend);
                            stream.Write(dataToSend, 0, dataToSend.Length);
                            stream.Close();
                            client.Close();
                        }
                    }
                    catch { }
                    MouseManager.ResetScrollDelta();
                    WaitToFetch = true;
                }

                if(MouseManager.MouseState == MouseState.None && clicked)
                {
                    if(Math.Abs(Left - (int)(MouseManager.X - x - 10) * 2) > 10 || Math.Abs(Top - (MouseManager.Y - y - 118) * 2 + Scroll[0].Value) > 10)
                    {
                        int Xdiff = (int)(MouseManager.X - x - 10) * 2 - Left;
                        int Ydiff = (int)(MouseManager.Y - y - 118) * 2 + Scroll[0].Value - Top;
                        Tabs.Find(d => d.IsActive == true).SendDrag(Left, Top, Xdiff, Ydiff);
                    }
                    else
                    {
                        if(MouseManager.X > x + 10 && MouseManager.X < x + 10 + Container.Width && MouseManager.Y > y + 118 && MouseManager.Y < y + 118 + Container.Height)
                        {
                            Tabs.Find(d => d.IsActive == true).SendClick(Left, Top);
                        }
                        //Tabs.Find(d => d.IsActive == true).SendClick(Left, Top);
                    }
                    clicked = false;
                }

                if (MouseManager.MouseState == MouseState.Right && KernelCycle >= 150)
                {
                    Tabs.Find(d => d.IsActive == true).SendClick(-1, -1);
                    //Tabs.Find(d => d.IsActive == true).SendDrag((int)MouseManager.X, (int)MouseManager.Y, 100, 0);
                    KernelCycle = 0;
                }
                else
                {
                    KernelCycle++;
                }
            }

            if (temp == true || Tabs.Find(d => d.IsActive == true).Render(Container, Scroll[0].Value))
            {
                if(temp == true)
                {
                    //Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                    foreach (var Tab in Tabs)
                    {
                        Tab.RenderTab(window);
                        if(Tab.IsActive == true)
                        {
                            if (Tab.URL.Contains("https://www."))
                            {
                                TextBoxes[0].Text = Tab.URL.Remove(0, "https://www.".Length + 1);
                            }
                        }
                    }

                    foreach (var vscroll in Scroll)
                    {
                        vscroll.X = width - 22;
                        vscroll.Height = height - 126;
                        vscroll.Render(window);
                    }
                    foreach (var Box in TextBoxes)
                    {
                        if(Box.ID == "URL")
                        {
                            Box.Width = 430;
                        }
                        Box.Render(window);
                    }
                }

                ImprovedVBE.DrawImage(Container, 10, 118, window);

                temp = false;
            }
        }

        public void RightClick()
        {

        }
    }
}
