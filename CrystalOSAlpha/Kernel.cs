using Cosmos.System;
using Cosmos.System.FileSystem;
using Cosmos.System.Graphics;
using CrystalOS_Alpha.Graphics.Widgets;
using CrystalOSAlpha;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.Graphics.TaskBar;
using CrystalOSAlpha.Graphics.Widgets;
using CrystalOSAlpha.System32;
using IL2CPU.API.Attribs;
using System.Net.Sockets;
using System.Text;
using System;
using Sys = Cosmos.System;
using System.IO;
using System.Collections.Generic;

namespace CrystalOS_Alpha
{
    public class Kernel : Sys.Kernel
    {
        public static VBECanvas vbe = new VBECanvas(new Mode(1920, 1080, ColorDepth.ColorDepth32));
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Icons.Cursor.bmp")] public static byte[] Cursor;
        public static Bitmap C = new Bitmap(Cursor);
        public static CosmosVFS fs = new CosmosVFS();
        public static bool Is_KeyboardMouse = false;
        public static bool IsDiskSupport = false;
        public static bool IsNetSupport = false;
        protected override void BeforeRun()
        {
            #region Font Registering
            Fonts.RegisterFonts();
            #endregion Font Registering

            #region Mouse
            MouseManager.ScreenWidth = (uint)ImprovedVBE.width;
            MouseManager.ScreenHeight = (uint)ImprovedVBE.height;

            MouseManager.X = (uint)ImprovedVBE.width / 2;
            MouseManager.Y = (uint)ImprovedVBE.height / 2;
            #endregion Mouse

            #region System initials
            //Questions and setup, if needed
            Boot.Initialise();

            //Boot animation. Has no purpose, just there for the show. You can adjust the time in seconds. 0 -> Completely skips it 60 -> Plays the animation for 60 seconds(max value)
            Boot.Animation(5);
            #endregion System initials

            #region Widgets
            FPS_Counter f = new FPS_Counter();
            f.x = 200;
            f.y = 200;
            f.z = 0;
            f.name = null;
            f.minimised = false;
            f.icon = ImprovedVBE.ScaleImageStock(Resources.Web, 56, 56);
            TaskScheduler.Apps.Add(f);

            ImageViewer i = new ImageViewer();
            i.x = ImprovedVBE.width;
            i.y = 200;
            i.z = 999;
            i.minimised = false;
            i.icon = ImprovedVBE.ScaleImageStock(Resources.Web, 56, 56);
            TaskScheduler.Apps.Add(i);

            Note n = new Note();
            n.x = ImprovedVBE.width;
            n.y = 200;
            n.z = 999;
            n.name = null;
            n.minimised = false;
            n.icon = ImprovedVBE.ScaleImageStock(Resources.Web, 56, 56);
            TaskScheduler.Apps.Add(n);

            if (IsNetSupport)
            {
                //Init weather data from server
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
                        string messageToSend = "Weather:London:UK";
                        byte[] dataToSend = Encoding.ASCII.GetBytes(messageToSend);
                        stream.Write(dataToSend, 0, dataToSend.Length);
                        stream.Close();
                        client.Close();
                    }
                }
                catch { }

                WeatherForecast Weather = new WeatherForecast();
                Weather.x = ImprovedVBE.width;
                Weather.y = 200;
                Weather.z = 999;
                Weather.name = null;
                Weather.minimised = false;
                Weather.icon = ImprovedVBE.ScaleImageStock(Resources.Web, 56, 56);
                TaskScheduler.Apps.Add(Weather);

                //Init fact data from server
                try
                {
                    using (TcpClient client = new TcpClient())
                    {
                        /**Connect to server **/
                        client.Connect(serverIp, serverPort);
                        NetworkStream stream = client.GetStream();

                        /** Send data **/
                        string messageToSend = "Chuck";
                        byte[] dataToSend = Encoding.ASCII.GetBytes(messageToSend);
                        stream.Write(dataToSend, 0, dataToSend.Length);
                        stream.Close();
                        client.Close();
                    }
                }
                catch { }
                ChuckNorrisFacts chuckNorrisFacts = new ChuckNorrisFacts();
                chuckNorrisFacts.x = 600;
                chuckNorrisFacts.y = 200;
                chuckNorrisFacts.z = 999;
                chuckNorrisFacts.name = null;
                chuckNorrisFacts.minimised = false;
                chuckNorrisFacts.icon = ImprovedVBE.ScaleImageStock(Resources.Web, 56, 56);
                TaskScheduler.Apps.Add(chuckNorrisFacts);
            }
            #endregion Widgets

            #region Mouse
            MouseManager.ScreenWidth = (uint)ImprovedVBE.width;
            MouseManager.ScreenHeight = (uint)ImprovedVBE.height;

            MouseManager.X = (uint)ImprovedVBE.width / 2;
            MouseManager.Y = (uint)ImprovedVBE.height / 2;
            #endregion Mouse

            ImprovedVBE.Clear(true);
        }

        /// <summary>
        ///When an app with a width over 800 pixels is present, this integer is used to call Helap.Collect() every second kernel cycle.
        /// </summary>
        public static int collect = 0;

        /// <summary>
        /// Used for debugging purpose only
        /// </summary>
        public static string Clipboard = "";

        protected override void Run()
        {
            //If keyboard mouse is used (W,A,S,D keys to move the cursor) a readkey is placed in the beginning of every kernel cycle
            if (Is_KeyboardMouse == true)
            {
                KeyEvent k;
                if(KeyboardManager.TryReadKey(out k))
                {
                    Keyboard.HandleKeyboard("", k);
                }
            }

            //Layer organising between the menu, taskbar, calendar and apps
            //if(TaskManager.MenuOpened == false && TaskManager.calendar == false)
            //{
            //    if(GlobalValues.TaskBarType == "Classic")
            //    {
            //        SideNav.Core();
            //        TaskScheduler.Exec();
            //        TaskManager.Main();
            //    }
            //    else if(GlobalValues.TaskBarType == "Nostalgia")
            //    {
            //        SideNav.Core();
            //        TaskScheduler.Exec();
            //        TaskManager.Main();
            //    }
            //}
            //else
            //{
            //    SideNav.Core();
            //    TaskScheduler.Exec();
            //    TaskManager.Main();
            //}
            SideNav.Core();
            TaskScheduler.Exec();
            TaskManager.Main();

            //Layer organising between the menu, taskbar, calendar and apps
            if (TaskManager.MenuOpened == true && GlobalValues.TaskBarType == "Nostalgia")
            {
                TaskManager.Dynamic_Menu(ImprovedVBE.width / 2 - 200, 50, 400, 400);
            }

            ImprovedVBE.RequestRedraw = false;

            //Display to the screen
            ImprovedVBE.Display(vbe);

            //For more sensitive scrolling, MouseManager.ResetScrollDelta is called in every second kernel cycle
            switch(collect)
            {
                case 3:
                    MouseManager.ResetScrollDelta();
                    collect = 0;
                    break;
                default:
                    collect++;
                    break;
            }
        }

        public static string getContent(string url, string FileName = "")
        {
            string IP = "";
            using (TcpClient client = new TcpClient())
            {
                string serverIp = GlobalValues.ServerIP;//address.ToString();
                int serverPort = 80;
                client.Connect(serverIp, serverPort);
                NetworkStream stream = client.GetStream();

                //StringBuilder httpget = new StringBuilder();
                //httpget.Append("GET /");
                //httpget.Append(FileName);
                //httpget.Append(" HTTP/1.1\r\n");
                //httpget.Append("User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:90.0) Gecko/20100101 Firefox/90.0\r\n");
                //httpget.Append("Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8\r\n");
                //httpget.Append("Accept-Encoding: identity\r\n");
                //httpget.Append("Accept-Language: en-US,en;q=0.5\r\n");
                //httpget.Append("Host: ");
                //httpget.Append(url);
                //httpget.Append("\r\n");
                //httpget.Append("Connection: Keep-Alive\r\n\r\n");
                StringBuilder httpget = new StringBuilder();
                httpget.Append($"GET /{FileName} HTTP/1.1\r\n");
                httpget.Append("User-Agent: CustomClient/1.0\r\n");
                httpget.Append("Accept: */*\r\n");
                httpget.Append($"Host: {serverIp}\r\n");
                httpget.Append("Connection: Close\r\n\r\n");

                //string httpget = "GET /index" + Part + ".html HTTP/1.1\r\n" +
                // "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:90.0) Gecko/20100101 Firefox/90.0\r\n" +
                // "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8\r\n" +
                // "Accept-Encoding: identity\r\n" +
                // "Accept-Language: en-US,en;q=0.5\r\n" +
                // "Host: " + url + "\r\n" +
                // "Connection: Keep-Alive\r\n\r\n";

                //string messageToSend = httpget;
                byte[] dataToSend = Encoding.ASCII.GetBytes(httpget.ToString());
                stream.Write(dataToSend, 0, dataToSend.Length);
                /** Receive data **/
                byte[] receivedData = new byte[client.ReceiveBufferSize];
                int bytesRead = stream.Read(receivedData, 0, receivedData.Length);
                StringBuilder sb = new StringBuilder();
                sb.Append(Encoding.ASCII.GetString(receivedData, 0, bytesRead));

                string[] responseParts = sb.ToString().Split(new[] { "\r\n\r\n" }, 2, StringSplitOptions.None);
                if(responseParts.Length > 1)
                {
                    return responseParts[1];
                }
                else
                {
                    return sb.ToString();
                }
            }
        }//Also used as download files as UTF-8 string. CAUTION: Beyond certain string legth, it causes the system to crash. Use getContentBytes for downloading files instead.

        public static byte[] getContentBytes(string url, string saveTo, string fileName = "")
        {
            using (TcpClient client = new TcpClient())
            {
                try
                {
                    string serverIp = GlobalValues.ServerIP;
                    int serverPort = 80;
                    client.Connect(serverIp, serverPort);

                    using (NetworkStream stream = client.GetStream())
                    {
                        // Construct HTTP GET request
                        StringBuilder requestBuilder = new StringBuilder();
                        requestBuilder.Append($"GET /{fileName} HTTP/1.1\r\n");
                        requestBuilder.Append("User-Agent: CustomClient/1.0\r\n");
                        requestBuilder.Append("Accept: */*\r\n");
                        requestBuilder.Append($"Host: {serverIp}\r\n");
                        requestBuilder.Append("Connection: Close\r\n\r\n");

                        byte[] requestBytes = Encoding.ASCII.GetBytes(requestBuilder.ToString());
                        stream.Write(requestBytes, 0, requestBytes.Length);

                        int contentLength = -1;
                        bool headersParsed = false;
                        byte[] buffer = new byte[8192]; // 8 KB buffer

                        using (FileStream fileStream = !string.IsNullOrEmpty(saveTo) ? new FileStream(saveTo, FileMode.Create) : null)
                        {
                            int bytesRead;
                            List<byte> resultBytes = new List<byte>(); // Store response if no file path given

                            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                if (!headersParsed)
                                {
                                    string responsePart = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                                    int headerEndIdx = responsePart.IndexOf("\r\n\r\n");

                                    if (headerEndIdx != -1)
                                    {
                                        headersParsed = true;
                                        headerEndIdx += 4; // Position after headers
                                        string headers = responsePart.Substring(0, headerEndIdx);

                                        // Check for Content-Length directly
                                        int contentIndex = headers.IndexOf("Content-Length:", StringComparison.OrdinalIgnoreCase);
                                        if (contentIndex != -1)
                                        {
                                            int start = contentIndex + "Content-Length:".Length;
                                            int end = headers.IndexOf("\r\n", start);
                                            contentLength = int.Parse(headers.Substring(start, end - start).Trim());
                                        }

                                        int dataStart = headerEndIdx;
                                        int dataLength = bytesRead - dataStart;

                                        if (fileStream != null)
                                            fileStream.Write(buffer, dataStart, dataLength);
                                        else
                                            resultBytes.AddRange(new ArraySegment<byte>(buffer, dataStart, dataLength));
                                    }
                                }
                                else
                                {
                                    if (fileStream != null)
                                        fileStream.Write(buffer, 0, bytesRead);
                                    else
                                        resultBytes.AddRange(new ArraySegment<byte>(buffer, 0, bytesRead));
                                }

                                if (contentLength != -1 && (fileStream?.Length ?? resultBytes.Count) >= contentLength)
                                    break;
                            }

                            if(saveTo != "")
                            {
                                File.WriteAllBytes(saveTo, resultBytes.ToArray());
                            }
                            return fileStream == null ? resultBytes.ToArray() : File.ReadAllBytes(saveTo); // Read directly if saved
                        }
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public static List<byte> getContentBytesList(string url, string saveTo, string fileName = "")
        {
            using (TcpClient client = new TcpClient())
            {
                try
                {
                    string serverIp = GlobalValues.ServerIP;
                    int serverPort = 80;
                    client.Connect(serverIp, serverPort);

                    using (NetworkStream stream = client.GetStream())
                    {
                        // Construct HTTP GET request
                        StringBuilder requestBuilder = new StringBuilder();
                        requestBuilder.Append($"GET /{fileName} HTTP/1.1\r\n");
                        requestBuilder.Append("User-Agent: CustomClient/1.0\r\n");
                        requestBuilder.Append("Accept: */*\r\n");
                        requestBuilder.Append($"Host: {serverIp}\r\n");
                        requestBuilder.Append("Connection: Close\r\n\r\n");

                        byte[] requestBytes = Encoding.ASCII.GetBytes(requestBuilder.ToString());
                        stream.Write(requestBytes, 0, requestBytes.Length);

                        int contentLength = -1;
                        bool headersParsed = false;
                        byte[] buffer = new byte[8192]; // 8 KB buffer

                        using (FileStream fileStream = !string.IsNullOrEmpty(saveTo) ? new FileStream(saveTo, FileMode.Create) : null)
                        {
                            int bytesRead;
                            List<byte> resultBytes = new List<byte>(); // Store response if no file path given

                            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                if (!headersParsed)
                                {
                                    string responsePart = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                                    int headerEndIdx = responsePart.IndexOf("\r\n\r\n");

                                    if (headerEndIdx != -1)
                                    {
                                        headersParsed = true;
                                        headerEndIdx += 4; // Position after headers
                                        string headers = responsePart.Substring(0, headerEndIdx);

                                        // Check for Content-Length directly
                                        int contentIndex = headers.IndexOf("Content-Length:", StringComparison.OrdinalIgnoreCase);
                                        if (contentIndex != -1)
                                        {
                                            int start = contentIndex + "Content-Length:".Length;
                                            int end = headers.IndexOf("\r\n", start);
                                            contentLength = int.Parse(headers.Substring(start, end - start).Trim());
                                        }

                                        int dataStart = headerEndIdx;
                                        int dataLength = bytesRead - dataStart;

                                        if (fileStream != null)
                                            fileStream.Write(buffer, dataStart, dataLength);
                                        else
                                            resultBytes.AddRange(new ArraySegment<byte>(buffer, dataStart, dataLength));
                                    }
                                }
                                else
                                {
                                    if (fileStream != null)
                                        fileStream.Write(buffer, 0, bytesRead);
                                    else
                                        resultBytes.AddRange(new ArraySegment<byte>(buffer, 0, bytesRead));
                                }

                                if (contentLength != -1 && (fileStream?.Length ?? resultBytes.Count) >= contentLength)
                                    break;
                            }

                            switch(fileStream == null)
                            {
                                case true:
                                    return resultBytes;
                                case false:
                                    File.ReadAllBytes(saveTo);
                                    return resultBytes;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
//https://discord.gg/5VYV4MjEAv