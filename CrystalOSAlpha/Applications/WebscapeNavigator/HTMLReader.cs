using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Icons;
using System.Collections.Generic;

namespace CrystalOSAlpha.Applications.WebscapeNavigator
{
    class HTMLReader
    {
        public static void Interpret()
        {
            string htmlCode = "<!DOCTYPE html>\n<html>\n<head>\n    <title>Example HTML</title>\n</head>\n<body>\n    <h1>Hello, world!</h1>\n    <p>This is an example HTML file.</p>\n</body>\n</html>";

            //CrystalOSAlpha.Applications.Notepad.Notepad Notepad = new CrystalOSAlpha.Applications.Notepad.Notepad();
            //Notepad.x = 100;
            //Notepad.y = 100;
            //Notepad.width = 700;
            //Notepad.height = 420;
            //Notepad.name = "Notepad";
            //Notepad.z = 999;
            //Notepad.icon = Resources.Notepad;
            //TaskScheduler.Apps.Add(Notepad);
        }

        public static List<string> Chunkify(string content)
        {
            List<string> chunks = new List<string>();

            string[] KeyTags = new string[] { "<html>", "</html>", "<head>", "</head>" };

            return chunks;
        }
    }
}
