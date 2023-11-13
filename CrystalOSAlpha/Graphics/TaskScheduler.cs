using Cosmos.System;
using CrystalOSAlpha.Applications;
using CrystalOSAlpha.Applications.Calculator;
using CrystalOSAlpha.Applications.Minecraft;
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
                                        found = true;
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
                                app.z = 999;
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
                app.App();
                index++;
            }
            index = 0;
        }
    }
}
