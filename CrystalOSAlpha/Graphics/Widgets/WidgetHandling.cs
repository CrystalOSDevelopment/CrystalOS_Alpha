using Cosmos.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOSAlpha.Graphics.Widgets
{
    public interface WidgetHandling
    {
        int X { get; set; }
        int Y { get; set; }
        int Z { get; set; }
        void Core() { }
    }

    public class Rendering
    {
        public static List<WidgetHandling> widgets = new List<WidgetHandling>();

        public static void render()
        {
            for (int i = 0; i < widgets.Count; i++)
            {
                for (int j = 0; j < widgets.Count - i - 1; j++)
                {
                    if (widgets[j].Z > widgets[j + 1].Z)
                    {
                        // Swap objects
                        WidgetHandling temp = widgets[j];
                        widgets[j] = widgets[j + 1];
                        widgets[j + 1] = temp;
                    }
                }
            }
            for (int i = 0; i < widgets.Count; i++)
            {
                widgets[i].Z = i;
            }

            foreach (var widget in widgets)
            {
                widget.Core();
            }
        }

    }
}
