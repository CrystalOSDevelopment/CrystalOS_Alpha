using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CrystalOSAlpha.Applications.Minecraft
{
    class Render : App
    {
        #region vars
        public bool movable {get; set;}

        public int desk_ID { get; set; }
        public int AppID { get; set; }
        public string name { get; set; }

        public bool minimised { get; set; }
        public int z { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        public int x_1 = 0;
        public int y_1 = 0;
        public int width {get; set; }
        public int height { get; set; }

        public Bitmap icon { get; set; }
        public Bitmap canvas;
        public bool once = true;
        public Bitmap window;

        public string source = "";
        public int CurrentColor = ImprovedVBE.colourToNumber(113, 188, 225);

        public static int Last(string s, char searc_char)
        {
            int i = 0;
            int temp = 0;
            if (s.Contains(searc_char))
            {
                foreach (char c in s)
                {
                    if (c == searc_char)
                    {
                        i = temp;
                    }
                    temp++;
                }
            }
            return i;
        }
        public static int Count(string s)
        {
            int i = 0;
            foreach (char c in s)
            {
                if (c == '\n')
                {
                    i++;
                }
            }
            return i;
        }

        public static int FirstLast(string s)
        {
            int i = 0;
            foreach (char c in s)
            {
                if (c != ' ')
                {
                    return i;
                }
                i++;
            }
            return i;
        }

        //public List<UI_Elements> Elements = new List<UI_Elements>();

        //public List<Variables> Variable = new List<Variables>();

        public bool WasTrue = false;
        public List<bool> Statements_Cond = new List<bool> { false };

        public bool windowed = true;
        public bool rounded = true;
        public bool fullscreen = true;

        public string command = "";
        public bool WasIF = false;
        public bool VoidStarted = false;
        public bool update = true;
        public int kernelCycle = 1;

        public int x_offset = 300;
        public int y_offset = 100;
        #endregion vars

        #region Blocks
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.DIAMOND.bmp")] public static byte[] Dia;
        public static Bitmap Diamond = new Bitmap(Dia);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.DIRT.bmp")] public static byte[] Dir;
        public static Bitmap Dirt = new Bitmap(Dir);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.BRICK.bmp")] public static byte[] BRI;
        public static Bitmap Brick = new Bitmap(BRI);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.BOOKSHELF.bmp")] public static byte[] BOOK;
        public static Bitmap BookShelf = new Bitmap(BOOK);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.STONE.bmp")] public static byte[] STONE;
        public static Bitmap Stone = new Bitmap(STONE);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.WOOD.bmp")] public static byte[] WOOD;
        public static Bitmap Wood = new Bitmap(WOOD);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.LEAVES.bmp")] public static byte[] LEAVES;
        public static Bitmap Leaves = new Bitmap(LEAVES);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.AMETHYST.bmp")] public static byte[] AMETHYST;
        public static Bitmap Amethyst = new Bitmap(AMETHYST);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.EMERALD.bmp")] public static byte[] EMERALD;
        public static Bitmap Emerald = new Bitmap(EMERALD);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.GOLD.bmp")] public static byte[] GOLD;
        public static Bitmap Gold = new Bitmap(GOLD);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.IRON.bmp")] public static byte[] IRON;
        public static Bitmap Iron = new Bitmap(IRON);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.CACTUS.bmp")] public static byte[] CACTUS;
        public static Bitmap Cactus = new Bitmap(CACTUS);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.CHEST.bmp")] public static byte[] CHEST;
        public static Bitmap Chest = new Bitmap(CHEST);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.FURNACE.bmp")] public static byte[] FURNACE;
        public static Bitmap Furnace = new Bitmap(FURNACE);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.ICE.bmp")] public static byte[] ICE;
        public static Bitmap Ice = new Bitmap(ICE);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.LAVA.bmp")] public static byte[] LAVA;
        public static Bitmap Lava = new Bitmap(LAVA);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.MELON.bmp")] public static byte[] MELON;
        public static Bitmap Melon = new Bitmap(MELON);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.PLANKS.bmp")] public static byte[] PLANKS;
        public static Bitmap Planks = new Bitmap(PLANKS);

        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.SAND.bmp")] public static byte[] SAND;
        public static Bitmap Sand = new Bitmap(SAND);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.TNT.bmp")] public static byte[] TNT;
        public static Bitmap Tnt = new Bitmap(TNT);
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.WATER.bmp")] public static byte[] WATER;
        public static Bitmap Water = new Bitmap(WATER);

        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Applications.Minecraft.CURSOR.bmp")] public static byte[] CUR;
        public static Bitmap Cursor = new Bitmap(CUR);
        #endregion Blocks

        public List<Bitmap> Layers = new List<Bitmap>();

        public int LayerofCur = 1;
        public int X_cur = 140;
        public int Y_cur = 70;
        public int Current_Block = 1;
        public int dist_y = 0;
        public int dist_x = 0;

        public bool inventory = false;
        public Bitmap Inv;

        #region Game values
        public int Health = 100;
        public int Hunger = 100;
        public int Level = 1;
        #endregion game values

        public void App()
        {
            KeyEvent key;
            if (KeyboardManager.TryReadKey(out key))
            {
                if (key.Key == ConsoleKeyEx.W)
                {
                    if (inventory == true)
                    {
                        if (Inventory.Cursor >= 8)
                        {
                            Inventory.Cursor -= 8;
                        }
                    }
                    else
                    {
                        y_offset -= 8;//16
                        //x_offset -= 8;//1
                        Y_cur -= 1;
                        X_cur += 15;
                    }
                    once = true;
                }
                else if (key.Key == ConsoleKeyEx.S)
                {
                    if (inventory == true)
                    {
                        Inventory.Cursor += 8;
                    }
                    else
                    {
                        y_offset += 8;//16
                        //x_offset += 8;//1
                        Y_cur += 1;
                        X_cur -= 15;
                    }
                    once = true;
                }
                else if (key.Key == ConsoleKeyEx.A)
                {
                    if (inventory == true)
                    {
                        Inventory.Cursor -= 1;
                    }
                    else
                    {
                        x_offset += 9;//16
                        y_offset += 4;//16
                        X_cur += 1;
                    }
                    once = true;
                }
                else if (key.Key == ConsoleKeyEx.D)
                {
                    if (inventory == true)
                    {
                        Inventory.Cursor += 1;
                    }
                    else
                    {
                        x_offset -= 9;//16
                        y_offset -= 4;//16
                        X_cur -= 1;
                    }
                    once = true;
                }
                else if (key.Key == ConsoleKeyEx.I)
                {
                    if (inventory == true)
                    {
                        inventory = false;
                    }
                    else
                    {
                        inventory = true;
                    }
                    once = true;
                }
                else if (key.Key == ConsoleKeyEx.UpArrow)
                {
                    LayerofCur++;
                    once = true;
                }
                else if (key.Key == ConsoleKeyEx.DownArrow)
                {
                    LayerofCur--;
                    once = true;
                }
                else if (key.Key == ConsoleKeyEx.Enter)
                {
                    Current_Block = Inventory.Cursor + 1;
                    inventory = false;
                    once = true;
                }
                else
                {
                    int res = 0;
                    if (int.TryParse(key.KeyChar.ToString(), out res))
                    {
                        if (res != 0)
                        {
                            Current_Block = res;
                        }
                    }
                }
            }

            if (MouseManager.X > x && MouseManager.X < x + width)
            {
                if (MouseManager.Y > y + 22 && MouseManager.Y < y + height)
                {
                    if (MouseManager.MouseState == MouseState.Right)
                    {
                        Layers[LayerofCur].RawData[Y_cur * 140 + X_cur] = 0;
                        once = true;
                    }
                    else if (MouseManager.MouseState == MouseState.Left)
                    {
                        Layers[LayerofCur].RawData[Y_cur * 140 + X_cur] = Current_Block;
                        once = true;
                    }
                }
            }

            if (once == true)
            {
                if (update == true)
                {
                    //This is the part where we set the values of the windows (propeties)
                    canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32); //new int[width * height];
                    window = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);

                    #region corners
                    if (rounded == true)
                    {
                        DrawFilledEllipse(10, 10, 10, 10, CurrentColor);
                        DrawFilledEllipse(width - 11, 10, 10, 10, CurrentColor);
                        DrawFilledEllipse(10, height - 10, 10, 10, CurrentColor);
                        DrawFilledEllipse(width - 11, height - 10, 10, 10, CurrentColor);

                        DrawFilledRectangle(CurrentColor, 0, 10, width, height - 20);
                        DrawFilledRectangle(CurrentColor, 5, 0, width - 10, 15);
                        DrawFilledRectangle(CurrentColor, 5, height - 15, width - 10, 15);
                    }
                    else
                    {
                        Array.Fill(canvas.RawData, CurrentColor);
                    }
                    #endregion corners

                    #region base

                    if (windowed == true)
                    {
                        DrawGradientLeftToRight();
                    }

                    DrawFilledEllipse(width - 13, 10, 8, 8, ImprovedVBE.colourToNumber(255, 0, 0));

                    DrawFilledEllipse(width - 34, 10, 8, 8, ImprovedVBE.colourToNumber(227, 162, 37));

                    //canvas.RawData = Word_processor.draw_text(name, 2, 2, ImprovedVBE.colourToNumber(255, 255, 255), canvas.RawData, width, height);
                    BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, name, 2, 2);
                    #endregion base
                }

                int pos_y = 0;
                int pos_x = 0;

                double DriftUp = 0;
                double DriftLeft = 0;
                int counter = 0;

                for (int c = 0; c < Layers.Count; c++)
                {
                    int temp_x = 0;
                    int temp_y = 0;

                    int BlockCounter = 0;
                    for (int j = 0; j < 140; j++)
                    {
                        pos_x = temp_x;
                        pos_y = temp_y;
                        for (int i = pos_x; i > -((140 - j) * 8); i -= 9)
                        {
                            if (pos_x > -16)
                            {
                                switch (Layers[c].RawData[BlockCounter])
                                {
                                    case 1:
                                        EnableTransparency(Dirt, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                        break;
                                    case 2:
                                        EnableTransparency(Diamond, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                        break;
                                    case 3:
                                        EnableTransparency(Brick, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                        break;
                                    case 4:
                                        EnableTransparency(BookShelf, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                        break;
                                    case 5:
                                        EnableTransparency(Stone, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                        break;
                                    case 6:
                                        EnableTransparency(Wood, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                        break;
                                    case 7:
                                        EnableTransparency(Leaves, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                        break;
                                    case 8:
                                        EnableTransparency(Amethyst, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                        break;
                                    case 9:
                                        EnableTransparency(Emerald, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                        break;
                                    case 10:
                                        EnableTransparency(Gold, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                        break;
                                    case 11:
                                        EnableTransparency(Iron, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                        break;
                                    case 12:
                                        EnableTransparency(Cactus, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                        break;
                                    case 13:
                                        EnableTransparency(Chest, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                        break;
                                    case 14:
                                        EnableTransparency(Furnace, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                        break;
                                    case 15:
                                        EnableTransparency(Ice, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                        break;
                                    case 16:
                                        EnableTransparency(Lava, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                        break;
                                    case 17:
                                        EnableTransparency(Melon, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                        break;
                                    case 18:
                                        EnableTransparency(Planks, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                        break;
                                    case 19:
                                        EnableTransparency(Sand, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                        break;
                                    case 20:
                                        EnableTransparency(Tnt, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                        break;
                                    case 21:
                                        EnableTransparency(Water, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                        break;
                                }
                                /*
                                if (Layers[c].RawData[BlockCounter] == 1)
                                {
                                    EnableTransparency(Dirt, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                }
                                else if (Layers[c].RawData[BlockCounter] == 2)
                                {
                                    EnableTransparency(Diamond, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                }
                                else if (Layers[c].RawData[BlockCounter] == 3)
                                {
                                    EnableTransparency(Brick, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                }
                                else if (Layers[c].RawData[BlockCounter] == 4)
                                {
                                    EnableTransparency(BookShelf, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                }
                                else if (Layers[c].RawData[BlockCounter] == 5)
                                {
                                    EnableTransparency(Stone, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                }
                                else if (Layers[c].RawData[BlockCounter] == 6)
                                {
                                    EnableTransparency(Wood, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                }
                                else if (Layers[c].RawData[BlockCounter] == 7)
                                {
                                    EnableTransparency(Leaves, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                }
                                else if (Layers[c].RawData[BlockCounter] == 8)
                                {
                                    EnableTransparency(Amethyst, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                }
                                else if (Layers[c].RawData[BlockCounter] == 9)
                                {
                                    EnableTransparency(Emerald, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                }
                                else if (Layers[c].RawData[BlockCounter] == 10)
                                {
                                    EnableTransparency(Gold, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                }
                                if (Layers[c].RawData[BlockCounter] == 11)
                                {
                                    EnableTransparency(Iron, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                }
                                else if (Layers[c].RawData[BlockCounter] == 12)
                                {
                                    EnableTransparency(Cactus, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                }
                                else if (Layers[c].RawData[BlockCounter] == 13)
                                {
                                    EnableTransparency(Chest, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                }
                                else if (Layers[c].RawData[BlockCounter] == 14)
                                {
                                    EnableTransparency(Furnace, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                }
                                else if (Layers[c].RawData[BlockCounter] == 15)
                                {
                                    EnableTransparency(Ice, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                }
                                else if (Layers[c].RawData[BlockCounter] == 16)
                                {
                                    EnableTransparency(Lava, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                }
                                else if (Layers[c].RawData[BlockCounter] == 17)
                                {
                                    EnableTransparency(Melon, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                }
                                else if (Layers[c].RawData[BlockCounter] == 18)
                                {
                                    EnableTransparency(Planks, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                }
                                else if (Layers[c].RawData[BlockCounter] == 19)
                                {
                                    EnableTransparency(Sand, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                }
                                else if (Layers[c].RawData[BlockCounter] == 20)
                                {
                                    EnableTransparency(Tnt, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                }
                                else if (Layers[c].RawData[BlockCounter] == 21)
                                {
                                    EnableTransparency(Water, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                }
                                */
                            }
                            else
                            {
                                break;
                            }
                            if (BlockCounter == Y_cur * 140 + X_cur && c == LayerofCur)
                            {
                                dist_x = (int)(i + x_offset - DriftLeft);
                                dist_y = (int)(pos_y - y_offset - DriftUp);
                                EnableTransparency(Cursor, (int)(i + x_offset - DriftLeft), (int)(pos_y - y_offset - DriftUp));
                                /*
                                if (MouseManager.MouseState == MouseState.Right)
                                {
                                    Layers[1][BlockCounter] = 0;
                                }
                                else if (MouseManager.MouseState == MouseState.Left)
                                {
                                    Layers[1][BlockCounter] = Current_Block;
                                }
                                */
                            }
                            BlockCounter++;

                            pos_y += 4;
                        }
                        temp_x += 8;
                        temp_y += 4;
                    }
                    BlockCounter = 0;

                    pos_y = -8 * (c + 1);
                    pos_x = -4 * (c + 1);
                    temp_x = -8 * (c + 1);
                    temp_y = -4 * (c + 1);
                    //DriftLeft += 1;
                    DriftUp += 8.5;
                }

                if (inventory == true)
                {
                    Inventory inv = new Inventory();
                    Inv = inv.Render(width - 40, height - 60, canvas.RawData, Health, Hunger, Level);
                    EnableTransparency(Inv, 20, 40);

                }

                window.RawData = canvas.RawData;
                if (dist_x < width / 2 || dist_y < height / 2)
                {
                    x_offset += width / 2 - dist_x;
                    y_offset -= height / 2 - dist_y;
                }
                else if (dist_x > width / 2 || dist_y > height / 2)
                {
                    x_offset -= dist_x - width / 2;
                    y_offset += dist_y - height / 2;
                }
                else
                {
                    once = false;
                }
            }

            ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);

            #region mechanical
            if (MouseManager.MouseState == MouseState.Left)
            {
                if (MouseManager.X > x + width - 21 && MouseManager.X < x + width - 5)
                {
                    if (MouseManager.Y > y && MouseManager.Y < y + 20)
                    {
                        //Task_Manager.calculators.RemoveAt(Task_Manager.indicator);
                    }
                }
                if (movable == false)
                {
                    if (MouseManager.X > x && MouseManager.X < x + 352)
                    {
                        if (MouseManager.Y > y && MouseManager.Y < y + 18)
                        {
                            movable = true;
                        }
                    }
                }
            }
            if (movable == true)
            {
                x = (int)MouseManager.X;
                y = (int)MouseManager.Y;
                if (MouseManager.MouseState == MouseState.Right)
                {
                    movable = false;
                }
            }
            #endregion core
        }

        public void DrawFilledEllipse(int xCenter, int yCenter, int yR, int xR, int color)
        {
            /*
            int r = (color & 0xff0000) >> 16;
            int g = (color & 0x00ff00) >> 8;
            int b = (color & 0x0000ff);

            float blendFactor = 0.5f;
            float inverseBlendFactor = 1 - blendFactor;
            */
            for (int y = -yR; y <= yR; y++)
            {
                for (int x = -xR; x <= xR; x++)
                {
                    if (x * x * yR * yR + y * y * xR * xR <= yR * yR * xR * xR)
                    {
                        /*
                        int r3 = (cover.RawData[(yCenter + y) * width + xCenter + x] & 0xff0000) >> 16;
                        int g3 = (cover.RawData[(yCenter + y) * width + xCenter + x] & 0x00ff00) >> 8;
                        int b3 = (cover.RawData[(yCenter + y) * width + xCenter + x] & 0x0000ff);
                        //Color c = Color.FromArgb(cover.RawData[j * width + i]);

                        int r2 = (int)(inverseBlendFactor * r3 + blendFactor * r);
                        int g2 = (int)(inverseBlendFactor * g3 + blendFactor * g);
                        int b2 = (int)(inverseBlendFactor * b3 + blendFactor * b);
                        */
                        if (xCenter + x > 0 && xCenter + x < width - 1 && yCenter + y > 0 && yCenter + y < height)
                        {
                            canvas.RawData[(yCenter + y) * width + xCenter + x] = color;
                        }

                        //DrawPixel(xCenter + x, yCenter + y, GetGradientColor(x, 0, width, height));

                        //DrawPixel(xCenter + x, yCenter + y, color);
                    }
                }
            }
        }

        public void DrawFilledRectangle(int color, int X, int Y, int Width, int Height)
        {
            /*
            int r = (color & 0xff0000) >> 16;
            int g = (color & 0x00ff00) >> 8;
            int b = (color & 0x0000ff);

            float blendFactor = 0.5f;
            float inverseBlendFactor = 1 - blendFactor;

            for (int j = Y; j < Y + Height; j++)
            {
                for (int i = X; i < X + Width; i++)
                {
                    int r3 = (cover.RawData[j * width + i] & 0xff0000) >> 16;
                    int g3 = (cover.RawData[j * width + i] & 0x00ff00) >> 8;
                    int b3 = (cover.RawData[j * width + i] & 0x0000ff);
                    //Color c = Color.FromArgb(cover.RawData[j * width + i]);

                    int r2 = (int)(inverseBlendFactor * r3 + blendFactor * r);
                    int g2 = (int)(inverseBlendFactor * g3 + blendFactor * g);
                    int b2 = (int)(inverseBlendFactor * b3 + blendFactor * b);

                    DrawPixel(i, j, colourToNumber(r2, g2, b2));
                }
            }*/

            if (X < width && Y < height)
            {
                int[] line = new int[Width];
                if (X < 0)
                {
                    line = new int[Width + X];
                }
                else if (X + Width > width)
                {
                    line = new int[Width - (X + Width - width)];
                }
                Array.Fill(line, color);

                for (int i = Y; i < Y + Height; i++)
                {
                    Array.Copy(line, 0, canvas.RawData, i * width + X, line.Length);
                }
            }

        }
        public int GetGradientColor(int x, int y, int width, int height)
        {
            int r = (int)((double)x / width * 255);
            int g = (int)((double)y / height * 255);
            int b = 255;

            return ImprovedVBE.colourToNumber(r, g, b);
        }
        public void DrawGradientLeftToRight()
        {
            int gradientColorStart = GetGradientColor(0, 0, width, height); int gradientColorEnd = GetGradientColor(width + 1, 0, width, height);

            int rStart = Color.FromArgb(gradientColorStart).R; int gStart = Color.FromArgb(gradientColorStart).G; int bStart = Color.FromArgb(gradientColorStart).B;

            int rEnd = Color.FromArgb(gradientColorEnd).R; int gEnd = Color.FromArgb(gradientColorEnd).G; int bEnd = Color.FromArgb(gradientColorEnd).B;

            for (int i = 0; i < canvas.RawData.Length; i++)
            {
                if (x_1 == width - 1)
                {
                    x_1 = 0;
                }
                else
                {
                    x_1++;
                }
                int r = (int)((double)x_1 / width * (rEnd - rStart)) + rStart;
                int g = (int)((double)x_1 / width * (gEnd - gStart)) + gStart;
                int b = (int)((double)x_1 / width * (bEnd - bStart)) + bStart;
                if (canvas.RawData[i] == ImprovedVBE.colourToNumber(113, 188, 225))
                {
                    canvas.RawData[i] = ImprovedVBE.colourToNumber(r, g, b);
                }
                if (i / width > 20)
                {
                    break;
                }
            }
            x_1 = 0;
        }

        public bool EnableTransparency(Image image, int x, int y)
        {
            int counter = 0;
            if (y < 14 || y > height || x < -16 || x > width)
            {
                return false;
            }
            for (int _y = y; _y < y + image.Height; _y++)
            {
                if (_y > 20)
                {
                    for (int _x = x; _x < x + image.Width; _x++)
                    {
                        if (_y < height)
                        {
                            if (_x < width && _x > 0)
                            {
                                if (image.RawData[counter] == 0)
                                {
                                    counter++;
                                }
                                else
                                {
                                    canvas.RawData[_y * width - (width - _x)] = image.RawData[counter];
                                    counter++;
                                }
                            }
                            else
                            {
                                counter++;
                            }
                        }
                        else
                        {
                            counter += (int)image.Width;
                            return false;
                        }
                    }
                }
                else
                {
                    counter += (int)image.Width;
                }
            }
            return true;
        }
    }

    class Layer
    {
        public int[] layer { get; set; }
    }
}
