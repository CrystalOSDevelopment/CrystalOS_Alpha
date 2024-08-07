using CrystalOSAlpha.UI_Elements;

namespace CrystalOSAlpha.Programming.CrystalSharp.Graphics
{
    public class CodeGenerator
    {
        public static string GenerateBase()
        {
            string code = "Main\n" +
                            "{\n" +
                                "this.x = 30;\n" +
                                "this.y = 40;\n" +
                                "this.width = 800;\n" +
                                "this.height = 600;\n" +
                                "this.title = \"Main Window\";\n" +
                                "this.color = 60, 60, 60;\n" +
                                "this.AlwaysOnTop = true;\n" +
                            "}";
            
            return code;
        }

        public static string ModifyCode(string code, string Propety, string Value)
        {
            string[] lines = code.Split('\n');

            // Find the property and replace it with the new value
            bool Found = false;
            for(int i = 0; i < lines.Length && Found == false; i++)
            {
                string Export = lines[i].Trim();
                if (Export.ToLower().Contains(Propety.Split('.')[1].ToLower()))
                {
                    lines[i] = "this." + Propety.Split('.')[1].ToLower() + " = " + Value + ";";
                    Found = true;
                }
            }

            // Rebuild the code
            string Output = "";
            foreach (string line in lines)
            {
                Output += line + "\n";
            }
            Output = Output.Trim('\n');
            return Output;
        }

        public static string AddUIElement(string code, UIElementHandler UI, string Extension = "", bool visible = true)
        {
            switch (UI.TypeOfElement)
            {
                case TypeOfElement.Label:
                    int LatsSemiColon = code.LastIndexOf(';');
                    code = code.Insert(LatsSemiColon + 2,  "AddElement(new Label(" + UI.X + ", " + UI.Y + ", " + UI.Color +  ", \"" + UI.Text + "\", \"" + UI.ID + "\");\n");
                    return code;
                case TypeOfElement.Button:
                    LatsSemiColon = code.LastIndexOf(';');
                    code = code.Insert(LatsSemiColon + 2, "AddElement(new Button(" + UI.X + ", " + UI.Y + ", " + UI.Width + ", " + UI.Height + ", " + UI.Color + ", \"" + UI.Text + "\", \"" + UI.ID + "\");\n");
                    return code;
                case TypeOfElement.TextBox:
                    LatsSemiColon = code.LastIndexOf(';');
                    code = code.Insert(LatsSemiColon + 2, "AddElement(new TextBox(" + UI.X + ", " + UI.Y + ", " + UI.Width + ", " + UI.Height + ", " + UI.Color + ", \"" + UI.Text + "\", \"" + Extension + "\", \"" + UI.ID + "\");\n");
                    return code;
                case TypeOfElement.Slider:
                    LatsSemiColon = code.LastIndexOf(';');
                    code = code.Insert(LatsSemiColon + 2, "AddElement(new Slider(" + UI.X + ", " + UI.Y + ", " + UI.Width + ", " + UI.MinVal + ", " + UI.MaxVal + ", " + UI.Value + ", \"" + UI.ID + "\");\n");
                    return code;
                case TypeOfElement.PictureBox:
                    LatsSemiColon = code.LastIndexOf(';');
                    code = code.Insert(LatsSemiColon + 2, "AddElement(new PictureBox(" + UI.X + ", " + UI.Y + ", " + visible + ", \"" + Extension + "\", \"" + UI.ID + "\");\n");
                    return code;
                default:
                    return code;
            }
        }
    }
}