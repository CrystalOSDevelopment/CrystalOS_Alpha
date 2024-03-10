#Define Window_Main
{
    this.Title = "SubwaySim 24";
    this.X = 0;
    this.Y = 0;
    this.Width = 1920;
    this.Height = 1005;
    this.Titlebar = true;
    this.RGB = 60, 60, 60;
    PictureBox Interior = new PictureBox(0, 0, "1:\SubwaySim24\Assets\Interior.bmp", true);
    //PictureBox Tunnel = new PictureBox(0, 0, "1:\SubwaySim24\Assets\Tunnel1.bmp", true);
    PictureBox Tunnel = new PictureBox(0, 0, 1920, 1080, true);

    //Points
    //Rails

    //Rail1
    //Left side
    Point p1 = new Point(770, 720);
    Point p2 = new Point(954, 463);
    Point p3 = new Point(993, 410);
    Point p4 = new Point(1009, 387);
    Point p40 = new Point(1024, 369);
    
    //Right side
    Point p11 = new Point(1030, 370);
    Point p21 = new Point(1015, 390);
    Point p31 = new Point(1000, 411);
    Point p41 = new Point(962, 467);
    Point p42 = new Point(788, 720);

    Point p5 = new Point(1097, 720);
    Point p6 = new Point(1079, 360);
    Point p7 = new Point(1083, 363);
    Point p8 = new Point(1116, 720);
    
    //Station
    Point First = new Point(885, 385);
    Point Second = new Point(890, 388);
    Point Third = new Point(869, 407);
    Point Fourth = new Point(869, 466);
    Point Fifth = new Point(563, 717);
    Point Sixth = new Point(261, 719);
    Point Seventh = new Point(231, 567);

    //Ground
    Point Ground1 = new Point(231, 574);
    Point Ground2 = new Point(973, 332);
    Point Ground3 = new Point(1089, 334);
    Point Ground4 = new Point(1287, 720);
    Point Ground5 = new Point(230, 720, 720);

    //Tunnel
    //Left Side
    Point T1 = new Point(1, 505);
    Point T2 = new Point(2, 1);
    Point T3 = new Point(987, 1);
    Point T4 = new Point(986, 185);
    Point T5 = new Point(947, 220);
    Point T6 = new Point(935, 278);
    Point T7 = new Point(970, 332);
    Point T8 = new Point(619, 452);
    Point T9 = new Point(507, 505);
    Point T10 = new Point(0, 505);
    //Right Side
    Point T11 = new Point(985, 1);
    Point T12 = new Point(1920, 1);
    Point T13 = new Point(1919, 505);
    Point T14 = new Point(1920, 505);
    Point T15 = new Point(1412, 505);
    Point T16 = new Point(1140, 435);
    Point T17 = new Point(1090, 333);
    Point T18 = new Point(1114, 273);
    Point T19 = new Point(1098, 216);
    Point T20 = new Point(1055, 185);
    Point T21 = new Point(986, 185);

    Tunnel.Clear(42, 42, 42);
    Tunnel.FilledPollygon(25, 25, 25, Ground1, Ground2, Ground3, Ground4, Ground5);
    Tunnel.FilledPollygon(64, 63, 60, p1, p2, p3, p4, p40, p11, p21, p31, p41, p42);
    Tunnel.FilledPollygon(64, 63, 60, p5, p6, p7, p8);
    Tunnel.FilledPollygon(64, 63, 60, First, Second, Third, Fourth, Fifth, Sixth, Seventh);
    Tunnel.FilledPollygon(30, 30, 30, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10);
    Tunnel.FilledPollygon(30, 30, 30, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21);

    //Game Graphics
    Interior.MergeOnto(Tunnel);
    
    Label VehicleName = new Label(125, 853, VehicleBrand, 255, 255, 255);
    Label VehicleL = new Label(125, 880, VehicleLength, 255, 255, 255);
    Label VehicleW = new Label(125, 907, VehicleWeight, 255, 255, 255);
    Label VehicleMaxS = new Label(125, 934, VehicleMaxSpeed, 255, 255, 255);

    Label VehicleS = new Label(345, 853, VehicleSpeed, 255, 255, 255);
    Label VehicleT = new Label(345, 880, VehicleThrotle, 255, 255, 255);
    Label VehicleB = new Label(345, 907, VehicleBreak, 255, 255, 255);

    Label Time = new Label(1480, 25, Seconds, 255, 162, 0);
    Label WaitTime = new Label(1480, 50, WaitInStation, 255, 162, 0);
    Label Travelled = new Label(1480, 75, TravelledDistance, 255, 162, 0);
    //Control UI
    //Throtle
    Button ThrotleUp = new Button(1660, 540, 225, 60, "Throtle Up", 1, 1, 1);
    Button ThrotleDown = new Button(1660, 620, 225, 60, "Throtle Down", 1, 1, 1);
    //Index
    Button IndexL = new Button(1660, 463, 104, 60, "Index Left", 1, 1, 1);
    Button IndexR = new Button(1781, 463, 104, 60, "Index Right", 1, 1, 1);
    //Door handling
    Button DoorL = new Button(1660, 385, 104, 60, "Door Left", 1, 1, 1);
    Button DoorR = new Button(1781, 385, 104, 60, "Door Right", 1, 1, 1);
    //Horn
    Button Horn = new Button(1660, 308, 225, 60, "Horn", 1, 1, 1);
}
#Define Variables
{
    //Vehicle status
    string VehicleBrand = "Test subway";
    string VehicleLength = "25 Meter";
    string VehicleWeight = "34 Tonn";
    string VehicleMaxSpeed = "120 KM/H";
    string VehicleGear = "Neutral";
    string VehicleThrotle = "0";
    string VehicleBreak = "0";
    int VehicleSpeed = 0;
    bool Horn = false;

    //Game mechanics
    int Seconds = 0;
    int Now = DateTime.UtcNow.Second;
    int WaitInStation = 8;
    int TravelledDistance = 0;

    //Game Rendering
    int Rail_Centering = 0;
    bool Change = false;

    //Player status
    int Points = 0;
}
#void Looping
{
    //Gametic
    int CurrentSecond = DateTime.UtcNow.Second;
    if(CurrentSecond != Now)
    {
        //Time spent in-game
        Seconds += 1;
        Now = CurrentSecond;
        string temp = "Ellapsed time: " + Seconds + "s";
        Time.Content = temp;

        //Time spent waiting in station
        if(WaitInStation > 0)
        {
            WaitInStation -= 1;
            string countBack = "You can leave the station after " + WaitInStation + " seconds.";
            WaitTime.Content = countBack;
            WaitTime.Color = 255, 162, 0;
        }
        else
        {
            string countBack = "You can now leave the station! Drive safe!";
            WaitTime.Content = countBack;
            WaitTime.Color = 0, 255, 0;
        }
        //Measure distance in meter
        int Dist = VehicleSpeed * 0.277778;
        TravelledDistance += Dist;
        string TravelledDist = "Distance travelled: " + TravelledDistance + " meter(s)";
        Travelled.Content = TravelledDist;
        
        //Rendering railway
        if(TravelledDistance == 10)
        {
            p3.X += 3;
            p4.X += 6;
            p40.X += 8;
            
            p31.X += 3;
            p21.X += 6;
            p11.X += 8;
        }
        else if(TravelledDistance == 20)
        {
            p4.X -= 3;
            p40.X -= 4;
            
            p21.X -= 3;
            p11.X -= 4;
        }
        else if(TravelledDistance == 40)
        {
            p3.X -= 3;
            p4.X -= 3;
            p40.X -= 4;
            
            p31.X -= 3;
            p21.X -= 3;
            p11.X -= 4;
        }
        //End of rendering railway

        Tunnel.Clear(42, 42, 42);
        Tunnel.FilledPollygon(25, 25, 25, Ground1, Ground2, Ground3, Ground4, Ground5);
        Tunnel.FilledPollygon(64, 63, 60, p1, p2, p3, p4, p40, p11, p21, p31, p41, p42);
        Tunnel.FilledPollygon(64, 63, 60, p5, p6, p7, p8);
        Tunnel.FilledPollygon(64, 63, 60, First, Second, Third, Fourth, Fifth, Sixth, Seventh);

        Tunnel.FilledPollygon(30, 30, 30, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10);
        Tunnel.FilledPollygon(30, 30, 30, T11, T12, T13, T14, T15, T16, T17, T18, T19, T20, T21);
        Interior.MergeOnto(Tunnel);
    }
    //End of Gametic
}
#OnClick ThrotleUp
{
    if(VehicleSpeed < 120)
    {
        VehicleSpeed += 10;
        VehicleS.Content = VehicleSpeed;
    }
    if(VehicleSpeed > 80)
    {
        VehicleT.Content = "5";
    }
    if(VehicleSpeed < 80)
    {
        VehicleT.Content = "4";
    }
    if(VehicleSpeed < 60)
    {
        VehicleT.Content = "3";
    }
    if(VehicleSpeed < 40)
    {
        VehicleT.Content = "2";
    }
    if(VehicleSpeed < 20)
    {
        VehicleT.Content = "1";
    }
}
#OnClick ThrotleDown
{
    if(0 < VehicleSpeed)
    {
        VehicleSpeed -= 10;
        VehicleS.Content = VehicleSpeed;
    }
    if(VehicleSpeed > 80)
    {
        VehicleT.Content = "5";
    }
    if(VehicleSpeed < 80)
    {
        VehicleT.Content = "4";
    }
    if(VehicleSpeed < 60)
    {
        VehicleT.Content = "3";
    }
    if(VehicleSpeed < 40)
    {
        VehicleT.Content = "2";
    }
    if(VehicleSpeed < 20)
    {
        VehicleT.Content = "1";
    }
}
#OnClick Horn
{
    PCSpeaker.Beep(350, 1200);
}