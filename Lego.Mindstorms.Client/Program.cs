using System;
using System.Threading.Tasks;

namespace Lego.Mindstorms.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Opening connection to Mindstorms EV3 brick using USB...");
            ICommunication communication = new UsbCommunication();
            //ICommunication communication = new Brick(new BluetoothCommunication("COM0"));
            //ICommunication communication = new Brick(new NetworkCommunication("0.0.0.0"));

            Brick brick = new Brick(communication, true);

            Console.WriteLine("Connecting...");
            await brick.ConnectAsync();

            Console.WriteLine("Connected...Turning motor...");
            await brick.DirectCommand.TurnMotorAtPowerForTimeAsync(OutputPort.B, 50, 3000u, false);

            System.Console.WriteLine("Motor turned...beeping...");
            await brick.DirectCommand.PlayToneAsync(50, 5000, 500);

            System.Console.WriteLine("Done...");
        }
    }
}