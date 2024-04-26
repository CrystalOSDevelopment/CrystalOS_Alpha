using Cosmos.System.Graphics;

namespace CrystalOSAlpha.Applications
{
    public interface App
    {
        void App();
        void RightClick();
        int x { get; set; }
        int y { get; set; }
        int z { get; set; }
        int width { get; set; }
        int height { get; set; }
        int desk_ID { get; }
        int AppID { get; set; }
        string name { get; set; }

        bool minimised { get; set; }
        bool movable { get; set; }
        bool once { get; set; }

        Bitmap icon { get; set; }

        Bitmap window { get; set; }

    }
}
