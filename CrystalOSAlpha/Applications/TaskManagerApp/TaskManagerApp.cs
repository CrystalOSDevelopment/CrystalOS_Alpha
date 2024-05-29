using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;

namespace CrystalOSAlpha.Applications.TaskManagerApp
{
    class TaskManagerApp : App
    {
        public TaskManagerApp(int X, int Y, int Z, int Width, int Height, string Name, Bitmap Icon)
        {
            this.x = X;
            this.y = Y;
            this.z = Z;
            this.width = Width;
            this.height = Height;
            this.name = Name;
            this.icon = Icon;
        }

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
        public bool once { get; set; }
        public Bitmap icon { get; set; }
        public Bitmap window { get; set; }
        public Bitmap canvas;
        public Bitmap back_canvas;

        public bool initial = true;
        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);
        public List<UIElementHandler> Elements = new List<UIElementHandler>();
        public int i = 0;
        public void App()
        {
            width = 355;
            height = 300;
            if (initial == true)
            {
                //Add UI elements here
                Elements.Add(new Button(3, 2, 90, 22, "Terminate", 1, "Term"));

                //Table
                Elements.Add(new Table(3, 49, width - 6, height - 52, 2, 10, "Test"));
                i = Elements.FindIndex(d => d.ID == "Test");
                Elements[i].SetValue(0, 0, "Process name", true);
                Elements[i].SetValue(1, 0, "ID", true);
                once = true;
                initial = false;
            }
            if (once == true)
            {
                (canvas, back_canvas, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                once = false;
            }
            foreach (var Element in Elements)
            {
                if (TaskScheduler.Apps.FindAll(d => d.name == "Error!").Count == 0)
                {
                    if(Element.TypeOfElement == TypeOfElement.Table)
                    {
                        //Clears the table starting from the second row
                        for(int k = 1; k < 10; k++)
                        {
                            for(int j = 0; j < 2; j++)
                            {
                                Elements[i].SetValue(j, k, "", true);
                            }
                        }
                        //Fills up the table with data
                        int A = 1;
                        for (int j = 0; j < TaskScheduler.Apps.Count; j++)
                        {
                            Elements[i].SetValue(0, A, "", true);
                            Elements[i].SetValue(1, A, "", true);
                            if (TaskScheduler.Apps[j].name != null)
                            {
                                Elements[i].SetValue(0, A, TaskScheduler.Apps[j].name, true);
                                Elements[i].SetValue(1, A, TaskScheduler.Apps[j].AppID.ToString(), true);
                                A++;
                            }
                        }
                    }
                    Element.Render(window);
                    if (MouseManager.MouseState == MouseState.Left)
                    {
                        if (MouseManager.X > x + Element.X && MouseManager.X < x + Element.X + Element.Width)
                        {
                            if (MouseManager.Y > y + Element.Y && MouseManager.Y < y + Element.Y + Element.Height)
                            {
                                switch (Element.TypeOfElement)
                                {
                                    case TypeOfElement.Button:
                                        if(Element.Clicked == false)
                                        {
                                            switch (Element.ID)
                                            {
                                                case "Term":
                                                    string SelectedCell = Elements[i].Text;
                                                    string[] coords = SelectedCell.Split(',');
                                                    if(int.Parse(coords[1]) > 0)
                                                    {
                                                        string Received = Elements[i].GetValue(int.Parse(coords[0]), int.Parse(coords[1]));
                                                        if (!int.TryParse(Received, out int result))
                                                        {
                                                            Received = Elements[i].GetValue(int.Parse(coords[0]) + 1, int.Parse(coords[1]));
                                                        }
                                                        int index = TaskScheduler.Apps.FindIndex(d => d.AppID.ToString() == Received);
                                                        TaskScheduler.Apps.RemoveAt(index);
                                                    }
                                                    break;

                                            }
                                        }
                                        int Col = Element.Color;
                                        Element.Color = ImprovedVBE.colourToNumber(255, 255, 255);
                                        Element.Render(window);
                                        Element.Clicked = true;
                                        Element.Color = Col;
                                        break;
                                    case TypeOfElement.Table:
                                        Element.CheckClick(x, y);
                                        break;
                                }
                            }
                        }
                    }
                    else if (Element.TypeOfElement != TypeOfElement.TextBox)
                    {
                        Element.Clicked = false;
                    }
                }
            }
            ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
        }

        public void RightClick()
        {
            
        }
    }
}