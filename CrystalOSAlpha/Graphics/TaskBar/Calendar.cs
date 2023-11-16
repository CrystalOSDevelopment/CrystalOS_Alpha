using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOSAlpha.Graphics.TaskBar
{
    class Calendar
    {
        public static Bitmap render;
        public static bool get_Render = true;
        public static string DayOfWeek = "";
        public static bool clicked = false;

        public static List<Button_prop> Buttons = new List<Button_prop>();

        public static bool initial = true;

        public static int Month = DateTime.Now.Month;
        public static int Year = DateTime.Now.Year;
        public static string Month_In_String = "";
        public static void Calendar_Widget(int X, int Y)
        {
            if (get_Render == true)
            {
                if(initial == true)
                {
                    Buttons.Clear();
                    Buttons.Add(new Button_prop(280, 270, 20, 20, "<", 1));
                    Buttons.Add(new Button_prop(310, 270, 20, 20, ">", 1));
                    initial = false;
                }

                switch (Month)
                {
                    case 1:
                        Month_In_String = "January";
                        break;
                    case 2:
                        Month_In_String = "February";
                        break;
                    case 3:
                        Month_In_String = "March";
                        break;
                    case 4:
                        Month_In_String = "April";
                        break;
                    case 5:
                        Month_In_String = "May";
                        break;
                    case 6:
                        Month_In_String = "June";
                        break;
                    case 7:
                        Month_In_String = "July";
                        break;
                    case 8:
                        Month_In_String = "August";
                        break;
                    case 9:
                        Month_In_String = "September";
                        break;
                    case 10:
                        Month_In_String = "October";
                        break;
                    case 11:
                        Month_In_String = "November";
                        break;
                    case 12:
                        Month_In_String = "December";
                        break;
                }

                render = Widgets.Base.Widget_Back(340, 320, ImprovedVBE.colourToNumber(Global_integers.R, Global_integers.G, Global_integers.B));
                render = ImprovedVBE.DrawImageAlpha2(render, X, Y, render);
                BitFont.DrawBitFontString(render, "ArialCustomCharset16", Color.White, Year + ". " + Month_In_String, 20, 15);

                //For loop the dates

                var firstDay = new DateTime(Year, Month, 1);

                int x_index = 300 / 7 + 4;
                int y_index = 40;

                for (int i = 0; i < 7; i++)
                {
                    switch (i)
                    {
                        case 0:
                            DayOfWeek = "Monday";
                            break;
                        case 1:
                            DayOfWeek = "Tuesday";
                            break;
                        case 2:
                            DayOfWeek = "Wednesday";
                            break;
                        case 3:
                            DayOfWeek = "Thursday";
                            break;
                        case 4:
                            DayOfWeek = "Friday";
                            break;
                        case 5:
                            DayOfWeek = "Saturday";
                            break;
                        case 6:
                            DayOfWeek = "Sunday";
                            break;
                    }
                    BitFont.DrawBitFontString(render, "ArialCustomCharset16", Color.White, DayOfWeek.Remove(3), x_index - 20, y_index);
                    x_index += 300 / 7 + 4;
                }

                x_index = 300 / 7 + 4;
                y_index += 40;

                switch (firstDay.DayOfWeek)
                {
                    case System.DayOfWeek.Monday:
                        DayOfWeek = "Monday";
                        break;
                    case System.DayOfWeek.Tuesday:
                        DayOfWeek = "Tuesday";
                        x_index *= 2;
                        break;
                    case System.DayOfWeek.Wednesday:
                        DayOfWeek = "Wednesday";
                        x_index *= 3;
                        break;
                    case System.DayOfWeek.Thursday:
                        DayOfWeek = "Thursday";
                        x_index *= 4;
                        break;
                    case System.DayOfWeek.Friday:
                        DayOfWeek = "Friday";
                        x_index *= 5;
                        break;
                    case System.DayOfWeek.Saturday:
                        DayOfWeek = "Saturday";
                        x_index *= 6;
                        break;
                    case System.DayOfWeek.Sunday:
                        DayOfWeek = "Sunday";
                        x_index *= 7;
                        break;
                }

                for (int i = 0; i < DateTime.DaysInMonth(firstDay.Year, firstDay.Month); i++)
                {
                    if(i + 1 == DateTime.UtcNow.Day)
                    {
                        ImprovedVBE.DrawFilledEllipse(render, x_index - 13, y_index + 10, 17, 17, ImprovedVBE.colourToNumber(0, 20, 200));
                    }
                    BitFont.DrawBitFontString(render, "ArialCustomCharset16", Color.White, (i + 1).ToString(), x_index - 20, y_index);
                    if (x_index + 300 / 7 > 320)
                    {
                        y_index += 40;
                        x_index = 300 / 7;
                    }
                    else
                    {
                        x_index += 300 / 7 + 4;
                    }
                }

                foreach (var button in Buttons)
                {
                    if (button.Clicked == true)
                    {
                        Button.Button_render(render, button.X, button.Y, button.Width, button.Height, Color.White.ToArgb(), button.Text);
                        switch (button.Text)
                        {
                            case ">":
                                get_Render = true;
                                break;
                            case "<":
                                get_Render = true;
                                break;
                        }
                        get_Render = true;
                    }
                    else
                    {
                        Button.Button_render(render, button.X, button.Y, button.Width, button.Height, button.Color, button.Text);
                    }
                    if (MouseManager.MouseState == MouseState.None)
                    {
                        button.Clicked = false;
                    }
                }

                get_Render = false;
            }

            foreach (var button in Buttons)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if(MouseManager.X > X + button.X && MouseManager.X < X + button.X + button.Width)
                    {
                        if(MouseManager.Y > Y + button.Y && MouseManager.Y < Y + button.Y + button.Y)
                        {
                            if(clicked == false)
                            {
                                switch (button.Text)
                                {
                                    case ">":
                                        get_Render = true;
                                        clicked = true;
                                        button.Clicked = true;
                                        if(Month < 12)
                                        {
                                            Month++;
                                        }
                                        else
                                        {
                                            Year++;
                                        }
                                        break;
                                    case "<":
                                        get_Render = true;
                                        clicked = true;
                                        button.Clicked = true;
                                        if (Month == 1)
                                        {
                                            Year--;
                                        }
                                        else
                                        {
                                            Month--;
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    if(button.Clicked == true)
                    {
                        get_Render = true;
                    }
                }
                else
                {

                }
                if (MouseManager.MouseState == MouseState.None && clicked == true)
                {
                    button.Clicked = false;
                    clicked = false;
                    get_Render = true;
                }
            }

            ImprovedVBE.DrawImageAlpha(render, X, Y, ImprovedVBE.cover);
        }
    }
}
