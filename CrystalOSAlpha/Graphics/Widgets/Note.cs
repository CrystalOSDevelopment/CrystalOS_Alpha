using Cosmos.System.Graphics;
using Cosmos.System;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.TaskBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalOSAlpha.UI_Elements;
using CrystalOSAlpha.Applications.Calculator;
using System.Drawing;
using System.Reflection.Metadata;
using Cosmos.HAL.Drivers.Video.SVGAII;
using CrystalOSAlpha.Applications;

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

        public List<Button_prop> Buttons = new List<Button_prop>();
        public List<Dropdown> dropdowns = new List<Dropdown>();
        public List<values> value = new List<values>();

        public void App()
        {
            string output = "Conversion tool";
            if(initial == true)
            {
                Buttons.Add(new Button_prop((200 - sizeDec) / 2 - 40, 130, 80, 20, "Convert", 1));
                Dropdown d = new Dropdown(58, 30, 100, 20, "First");
                dropdowns.Add(d);

                Dropdown c = new Dropdown(58, 80, 100, 20, "Second");
                dropdowns.Add(c);

                value.Add(new values(false, "Mile", "First"));
                value.Add(new values(true, "Hour", "First"));
                value.Add(new values(false, "Kilometre", "First"));
                value.Add(new values(false, "Minute", "First"));

                value.Add(new values(false, "Kilometre", "Second"));
                value.Add(new values(true, "Minute", "Second"));
                value.Add(new values(false, "Mile", "Second"));
                value.Add(new values(false, "Hour", "Second"));

                initial = false;
            }
            if (Get_Back == true)
            {
                bool extraction = Buttons[0].Clicked;
                Buttons[0] = new Button_prop((200 - sizeDec) / 2 - 40, 130, 80, 20, "Convert", 1);
                Back = Base.Widget_Back(200 - sizeDec, 200 - sizeDec, ImprovedVBE.colourToNumber(255, 255, 255));
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
                        Button.Button_render(Back, button.X, button.Y, button.Width, button.Height, Color.White.ToArgb(), button.Text);

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
                        Button.Button_render(Back, button.X, button.Y, button.Width, button.Height, button.Color, button.Text);
                    }
                    if (MouseManager.MouseState == MouseState.None)
                    {
                        button.Clicked = false;
                    }
                }

                BitFont.DrawBitFontString(Back, "ArialCustomCharset16", Global_integers.c, "From: ", 10, 30);
                TextBox.Box(Back, 10, 55, 180 - sizeDec, 20, ImprovedVBE.colourToNumber(60, 60, 60), input, "Input", TextBox.Options.left);
                BitFont.DrawBitFontString(Back, "ArialCustomCharset16", Global_integers.c, "To: ", 10, 80);
                TextBox.Box(Back, 10, 105, 180 - sizeDec, 20, ImprovedVBE.colourToNumber(60, 60, 60), result, "Output", TextBox.Options.left);
                BitFont.DrawBitFontString(Back, "ArialCustomCharset16", Global_integers.c, output, ((100 - sizeDec / 2) - output.Length * 4), 3);
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
                        Dropd.Draw(Back, value);
                    }
                    ind++;
                }
                Get_Back = false;
            }

            #region Mechanical
            if (TaskScheduler.counter == TaskScheduler.Apps.Count - 1 && TaskManager.MenuOpened == false)
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
                Dropd.Render(x, y);
            }

            ImprovedVBE.DrawImageAlpha(Back, x, y, ImprovedVBE.cover);
            if (MouseManager.MouseState == MouseState.None && clicked == true)
            {
                clicked = false;
                Get_Back = true;
            }

            if (MouseManager.MouseState == MouseState.Left)
            {
                if (((MouseManager.X > x && MouseManager.X < x + Back.Width) && (MouseManager.Y > y && MouseManager.Y < y + Back.Height)) || mem == false)
                {
                    if (mem == true)
                    {
                        x_dif = (int)MouseManager.X - x;
                        y_dif = (int)MouseManager.Y - y;
                        mem = false;
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
                        y = SideNav.start_y;
                    }
                }
                SideNav.start_y += (int)Back.Height + 20;
                if (mem == false)
                {
                    x = (int)MouseManager.X - x_dif;
                    y = (int)MouseManager.Y - y_dif;
                }
            }
            else
            {
                if (x + Back.Width > ImprovedVBE.width - 200)
                {
                    x = SideNav.X + 15;
                    y = SideNav.start_y;
                    SideNav.start_y += (int)Back.Height + 20;
                }
            }
            if (mem == false && MouseManager.MouseState == MouseState.None)
            {
                mem = true;
                Get_Back = true;
            }
            #endregion Graphical
        }

        public void RightClick()
        {

        }
    }
}
