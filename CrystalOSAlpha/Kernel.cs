using Cosmos.Core.Memory;
using Cosmos.HAL;
using Cosmos.System;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.Listing;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.Graphics;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.TCP;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using Cosmos.System.Network.IPv4.UDP.DNS;
using CrystalOS_Alpha.Graphics.Widgets;
using CrystalOSAlpha;
using CrystalOSAlpha.Applications.FileSys;
using CrystalOSAlpha.Applications.Minecraft;
using CrystalOSAlpha.Applications.Video_Player;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.Graphics.TaskBar;
using CrystalOSAlpha.Graphics.Widgets;
using CrystalOSAlpha.Programming;
using CrystalOSAlpha.UI_Elements;
using IL2CPU.API.Attribs;
using ProjectDMG;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using EndPoint = Cosmos.System.Network.IPv4.EndPoint;
using Sys = Cosmos.System;

namespace CrystalOS_Alpha
{
    public class Kernel : Sys.Kernel
    {
        public VBECanvas vbe = new VBECanvas(new Mode(1920, 1080, ColorDepth.ColorDepth32));
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Engine.Cursor.bmp")] public static byte[] Cursor;
        public static Bitmap C = new Bitmap(Cursor);
        public static CosmosVFS fs = new CosmosVFS();
        public static bool Is_KeyboardMouse = false;
        protected override void BeforeRun()
        {
            if(VMTools.IsVMWare == true)
            {
                Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
                using (var xClient = new DHCPClient())
                {
                    /** Send a DHCP Discover packet **/
                    //This will automatically set the IP config after DHCP response
                    xClient.SendDiscoverPacket();
                }
                if (!File.Exists("0:\\index.html"))
                {
                    File.Create("0:\\index.html");
                }
                File.WriteAllText("0:\\index.html", Network("example.com/index.html"));
                Directory.CreateDirectory("0:\\User");
            }

            FPS_Counter f = new FPS_Counter();
            f.X = 200;
            f.Y = 200;
            f.Z = 0;
            Rendering.widgets.Add(f);

            ImageViewer i = new ImageViewer();
            i.x = 600;
            i.y = 200;
            i.z = 999;
            i.minimised = false;
            i.icon = ImprovedVBE.ScaleImageStock(Resources.Web, 56, 56);
            TaskScheduler.Apps.Add(i);

            Note n = new Note();
            n.x = 900;
            n.y = 200;
            n.z = 999;
            n.name = null;
            i.minimised = false;
            n.icon = ImprovedVBE.ScaleImageStock(Resources.Web, 56, 56);
            TaskScheduler.Apps.Add(n);

            MouseManager.ScreenWidth = 1920;
            MouseManager.ScreenHeight = 1080;

            Fonts.RegisterFonts();
        }

        public static int FPS = 0;

        public static int LastS = -1;
        public static int Ticken = 0;

        public static int collect = 0;

        public static Bitmap temp = new Bitmap(40, 40, ColorDepth.ColorDepth32);

        public static void Update()
        {
            if (LastS == -1)
            {
                LastS = DateTime.UtcNow.Second;
            }
            if (DateTime.UtcNow.Second - LastS != 0)
            {
                if (DateTime.UtcNow.Second > LastS)
                {
                    FPS = Ticken / (DateTime.UtcNow.Second - LastS);
                }
                LastS = DateTime.UtcNow.Second;
                Ticken = 0;
            }
            Ticken++;
        }

        protected override void Run()
        {
            SideNav.Core();
            Rendering.render();
            TaskScheduler.Exec();

            TaskManager.Main();

            ImprovedVBE.DrawImageAlpha(temp, 10, 10, ImprovedVBE.cover);
            //BitFont.DrawBitFontString(ImprovedVBE.cover, "ArialCustomCharset16", System.Drawing.Color.White, CSharp.Clipboard, 500, 40);

            ImprovedVBE.DrawImageAlpha3(C, (int)MouseManager.X, (int)MouseManager.Y);

            ImprovedVBE.display(vbe);
            vbe.Display();
            if (collect >= 6)
            {
                Heap.Collect();
                collect = 0;
            }
            else
            {
                collect++;
            }
        }

        public static string Network(string url)
        {
            try
            {
                var dnsClient = new DnsClient();
                var tcpClient = new TcpClient();

                // DNS
                dnsClient.Connect(DNSConfig.DNSNameservers[0]);
                dnsClient.SendAsk(url.Remove(url.IndexOf("/")));//Working: dpgraph.com, cgi-resources.com, rdrop.com, digitalresearch.biz

                // Address from ip
                Address address = dnsClient.Receive();
                dnsClient.Close();

                // tcp
                tcpClient.Connect(address, 80);

                // httpget
                string httpget = "GET /" + url.Remove(0, url.IndexOf("/") + 1) + "HTTP/1.1\r\n" +
                                 "User-Agent: CrystalOSAlpha\r\n" +
                                 "Accept: */*\r\n" +
                                 "Accept-Encoding: identity\r\n" +
                                 "Host: " + url.Remove(url.IndexOf("/")) + "\r\n" +
                                 "Connection: Keep-Alive\r\n\r\n";

                tcpClient.Send(Encoding.ASCII.GetBytes(httpget));

                // get http response
                var ep = new EndPoint(Address.Zero, 0);
                var data = tcpClient.Receive(ref ep);
                tcpClient.Close();


                string httpresponse = Encoding.ASCII.GetString(data);


                string[] responseParts = httpresponse.Split(new[] { "\r\n\r\n" }, 2, StringSplitOptions.None);

                if (responseParts.Length == 2)
                {
                    string headers = responseParts[0];
                    string content = responseParts[1];
                    //BitFont.DrawBitFontString(ImprovedVBE.cover, "ArialCustomCharset16", Color.Black, content, 10, 10);
                    return content;
                }
            }
            catch (Exception ex)
            {
                
            }
            return "";
        }
    }
}
