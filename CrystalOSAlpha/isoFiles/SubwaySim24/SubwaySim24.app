#Define Window_Main
{
    this.Title = "SubwaySim 24";
    this.X = 0;
    this.Y = 0;
    this.Width = 1920;
    this.Height = 1005;
    this.Titlebar = true;
    this.RGB = 60, 60, 60;
    PictureBox Interior = new PictureBox(0, 0, "1:\SubwaySim24\Assets\Interior.bmp");
    PictureBox Tunnel = new PictureBox(0, 0, "1:\SubwaySim24\Assets\Tunnel1.bmp");
    
    Label VehicleName = new Label(125, 853, VehicleBrand, 255, 255, 255);
    Label VehicleL = new Label(125, 880, VehicleLength, 255, 255, 255);
    Label VehicleW = new Label(125, 907, VehicleWeight, 255, 255, 255);
    Label VehicleMaxS = new Label(125, 934, VehicleMaxSpeed, 255, 255, 255);

    Label VehicleS = new Label(345, 853, VehicleSpeed, 255, 255, 255);
    Label VehicleT = new Label(345, 880, VehicleThrotle, 255, 255, 255);
    Label VehicleB = new Label(345, 907, VehicleBreak, 255, 255, 255);

    Button ThrotleUp = new Button(1660, 540, 225, 60, "Throtle Up", 1, 1, 1);
    Button ThrotleDown = new Button(1660, 620, 225, 60, "Throtle Down", 1, 1, 1);
}
#Define Variables
{
    string VehicleBrand = "Test subway";
    string VehicleLength = "25 Meter";
    string VehicleWeight = "34 Tonn";
    string VehicleMaxSpeed = "120 KM/H";
    int VehicleSpeed = 0;
    string VehicleGear = "Neutral";
    string VehicleThrotle = "0";
    string VehicleBreak = "0";
}
#void Looping
{
    //speed.Content = VehicleSpeed;
}
#OnClick ThrotleUp
{
    VehicleSpeed += 10;
    VehicleS.Content = VehicleSpeed;
}