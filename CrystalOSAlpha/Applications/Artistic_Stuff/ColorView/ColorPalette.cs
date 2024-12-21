using Cosmos.System.Graphics;
namespace CrystalOSAlpha.Applications.Artistic_Stuff.ColorView
{
    class ColorPalette : App
    {
        #region Window porpeties
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
        public Bitmap window { get; set; }
        public bool once { get; set; }
        #endregion Window porpeties
        public void App()
        {
            //TODO: Needs a comple re-write, since the other one wasn't working at all
        }

        public void Render()
        {
            throw new System.NotImplementedException();
        }

        public void RightClick()
        {

        }
    }
}