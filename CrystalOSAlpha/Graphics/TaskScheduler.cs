﻿using Cosmos.System;
using Cosmos.System.Graphics;
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
        public static int MaxApp = 0;

        public static Bitmap Preview = new Bitmap(13, 13, ColorDepth.ColorDepth32);

        public static bool Clicked = false;
        public static bool get_values = true;
        public static bool isResizing = false;

        public static List<App> Apps = new List<App>();
        public static List<App> AppsQuick = new List<App>();
        public static List<Button> Buttons = new List<Button>();
        public static Random rnd = new Random();
        public static void Exec()
        {
            if(Apps.Count != MaxApp)
            {
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
                MaxApp = Apps.Count;
                for (int i = 0; i < Apps.Count; i++)
                {
                    Apps[i].z = i;
                }
            }

            if (MouseManager.MouseState == MouseState.None)
            {
                Clicked = false;
            }

            foreach (var app in Apps)
            {
                switch(app.AppID < 1000)
                {
                    case true:
                        app.AppID = rnd.Next(1000, 10000);
                        app.once = true;
                        break;
                }
                switch(app.y < 1)
                {
                    case true:
                        app.y = 1;
                        break;
                    case false:
                        switch(app.y < TaskManager.TaskBar.Height && GlobalValues.TaskBarType == "Nostalgia")
                        {
                            case true:
                                app.y = (int)TaskManager.TaskBar.Height;
                                break;
                        }
                        break;
                }
                switch(app.x <= 0 && app.name != null)
                {
                    case true:
                        app.x = 0;
                        break;
                }

                switch((TaskManager.MenuOpened == false || TaskManager.calendar == false) && TaskManager.clicked == false) //Checks if the menu/calendar isn't opened
                {
                    case true:
                        switch (MouseManager.MouseState == MouseState.Left && app.movable == false && ImprovedVBE.isMoving == false) //Checks if the left mouse button is pressed and if the app is movable(widget)
                        {
                            case true:
                                switch(MouseManager.Y > app.y && MouseManager.Y < app.y + 21) //Checks if the mouse is within the bounds of the app titlebar
                                {
                                    case true:
                                        int WidthAndX = app.x + app.width;
                                        if (MouseManager.X < WidthAndX && MouseManager.X > WidthAndX - 21)
                                        {
                                            if (Clicked == false)
                                            {
                                                bool found = false;
                                                for (int i = index + 1; i < Apps.Count; ++i)
                                                {
                                                    if (Apps[i].x + Apps[i].width >= WidthAndX - 21 && Apps[i].x + Apps[i].width <= WidthAndX + 21)
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
                                                if (found == false)
                                                {
                                                    Apps.Remove(app);
                                                    Clicked = true;
                                                }
                                            }
                                        } //Close button
                                        if (MouseManager.X < WidthAndX - 26 && MouseManager.X > WidthAndX - 42)
                                        {
                                            app.minimised = true;
                                        } // Minimise button
                                        if (MouseManager.X < WidthAndX - 45 && MouseManager.X > app.x)
                                        {
                                            bool found = false;
                                            for (int i = index + 1; i < Apps.Count; i++)
                                            {
                                                if (MouseManager.X > Apps[i].x && MouseManager.X < Apps[i].x + Apps[i].width)
                                                {
                                                    if (MouseManager.Y > Apps[i].y && MouseManager.Y < Apps[i].y + Apps[i].height)
                                                    {
                                                        if (Apps[i].minimised == false)
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
                                                if (TaskManager.disable == false)
                                                {
                                                    app.z = 999;
                                                    TaskManager.disable = true;
                                                }
                                            }
                                        } //Move window by titlebar
                                        break;
                                }
                                break;
                        }
                        switch (app.movable)
                        {
                            case true:
                                ImprovedVBE.isMoving = true;
                                app.x = (int)MouseManager.X - neg_x;
                                app.y = (int)MouseManager.Y - neg_y;
                                if (MouseManager.MouseState == MouseState.None)
                                {
                                    app.movable = false;
                                    get_values = true;
                                    ImprovedVBE.isMoving = false;
                                }
                                break;
                        }
                        break;
                }

                switch (app.minimised)
                {
                    case false:
                        try
                        {
                            app.App();
                            if (ImprovedVBE.RequestRedraw == true && app.window != null && app.width != ImprovedVBE.width)
                            {
                                switch (GlobalValues.TaskBarType)
                                {
                                    case "Classic":
                                        ImprovedVBE.DrawImageAlpha(app.window, app.x, app.y, ImprovedVBE.cover);
                                        break;
                                    case "Nostalgia":
                                        ImprovedVBE.DrawImageAlpha(app.window, app.x, app.y, ImprovedVBE.cover);
                                        break;
                                }
                            }
                            //Todo: Add Rendering when requested
                            app.RightClick();
                        }
                        catch (Exception e)
                        {
                            if (!e.Message.Contains("TCP"))
                            {
                                if(e.Message.Length > 5)
                                {
                                    Apps.Add(new MsgBox(999, ImprovedVBE.width / 2 - 200, ImprovedVBE.height / 2 - 100, 400, 200, "Error!", e.Message, Resources.Celebration));
                                }
                                else
                                {
                                    Apps.Add(new MsgBox(999, ImprovedVBE.width / 2 - 300, ImprovedVBE.height / 2 - 100, 600, 200, "Error!", "An unknown error occoured!\nIf restarting the app doesn't help, open a github issue at:\nhttps://github.com/CrystalOSDevelopment/CrystalOS_Alpha\n" + e.Message, Resources.Celebration));
                                }
                                Apps.Remove(app);
                            }
                        }
                        //Resize if every requirement checks out
                        switch(MouseManager.MouseState == MouseState.Left)
                        {
                            case true:
                                if (MouseManager.X > app.x + app.width - 10 && MouseManager.X < app.x + app.width)
                                {
                                    if (MouseManager.Y > app.y + app.height - 10 && MouseManager.Y < app.y + app.height)
                                    {
                                        bool Found = false;
                                        for (int i = counter + 1; i < Apps.Count; i++)
                                        {
                                            if (Apps[i].minimised == false)
                                            {
                                                if (Apps[i].x + Apps[i].width > app.width - 10)
                                                {
                                                    if (Apps[i].y + Apps[i].height > app.height - 10)
                                                    {
                                                        Found = true;
                                                    }
                                                }
                                            }
                                        }
                                        if (Found == false)
                                        {
                                            FromX = app.x;
                                            FromY = app.y;
                                            isResizing = true;
                                        }
                                    }
                                }
                                switch (isResizing)
                                {
                                    case true: ImprovedVBE.DrawRectangle(ImprovedVBE.cover, FromX, FromY, (int)MouseManager.X - FromX, (int)MouseManager.Y - FromY, ImprovedVBE.colourToNumber(255, 255, 255)); break;
                                }
                                break;
                            case false:
                                switch(MouseManager.MouseState == MouseState.None && isResizing == true)
                                {
                                    case true:
                                        if (app.x == FromX && app.y == FromY)
                                        {
                                            app.width = (int)MouseManager.X - FromX;
                                            app.height = (int)MouseManager.Y - FromY;
                                            app.once = true;
                                            FromX = 0;
                                            FromY = 0;
                                            isResizing = false;
                                            if (app.width < 150)
                                            {
                                                app.width = 150;
                                            }
                                            if (app.height < 150)
                                            {
                                                app.height = 150;
                                            }
                                            MaxApp = 0;
                                            ImprovedVBE.RequestRedraw = true;
                                            ImprovedVBE.Clear(true);
                                            ImprovedVBE.Counter = 0;
                                        }
                                        break;
                                }
                                break;
                        }
                        break;
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
                        switch (btn.name)
                        {
                            case null:
                                break;
                            default:
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
                                Button button = new Button(XVal, YVal, 70, 25, nameTitle, 1);
                                if(ImprovedVBE.RequestRedraw == true)
                                {
                                    button.Render(ImprovedVBE.cover);
                                }
                                //button.Render(ImprovedVBE.cover);
                                if(MouseManager.MouseState == MouseState.Left)
                                {
                                    if(MouseManager.X > XVal && MouseManager.X < XVal + 70)
                                    {
                                        if (MouseManager.Y > YVal && MouseManager.Y < YVal + 25)
                                        {
                                            button = new Button(XVal, YVal, 70, 25, nameTitle, ImprovedVBE.colourToNumber(255, 255, 255));
                                            button.Render(ImprovedVBE.cover);

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
                                            button = new Button(XVal, YVal, 70, 25, nameTitle, ImprovedVBE.colourToNumber(25, 25, 25));
                                            button.Render(ImprovedVBE.cover);

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
                                break;

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
