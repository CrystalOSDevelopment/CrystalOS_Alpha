using Cosmos.System;
using CrystalOSAlpha.Applications;
using CrystalOSAlpha.Applications.Calculator;
using CrystalOSAlpha.Applications.Minecraft;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.TaskBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOSAlpha.Graphics
{
    class TaskScheduler
    {
        public static List<App> Apps = new List<App>();
        public static int neg_x = 0;
        public static int neg_y = 0;
        public static bool get_values = true;
        public static int index = 0;

        public static int x_offset = (int)(TaskManager.Left - TaskManager.X_offset) + 15;
        public static int y_offset = TaskManager.Top - 35;

        public static bool Clicked = false;

        public static int counter = 0;

        public static void Exec()
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
            for (int i = 0; i < Apps.Count; i++)
            {
                Apps[i].z = i;
            }
            foreach (var app in Apps)
            {
                if(MouseManager.MouseState == MouseState.Left)
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
                    app.App();
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
        }
    }
}
