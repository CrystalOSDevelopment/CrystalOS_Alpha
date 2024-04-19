using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Drawing;

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
        public int width {get; set; }
        public int height { get; set; }

        public Bitmap icon { get; set; }
        public Bitmap canvas;
        public Bitmap window { get; set; }

        public List<bool> Statements_Cond = new List<bool> { false };

        public bool WasTrue = false;
        public bool windowed = true;
        public bool rounded = true;
        public bool fullscreen = true;
        public bool once { get; set; }

        public string source = "";
        public string command = "";
        public bool WasIF = false;
        public bool VoidStarted = false;
        public bool update = true;

        public int x_offset = 300;
        public int y_offset = 100;
        public int CurrentColor = ImprovedVBE.colourToNumber(113, 188, 225);
        public int kernelCycle = 1;
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
                switch (key.Key)
                {
                    case ConsoleKeyEx.W:
                        if (inventory == true)
                        {
                            if (Inventory.Cursor >= 8)
                            {
                                Inventory.Cursor -= 8;
                            }
                        }
                        else
                        {
                            y_offset -= 8;
                            Y_cur -= 1;
                            X_cur += 15;
                        }
                        once = true;
                        break;
                    case ConsoleKeyEx.S:
                        if (inventory == true)
                        {
                            Inventory.Cursor += 8;
                        }
                        else
                        {
                            y_offset += 8;
                            Y_cur += 1;
                            X_cur -= 15;
                        }
                        once = true;
                        break;
                    case ConsoleKeyEx.A:
                        if (inventory == true)
                        {
                            Inventory.Cursor -= 1;
                        }
                        else
                        {
                            x_offset += 9;
                            y_offset += 4;
                            X_cur += 1;
                        }
                        once = true;
                        break;
                    case ConsoleKeyEx.D:
                        if (inventory == true)
                        {
                            Inventory.Cursor += 1;
                        }
                        else
                        {
                            x_offset -= 9;
                            y_offset -= 4;
                            X_cur -= 1;
                        }
                        once = true;
                        break;
                    case ConsoleKeyEx.I:
                        if (inventory == true)
                        {
                            inventory = false;
                        }
                        else
                        {
                            inventory = true;
                        }
                        once = true;
                        break;
                    case ConsoleKeyEx.UpArrow:
                        if(inventory == false)
                        {
                            LayerofCur++;
                            once = true;
                        }
                        break;
                    case ConsoleKeyEx.DownArrow:
                        if(inventory == false)
                        {
                            LayerofCur--;
                            once = true;
                        }
                        break;
                    case ConsoleKeyEx.Enter:
                        Current_Block = Inventory.Cursor + 1;
                        inventory = false;
                        once = true;
                        break;
                    default:
                        int res = 0;
                        if (int.TryParse(key.KeyChar.ToString(), out res))
                        {
                            if (res != 0)
                            {
                                Current_Block = res;
                            }
                        }
                        break;
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
                    Bitmap back = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                    (canvas, back, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);
                }

                int pos_y = 0;
                int pos_x = 0;

                double DriftUp = 0;
                double DriftLeft = 0;

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
        }

        public void RightClick()
        {

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
}
