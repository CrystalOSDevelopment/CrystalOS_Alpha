using Cosmos.Core.Memory;
using Cosmos.HAL;
using Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.TaskBar;
using CrystalOSAlpha.System32.Installer;
using System;
using System.Drawing;
using System.IO;
using Kernel = CrystalOS_Alpha.Kernel;
using Sys = Cosmos.System;

namespace CrystalOSAlpha.System32
{
    class Boot
    {
        public static void Initialise()
        {
            string input = "";
            while (true)
            {
                Array.Fill(ImprovedVBE.cover.RawData, 0);
                KeyEvent key;
                if (KeyboardManager.TryReadKey(out key))
                {
                    if (key.Key == ConsoleKeyEx.Enter)
                    {
                        break;
                    }
                    if (input.Length < 10)
                    {
                        input = Keyboard.HandleKeyboard(input, key);
                    }
                }
                BitFont.DrawBitFontString(ImprovedVBE.cover, "ArialCustomCharset16", Color.White, "Start up with disk support(Y/N): " + input, 2, 2);
                ImprovedVBE.Display(Kernel.vbe, false);
                Heap.Collect();
            }
            string option = "";
            while (true)
            {
                Array.Fill(ImprovedVBE.cover.RawData, 0);
                KeyEvent key;
                if (KeyboardManager.TryReadKey(out key))
                {
                    if (key.Key == ConsoleKeyEx.Enter)
                    {
                        break;
                    }
                    if (option.Length < 10)
                    {
                        option = Keyboard.HandleKeyboard(option, key);
                    }
                }
                BitFont.DrawBitFontString(ImprovedVBE.cover, "ArialCustomCharset16", Color.White, "Enter setup mode(Y) or continue with preset?(N): " + option, 2, 2);
                ImprovedVBE.Display(Kernel.vbe, false);
                Heap.Collect();
            }
            string IP = "";
            while (true)
            {
                Array.Fill(ImprovedVBE.cover.RawData, 0);
                KeyEvent key;
                if (KeyboardManager.TryReadKey(out key))
                {
                    if (key.Key == ConsoleKeyEx.Enter)
                    {
                        break;
                    }
                    if (option.Length < 10)
                    {
                        IP = Keyboard.HandleKeyboard(IP, key);
                    }
                }
                BitFont.DrawBitFontString(ImprovedVBE.cover, "ArialCustomCharset16", Color.White, "Enable network? (if yes, please type in the ip of the TCP client and the local web server): " + IP + "\n  Example: x.x.x.x x.x.x.x or blank to disable", 2, 2);
                ImprovedVBE.Display(Kernel.vbe, false);
                Heap.Collect();
            }
            if(IP != "")
            {
                string[] Split = IP.Split(' ');
                if(Split.Length == 2)
                {
                    string[] Left = Split[0].Split('.');
                    string[] Right = Split[1].Split('.');
                    if (Left.Length == 4 && Right.Length == 4)
                    {
                        try
                        {
                            try
                            {
                                NetworkDevice nic = NetworkDevice.GetDeviceByName("eth0"); //get network device by name
                                IPConfig.Enable(nic, new Address(192, 168, 1, 69), new Address(255, 255, 255, 0), new Address(192, 168, 1, 254)); //enable IPv4 configuration

                                using (var xClient = new Cosmos.System.Network.IPv4.UDP.DHCP.DHCPClient())
                                {
                                    /** Send a DHCP Discover packet **/
                                    //This will automatically set the IP config after DHCP response
                                    xClient.SendDiscoverPacket();

                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            GlobalValues.TCPIP = Split[0];              //This is for the console app
                            GlobalValues.ServerIP = Split[1];           //This is for the web server
                            //Comment out before pushing to GitHub
                            GlobalValues.TCPIP = "192.168.159.1";       //IP of your vm (Ethernet adapter VMware Network Adapter VMnet8 in my case)
                            GlobalValues.ServerIP = "192.168.0.22";     //This is the ip of your computer (Wireless LAN adapter Wi-Fi in my case)
                            Kernel.IsNetSupport = true;
                        }
                        catch
                        {
                            
                        }
                    }
                }
            }
            if (VMTools.IsVMWare == true)
            {
                if (input.ToLower() == "y" || input.ToLower() == "yes")
                {
                    Sys.FileSystem.VFS.VFSManager.RegisterVFS(Kernel.fs);
                    Kernel.IsDiskSupport = true;
                    //Insert install here
                    if (option.ToLower() == "y" || option.ToLower() == "yes")
                    {
                        Engine Setup = new Engine();
                        while (true)
                        {
                            //Running the core of the setup
                            Setup.Main();
                            //As the name suggests, it executes every (opened) app in the list
                            TaskScheduler.Exec();

                            //Draws the cursor
                            ImprovedVBE.DrawImageAlpha(Kernel.C, (int)MouseManager.X, (int)MouseManager.Y, ImprovedVBE.cover);

                            //Renders to the screen
                            ImprovedVBE.Display(Kernel.vbe, false);

                            //By calling this, the system won't crash after a certain amount of time
                            Heap.Collect();
                        }
                    }
                    //Problem: Not implemented, so don't even attempt to search after it dear GitHub user!
                    //Until then, this is the block of code that substitutes it
                    if (Kernel.fs.Disks.Count != 0)
                    {
                        #region Config
                        //Customization file
                        if (!File.Exists("0:\\System\\Layout.sys"))
                        {
                            Engine Setup = new Engine();
                            while (true)
                            {
                                //Running the core of the setup
                                Setup.Main();
                                //As the name suggests, it executes every (opened) app in the list
                                TaskScheduler.Exec();

                                //Draws the cursor
                                ImprovedVBE.DrawImageAlpha(Kernel.C, (int)MouseManager.X, (int)MouseManager.Y, ImprovedVBE.cover);

                                //Renders to the screen
                                ImprovedVBE.Display(Kernel.vbe, false);

                                //By calling this, the system won't crash after a certain amount of time
                                Heap.Collect();
                                if (Setup.Done == true)
                                {
                                    break;
                                }
                            }
                            if (GlobalValues.Background_type == "Monocolor")
                            {
                                switch (GlobalValues.Background_color)
                                {
                                    case "CrystalGreen":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Green.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalBlue":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Blue.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalYellow":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Yellow.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalOrange":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Orange.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalRed":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Red.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalBlack":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Black.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalPink":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Pink.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalPurple":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Purple.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalAqua":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Aqua.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalWhite":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.White.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;

                                    case "GoldenSunshine":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(255, 215, 0));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CoralOrange":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(255, 127, 80));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "PeachPink":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(255, 204, 104));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "SkyBlue":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.SkyBlue.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "OceanBlue":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(30, 144, 255));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "TurquoiseTeal":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(64, 224, 208));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "EmeraldGreen":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(0, 128, 0));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "MintGreen":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(152, 255, 152));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "LavenderPurple":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(204, 204, 255));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "SoothingGray":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(192, 192, 192));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                }
                            }
                            else
                            {
                                ImprovedVBE.Temp.RawData.CopyTo(ImprovedVBE.data.RawData, 0);
                            }
                            TaskManager.resize = true;
                            TaskManager.Time = 99;
                            TaskManager.update = true;
                            TaskManager.Back_Buffer = null;
                        }
                        else
                        {
                            string[] Lines = File.ReadAllLines("0:\\System\\Layout.sys");
                            foreach (string s in Lines)
                            {
                                string[] Split = s.Split('=');
                                switch (Split[0])
                                {
                                    case "WindowR":
                                        GlobalValues.R = int.Parse(Split[1]);
                                        break;
                                    case "WindowG":
                                        GlobalValues.G = int.Parse(Split[1]);
                                        break;
                                    case "WindowB":
                                        GlobalValues.B = int.Parse(Split[1]);
                                        break;

                                    case "TaskbarR":
                                        GlobalValues.TaskBarR = int.Parse(Split[1]);
                                        break;
                                    case "TaskbarG":
                                        GlobalValues.TaskBarG = int.Parse(Split[1]);
                                        break;
                                    case "TaskbarB":
                                        GlobalValues.TaskBarB = int.Parse(Split[1]);
                                        break;

                                    case "TaskbarType":
                                        GlobalValues.TaskBarType = Split[1];
                                        break;

                                    case "Username":
                                        GlobalValues.Username = Split[1];
                                        break;

                                    case "IconR":
                                        GlobalValues.IconR = int.Parse(Split[1]);
                                        break;
                                    case "IconG":
                                        GlobalValues.IconG = int.Parse(Split[1]);
                                        break;
                                    case "IconB":
                                        GlobalValues.IconB = int.Parse(Split[1]);
                                        break;

                                    case "Iconwidth":
                                        GlobalValues.IconWidth = uint.Parse(Split[1]);
                                        break;
                                    case "Iconheight":
                                        GlobalValues.IconHeight = uint.Parse(Split[1]);
                                        break;

                                    case "Startcolor":
                                        GlobalValues.StartColor = Color.FromArgb(int.Parse(Split[1]));
                                        break;
                                    case "Endcolor":
                                        GlobalValues.EndColor = Color.FromArgb(int.Parse(Split[1]));
                                        break;

                                    case "Bakground":
                                        GlobalValues.Background_type = Split[1];
                                        break;
                                    case "Backgroundcolor":
                                        GlobalValues.Background_color = Split[1];
                                        break;

                                    case "Transparency":
                                        GlobalValues.LevelOfTransparency = int.Parse(Split[1]);
                                        TaskManager.update = true;
                                        TaskManager.resize = true;
                                        SideNav.Get_Back = true;
                                        break;

                                    case "Keyboard":
                                        switch (Split[1])
                                        {
                                            case "EN_US":
                                                GlobalValues.KeyboardLayout = KeyboardLayout.EN_US;
                                                break;
                                            case "Hungarian":
                                                GlobalValues.KeyboardLayout = KeyboardLayout.HUngarian;
                                                break;
                                        }
                                        break;
                                }
                            }
                            if (GlobalValues.Background_type == "Monocolor")
                            {
                                switch (GlobalValues.Background_color)
                                {
                                    case "CrystalGreen":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Green.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalBlue":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Blue.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalYellow":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Yellow.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalOrange":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Orange.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalRed":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Red.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalBlack":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Black.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalPink":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Pink.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalPurple":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Purple.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalAqua":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Aqua.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalWhite":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.White.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;

                                    case "GoldenSunshine":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(255, 215, 0));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CoralOrange":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(255, 127, 80));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "PeachPink":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(255, 204, 104));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "SkyBlue":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.SkyBlue.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "OceanBlue":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(30, 144, 255));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "TurquoiseTeal":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(64, 224, 208));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "EmeraldGreen":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(0, 128, 0));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "MintGreen":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(152, 255, 152));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "LavenderPurple":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(204, 204, 255));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "SoothingGray":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(192, 192, 192));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                }
                            }
                        }
                        ////Applications folder
                        Directory.CreateDirectory("0:\\Programs");
                        ////CarbonIDE folder
                        Directory.CreateDirectory("0:\\User\\Source");
                        #endregion Config
                    }
                }
                else if (option.ToLower() == "y" || option.ToLower() == "yes")
                {
                    Engine Setup = new Engine();
                    while (true)
                    {
                        //Running the core of the setup
                        Setup.Main();
                        //As the name suggests, it executes every (opened) app in the list
                        TaskScheduler.Exec();

                        //Draws the cursor
                        ImprovedVBE.DrawImageAlpha(Kernel.C, (int)MouseManager.X, (int)MouseManager.Y, ImprovedVBE.cover);

                        //Renders to the screen
                        ImprovedVBE.Display(Kernel.vbe, false);

                        //By calling this, the system won't crash after a certain amount of time
                        Heap.Collect();
                        if(Setup.Done == true)
                        {
                            break;
                        }
                    }
                    if (GlobalValues.Background_type == "Monocolor")
                    {
                        switch (GlobalValues.Background_color)
                        {
                            case "CrystalGreen":
                                Array.Fill(ImprovedVBE.data.RawData, Color.Green.ToArgb());
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "CrystalBlue":
                                Array.Fill(ImprovedVBE.data.RawData, Color.Blue.ToArgb());
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "CrystalYellow":
                                Array.Fill(ImprovedVBE.data.RawData, Color.Yellow.ToArgb());
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "CrystalOrange":
                                Array.Fill(ImprovedVBE.data.RawData, Color.Orange.ToArgb());
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "CrystalRed":
                                Array.Fill(ImprovedVBE.data.RawData, Color.Red.ToArgb());
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "CrystalBlack":
                                Array.Fill(ImprovedVBE.data.RawData, Color.Black.ToArgb());
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "CrystalPink":
                                Array.Fill(ImprovedVBE.data.RawData, Color.Pink.ToArgb());
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "CrystalPurple":
                                Array.Fill(ImprovedVBE.data.RawData, Color.Purple.ToArgb());
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "CrystalAqua":
                                Array.Fill(ImprovedVBE.data.RawData, Color.Aqua.ToArgb());
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "CrystalWhite":
                                Array.Fill(ImprovedVBE.data.RawData, Color.White.ToArgb());
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;

                            case "GoldenSunshine":
                                Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(255, 215, 0));
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "CoralOrange":
                                Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(255, 127, 80));
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "PeachPink":
                                Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(255, 204, 104));
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "SkyBlue":
                                Array.Fill(ImprovedVBE.data.RawData, Color.SkyBlue.ToArgb());
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "OceanBlue":
                                Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(30, 144, 255));
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "TurquoiseTeal":
                                Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(64, 224, 208));
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "EmeraldGreen":
                                Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(0, 128, 0));
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "MintGreen":
                                Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(152, 255, 152));
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "LavenderPurple":
                                Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(204, 204, 255));
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "SoothingGray":
                                Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(192, 192, 192));
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                        }
                    }
                    else
                    {
                        ImprovedVBE.Temp.RawData.CopyTo(ImprovedVBE.data.RawData, 0);
                    }
                    TaskManager.resize = true;
                    TaskManager.Time = 99;
                    TaskManager.update = true;
                    TaskManager.Back_Buffer = null;
                }
            }
            else
            {
                Kernel.Is_KeyboardMouse = true;
                if (input.ToLower() == "y" || input.ToLower() == "yes")
                {
                    Sys.FileSystem.VFS.VFSManager.RegisterVFS(Kernel.fs);
                    Kernel.IsDiskSupport = true;
                    //Insert install here
                    if (option.ToLower() == "y" || option.ToLower() == "yes")
                    {
                        Engine Setup = new Engine();
                        while (true)
                        {
                            //Running the core of the setup
                            Setup.Main();
                            //As the name suggests, it executes every (opened) app in the list
                            TaskScheduler.Exec();

                            //Draws the cursor
                            ImprovedVBE.DrawImageAlpha(Kernel.C, (int)MouseManager.X, (int)MouseManager.Y, ImprovedVBE.cover);

                            //Renders to the screen
                            ImprovedVBE.Display(Kernel.vbe, false);

                            //By calling this, the system won't crash after a certain amount of time
                            Heap.Collect();
                        }
                    }
                    //Problem: Not implemented, so don't even attempt to search after it dear GitHub user!
                    //Until then, this is the block of code that substitutes it
                    if (Kernel.fs.Disks.Count != 0)
                    {
                        #region Config
                        //Customization file
                        if (!File.Exists("0:\\System\\Layout.sys"))
                        {
                            Engine Setup = new Engine();
                            while (true)
                            {
                                //Running the core of the setup
                                Setup.Main();
                                //As the name suggests, it executes every (opened) app in the list
                                TaskScheduler.Exec();

                                //Draws the cursor
                                ImprovedVBE.DrawImageAlpha(Kernel.C, (int)MouseManager.X, (int)MouseManager.Y, ImprovedVBE.cover);

                                //Renders to the screen
                                ImprovedVBE.Display(Kernel.vbe, false);

                                //By calling this, the system won't crash after a certain amount of time
                                Heap.Collect();
                                if (Setup.Done == true)
                                {
                                    break;
                                }
                            }
                            if (GlobalValues.Background_type == "Monocolor")
                            {
                                switch (GlobalValues.Background_color)
                                {
                                    case "CrystalGreen":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Green.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalBlue":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Blue.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalYellow":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Yellow.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalOrange":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Orange.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalRed":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Red.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalBlack":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Black.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalPink":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Pink.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalPurple":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Purple.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalAqua":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Aqua.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalWhite":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.White.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;

                                    case "GoldenSunshine":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(255, 215, 0));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CoralOrange":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(255, 127, 80));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "PeachPink":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(255, 204, 104));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "SkyBlue":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.SkyBlue.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "OceanBlue":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(30, 144, 255));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "TurquoiseTeal":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(64, 224, 208));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "EmeraldGreen":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(0, 128, 0));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "MintGreen":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(152, 255, 152));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "LavenderPurple":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(204, 204, 255));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "SoothingGray":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(192, 192, 192));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                }
                            }
                            else
                            {
                                ImprovedVBE.Temp.RawData.CopyTo(ImprovedVBE.data.RawData, 0);
                            }
                            TaskManager.resize = true;
                            TaskManager.Time = 99;
                            TaskManager.update = true;
                            TaskManager.Back_Buffer = null;
                        }
                        else
                        {
                            string[] Lines = File.ReadAllLines("0:\\System\\Layout.sys");
                            foreach (string s in Lines)
                            {
                                string[] Split = s.Split('=');
                                switch (Split[0])
                                {
                                    case "WindowR":
                                        GlobalValues.R = int.Parse(Split[1]);
                                        break;
                                    case "WindowG":
                                        GlobalValues.G = int.Parse(Split[1]);
                                        break;
                                    case "WindowB":
                                        GlobalValues.B = int.Parse(Split[1]);
                                        break;

                                    case "TaskbarR":
                                        GlobalValues.TaskBarR = int.Parse(Split[1]);
                                        break;
                                    case "TaskbarG":
                                        GlobalValues.TaskBarG = int.Parse(Split[1]);
                                        break;
                                    case "TaskbarB":
                                        GlobalValues.TaskBarB = int.Parse(Split[1]);
                                        break;

                                    case "TaskbarType":
                                        GlobalValues.TaskBarType = Split[1];
                                        break;

                                    case "Username":
                                        GlobalValues.Username = Split[1];
                                        break;

                                    case "IconR":
                                        GlobalValues.IconR = int.Parse(Split[1]);
                                        break;
                                    case "IconG":
                                        GlobalValues.IconG = int.Parse(Split[1]);
                                        break;
                                    case "IconB":
                                        GlobalValues.IconB = int.Parse(Split[1]);
                                        break;

                                    case "Iconwidth":
                                        GlobalValues.IconWidth = uint.Parse(Split[1]);
                                        break;
                                    case "Iconheight":
                                        GlobalValues.IconHeight = uint.Parse(Split[1]);
                                        break;

                                    case "Startcolor":
                                        GlobalValues.StartColor = Color.FromArgb(int.Parse(Split[1]));
                                        break;
                                    case "Endcolor":
                                        GlobalValues.EndColor = Color.FromArgb(int.Parse(Split[1]));
                                        break;

                                    case "Bakground":
                                        GlobalValues.Background_type = Split[1];
                                        break;
                                    case "Backgroundcolor":
                                        GlobalValues.Background_color = Split[1];
                                        break;

                                    case "Transparency":
                                        GlobalValues.LevelOfTransparency = int.Parse(Split[1]);
                                        TaskManager.update = true;
                                        TaskManager.resize = true;
                                        SideNav.Get_Back = true;
                                        break;

                                    case "Keyboard":
                                        switch (Split[1])
                                        {
                                            case "EN_US":
                                                GlobalValues.KeyboardLayout = KeyboardLayout.EN_US;
                                                break;
                                            case "Hungarian":
                                                GlobalValues.KeyboardLayout = KeyboardLayout.HUngarian;
                                                break;
                                        }
                                        break;
                                }
                            }
                            if (GlobalValues.Background_type == "Monocolor")
                            {
                                switch (GlobalValues.Background_color)
                                {
                                    case "CrystalGreen":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Green.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalBlue":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Blue.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalYellow":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Yellow.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalOrange":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Orange.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalRed":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Red.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalBlack":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Black.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalPink":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Pink.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalPurple":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Purple.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalAqua":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.Aqua.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CrystalWhite":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.White.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;

                                    case "GoldenSunshine":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(255, 215, 0));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "CoralOrange":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(255, 127, 80));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "PeachPink":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(255, 204, 104));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "SkyBlue":
                                        Array.Fill(ImprovedVBE.data.RawData, Color.SkyBlue.ToArgb());
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "OceanBlue":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(30, 144, 255));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "TurquoiseTeal":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(64, 224, 208));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "EmeraldGreen":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(0, 128, 0));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "MintGreen":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(152, 255, 152));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "LavenderPurple":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(204, 204, 255));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                    case "SoothingGray":
                                        Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(192, 192, 192));
                                        TaskManager.resize = true;
                                        TaskManager.Time = 99;
                                        break;
                                }
                            }
                        }
                        ////Applications folder
                        Directory.CreateDirectory("0:\\Programs");
                        ////CarbonIDE folder
                        Directory.CreateDirectory("0:\\User\\Source");
                        #endregion Config
                    }
                }
                else if (option.ToLower() == "y" || option.ToLower() == "yes")
                {
                    Engine Setup = new Engine();
                    while (true)
                    {
                        //Running the core of the setup
                        Setup.Main();
                        //As the name suggests, it executes every (opened) app in the list
                        TaskScheduler.Exec();

                        //Draws the cursor
                        ImprovedVBE.DrawImageAlpha(Kernel.C, (int)MouseManager.X, (int)MouseManager.Y, ImprovedVBE.cover);

                        //Renders to the screen
                        ImprovedVBE.Display(Kernel.vbe, false);

                        //By calling this, the system won't crash after a certain amount of time
                        Heap.Collect();
                        if (Setup.Done == true)
                        {
                            break;
                        }
                    }
                    if (GlobalValues.Background_type == "Monocolor")
                    {
                        switch (GlobalValues.Background_color)
                        {
                            case "CrystalGreen":
                                Array.Fill(ImprovedVBE.data.RawData, Color.Green.ToArgb());
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "CrystalBlue":
                                Array.Fill(ImprovedVBE.data.RawData, Color.Blue.ToArgb());
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "CrystalYellow":
                                Array.Fill(ImprovedVBE.data.RawData, Color.Yellow.ToArgb());
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "CrystalOrange":
                                Array.Fill(ImprovedVBE.data.RawData, Color.Orange.ToArgb());
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "CrystalRed":
                                Array.Fill(ImprovedVBE.data.RawData, Color.Red.ToArgb());
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "CrystalBlack":
                                Array.Fill(ImprovedVBE.data.RawData, Color.Black.ToArgb());
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "CrystalPink":
                                Array.Fill(ImprovedVBE.data.RawData, Color.Pink.ToArgb());
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "CrystalPurple":
                                Array.Fill(ImprovedVBE.data.RawData, Color.Purple.ToArgb());
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "CrystalAqua":
                                Array.Fill(ImprovedVBE.data.RawData, Color.Aqua.ToArgb());
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "CrystalWhite":
                                Array.Fill(ImprovedVBE.data.RawData, Color.White.ToArgb());
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;

                            case "GoldenSunshine":
                                Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(255, 215, 0));
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "CoralOrange":
                                Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(255, 127, 80));
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "PeachPink":
                                Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(255, 204, 104));
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "SkyBlue":
                                Array.Fill(ImprovedVBE.data.RawData, Color.SkyBlue.ToArgb());
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "OceanBlue":
                                Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(30, 144, 255));
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "TurquoiseTeal":
                                Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(64, 224, 208));
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "EmeraldGreen":
                                Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(0, 128, 0));
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "MintGreen":
                                Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(152, 255, 152));
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "LavenderPurple":
                                Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(204, 204, 255));
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                            case "SoothingGray":
                                Array.Fill(ImprovedVBE.data.RawData, ImprovedVBE.colourToNumber(192, 192, 192));
                                TaskManager.resize = true;
                                TaskManager.Time = 99;
                                break;
                        }
                    }
                    else
                    {
                        ImprovedVBE.Temp.RawData.CopyTo(ImprovedVBE.data.RawData, 0);
                    }
                    TaskManager.resize = true;
                    TaskManager.Time = 99;
                    TaskManager.update = true;
                    TaskManager.Back_Buffer = null;
                }
            }

            ImprovedVBE.Display(Kernel.vbe, false);
        }

        public static void Animation(int Seconds)
        {
            int MaxTime = Math.Clamp(Seconds, 0, 60);
            int Time = DateTime.Now.Second;
            Bitmap Temp = ImprovedVBE.ScaleImageStock(new Bitmap(TaskManager.Elephant), 475, 450);
            while (true)
            {
                if (MaxTime == 0)
                {
                    break;
                }
                else
                {
                    if (DateTime.Now.Second != Time)
                    {
                        MaxTime--;
                        Time = DateTime.Now.Second;
                    }
                    Array.Fill(ImprovedVBE.cover.RawData, 0);

                    ImprovedVBE.DrawImageAlpha(Temp, (int)ImprovedVBE.width / 2 - (int)Temp.Width / 2, 100, ImprovedVBE.cover);
                    BitFont.DrawBitFontString(ImprovedVBE.cover, "VerdanaCustomCharset32", Color.White, "CrystalOS Alpha", (int)ImprovedVBE.width / 2 - (int)Temp.Width / 2 + 118, 595);

                    ImprovedVBE.Display(Kernel.vbe, false);
                }
                Heap.Collect();
            }
        }
    }
}