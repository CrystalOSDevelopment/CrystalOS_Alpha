using Cosmos.System.Graphics;
using Cosmos.System;
using CrystalOSAlpha.Graphics.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalOSAlpha.Graphics.Widgets;
using System.Drawing;

namespace CrystalOSAlpha.Graphics.TaskBar
{
    public class SideNav
    {
        public static int X = ImprovedVBE.width;
        public static int Y = 0;

        public static bool Get_Back = true;
        public static Bitmap Back;
        public static bool keyframes = false;

        public static List<Bitmap> frames = new List<Bitmap>();
        public static int c = 1;

        public static int start_y = 40;
        public static void Core()
        {
            if(TaskScheduler.Apps.Count(d => d.minimised == false) - 2 == 0)
            {
                if (MouseManager.X > ImprovedVBE.width - 200 && MouseManager.X < ImprovedVBE.width)
                {
                    if (MouseManager.Y > Y && MouseManager.Y < Y + Back.Height - 70)
                    {
                        /*
                        if(X > ImprovedVBE.width - 190 && keyframes == false)
                        {
                            X -= 5;
                            Get_Back = true;
                        }
                        else if(X > ImprovedVBE.width - 190 && keyframes == true)
                        {
                            X -= 5;
                            Back = frames[frames.Count - c];
                            if (c != 1)
                            {
                                c--;
                            }
                        }
                        else
                        {
                            keyframes = true;
                        }
                        */
                        if (X > ImprovedVBE.width - 200)
                        {
                            X -= 5;
                        }
                    }
                }
                else
                {
                    if (X < ImprovedVBE.width)
                    {
                        X += 5;
                        /*
                        Back = frames[frames.Count - c];
                        if(c < frames.Count - 1)
                        {
                            c++;
                        }
                        */
                    }
                }

                if (Get_Back == true)
                {
                    Back = Base.Widget_Back(200, ImprovedVBE.height - 1, ImprovedVBE.colourToNumber(Global_integers.R, Global_integers.G, Global_integers.B));
                    Back = ImprovedVBE.DrawImageAlpha2(Back, X - 200, Y, Back);
                    BitFont.DrawBitFontString(Back, "VerdanaCustomCharset24", Color.White, "Widgets", 2, 5);
                    //frames.Add(Back);
                    Get_Back = false;
                }

                ImprovedVBE.DrawImageAlpha(Back, X, Y, ImprovedVBE.cover);
            }
        }
    }
}
