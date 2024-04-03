using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOSAlpha.Graphics
{
    class Global_integers
    {
        //Window color
        public static int R = 60;
        public static int G = 60;
        public static int B = 60;

        //
        public static int TaskBarR = 60;
        public static int TaskBarG = 60;
        public static int TaskBarB = 60;

        //Sets text color in title bar - Classic only
        public static Color c = Color.White;

        //Modes: Classic(float-up animation), Nostalgia(Just like in the good-old days), Window basic
        public static string TaskBarType = "Nostalgia";

        //Username
        public static string Username = "Username666";

        //Use these RGB values to adjust prefered color and size of the icon background in start menu
        public static int IconR = 255;
        public static int IconG = 0;
        public static int IconB = 0;
        public static uint IconWidth = 56;
        public static uint IconHeight = 56;

        //To change the start and end color of the gradient window titlebar modify these values
        public static Color StartColor = Color.Blue;
        public static Color EndColor = Color.DeepPink;

        //Modes: Default(Preloaded wallpaper), Monocolor(One single color as wallpaper), Loaded(Load from file. WARNING: Loading from filesystem may result in higher memory usage)
        public static string Background_type = "Default";
        //Colors 
        public static string Background_color = "CystalBlack";
        public static int LevelOfTransparency = 85;
    }
}
