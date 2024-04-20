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
using System.Drawing;
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
                //Insert install here
                //Problem: Not implemented, so don't even attempt to search after it dear GitHub user!
                //Until then, this is the block of code that substitutes it
                #region Config
                //Create shortcut directories
                Directory.CreateDirectory("0:\\User\\" + Global_integers.Username + "\\Favorites");
                Directory.CreateDirectory("0:\\User\\" + Global_integers.Username + "\\Documents");
                Directory.CreateDirectory("0:\\User\\" + Global_integers.Username + "\\Pictures");
                Directory.CreateDirectory("0:\\User\\" + Global_integers.Username + "\\Films");
                Directory.CreateDirectory("0:\\User\\" + Global_integers.Username + "\\Wastebasket");
                Directory.CreateDirectory("0:\\User\\" + Global_integers.Username + "\\SecurityArea");

                //System config files
                //if (!File.Exists("0:\\User\\System\\Appearance.sys"))
                //{
                //    File.Create("0:\\System\\Appearance.sys");
                //    File.WriteAllText("0:\\System\\Appearance.sys", "Wallpaper=Default");
                //}
                Directory.CreateDirectory("0:\\System");
                File.Create("0:\\System\\FrequentApps.sys");
                File.WriteAllText("0:\\System\\FrequentApps.sys", "Settings\nGameboy\nMinecraft\nFileSystem");
                //Customization file
                if (!File.Exists("0:\\System\\Layout.sys"))
                {
                    File.Create("0:\\System\\Layout.sys");
                    string Layout =
                        "WindowR=" + Global_integers.R +
                        "\nWindowG=" + Global_integers.G +
                        "\nWindowB=" + Global_integers.B +
                        "\nTaskbarR=" + Global_integers.TaskBarR +
                        "\nTaskbarG=" + Global_integers.TaskBarG +
                        "\nTaskbarB=" + Global_integers.TaskBarB +
                        "\nTaskbarType=" + Global_integers.TaskBarType +
                        "\nUsername=" + Global_integers.Username +
                        "\nIconR=" + Global_integers.IconR +
                        "\nIconG=" + Global_integers.IconG +
                        "\nIconB=" + Global_integers.IconB +
                        "\nIconwidth=" + Global_integers.IconWidth +
                        "\nIconheight=" + Global_integers.IconHeight +
                        "\nStartcolor=" + Global_integers.StartColor.ToArgb() +
                        "\nEndcolor=" + Global_integers.EndColor.ToArgb() +
                        "\nBakground=" + Global_integers.Background_type +
                        "\nBackgroundcolor=" + Global_integers.Background_color +
                        "\nTransparency=" + Global_integers.LevelOfTransparency;
                    File.WriteAllText("0:\\System\\Layout.sys", Layout);
                }
                else
                {
                    string[] Lines = File.ReadAllLines("0:\\System\\Layout.sys");
                    foreach(string s in Lines)
                    {
                        string[] Split = s.Split('=');
                        switch(Split[0])
                        {
                            case "WindowR":
                                Global_integers.R = int.Parse(Split[1]);
                                break;
                            case "WindowG":
                                Global_integers.G = int.Parse(Split[1]);
                                break;
                            case "WindowB":
                                Global_integers.B = int.Parse(Split[1]);
                                break;

                            case "TaskbarR":
                                Global_integers.TaskBarR = int.Parse(Split[1]);
                                break;
                            case "TaskbarG":
                                Global_integers.TaskBarG = int.Parse(Split[1]);
                                break;
                            case "TaskbarB":
                                Global_integers.TaskBarB = int.Parse(Split[1]);
                                break;

                            case "TaskbarType":
                                Global_integers.TaskBarType = Split[1];
                                break;

                            case "Username":
                                Global_integers.Username = Split[1];
                                break;

                            case "IconR":
                                Global_integers.IconR = int.Parse(Split[1]);
                                break;
                            case "IconG":
                                Global_integers.IconG = int.Parse(Split[1]);
                                break;
                            case "IconB":
                                Global_integers.IconB = int.Parse(Split[1]);
                                break;

                            case "Iconwidth":
                                Global_integers.IconWidth = uint.Parse(Split[1]);
                                break;
                            case "Iconheight":
                                Global_integers.IconHeight = uint.Parse(Split[1]);
                                break;

                            case "Startcolor":
                                Global_integers.StartColor = Color.FromArgb(int.Parse(Split[1]));
                                break;
                            case "Endcolor":
                                Global_integers.EndColor = Color.FromArgb(int.Parse(Split[1]));
                                break;

                            case "Bakground":
                                Global_integers.Background_type = Split[1];
                                break;
                            case "Backgroundcolor":
                                Global_integers.Background_color = Split[1];
                                break;

                            case "Transparency":
                                Global_integers.LevelOfTransparency = int.Parse(Split[1]);
                                break;
                        }
                    }
                }
                ////Applications folder
                Directory.CreateDirectory("0:\\Programs");
                ////CarbonIDE folder
                Directory.CreateDirectory("0:\\User\\Source");
                #endregion Config
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

            if(TaskManager.MenuOpened == false)
            {
                if(Global_integers.TaskBarType == "Classic")
                {
                    SideNav.Core();
                    TaskScheduler.Exec();
                    TaskManager.Main();
                }
                else if(Global_integers.TaskBarType == "Nostalgia")
                {
                    SideNav.Core();
                    TaskManager.Main();
                    TaskScheduler.Exec();
                }
            }
            else
            {
                SideNav.Core();
                TaskScheduler.Exec();
                TaskManager.Main();
            }

            ImprovedVBE.DrawImageAlpha(C, (int)MouseManager.X, (int)MouseManager.Y, ImprovedVBE.cover);

            ImprovedVBE.display(vbe);
        }
    }
}
//https://discord.gg/5VYV4MjEAv