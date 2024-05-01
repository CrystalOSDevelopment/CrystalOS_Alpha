using Cosmos.Core.Memory;
using Cosmos.System;
using Cosmos.System.FileSystem;
using Cosmos.System.Graphics;
using CrystalOS_Alpha.Graphics.Widgets;
using CrystalOSAlpha;
using CrystalOSAlpha.Applications.Clock;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.Graphics.TaskBar;
using CrystalOSAlpha.Graphics.Widgets;
using CrystalOSAlpha.UI_Elements;
using IL2CPU.API.Attribs;
using System.Drawing;
using System.IO;
using Sys = Cosmos.System;

namespace CrystalOS_Alpha
{
    public class Kernel : Sys.Kernel
    {
        public static VBECanvas vbe = new VBECanvas(new Mode(1920, 1080, ColorDepth.ColorDepth32));
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Icons.Cursor.bmp")] public static byte[] Cursor;
        public static Bitmap C = new Bitmap(Cursor);
        public static CosmosVFS fs = new CosmosVFS();
        public static bool Is_KeyboardMouse = false;
        protected override void BeforeRun()
        {
            ImprovedVBE.Display(vbe);
            if(VMTools.IsVMWare == true)
            {
                Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
                //Insert install here
                //Problem: Not implemented, so don't even attempt to search after it dear GitHub user!
                //Until then, this is the block of code that substitutes it
                #region Config
                //Create shortcut directories
                Directory.CreateDirectory("0:\\User\\" + GlobalValues.Username + "\\Favorites");
                Directory.CreateDirectory("0:\\User\\" + GlobalValues.Username + "\\Documents");
                Directory.CreateDirectory("0:\\User\\" + GlobalValues.Username + "\\Pictures");
                Directory.CreateDirectory("0:\\User\\" + GlobalValues.Username + "\\Films");
                Directory.CreateDirectory("0:\\User\\" + GlobalValues.Username + "\\Wastebasket");
                Directory.CreateDirectory("0:\\User\\" + GlobalValues.Username + "\\SecurityArea");

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
                        "WindowR=" + GlobalValues.R +
                        "\nWindowG=" + GlobalValues.G +
                        "\nWindowB=" + GlobalValues.B +
                        "\nTaskbarR=" + GlobalValues.TaskBarR +
                        "\nTaskbarG=" + GlobalValues.TaskBarG +
                        "\nTaskbarB=" + GlobalValues.TaskBarB +
                        "\nTaskbarType=" + GlobalValues.TaskBarType +
                        "\nUsername=" + GlobalValues.Username +
                        "\nIconR=" + GlobalValues.IconR +
                        "\nIconG=" + GlobalValues.IconG +
                        "\nIconB=" + GlobalValues.IconB +
                        "\nIconwidth=" + GlobalValues.IconWidth +
                        "\nIconheight=" + GlobalValues.IconHeight +
                        "\nStartcolor=" + GlobalValues.StartColor.ToArgb() +
                        "\nEndcolor=" + GlobalValues.EndColor.ToArgb() +
                        "\nBakground=" + GlobalValues.Background_type +
                        "\nBackgroundcolor=" + GlobalValues.Background_color +
                        "\nTransparency=" + GlobalValues.LevelOfTransparency;
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
                                GlobalValues.R = int.Parse(Split[1]);
                                break;
                            case "WindowG":
                                GlobalValues.G = int.Parse(Split[1]);
                                break;
                            case "WindowB":
                                GlobalValues.B = int.Parse(Split[1]);
                                break;

                            case "TaskbarR":
                                GlobalValues.TaskBarR = int.Parse(Split[1]);
                                break;
                            case "TaskbarG":
                                GlobalValues.TaskBarG = int.Parse(Split[1]);
                                break;
                            case "TaskbarB":
                                GlobalValues.TaskBarB = int.Parse(Split[1]);
                                break;

                            case "TaskbarType":
                                GlobalValues.TaskBarType = Split[1];
                                break;

                            case "Username":
                                GlobalValues.Username = Split[1];
                                break;

                            case "IconR":
                                GlobalValues.IconR = int.Parse(Split[1]);
                                break;
                            case "IconG":
                                GlobalValues.IconG = int.Parse(Split[1]);
                                break;
                            case "IconB":
                                GlobalValues.IconB = int.Parse(Split[1]);
                                break;

                            case "Iconwidth":
                                GlobalValues.IconWidth = uint.Parse(Split[1]);
                                break;
                            case "Iconheight":
                                GlobalValues.IconHeight = uint.Parse(Split[1]);
                                break;

                            case "Startcolor":
                                GlobalValues.StartColor = Color.FromArgb(int.Parse(Split[1]));
                                break;
                            case "Endcolor":
                                GlobalValues.EndColor = Color.FromArgb(int.Parse(Split[1]));
                                break;

                            case "Bakground":
                                GlobalValues.Background_type = Split[1];
                                break;
                            case "Backgroundcolor":
                                GlobalValues.Background_color = Split[1];
                                break;

                            case "Transparency":
                                GlobalValues.LevelOfTransparency = int.Parse(Split[1]);
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

            //Testing section
            //Solitare sol = new Solitare();
            //sol.x = 10;
            //sol.y = 100;
            //sol.width = 500;
            //sol.height = 400;
            //sol.z = 999;
            //sol.name = "Solitare";
            //sol.minimised = false;
            //sol.once = true;
            //sol.icon = ImprovedVBE.ScaleImageStock(Resources.Web, 56, 56);
            //TaskScheduler.Apps.Add(sol);

            Clock clock = new Clock();
            clock.x = 500;
            clock.y = 50;
            clock.z = 999;
            clock.width = 800;
            clock.height = 500;
            clock.name = "Clock";
            clock.minimised = false;
            clock.once = true;
            clock.icon = ImprovedVBE.ScaleImageStock(Resources.Web, 56, 56);
            TaskScheduler.Apps.Add(clock);
            #endregion Widgets

            #region Mouse
            MouseManager.ScreenWidth = (uint)ImprovedVBE.width;
            MouseManager.ScreenHeight = (uint)ImprovedVBE.height;

            MouseManager.X = (uint)ImprovedVBE.width / 2;
            MouseManager.Y = (uint)ImprovedVBE.height / 2;
            #endregion Mouse

            #region Font Registering
            Fonts.RegisterFonts();
            #endregion Font Registering
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

            ImprovedVBE.Display(vbe);

            if(collect % 2 == 0)
            {
                MouseManager.ResetScrollDelta();
            }
        }
    }
}
//https://discord.gg/5VYV4MjEAv