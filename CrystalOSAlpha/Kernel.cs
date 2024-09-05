using Cosmos.Core.Memory;
using Cosmos.System;
using Cosmos.System.FileSystem;
using Cosmos.System.Graphics;
using Cosmos.System.Network.IPv4;
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
using Cosmos.HAL;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4.UDP.DNS;

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
        protected override void BeforeRun()
        {
            try
            {
                NetworkDevice nic = NetworkDevice.GetDeviceByName("eth0"); //get network device by name
                IPConfig.Enable(nic, new Address(192, 168, 1, 69), new Address(255, 255, 255, 0), new Address(192, 168, 1, 254)); //enable IPv4 configuration

                using (var xClient = new Cosmos.System.Network.IPv4.UDP.DHCP.DHCPClient())
                {
                    /** Send a DHCP Discover packet **/
                    //This will automatically set the IP config after DHCP response
                    xClient.SendDiscoverPacket();

                }
            }
            catch (Exception ex)
            {
               
            }

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
            i.minimised = false;
            n.icon = ImprovedVBE.ScaleImageStock(Resources.Web, 56, 56);
            TaskScheduler.Apps.Add(n);
            #endregion Widgets

            #region Mouse
            MouseManager.ScreenWidth = (uint)ImprovedVBE.width;
            MouseManager.ScreenHeight = (uint)ImprovedVBE.height;

            MouseManager.X = (uint)ImprovedVBE.width / 2;
            MouseManager.Y = (uint)ImprovedVBE.height / 2;
            #endregion Mouse
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
            if(Is_KeyboardMouse == true)
            {
                KeyEvent k;
                if(KeyboardManager.TryReadKey(out k))
                {
                    Keyboard.HandleKeyboard("", k);
                }
            }

            //If there are more than one window open that is over 800 pixel wide, Heap.Collect is called every second frame, providing more stability.
            if (TaskScheduler.Apps.FindAll(d => d.width > 800).Count != 0)
            {
                if (collect >= 2)
                {
                    Heap.Collect();
                    collect = 0;
                }
                else
                {
                    collect++;
                }
            }
            else
            {
                if (collect >= 6)
                {
                    collect = 0;
                }
                else
                {
                    collect++;
                }
            }

            //Layer organising between the menu, taskbar, calendar and apps
            if(TaskManager.MenuOpened == false && TaskManager.calendar == false)
            {
                if(GlobalValues.TaskBarType == "Classic")
                {
                    SideNav.Core();
                    TaskScheduler.Exec();
                    TaskManager.Main();
                }
                else if(GlobalValues.TaskBarType == "Nostalgia")
                {
                    SideNav.Core();
                    TaskScheduler.Exec();
                    TaskManager.Main();
                }
            }
            else
            {
                SideNav.Core();
                TaskScheduler.Exec();
                TaskManager.Main();
            }

            //Layer organising between the menu, taskbar, calendar and apps
            if(TaskManager.MenuOpened == true && GlobalValues.TaskBarType == "Nostalgia")
            {
                TaskManager.Dynamic_Menu(ImprovedVBE.width / 2 - 200, 50, 400, 400);
            }

            //Renders the cursor
            ImprovedVBE.DrawImageAlpha(C, (int)MouseManager.X, (int)MouseManager.Y, ImprovedVBE.cover);

            //Display to the screen
            ImprovedVBE.Display(vbe);

            //For more sensitive scrolling, MouseManager.ResetScrollDelta is called in every second kernel cycle
            if (collect % 2 == 0)
            {
                MouseManager.ResetScrollDelta();
            }
        }

        public static string getContent(string url)
        {
            string IP = "";
            using (TcpClient client = new TcpClient())
            {
                using (var xClient = new DnsClient())
                {
                    xClient.Connect(DNSConfig.DNSNameservers[0]); //DNS Server address. We recommend a Google or Cloudflare DNS, but you can use any you like!

                    /** Send DNS ask for a single domain name **/
                    xClient.SendAsk(url);

                    /** Receive DNS Response **/
                    Address destination = xClient.Receive(); //can set a timeout value
                    IP = destination.ToString();
                    xClient.Close();
                }

                Address address = Address.Parse(IP);
                string serverIp = address.ToString();
                int serverPort = 80;
                client.Connect(serverIp, serverPort);
                NetworkStream stream = client.GetStream();

                string httpget = "GET " + "/index.html" + " HTTP/1.1\r\n" +
                                         "User-Agent: CrystalOS\r\n" +
                                         "Accept: */*\r\n" +
                                         "Accept-Encoding: identity\r\n" +
                                         "Host: " + IP + "\r\n" +
                                         "Connection: Keep-Alive\r\n\r\n";
                string messageToSend = httpget;
                byte[] dataToSend = Encoding.ASCII.GetBytes(messageToSend);
                stream.Write(dataToSend, 0, dataToSend.Length);
                /** Receive data **/
                byte[] receivedData = new byte[client.ReceiveBufferSize];
                int bytesRead = stream.Read(receivedData, 0, receivedData.Length);
                string receivedMessage = Encoding.ASCII.GetString(receivedData, 0, bytesRead);


                string[] responseParts = receivedMessage.Split(new[] { "\r\n\r\n" }, 2, StringSplitOptions.None);
                return receivedMessage;
            }
        }
    }
}
//https://discord.gg/5VYV4MjEAv