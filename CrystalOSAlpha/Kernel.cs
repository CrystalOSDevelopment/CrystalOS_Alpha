using _3DRendering;
using Cosmos.Core.Memory;
using Cosmos.HAL;
using Cosmos.HAL.Audio;
using Cosmos.HAL.BlockDevice.Registers;
using Cosmos.HAL.Drivers.Audio;
using Cosmos.System;
using Cosmos.System.Audio.IO;
using Cosmos.System.Audio;
using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.Listing;
using Cosmos.System.FileSystem.VFS;
using Cosmos.System.Graphics;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.TCP;
using Cosmos.System.Network.IPv4.UDP.DHCP;
using Cosmos.System.Network.IPv4.UDP.DNS;
using CrystalOS_Alpha;
using CrystalOS_Alpha.Graphics.Widgets;
using CrystalOSAlpha;
using CrystalOSAlpha.Applications.CarbonIDE;
using CrystalOSAlpha.Applications.FileSys;
using CrystalOSAlpha.Applications.Video_Player;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.Graphics.TaskBar;
using CrystalOSAlpha.Graphics.Widgets;
using CrystalOSAlpha.Programming;
using CrystalOSAlpha.SystemApps;
using CrystalOSAlpha.UI_Elements;
using IL2CPU.API.Attribs;
using ProjectDMG;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using EndPoint = Cosmos.System.Network.IPv4.EndPoint;
using Kernel = CrystalOS_Alpha.Kernel;
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
            ImprovedVBE.display(vbe);
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
                //File.WriteAllText("0:\\index.html", Network("example.com/index.html"));
                Directory.CreateDirectory("0:\\User\\Source");
                Directory.CreateDirectory("0:\\User\\Documents");
                Directory.CreateDirectory("0:\\Programs");
                Directory.CreateDirectory("0:\\Programs\\Office");
            }

            FPS_Counter f = new FPS_Counter();
            f.x = 200;
            f.y = 200;
            f.z = 0;
            f.name = null;
            f.minimised = false;
            f.icon = ImprovedVBE.ScaleImageStock(Resources.Web, 56, 56);
            TaskScheduler.Apps.Add(f);

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

        public static int collect = 0;

        public static string Clipboard = "";
        protected override void Run()
        {
            if (collect >= 4)
            {
                Heap.Collect();
                collect = 0;
            }
            else
            {
                collect++;
            }

            SideNav.Core();
            TaskScheduler.Exec();

            TaskManager.Main();

            BitFont.DrawBitFontString(ImprovedVBE.cover, "ArialCustomCharset16", System.Drawing.Color.White, Clipboard, 500, 40);

            ImprovedVBE.DrawImageAlpha(C, (int)MouseManager.X, (int)MouseManager.Y, ImprovedVBE.cover);


            ImprovedVBE.display(vbe);
            vbe.Display();
        }

        //public static string Network(string url)
        //{

        //    try
        //    {
        //        var dnsClient = new DnsClient();
        //        var tcpClient = new TcpClient();

        //        // DNS
        //        dnsClient.Connect(DNSConfig.DNSNameservers[0]);
        //        dnsClient.SendAsk(url.Remove(url.IndexOf("/")));//Working: dpgraph.com, cgi-resources.com, rdrop.com, digitalresearch.biz

        //        // Address from ip
        //        Address address = dnsClient.Receive();
        //        dnsClient.Close();

        //        // tcp
        //        tcpClient.Connect(address, 80);

        //        // httpget
        //        string httpget = "GET /" + url.Remove(0, url.IndexOf("/") + 1) + "HTTP/1.1\r\n" +
        //                         "User-Agent: CrystalOSAlpha\r\n" +
        //                         "Accept: */*\r\n" +
        //                         "Accept-Encoding: identity\r\n" +
        //                         "Host: " + url.Remove(url.IndexOf("/")) + "\r\n" +
        //                         "Connection: Keep-Alive\r\n\r\n";

        //        tcpClient.Send(Encoding.ASCII.GetBytes(httpget));

        //        // get http response
        //        var ep = new EndPoint(Address.Zero, 0);
        //        var data = tcpClient.Receive(ref ep);
        //        tcpClient.Close();


        //        string httpresponse = Encoding.ASCII.GetString(data);


        //        string[] responseParts = httpresponse.Split(new[] { "\r\n\r\n" }, 2, StringSplitOptions.None);

        //        if (responseParts.Length == 2)
        //        {
        //            string headers = responseParts[0];
        //            string content = responseParts[1];
        //            //BitFont.DrawBitFontString(ImprovedVBE.cover, "ArialCustomCharset16", Color.Black, content, 10, 10);
        //            return content;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
                
        //    }
        //    return "";
        //    */
        //}
    }
}
//https://discord.gg/5VYV4MjEAv