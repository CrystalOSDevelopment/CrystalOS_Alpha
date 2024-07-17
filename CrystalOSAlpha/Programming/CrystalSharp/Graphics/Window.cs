using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Applications;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Programming.CrystalSharp.CodeStructure.Variables;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace CrystalOSAlpha.Programming.CrystalSharp.Graphics
{
    class Window : App
    {
        public Window(int x, int y, int z, int width, int height, int desk_ID, string name, bool minimised, Bitmap icon, string Code)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.width = width;
            this.height = height;
            this.desk_ID = desk_ID;
            this.name = name;
            this.minimised = minimised;
            this.icon = icon;
            //this.Code = Code;
        }

        #region Essential
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

        public int Counter = 0;
        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);
        public int part = 0;
        public int CycleCount = 0;

        public string Code = "Main.WFL\n" +
                            "Main\n" +
                            "{\n" +
                                "this.x = 30;\n" +
                                "this.y = 30;\n" +
                                "this.width = 600;\n" +
                                "this.height = 600;\n" +
                                "this.title = \"Main Window\";\n" +
                                "this.color = 60, 60, 60;\n" +
                                "this.AlwaysOnTop = true;\n" +
                                "//UIElements\n" +
                                        "  x,  y,  width, height, color, text, id\n" +
                                "AddElement(new PictureBox(0, 0, true, \"1:\\RAY.bmp\", \"PictureBoxID\"));\n" +
                                "AddElement(new Button(10, 10, 200, 100, 1, \"Button1\", \"btn_1\"));\n" +
                                "AddElement(new Label(10, 120, 1, \"SampleText\", \"TestLabel\"));\n" +
                                "AddElement(new TextBox(10, 160, 150, 25, " + ImprovedVBE.colourToNumber(60, 60, 60) + ", \"\", \"CoolPlaceHolder\", \"CoolID\"));\n" +
                                "AddElement(new CheckBox(10, 195, 20, 20, true, \"Agree to terms and services\", \"CheckBoxID\"));\n" +
                                "AddElement(new Table(300, 10, 200, 200, 2, 5, \"TableTest\"));\n" +
                                "AddElement(new Slider(150, 250, 300, 0, 255, 128, \"SliderTest\"));\n" +
                            "}\n" +
                            "-----||-----\n" +
                            "Main.cs\n" +
                            "class Main\n" +
                            "{\n" +
                                "public void Main()\n" +
                                "{\n" +
                                    "//Code\n" +
                                "}\n" +
                            "}\n" +
                            "-----||-----\n";
        public bool initial = true;
        public bool once { get; set; }
        public bool clicked = false;
        public bool HasTitlebar = true;
        public bool AlwaysOnTop = false;
        public bool temp = true;

        public Bitmap canvas;
        public Bitmap window { get; set; }
        #endregion Essential

        #region UI_Elements
        public List<UIElementHandler> UIElements = new List<UIElementHandler>();
        public List<Variable> Variables = new List<Variable>();

        public MenuBar mb;
        public bool HasMB = false;
        #endregion UI_Elements

        public List<string> CodeParts = new List<string>();
        public List<Separated> Separated = new List<Separated>();
        public string debug = "";
        public void App()
        {
            if (AlwaysOnTop == true)
            {
                z = 1000;
            }
            if (initial == true)
            {
                //Separate the code into 3 segments:
                //1. Initial window UI element layout **Done! All implemented**
                //2. Loop of the window
                //3. UI element actions
                //CodeParts = Separate(Code);
                initial = false;
            }

            if (once == true)
            {
                canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                window = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);

                #region corners
                ImprovedVBE.DrawFilledEllipse(canvas, 10, 10, 10, 10, CurrentColor);
                ImprovedVBE.DrawFilledEllipse(canvas, width - 11, 10, 10, 10, CurrentColor);
                ImprovedVBE.DrawFilledEllipse(canvas, 10, height - 10, 10, 10, CurrentColor);
                ImprovedVBE.DrawFilledEllipse(canvas, width - 11, height - 10, 10, 10, CurrentColor);

                ImprovedVBE.DrawFilledRectangle(canvas, CurrentColor, 0, 10, width, height - 20, false);
                ImprovedVBE.DrawFilledRectangle(canvas, CurrentColor, 5, 0, width - 10, 15, false);
                ImprovedVBE.DrawFilledRectangle(canvas, CurrentColor, 5, height - 15, width - 10, 15, false);
                #endregion corners

                canvas = ImprovedVBE.EnableTransparencyPreRGB(canvas, x, y, canvas, Color.FromArgb(CurrentColor).R, Color.FromArgb(CurrentColor).G, Color.FromArgb(CurrentColor).B, ImprovedVBE.cover);

                if (HasTitlebar == true)
                {
                    ImprovedVBE.DrawGradientLeftToRight(canvas);

                    ImprovedVBE.DrawFilledEllipse(canvas, width - 13, 10, 8, 8, ImprovedVBE.colourToNumber(255, 0, 0));

                    ImprovedVBE.DrawFilledEllipse(canvas, width - 34, 10, 8, 8, ImprovedVBE.colourToNumber(227, 162, 37));

                    BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, name, 2, 2);
                }

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                for (int i = 0; i < CodeParts.Count && part == 0; i++)
                {
                    if (CodeParts[i].Contains("#void Looping"))
                    {
                        part = i;
                    }
                }
                once = false;
            }

            if (MouseManager.MouseState == MouseState.None)
            {
                clicked = false;
            }

            if (x == 0 && window.Width == ImprovedVBE.width)
            {
                Array.Copy(window.RawData, 0, ImprovedVBE.cover.RawData, y * ImprovedVBE.width, window.RawData.Length);
            }
            else
            {
                ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
            }
        }

        public void App(Bitmap RenderTo)
        {
            if (AlwaysOnTop == true)
            {
                z = 1000;
            }
            if (initial == true)
            {
                //Separate the code into 3 segments:
                //1. Initial window UI element layout **Done! All implemented**
                //2. Loop of the window
                //3. UI element actions

                //This is the part, where the input code gets segmented
                #region Stage1
                CodeParts = CodeChunk.Chunkification(Code);
                string assembled = "";
                for (int i = 0; i < CodeParts.Count; i++)
                {
                    string header = CodeParts[i].Split('\n')[0];
                    string content = CodeParts[i].Remove(0, CodeParts[i].IndexOf('\n') + 1);
                    Separated.Add(new Separated(header, content));
                }
                #endregion Stage1

                //This is where the parsing happens for the UI elements of the window
                #region Stage2
                UIElements.Add(new label(10, 10, "", ImprovedVBE.colourToNumber(255, 255, 255), "debug"));
                foreach(string line in Separated[0].Content.Split('\n'))
                {
                    string Export = line.Trim();
                    Export = Export.Remove(Export.Length - 1);
                    if (Export.Contains(" = "))
                    {
                        int indexOf = Export.IndexOf(" = ");
                        Export = Export.Remove(indexOf, 3);
                        Export =  Export.Insert(indexOf, ".");
                    }
                    string[] Segments = { Export };
                    switch(Export.Contains("AddElement(new "))
                    {
                        case false:
                            Segments = Export.Split('.');
                            break;
                    }

                    if(Segments.Length > 1)
                    {
                        switch (Segments[1])
                        {
                            case "x":
                                this.x = int.Parse(Segments[2]);
                                break;
                            case "y":
                                this.y = int.Parse(Segments[2]);
                                break;
                            case "width":
                                this.width = int.Parse(Segments[2]);
                                break;
                            case "height":
                                this.height = int.Parse(Segments[2]);
                                break;
                            case "title":
                                this.name = Segments[2].Remove(Segments[2].Length - 1).Remove(0, 1);
                                break;
                            case "color":
                                string[] RGB = Segments[2].Replace(" ", "").Split(',');
                                this.CurrentColor = ImprovedVBE.colourToNumber(int.Parse(RGB[0]), int.Parse(RGB[1]), int.Parse(RGB[2]));
                                break;
                            case "AlwaysOnTop":
                                this.AlwaysOnTop = bool.Parse(Segments[2]);
                                break;
                        }
                    }
                    else
                    {
                        switch(Segments[0].Contains("AddElement(new "))
                        {
                            case true:
                                string CleanUp = Segments[0].Remove(0, "AddElement(new ".Length);
                                CleanUp = CleanUp.Remove(CleanUp.Length - 1);
                                string ElementType = CleanUp.Split('(')[0];
                                switch (ElementType)
                                {
                                    //The complete(?) set of UI elements
                                    case "Button":
                                        string[] ButtonSegments = CleanUp.Split('(')[1].Split(',');
                                        Export = CleanUp.Split('(')[1];
                                        for(int i = 0; i < ButtonSegments.Length; i++)
                                        {
                                            ButtonSegments[i] = ButtonSegments[i].Trim();
                                        }
                                        int ButtonX = int.Parse(ButtonSegments[0]);
                                        int ButtonY = int.Parse(ButtonSegments[1]);
                                        int ButtonWidth = int.Parse(ButtonSegments[2]);
                                        int ButtonHeight = int.Parse(ButtonSegments[3]);
                                        int ButtonColor = int.Parse(ButtonSegments[4]);
                                        string ButtonText = ButtonSegments[5].Remove(ButtonSegments[5].Length - 1).Remove(0, 1);
                                        string ButtonID = ButtonSegments[6].Remove(ButtonSegments[6].Length - 2);
                                        ButtonID = ButtonID.Remove(0, 1);
                                        UIElements.Add(new Button(ButtonX, ButtonY, ButtonWidth, ButtonHeight, ButtonText, ButtonColor, ButtonID));
                                        break;
                                    case "Label":
                                        string[] LabelSegments = CleanUp.Split('(')[1].Split(',');
                                        Export = CleanUp.Split('(')[1];
                                        for (int i = 0; i < LabelSegments.Length; i++)
                                        {
                                            LabelSegments[i] = LabelSegments[i].Trim();
                                        }
                                        int LabelX = int.Parse(LabelSegments[0]);
                                        int LabelY = int.Parse(LabelSegments[1]);
                                        int LabelColor = int.Parse(LabelSegments[2]);
                                        string LabelText = LabelSegments[3].Remove(LabelSegments[3].Length - 1).Remove(0, 1);
                                        string LabelID = LabelSegments[4].Remove(LabelSegments[4].Length - 2);
                                        LabelID = LabelID.Remove(0, 1);
                                        UIElements.Add(new label(LabelX, LabelY, LabelText, LabelColor, LabelID));
                                        break;
                                    case "TextBox":
                                        string[] TextBoxSegments = CleanUp.Split('(')[1].Split(',');
                                        Export = CleanUp.Split('(')[1];
                                        for (int i = 0; i < TextBoxSegments.Length; i++)
                                        {
                                            TextBoxSegments[i] = TextBoxSegments[i].Trim();
                                        }
                                        int TextBoxX = int.Parse(TextBoxSegments[0]);
                                        int TextBoxY = int.Parse(TextBoxSegments[1]);
                                        int TextBoxWidth = int.Parse(TextBoxSegments[2]);
                                        int TextBoxHeight = int.Parse(TextBoxSegments[3]);
                                        int TextBoxColor = int.Parse(TextBoxSegments[4]);
                                        string TextBoxText = TextBoxSegments[5].Remove(TextBoxSegments[5].Length - 1).Remove(0, 1);
                                        string TextBoxPlaceHolder = TextBoxSegments[6].Remove(TextBoxSegments[6].Length - 1).Remove(0, 1);
                                        string TextBoxID = TextBoxSegments[7].Remove(TextBoxSegments[7].Length - 2);
                                        TextBoxID = TextBoxID.Remove(0, 1);
                                        UIElements.Add(new TextBox(TextBoxX, TextBoxY, TextBoxWidth, TextBoxHeight, TextBoxColor, TextBoxText, TextBoxPlaceHolder, TextBox.Options.left, TextBoxID));
                                        break;
                                    case "CheckBox":
                                        string[] CheckBoxSegments = CleanUp.Split('(')[1].Split(',');
                                        for (int i = 0; i < CheckBoxSegments.Length; i++)
                                        {
                                            CheckBoxSegments[i] = CheckBoxSegments[i].Trim();
                                        }
                                        int CheckBoxX = int.Parse(CheckBoxSegments[0]);
                                        int CheckBoxY = int.Parse(CheckBoxSegments[1]);
                                        int CheckBoxWidth = int.Parse(CheckBoxSegments[2]);
                                        int CheckBoxHeight = int.Parse(CheckBoxSegments[3]);
                                        bool CheckBoxChecked = bool.Parse(CheckBoxSegments[4]);
                                        string CheckBoxText = CheckBoxSegments[5].Remove(CheckBoxSegments[5].Length - 1).Remove(0, 1);
                                        string CheckBoxID = CheckBoxSegments[6].Remove(CheckBoxSegments[6].Length - 2);
                                        CheckBoxID = CheckBoxID.Remove(0, 1);
                                        UIElements.Add(new CheckBox(CheckBoxX, CheckBoxY, CheckBoxWidth, CheckBoxHeight, CheckBoxChecked, CheckBoxID, CheckBoxText));
                                        break;
                                    case "Slider":
                                        string[] SliderSegments = CleanUp.Split('(')[1].Split(',');
                                        for (int i = 0; i < SliderSegments.Length; i++)
                                        {
                                            SliderSegments[i] = SliderSegments[i].Trim();
                                        }
                                        int SliderX = int.Parse(SliderSegments[0]);
                                        int SliderY = int.Parse(SliderSegments[1]);
                                        int SliderWidth = int.Parse(SliderSegments[2]);
                                        int SliderMinVal = int.Parse(SliderSegments[3]);
                                        int SliderMaxVal = int.Parse(SliderSegments[4]);
                                        int SliderValue = int.Parse(SliderSegments[5]);
                                        string SliderID = SliderSegments[6].Remove(SliderSegments[6].Length - 2);
                                        SliderID = SliderID.Remove(0, 1);
                                        UIElements.Add(new Slider(SliderX, SliderY, SliderWidth, SliderMinVal, SliderMaxVal, SliderValue, SliderID));
                                        break;
                                    case "Table"://Implemented by Github Copilot thingy, I'm not trusting it, so it's up for testing
                                        string[] TableSegments = CleanUp.Split('(')[1].Split(',');
                                        for (int i = 0; i < TableSegments.Length; i++)
                                        {
                                            TableSegments[i] = TableSegments[i].Trim();
                                        }
                                        int TableX = int.Parse(TableSegments[0]);
                                        int TableY = int.Parse(TableSegments[1]);
                                        int TableWidth = int.Parse(TableSegments[2]);
                                        int TableHeight = int.Parse(TableSegments[3]);
                                        int CellWidth = int.Parse(TableSegments[4]);
                                        int CellHeight = int.Parse(TableSegments[5]);
                                        string TableID = TableSegments[6].Remove(TableSegments[6].Length - 2);
                                        TableID = TableID.Remove(0, 1);
                                        UIElements.Add(new Table(TableX, TableY + 22, TableWidth, TableHeight, CellWidth, CellHeight, TableID));
                                        break;
                                    case "PictureBox"://This Element is untested, since I had no spare image in the vmdk to try
                                        string s = "";
                                        try
                                        {
                                            string[] PictureBoxSegments = CleanUp.Split('(')[1].Split(',');
                                            for (int i = 0; i < PictureBoxSegments.Length; i++)
                                            {
                                                PictureBoxSegments[i] = PictureBoxSegments[i].Trim();
                                            }
                                            int PictureBoxX = int.Parse(PictureBoxSegments[0]);
                                            int PictureBoxY = int.Parse(PictureBoxSegments[1]);
                                            bool PictureBoxVisible = bool.Parse(PictureBoxSegments[2]);
                                            string PictureBoxPath = PictureBoxSegments[3].Remove(PictureBoxSegments[3].Length - 1).Remove(0, 1);
                                            string PictureBoxID = PictureBoxSegments[4].Remove(PictureBoxSegments[4].Length - 2);
                                            switch (File.Exists(PictureBoxPath))
                                            {
                                                case true:
                                                    UIElements.Add(new PictureBox(PictureBoxX, PictureBoxY, PictureBoxID, true, new Bitmap(PictureBoxPath)));
                                                    break;
                                                case false:
                                                    if (ElementType.Length > 1)
                                                    {
                                                        UIElements.Find(d => d.ID == "debug").Text = "No such file!";
                                                    }
                                                    break;
                                            }
                                        }
                                        catch(Exception ex)
                                        {
                                            throw new Exception(ex.Message + "\n" + s);
                                        }
                                        break;
                                }
                                break;
                        }
                    }
                }
                #endregion Stage2

                initial = false;
                once = true;
            }
            if (once == true)
            {
                Bitmap back = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                (canvas, back, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                once = false;
            }

            foreach (UIElementHandler UIElement in UIElements)
            {
                UIElement.Render(window);
            }

            ImprovedVBE.DrawImageAlpha(window, x, y, RenderTo);
        }

        public void RightClick()
        {

        }
    }
}