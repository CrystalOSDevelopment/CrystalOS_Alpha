using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOS_Alpha;
using CrystalOSAlpha.Applications;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.Graphics.TaskBar;
using CrystalOSAlpha.SystemApps;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;

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
        public static int FromX = 0;
        public static int FromY = 0;

        public static Bitmap Preview = new Bitmap(13, 13, ColorDepth.ColorDepth32);

        public static bool Clicked = false;
        public static bool get_values = true;
        public static bool isResizing = false;

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

            if (MouseManager.MouseState == MouseState.None)
            {
                Clicked = false;
            }

            foreach (var app in Apps)
            {
                if (app.AppID < 1000)
                {
                    app.AppID = rnd.Next(1000, 10000);
                    app.once = true;
                }
                if (app.y <= 1)
                {
                    app.y = 1;
                }
                if ((TaskManager.MenuOpened == false || TaskManager.calendar == false) && TaskManager.clicked == false)
                {
                    if (MouseManager.MouseState == MouseState.Left && app.movable == false && ImprovedVBE.isMoving == false)
                    {
                        if(MouseManager.X < app.x + app.width && MouseManager.X > app.x + app.width - 21)
                        {
                            if(MouseManager.Y > app.y + 2 && MouseManager.Y < app.y + 18)
                            {
                                if (Clicked == false)
                                {
                                    bool found = false;
                                    for(int i = index + 1; i < Apps.Count; ++i)
                                    {
                                        if (Apps[i].x + Apps[i].width >= app.x + app.width - 21 && Apps[i].x + Apps[i].width <= app.x + app.width + 21)
                                        {
                                            if (Apps[i].y >= app.y - 22 && Apps[i].y <= app.y + 22)
                                            {
                                                if (Apps[i].minimised == false)
                                                {
                                                    found = true;
                                                }
                                            }
                                        }
                                    }
                                    if(found == false)
                                    {
                                        Apps.Remove(app);
                                        Clicked = true;
                                    }
                                }
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
                        ImprovedVBE.isMoving = true;
                        app.x = (int)MouseManager.X - neg_x;
                        app.y = (int)MouseManager.Y - neg_y;
                        if(MouseManager.MouseState == MouseState.None)
                        {
                            app.movable = false;
                            get_values = true;
                            ImprovedVBE.isMoving = false;
                        }
                    }
                }
                if(app.minimised == false)
                {
                    try
                    {
                        app.App();
                        app.RightClick();
                    }
                    catch (Exception e)
                    {
                        if(e.Message.Length > 5)
                        {
                            Apps.Add(new MsgBox(999, ImprovedVBE.width / 2 - 200, ImprovedVBE.height / 2 - 100, 400, 200, "Error!", e.Message, Resources.Celebration));
                        }
                        else
                        {
                            Apps.Add(new MsgBox(999, ImprovedVBE.width / 2 - 300, ImprovedVBE.height / 2 - 100, 600, 200, "Error!", "An unknown error occoured!\nIf restarting the app doesn't help, open a github issue at:\nhttps://github.com/CrystalOSDevelopment/CrystalOS_Alpha", Resources.Celebration));
                        }
                        Apps.Remove(app);
                    }
                    //Resize if every requirement checks out
                    if(MouseManager.MouseState == MouseState.Left)
                    {
                        if(MouseManager.X > app.x + app.width - 10 && MouseManager.X < app.x + app.width)
                        {
                            if(MouseManager.Y > app.y + app.height - 10 && MouseManager.Y < app.y + app.height)
                            {
                                bool Found = false;
                                for(int i = counter + 1; i < Apps.Count; i++)
                                {
                                    if (Apps[i].minimised == false)
                                    {
                                        if(Apps[i].x + Apps[i].width > app.width - 10)
                                        {
                                            if(Apps[i].y + Apps[i].height > app.height - 10)
                                            {
                                                Found = true;
                                            }
                                        }
                                    }
                                }
                                if(Found == false)
                                {
                                    FromX = app.x;
                                    FromY = app.y;
                                    isResizing = true;
                                }
                            }
                        }
                        if(isResizing == true)
                        {
                            ImprovedVBE.DrawRectangle(ImprovedVBE.cover, FromX, FromY, (int)MouseManager.X - FromX, (int)MouseManager.Y - FromY, ImprovedVBE.colourToNumber(255, 255, 255));
                        }
                    }
                    else if(MouseManager.MouseState == MouseState.None && isResizing == true)
                    {
                        if(app.x == FromX && app.y == FromY)
                        {
                            app.width = (int)MouseManager.X - FromX;
                            app.height = (int)MouseManager.Y - FromY;
                            app.once = true;
                            FromX = 0;
                            FromY = 0;
                            isResizing = false;
                            if(app.width < 150)
                            {
                                app.width = 150;
                            }
                            if(app.height < 150)
                            {
                                app.height = 150;
                            }
                        }
                    }
                }
                index++;
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
            switch (GlobalValues.TaskBarType)
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
                    bool exists = false;
                    foreach(var btn in Apps)
                    {
                        if(btn.name != null)
                        {
                            string name = btn.name;
                            string nameTitle = btn.name;
                            if(nameTitle.Length > 5)
                            {
                                nameTitle = nameTitle.Remove(5);
                                if (!nameTitle.EndsWith("..."))
                                {
                                    if (nameTitle.EndsWith(".."))
                                    {
                                        nameTitle += ".";
                                    }
                                    else if(nameTitle.EndsWith("."))
                                    {
                                        nameTitle += "..";
                                    }
                                    else if (!nameTitle.EndsWith("."))
                                    {
                                        nameTitle += "...";
                                    }
                                }
                            }
                            Button.Button_render(ImprovedVBE.cover, XVal, YVal, 70, 25, 1, nameTitle);
                            if(MouseManager.MouseState == MouseState.Left)
                            {
                                if(MouseManager.X > XVal && MouseManager.X < XVal + 70)
                                {
                                    if (MouseManager.Y > YVal && MouseManager.Y < YVal + 25)
                                    {
                                        Button.Button_render(ImprovedVBE.cover, XVal, YVal, 70, 25, ImprovedVBE.colourToNumber(255, 255, 255), nameTitle);
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
                                        Button.Button_render(ImprovedVBE.cover, XVal, YVal, 70, 25, ImprovedVBE.colourToNumber(25, 25, 25), nameTitle);
                                        if (Preview.Width == 13 && Preview.Height == 13)
                                        {
                                            Preview = Widgets.Base.Widget_Back((int)(btn.window.Width / 2.5) + 10, (int)(btn.window.Height / 2.5) + 35, ImprovedVBE.colourToNumber(GlobalValues.TaskBarR, GlobalValues.TaskBarG, GlobalValues.TaskBarB));
                                            Preview = ImprovedVBE.EnableTransparencyPreRGB(Preview, XVal, (int)TaskManager.TaskBar.Height + 5, Preview, GlobalValues.TaskBarR, GlobalValues.TaskBarG, GlobalValues.TaskBarB, ImprovedVBE.cover);
                                            exists = true;
                                        }
                                        else
                                        {
                                            Bitmap Temp = new Bitmap(Preview.Width, Preview.Height, ColorDepth.ColorDepth32);
                                            Array.Copy(Preview.RawData, Temp.RawData, Preview.RawData.Length);
                                            int MaxWidth = (int)Temp.Width - 30;
                                            int Chars = MaxWidth / 12 - 2;
                                            if (name.Length > Chars)
                                            {
                                                name = name.Remove(Chars);
                                            }
                                            BitFont.DrawBitFontString(Temp, "VerdanaCustomCharset24", Color.White, name, 28, 3);
                                            ImprovedVBE.DrawImageAlpha(ImprovedVBE.ScaleImageStock(btn.icon, 20, 20), 3, 3, Temp);
                                            ImprovedVBE.DrawImage(ImprovedVBE.ScaleImageStock(btn.window, Temp.Width - 10, Temp.Height - 40), 5, 35, Temp);
                                            ImprovedVBE.DrawImageAlpha(Temp, XVal, (int)TaskManager.TaskBar.Height + 5, ImprovedVBE.cover);
                                            exists = true;
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
                    if (exists == false && Preview.Width != 13 && Preview.Height != 13)
                    {
                        Preview = new Bitmap(13, 13, ColorDepth.ColorDepth32);
                    }
                    break;
            }
        }
    }
}
