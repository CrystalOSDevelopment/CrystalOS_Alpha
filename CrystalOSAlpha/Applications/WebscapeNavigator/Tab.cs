using Cosmos.System.Graphics;
using Cosmos.System.Network.IPv4.TCP;
using CrystalOS_Alpha;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Programming;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace CrystalOSAlpha.Applications.WebscapeNavigator
{
    public class Tab
    {
        public Tab(int x, int y, int width, int height, string title, string uRL, bool isActive)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Title = title;
            URL = uRL;
            IsActive = isActive;
            FinishedLoading = false;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int ScrollValue { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        public bool IsActive { get; set; }
        public bool FinishedLoading { get; set; }
        public bool TriggerLoading { get; set; }
        public bool RequestRerender { get; set; }
        public string Canvas { get; set; }

        private int KernelCycle = 0;
        private List<byte> Parts = new List<byte>();
        private int TimeTick = 0;
        private int CurrentTime;
        public void Load(string URL)
        {
            this.URL = URL;

            if(TriggerLoading == false)
            {
                Canvas = null;
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
                        string messageToSend = this.URL;
                        byte[] dataToSend = Encoding.ASCII.GetBytes(messageToSend);
                        stream.Write(dataToSend, 0, dataToSend.Length);

                        //stream.Close();
                        //client.Close();
                    }
                }
                catch { }
                TriggerLoading = true;
                FinishedLoading = false;
                KernelCycle = 0;
            }

            //5 second delay
            if (FinishedLoading == false)
            {
                if(CurrentTime != DateTime.Now.Second)
                {
                    TimeTick++;
                    CurrentTime = DateTime.Now.Second;
                }
                switch (TimeTick)
                {
                    case 2:
                        try
                        {
                            Parts = Kernel.getContentBytesList("", "", "screenshot.bmp");
                            //File.WriteAllText("0:\\Content.txt", );
                            string[] Line = Kernel.getContent("", "Content.txt").Split(":");
                            this.URL = Line[0] + Line[1];
                            this.Title = Line[2];
                        }
                        catch { }
                        FinishedLoading = true;
                        TimeTick = 0;
                        break;
                }
            }

            if(FinishedLoading == true && Canvas == null)
            {
                Canvas = "Not empty!";
                RequestRerender = true;
            }
        }

        public bool Render(Bitmap Window, int Scroll)
        {
            if (ScrollValue != Scroll || RequestRerender)
            {
                ScrollValue = Scroll;
                try
                {
                    int xVal = 0;
                    int yVal = 0 - Scroll;
                    int Top = (int)Window.Width * yVal;

                    for (int i = 0; i < Parts.Count && yVal < Window.Height;)
                    {
                        // Ensure xVal is within bounds
                        if (xVal >= 688)
                        {
                            yVal += 1;
                            Top = (int)Window.Width * yVal;
                            xVal = 0;
                        }

                        // Read the color byte and the repeat count
                        byte colorByte = Parts[i];
                        int repeatCount = Parts[i + 1]; // Read the count directly

                        // Draw the repeated pixels
                        int pixelColor = WebsafeColors.ReturnWithColor(colorByte);
                        for (int j = 0; j < repeatCount; j++)
                        {
                            // Ensure xVal is within bounds before drawing
                            if (xVal >= 688)
                            {
                                yVal += 1;
                                Top = (int)Window.Width * yVal;
                                xVal = 0;
                            }

                            // Only write to RawData if within bounds
                            if (yVal >= 0 && yVal < Window.Height && Top + xVal < Window.RawData.Length)
                            {
                                Window.RawData[Top + xVal] = pixelColor;
                            }

                            xVal++; // Move to the next pixel position
                        }

                        // Move to the next color-repetition pair
                        i += 2;
                    }
                }
                catch { }
                RequestRerender = false;
                return true;
            }
            return false;
        }

        public void SendKeys(string Keys)
        {
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
                    string messageToSend = "DiscordMSG:" + Keys;
                    byte[] dataToSend = Encoding.ASCII.GetBytes(messageToSend);
                    stream.Write(dataToSend, 0, dataToSend.Length);
                    //stream.Close();
                    //client.Close();
                }
            }
            catch { }
            Canvas = null;
            TriggerLoading = true;
            FinishedLoading = false;
            KernelCycle = 0;
        }

        public void SendClick(int Left, int Top)
        {
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
                    string messageToSend = "Click:" + Left + ":" + Top;
                    byte[] dataToSend = Encoding.ASCII.GetBytes(messageToSend);
                    stream.Write(dataToSend, 0, dataToSend.Length);
                    //stream.Close();
                    //client.Close();
                }
            }
            catch { }
            Canvas = null;
            TriggerLoading = true;
            FinishedLoading = false;
            KernelCycle = 0;
        }

        public void SendDrag(int Xorigin, int Yorigin, int Left, int Top)
        {
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
                    string messageToSend = "Drag:" + Xorigin + ":" + Yorigin + " " + Left + ":" + Top;
                    byte[] dataToSend = Encoding.ASCII.GetBytes(messageToSend);
                    stream.Write(dataToSend, 0, dataToSend.Length);
                    //stream.Close();
                    //client.Close();
                }
            }
            catch { }
            Canvas = null;
            TriggerLoading = true;
            FinishedLoading = false;
            KernelCycle = 0;
        }

        public void RenderTab(Bitmap Window)
        {
            Bitmap Tab = new Bitmap((uint)Width, (uint)Height, ColorDepth.ColorDepth32);
            Array.Fill(Tab.RawData, ImprovedVBE.colourToNumber(1, 1, 1));
            ImprovedVBE.DrawFilledRectangle(Tab, ImprovedVBE.colourToNumber(255, 255, 255), 2, 2, Width - 4, Height - 4);
            string displayText = "";
            if(Title.Length > 13)
            {
                displayText = Title.Substring(0, 10) + "...";
            }
            else
            {
                displayText = Title;
            }
            BitFont.DrawBitFontString(Tab, "ArialCustomCharset16", System.Drawing.Color.Black, displayText, 5, Height / 2 - 8);

            ImprovedVBE.DrawImage(Tab, X, Y, Window);
        }
    }
}
