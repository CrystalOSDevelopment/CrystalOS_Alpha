using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Applications.Calculator;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOSAlpha.Applications.Solitare
{
    class Solitare : App
    {
        #region core_values
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int desk_ID { get; set; }
        public int AppID { get; set; }
        public string name { get; set; }
        public bool minimised { get; set; }
        public bool movable { get; set; }
        public Bitmap icon { get; set; }
        #endregion core_values

        public void App()
        {
            
        }
    }
}
