using Cosmos.Core.Memory;
using Cosmos.System;
using Cosmos.System.FileSystem;
using Cosmos.System.Graphics;
using CrystalOS_Alpha.Graphics.Widgets;
using CrystalOSAlpha;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.Graphics.TaskBar;
using CrystalOSAlpha.Graphics.Widgets;
using CrystalOSAlpha.UI_Elements;
using IL2CPU.API.Attribs;
using System.Data;
using System.IO;
using System.Linq;
using Sys = Cosmos.System;

namespace CrystalOS_Alpha
{
    public class Kernel : Sys.Kernel
    {
        public static VBECanvas vbe = new VBECanvas(new Mode(1920, 1080, ColorDepth.ColorDepth32));
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
                Directory.CreateDirectory("0:\\User\\Source");
                Directory.CreateDirectory("0:\\User\\Documents");
                Directory.CreateDirectory("0:\\User\\System");
                File.Create("0:\\User\\System\\FrequentApps.sys");
                File.WriteAllText("0:\\User\\System\\FrequentApps.sys", "Settings\nGameboy\nMinecraft");
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
            if (TaskScheduler.Apps.Where(d => d.width > 800).Count() != 0)
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
                    Heap.Collect();
                    collect = 0;
                }
                else
                {
                    collect++;
                }
            }

            SideNav.Core();
            TaskScheduler.Exec();
            TaskManager.Main();

            ImprovedVBE.DrawImageAlpha(C, (int)MouseManager.X, (int)MouseManager.Y, ImprovedVBE.cover);

            ImprovedVBE.display(vbe);
            vbe.Display();
        }
    }
}
//https://discord.gg/5VYV4MjEAv