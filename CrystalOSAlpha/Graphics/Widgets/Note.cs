using Cosmos.System.Graphics;
using Cosmos.System;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.TaskBar;
using System.Collections.Generic;
using CrystalOSAlpha.UI_Elements;
using System.Drawing;
using CrystalOSAlpha.Applications;
using CrystalOSAlpha.System32;
using Kernel = CrystalOS_Alpha.Kernel;

namespace CrystalOSAlpha.Graphics.Widgets
{
    class Note : App
    {
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public int desk_ID {get; set; }
        public int AppID { get; set; }
        public string name {get; set; }
        public bool minimised { get; set; }
        public bool movable { get; set; }
        public bool once { get; set; }
        public Bitmap icon { get; set; }
        public Bitmap window { get; set; }

        public int memory = 0;
        public int x_dif = 10;
        public int y_dif = 10;
        public static int sizeDec = 0;

        public bool mem = true;
        public bool Get_Back = true;
        public bool initial = true;
        public bool clicked = false;
        public bool Which = true;

        public string a = "";
        public string b = "";
        public string input = "";
        public string result = "";
        
        public Bitmap Back;

        public List<Button> Buttons = new List<Button>();
        public List<Dropdown> dropdowns = new List<Dropdown>();
        public List<values> value = new List<values>();

        public void App()
        {
            string output = "Conversion tool";
            if(initial == true)
            {
                Buttons.Add(new Button((200 - sizeDec) / 2 - 40, 130, 80, 20, "Convert", 1));
                value.Add(new values(false, "Mile", "First"));
                value.Add(new values(true, "Hour", "First"));
                value.Add(new values(false, "Kilometre", "First"));
                value.Add(new values(false, "Minute", "First"));

                value.Add(new values(false, "Kilometre", "Second"));
                value.Add(new values(true, "Minute", "Second"));
                value.Add(new values(false, "Mile", "Second"));
                value.Add(new values(false, "Hour", "Second"));

                Dropdown d = new Dropdown(58, 30, 100, 20, "First", value);
                dropdowns.Add(d);

                Dropdown c = new Dropdown(58, 80, 100, 20, "Second", value);
                dropdowns.Add(c);

                initial = false;
            }
            if (Get_Back == true)
            {
                if (x >= ImprovedVBE.width)
                {
                    sizeDec = 40;
                }
                bool extraction = Buttons[0].Clicked;
                Buttons[0] = new Button((200 - sizeDec) / 2 - 40, 130, 80, 20, "Convert", 1);
                Back = Base.Widget_Back(200 - sizeDec, 200 - sizeDec, ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B));
                Back = ImprovedVBE.EnableTransparency(Back, x, y, Back);

                if(MouseManager.MouseState == MouseState.Left && extraction == true)
                {
                    Buttons[0].Clicked = extraction;
                }
                else
                {
                    Buttons[0].Clicked = false;
                }
                foreach (var button in Buttons)
                {
                    if (button.Clicked == true)
                    {
                        int Col = button.Color;
                        button.Color = Color.White.ToArgb();
                        button.Render(Back);
                        button.Color = Col;

                        switch (button.Text)
                        {
                            case "Convert":
                                //Sketchy calculations... GO!
                                string f = "";
                                string s = "";
                                foreach(var v in value)
                                {
                                    if(v.ID == dropdowns[0].ID && v.Highlighted == true)
                                    {
                                        f = v.content;
                                    }
                                    if (v.ID == dropdowns[1].ID && v.Highlighted == true)
                                    {
                                        s = v.content;
                                    }
                                }
                                switch (f)
                                {
                                    case "Mile":
                                        switch(s)
                                        {
                                            case "Kilometre":
                                                double num = double.Parse(input) * 1.609344;
                                                result = num.ToString();
                                                break;
                                        }
                                        break;
                                    case "Kilometre":
                                        switch (s)
                                        {
                                            case "Mile":
                                                double num = double.Parse(input) * 0.62137119223733;
                                                result = num.ToString();
                                                break;
                                        }
                                        break;
                                    case "Hour":
                                        switch (s)
                                        {
                                            case "Minute":
                                                double num = double.Parse(input) * 60;
                                                result = num.ToString();
                                                break;
                                        }
                                        break;
                                    case "Minute":
                                        switch (s)
                                        {
                                            case "Hour":
                                                double num = double.Parse(input) / 60;
                                                result = num.ToString();
                                                break;
                                        }
                                        break;
                                }
                                break;
                        }
                    }
                    else
                    {
                        button.Render(Back);
                    }
                    if (MouseManager.MouseState == MouseState.None)
                    {
                        button.Clicked = false;
                    }
                }

                BitFont.DrawBitFontString(Back, "ArialCustomCharset16", GlobalValues.c, "From: ", 10, 30);
                TextBox tb1 = new TextBox(10, 55, 180 - sizeDec, 20, ImprovedVBE.colourToNumber(60, 60, 60), input, "Input", TextBox.Options.left, "TextBox1");
                tb1.Render(Back);
                BitFont.DrawBitFontString(Back, "ArialCustomCharset16", GlobalValues.c, "To: ", 10, 80);
                TextBox tb2 = new TextBox(10, 105, 180 - sizeDec, 20, ImprovedVBE.colourToNumber(60, 60, 60), result, "Output", TextBox.Options.left, "textBox2");
                tb2.Render(Back);
                BitFont.DrawBitFontString(Back, "ArialCustomCharset16", GlobalValues.c, output, ((100 - sizeDec / 2) - output.Length * 4), 3);
                int ind = 0;
                foreach (var Dropd in dropdowns)
                {
                    bool render = true;
                    if(MouseManager.MouseState == MouseState.Left)
                    {
                        if (MouseManager.X > x + Dropd.X + (Dropd.canv.Width - 30) && MouseManager.X < x + Dropd.X + Dropd.canv.Width)
                        {
                            if (MouseManager.Y > y + Dropd.Y && MouseManager.Y < y + Dropd.Y + Dropd.Height)
                            {
                                Dropdown d = Dropd;
                                dropdowns.RemoveAt(ind);
                                dropdowns.Add(d);
                                if (clicked == false)
                                {
                                    if (Dropd.Clicked == true)
                                    {
                                        Dropd.Clicked = false;
                                    }
                                    else
                                    {
                                        Dropd.Clicked = true;
                                        render = false;

                                    }
                                    clicked = true;
                                }
                            }
                        }
                    }
                    if (render == true)
                    {
                        Dropd.Render(Back);
                    }
                    ind++;
                }
                Get_Back = false;
            }

            #region Mechanical
            if (TaskScheduler.Apps[^1] == this && TaskManager.MenuOpened == false && Kernel.Is_KeyboardMouse == false)
            {
                KeyEvent k;
                if (KeyboardManager.TryReadKey(out k))
                {
                    if (k.Key == ConsoleKeyEx.Enter)
                    {
                        Buttons[0].Clicked = true;
                        clicked = true;
                        Get_Back = true;
                    }
                    else
                    {
                        input = Keyboard.HandleKeyboard(input, k);
                        Get_Back = true;
                    }
                }
            }
            #endregion Mechanical

            #region Graphical
            foreach (var button in Buttons)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if (MouseManager.X > x + button.X && MouseManager.X < x + button.X + button.Width)
                    {
                        if (MouseManager.Y > y + button.Y && MouseManager.Y < y + button.Y + button.Height)
                        {
                            if (clicked == false)
                            {
                                button.Clicked = true;
                                Get_Back = true;
                                clicked = true;
                            }
                        }
                    }
                }
            }

            foreach (var Dropd in dropdowns)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if (MouseManager.X > x + Dropd.X + (Dropd.canv.Width - 30) && MouseManager.X < x + Dropd.X + Dropd.canv.Width)
                    {
                        if (MouseManager.Y > y + Dropd.Y && MouseManager.Y < y + Dropd.Y + Dropd.Height)
                        {
                            Dropdown d = Dropd;
                            dropdowns.Remove(Dropd);
                            dropdowns.Add(d);
                            if (clicked == false)
                            {
                                if (Dropd.Clicked == true)
                                {
                                    Dropd.Clicked = false;
                                }
                                else
                                {
                                    Dropd.Clicked = true;

                                }
                                clicked = true;
                            }
                        }
                    }
                }
                if(Dropd.Clicked != false)
                {
                    if (MouseManager.X > x + Dropd.X && MouseManager.X < x + Dropd.X + Dropd.canv.Width - 30)
                    {
                        if (MouseManager.Y > y + Dropd.Y + Dropd.Height && MouseManager.Y < y + Dropd.Y + Dropd.Height + 100)
                        {
                            int top = (int)(MouseManager.Y - y - Dropd.Y - Dropd.Height);
                            int discardable = 0;
                            int select = 1;
                            if (top < 20)
                            {
                                select = 1;
                            }
                            else if (top > 20 && top < 40)
                            {
                                select = 2;
                            }
                            else if (top > 40 && top < 60)
                            {
                                select = 3;
                            }
                            else if (top > 60 && top < 80)
                            {
                                select = 4;
                            }
                            if(select != memory)
                            {
                                foreach (var val in value)
                                {
                                    if (val.ID == Dropd.ID)
                                    {
                                        val.Highlighted = false;
                                        discardable++;
                                    }
                                    if (discardable == select && val.ID == Dropd.ID)
                                    {
                                        if (val.Highlighted == false)
                                        {
                                            val.Highlighted = true;
                                            Get_Back = true;
                                        }
                                    }
                                }
                                memory = select;
                            }
                            
                            if(MouseManager.MouseState == MouseState.Left)
                            {
                                Dropd.Clicked = false;
                            }
                        }
                    }
                }
                //Dropd.Render(x, y);
            }

            if (ImprovedVBE.RequestRedraw || SideNav.RequestDrawLocal == true)
            {
                ImprovedVBE.DrawImageAlpha(Back, x, y, ImprovedVBE.cover);
            }
            //ImprovedVBE.DrawImageAlpha(Back, x, y, ImprovedVBE.cover);
            if (MouseManager.MouseState == MouseState.None && clicked == true)
            {
                clicked = false;
                Get_Back = true;
            }

            switch (MouseManager.MouseState)
            {
                case MouseState.Left:
                    if ((MouseManager.X > x && MouseManager.X < x + Back.Width) && (MouseManager.Y > y && MouseManager.Y < y + Back.Height))
                    {
                        if (mem == false)
                        {
                            x_dif = (int)MouseManager.X - x;
                            y_dif = (int)MouseManager.Y - y;
                            mem = true;
                        }
                        x = (int)MouseManager.X - x_dif;
                        y = (int)MouseManager.Y - y_dif;
                        if (x + Back.Width > ImprovedVBE.width - 200)
                        {
                            if (sizeDec < 40)
                            {
                                Back = ImprovedVBE.ScaleImageStock(Back, (uint)(Back.Width - sizeDec), (uint)(Back.Height - sizeDec));
                                sizeDec += 10;
                                Get_Back = true;
                            }
                        }
                        else
                        {
                            if (sizeDec > 0)
                            {
                                Back = ImprovedVBE.ScaleImageStock(Back, (uint)(Back.Width - sizeDec), (uint)(Back.Height - sizeDec));
                                sizeDec -= 10;
                                Get_Back = true;
                            }
                        }
                    }
                    else
                    {
                        if (x + Back.Width > ImprovedVBE.width - 200)
                        {
                            x = SideNav.X + 15;
                        }
                    }
                    if (mem == true)
                    {
                        x = (int)MouseManager.X - x_dif;
                        y = (int)MouseManager.Y - y_dif;
                    }
                    break;
                default:
                    if (x + Back.Width > ImprovedVBE.width - 200)
                    {
                        x = SideNav.X + 15;
                        y = (int)(TaskScheduler.Apps.IndexOf(this) * (Back.Height + 20) + 80);
                    }
                    if (mem == true)
                    {
                        mem = false;
                        Get_Back = true;
                    }
                    break;
            }
            #endregion Graphical
        }

        public void RightClick()
        {

        }
    }
}
