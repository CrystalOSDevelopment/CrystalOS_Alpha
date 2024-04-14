using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Applications;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.TaskBar;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;

namespace CrystalOSAlpha.Graphics
{
    class TaskScheduler
    {
        public static int counter = 0;
        public static int neg_x = 0;
        public static int neg_y = 0;
        public static int index = 0;
        public static int x_offset = (int)(TaskManager.Left - TaskManager.X_offset) + 15;
        public static int y_offset = TaskManager.Top - 35;

        public static Bitmap Preview = new Bitmap(13, 13, ColorDepth.ColorDepth32);

        public static bool Clicked = false;
        public static bool get_values = true;

        public static List<App> Apps = new List<App>();
        public static List<App> AppsQuick = new List<App>();
        public static List<Button_prop> Buttons = new List<Button_prop>();
        public static void Exec()
        {
            Random rnd = new Random();
            for (int i = 0; i < Apps.Count; i++)
            {
                for (int j = 0; j < Apps.Count - i - 1; j++)
                {
                    if (Apps[j].z > Apps[j + 1].z)
                    {
                        // Swap objects
                        App temp = Apps[j];
                        Apps[j] = Apps[j + 1];
                        Apps[j + 1] = temp;
                    }
                }
            }
            for (int i = 0; i < Apps.Count; i++)
            {
                Apps[i].z = i;
            }
            foreach (var app in Apps)
            {
                if (app.AppID.ToString().Length == 0)
                {
                    app.AppID = rnd.Next(1000, 10000);
                }
                if (app.y <= 1)
                {
                    app.y = 1;
                }
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if(MouseManager.X < app.x + app.width && MouseManager.X > app.x + app.width - 21)
                    {
                        if(MouseManager.Y > app.y + 2 && MouseManager.Y < app.y + 18)
                        {
                            Apps.Remove(app);
                        }
                    }
                    if (MouseManager.X < app.x + app.width - 26 && MouseManager.X > app.x + app.width - 42)
                    {
                        if (MouseManager.Y > app.y + 2 && MouseManager.Y < app.y + 18)
                        {
                            app.minimised = true;
                        }
                    }
                    if (MouseManager.X < app.x + app.width - 21 && MouseManager.X > app.x)
                    {
                        if (MouseManager.Y > app.y && MouseManager.Y < app.y + 21)
                        {
                            bool found = false;
                            for (int i = index + 1; i < Apps.Count; i++)
                            {
                                if (MouseManager.X > Apps[i].x && MouseManager.X < Apps[i].x + Apps[i].width)
                                {
                                    if (MouseManager.Y > Apps[i].y && MouseManager.Y < Apps[i].y + Apps[i].height)
                                    {
                                        if(Apps[i].minimised == false)
                                        {
                                            found = true;
                                        }
                                    }
                                }
                            }
                            if (found == false)
                            {
                                if (get_values == true)
                                {
                                    neg_x = (int)MouseManager.X - app.x;
                                    neg_y = (int)MouseManager.Y - app.y;
                                    app.movable = true;
                                    get_values = false;
                                }
                            }
                        }
                    }
                    if(MouseManager.X > app.x && MouseManager.X < app.x + app.width)
                    {
                        if (MouseManager.Y > app.y && MouseManager.Y < app.y + app.height)
                        {
                            bool found = false;
                            for (int i = index + 1; i < Apps.Count; i++)
                            {
                                if (MouseManager.X > Apps[i].x && MouseManager.X < Apps[i].x + Apps[i].width)
                                {
                                    if (MouseManager.Y > Apps[i].y && MouseManager.Y < Apps[i].y + Apps[i].height)
                                    {
                                        found = true;
                                    }
                                }
                            }
                            if (found == false)
                            {
                                if(TaskManager.disable == false)
                                {
                                    app.z = 999;
                                    TaskManager.disable = true;
                                }
                            }
                        }
                    }
                }
                if(app.movable == true)
                {
                    app.x = (int)MouseManager.X - neg_x;
                    app.y = (int)MouseManager.Y - neg_y;
                    if(MouseManager.MouseState == MouseState.None)
                    {
                        app.movable = false;
                        get_values = true;
                    }
                }
                index++;
                if(app.minimised == false)
                {
                    try
                    {
                        app.App();
                        app.RightClick();
                    }
                    catch (Exception e)
                    {

                    }
                }
                counter++;
            }
            if(MouseManager.MouseState == MouseState.None && TaskManager.disable == true)
            {
                TaskManager.disable = false;
            }
            index = 0;
            counter = 0;
        }

        public static void Render_Icons()
        {
            switch (Global_integers.TaskBarType)
            {
                case "Classic":
                    foreach(var app in Apps)
                    {
                        if(MouseManager.MouseState == MouseState.Left)
                        {
                            if(MouseManager.X > x_offset && MouseManager.X < x_offset + app.icon.Width)
                            {
                                if(MouseManager.Y > y_offset && MouseManager.Y < y_offset + app.icon.Height + 15)
                                {
                                    if(Clicked == false && app.name != null)
                                    {
                                        if(app.minimised == true)
                                        {
                                            app.minimised = false;
                                        }
                                        else
                                        {
                                            app.minimised = true;
                                            app.z = 999;
                                        }
                                        Clicked = true;
                                    }
                                }
                            }
                        }
                        if(app.name != null)
                        {
                            ImprovedVBE.DrawImageAlpha(app.icon, x_offset, y_offset, ImprovedVBE.cover);
                        }
                        if(app.name.Length > 7)
                        {
                            BitFont.DrawBitFontString(ImprovedVBE.cover, "ArialCustomCharset16", System.Drawing.Color.White, app.name.Remove(7), x_offset + 4, (int)(y_offset + app.icon.Height + 3));
                        }
                        else
                        {
                            BitFont.DrawBitFontString(ImprovedVBE.cover, "ArialCustomCharset16", System.Drawing.Color.White, app.name, x_offset + 4, (int)(y_offset + app.icon.Height + 3));
                        }
                        if(app.name != null)
                        {
                            x_offset += (int)app.icon.Width + 8;
                        }
                    }
                    if (MouseManager.MouseState == MouseState.None)
                    {
                        Clicked = false;
                    }
                    x_offset = (int)(TaskManager.Left - TaskManager.X_offset) + 15;
                    y_offset = TaskManager.Top - 35;
                    break;
                case "Nostalgia":
                    int XVal = 10;
                    int YVal = 10;
                    foreach(var btn in Apps)
                    {
                        if(btn.name != null)
                        {
                            if(btn.name.Length > 5)
                            {
                                btn.name = btn.name.Remove(5);
                                if (!btn.name.EndsWith("..."))
                                {
                                    if (btn.name.EndsWith(".."))
                                    {
                                        btn.name += ".";
                                    }
                                    else if(btn.name.EndsWith("."))
                                    {
                                        btn.name += "..";
                                    }
                                    else if (!btn.name.EndsWith("."))
                                    {
                                        btn.name += "...";
                                    }
                                }
                            }
                            Button.Button_render(ImprovedVBE.cover, XVal, YVal, 70, 25, 1, btn.name);
                            if(MouseManager.MouseState == MouseState.Left)
                            {
                                if(MouseManager.X > XVal && MouseManager.X < XVal + 70)
                                {
                                    if (MouseManager.Y > YVal && MouseManager.Y < YVal + 25)
                                    {
                                        Button.Button_render(ImprovedVBE.cover, XVal, YVal, 70, 25, ImprovedVBE.colourToNumber(255, 255, 255), btn.name);
                                        if(btn.minimised == false && Clicked == false)
                                        {
                                            btn.minimised = true;
                                            Clicked = true;
                                        }
                                        else if(btn.minimised == true && Clicked == false)
                                        {
                                            btn.minimised = false;
                                            Clicked = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (MouseManager.X > XVal && MouseManager.X < XVal + 70)
                                {
                                    if (MouseManager.Y > YVal && MouseManager.Y < YVal + 25)
                                    {
                                        Button.Button_render(ImprovedVBE.cover, XVal, YVal, 70, 25, ImprovedVBE.colourToNumber(25, 25, 25), btn.name);
                                        if (Preview.Width == 13 && Preview.Height == 13)
                                        {
                                            Preview = Widgets.Base.Widget_Back(300, 180, ImprovedVBE.colourToNumber(Global_integers.TaskBarR, Global_integers.TaskBarG, Global_integers.TaskBarB));
                                            Preview = ImprovedVBE.EnableTransparencyPreRGB(Preview, XVal, (int)TaskManager.TaskBar.Height + 5, Preview, Global_integers.TaskBarR, Global_integers.TaskBarG, Global_integers.TaskBarB, ImprovedVBE.cover);
                                        }
                                        else
                                        {
                                            Bitmap Temp = new Bitmap(Preview.Width, Preview.Height, ColorDepth.ColorDepth32);
                                            Array.Copy(Preview.RawData, Temp.RawData, Preview.RawData.Length);
                                            ImprovedVBE.DrawImage(ImprovedVBE.ScaleImageStock(btn.window, Temp.Width - 20, Temp.Height - 20), 10, 10, Temp);
                                            ImprovedVBE.DrawImageAlpha(Temp, XVal, (int)TaskManager.TaskBar.Height + 5, ImprovedVBE.cover);
                                        }
                                    }
                                }
                            }
                            if(Clicked == true && MouseManager.MouseState == MouseState.None)
                            {
                                Clicked = false;
                            }
                            XVal += 80;
                        }
                    }
                    break;
            }
        }
    }
}
