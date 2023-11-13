using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOSAlpha.Applications
{
    public interface App
    {
        void App();
        int x { get; set; }
        int y { get; set; }
        int z { get; set; }
        int width { get; set; }
        int height { get; set; }
        int desk_ID { get; }
        string name { get; }

        bool minimised { get; set; }
        bool movable { get; set; }

    }
}
