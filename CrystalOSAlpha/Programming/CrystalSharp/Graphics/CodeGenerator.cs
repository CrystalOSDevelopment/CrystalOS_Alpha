using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.SystemApps;
using CrystalOSAlpha.UI_Elements;
using System.Collections.Generic;

namespace CrystalOSAlpha.Programming.CrystalSharp.Graphics
{
    class CodeGenerator
    {
        public static string GenerateBase(string Number)
        {
            string code = "Main" + Number + "\n" +
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
                    code = code.Insert(LatsSemiColon + 2, "AddElement(new PictureBox(" + UI.X + ", " + UI.Y + ", " + UI.Width + ", " + UI.Height + ", " + visible + ", \"" + Extension + "\", \"" + UI.ID + "\");\n");
                    return code;
                case TypeOfElement.CheckBox:
                    LatsSemiColon = code.LastIndexOf(';');
                    code = code.Insert(LatsSemiColon + 2, "AddElement(new CheckBox(" + UI.X + ", " + UI.Y + ", " + UI.Width + ", " + UI.Height + ", " + UI.Clicked + ", \"" + UI.Text + "\", \"" + UI.ID + "\");\n");//In this case Clicked is used as the value of the checkbox
                    return code;
                default:
                    return code;
            }
        }

        public static string ModifyUIElement(string code, UIElementHandler Data)
        {
            //Separate the code by lines
            string[] lines = code.Split('\n');

            //Find all the lines that have "Button" in them
            List<string> Found = ToList(lines).FindAll(Item => Item.Contains("AddElement(new " + Data.GetValue(0, 0).Split(".")[0] + "("));
            //Find the line that has the ID of the button
            for(int Height = 0; Height < 10; Height++)//Writing 10 here because a: I cannot acces to TableHeight and b: It'll stop anyways when it finds the ID
            {
                if (Data.GetValue(0, Height).Split(".")[1].Contains("ID"))
                {
                    //Find the line that has the ID of the button
                    string ID = Data.GetValue(1, Height);
                    string Element = Found.Find(Item => Item.Remove(Item.Length - 2).EndsWith("\"" + ID + "\""));
                    //Find the index of the line to modify
                    int Index = ToList(lines).FindIndex(Item => Item == Element);
                    //Set the new value
                    //Extract type of element
                    if(Index >= 0)
                    {
                        switch(Data.GetValue(0, 0).Split(".")[0])
                        {
                            case "Button":
                                lines[Index] = "AddElement(new Button(" + Data.GetValue(1, 0) + ", " + Data.GetValue(1, 1) + ", " + Data.GetValue(1, 2) + ", " + Data.GetValue(1, 3) + ", " + Data.GetValue(1, 4) + ", \"" + Data.GetValue(1, 5) + "\", \"" + Data.GetValue(1, 6) + "\");";
                                break;
                            case "Label":
                                lines[Index] = "AddElement(new Label(" + Data.GetValue(1, 0) + ", " + Data.GetValue(1, 1) + ", " + Data.GetValue(1, 2) + ", \"" + Data.GetValue(1, 3) + "\", \"" + Data.GetValue(1, 4) + "\");";
                                break;
                            case "TextBox":
                                lines[Index] = "AddElement(new TextBox(" + Data.GetValue(1, 0) + ", " + Data.GetValue(1, 1) + ", " + Data.GetValue(1, 2) + ", " + Data.GetValue(1, 3) + ", " + Data.GetValue(1, 4) + ", \"" + Data.GetValue(1, 5) + "\", \"" + Data.GetValue(1, 6) + "\", \"" + Data.GetValue(1, 7) + "\");";
                                break;
                            case "Slider":
                                lines[Index] = "AddElement(new Slider(" + Data.GetValue(1, 0) + ", " + Data.GetValue(1, 1) + ", " + Data.GetValue(1, 2) + ", " + Data.GetValue(1, 4) + ", " + Data.GetValue(1, 5) + ", " + Data.GetValue(1, 6) + ", \"" + Data.GetValue(1, 7) + "\");";
                                break;
                            case "PictureBox":
                                lines[Index] = "AddElement(new PictureBox(" + Data.GetValue(1, 0) + ", " + Data.GetValue(1, 1) + ", " + Data.GetValue(1, 2) + ", " + Data.GetValue(1, 3) + ", " + Data.GetValue(1, 6) + ", \"" + Data.GetValue(1, 4) + "\", \"" + Data.GetValue(1, 5) + "\");";
                                break;
                        }
                    }
                    
                    Height = 99;
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

        public static List<string> ToList(string[] content)
        {
            List<string> parts = new List<string>();
            foreach (string s in content)
            {
                parts.Add(s);
            }
            return parts;
        }
    }
}