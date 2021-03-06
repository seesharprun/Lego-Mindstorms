﻿using Lego.Mindstorms;
using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Opening connection to Mindstorms EV3 brick using USB");

        //using (BluetoothCommunication communication = new BluetoothCommunication("COM0"))
        //using (NetworkCommunication communication = new NetworkCommunication("0.0.0.0"))
        using (UsbCommunication communication = new UsbCommunication())
        using (MindstormsClient<UsbCommunication> client = new MindstormsClient<UsbCommunication>(communication))
        {
            Console.WriteLine("Connecting");
            await client.ConnectAsync();

            Console.WriteLine("Direct command");
            await client.TurnMotorAtPowerForTimeAsync(OutputPort.A, 50, 3000u, false);

            System.Console.WriteLine("System command");
            await client.PlayToneAsync(50, 5000, 500);

            System.Console.WriteLine("Batch commands");
            var command = new Command();
            command.TurnMotorAtPowerForTime(OutputPort.A, 50, 2500, false);
            command.PlayTone(50, 1000, 5000);
            await client.SendCommandAsync(command);
        }

        System.Console.WriteLine("Done...");
    }
}