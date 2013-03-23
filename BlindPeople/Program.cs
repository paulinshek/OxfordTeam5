using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using GTI = Gadgeteer.Interfaces;
using Gadgeteer.Modules.GHIElectronics;

namespace GadgeteerApp1
{
    public partial class Program
    {
        // needs to be global as it's initialised once and then used many times to take readings
        GTI.I2CBus i2c;

        void ProgramStarted()
        {
            Debug.Print("Program Started");

            // initialise the I2CBus on a socket
            const int socketNumber = 3;     // which socket on the mainboard is being used
            const int address = 112;        // the address of the I2C module, 112 is the factory default
            const int freq = 50;            // the frequency in MHz that the hub will run at
            GT.Socket socket = GT.Socket.GetSocket(socketNumber, true, null, null);
            i2c = new GTI.I2CBus(socket, address, freq, null);

            // set up a timer to just take readings constantly
            GT.Timer timer = new GT.Timer(100);
            timer.Tick += new GT.Timer.TickEventHandler(timer_Tick);
            timer.Start();
        }

        void timer_Tick(GT.Timer timer)
        {
            int range = takeReading();

            Debug.Print(range.ToString());
        }

        // instructs the sensor to take a single reading and returns the result
        // this function blocks while the reading is being taken so may take up to 30-40ms to return
        int takeReading()
        {
            // when communicating with the device, how long to wait before giving up if
            // we don't get a response. 100ms seems to be a good default value.
            const int timeout = 100;

            // send a single byte to the device, telling it to start range finding
            byte[] writeBuffer = new byte[1];
            writeBuffer[0] = 81;
            i2c.Write(writeBuffer, timeout);

            // the device now doesn't answer to read requests until it's done,
            // we'll know it's done once it returns something other than 0
            int range = 0;
            while (range == 0)
            {
                // output is given as a 16bit integer,
                // the most significant byte comming first
                byte[] readBuffer = new byte[2];
                i2c.Read(readBuffer, timeout);
                range = readBuffer[0] * 256 + readBuffer[1];
            }

            return range;
        }
    }
}
