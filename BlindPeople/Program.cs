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
        // used by the takeRange function
        GTI.I2CBus gSensor;

        // used by the takeRanges function
        GTI.I2CBus[] gSensors;

        // the frequency in MHz that hubs will run at
        const int freq = 10;

        // when communicating with the device, how long to wait before giving up if
        // we don't get a response. 100ms seems to be a good default value.
        const int timeout = 100;

        void ProgramStarted()
        {
            Debug.Print("Program Started");

            int[] sockets = { 3, 3 };
            byte[] addresses = { 1, 2 };
            startTakingRanges(sockets, addresses);
            
            //changeAddress(3, 3, 4);

            //testAllAddresses(3);
            
            Debug.Print("Initialisation Ended");
        }

        void startTakingRanges(int socketNumber, byte address)
        {
            // initialise the I2CBus on a socket
            const int freq = 10;            // the frequency in MHz that the hub will run at
            GT.Socket socket = GT.Socket.GetSocket(socketNumber, true, null, null);
            gSensor = new GTI.I2CBus(socket, address, freq, null);

            // set up a timer to just take readings constantly
            GT.Timer timer = new GT.Timer(100);
            timer.Tick += new GT.Timer.TickEventHandler(takeRangeTimerTick);
            timer.Start();
        }

        void startTakingRanges(int[] socketNumbers, byte[] addresses)
        {
            int n = addresses.Length;
            gSensors = new GTI.I2CBus[n];

            for (int i = 0; i < n; i++)
            {
                const int freq = 10;
                GT.Socket socket = GT.Socket.GetSocket(socketNumbers[i], true, null, null);
                gSensors[i] = new GTI.I2CBus(socket, addresses[i], freq, null);
            }

            GT.Timer timer = new GT.Timer(100);
            timer.Tick += new GT.Timer.TickEventHandler(takeRangesTimerTick);
            timer.Start();
        }

        void takeRangeTimerTick(GT.Timer timer)
        {
            int range = takeRange(gSensor);

            Debug.Print(range.ToString());
        }

        void takeRangesTimerTick(GT.Timer timer)
        {
            int n = gSensors.Length;

            for (int i = 0; i < n; i++)
            {
                int range = takeRange(gSensors[i]);

                Debug.Print("Sensor " + gSensors[i].Address.ToString() + " reports " + range.ToString());
            }
        }

        // instructs the sensor to take a single reading and returns the result
        // this function blocks while the reading is being taken so may take up to 30-40ms to return
        int takeRange(GTI.I2CBus sensor)
        {
            // send a single byte to the device, telling it to start range finding
            byte[] writeBuffer = new byte[1];
            writeBuffer[0] = 81;
            sensor.Write(writeBuffer, timeout);

            // the device now doesn't answer to read requests until it's done,
            // we'll know it's done once it returns something other than 0
            int range = 0;
            while (range == 0)
            {
                // output is given as a 16bit integer,
                // the most significant byte comming first
                byte[] readBuffer = new byte[2];
                sensor.Read(readBuffer, timeout);
                range = readBuffer[0] * 256 + readBuffer[1];
            }

            return range;
        }

        // changes the given sensor's address to the given value
        // valid addresses are 1-127
        void changeAddress(int socketNumber, byte oldAddress, byte newAddress)
        {
            // initialise the I2CBus on a socket
            const int freq = 10;            // the frequency in MHz that the hub will run at
            GT.Socket socket = GT.Socket.GetSocket(socketNumber, true, null, null);
            GTI.I2CBus sensor = new GTI.I2CBus(socket, oldAddress, freq, null);

            // need to send three bytes to the sensor, the first two
            // 170 and 165 instruct it to change the address,
            // the third byte is the new address
            // the reason for the * 2 is that the address is read
            // as 7 bits and then a trailing zero
            byte[] writeBuffer = new byte[3];
            writeBuffer[0] = 170;
            writeBuffer[1] = 165;
            writeBuffer[2] = (byte)(newAddress * 2);

            sensor.Write(writeBuffer, timeout);
        }

        // tests all possible addresses to see what modules are connected
        void testAllAddresses(int socket)
        {
            for (ushort n = 1; n <= 127; n++)
            {
                Debug.Print("Trying address " + n.ToString() + ":");

                GTI.I2CBus sensor = new GTI.I2CBus(GT.Socket.GetSocket(socket, true, null, null), n, 10, null);

                byte[] writeBuffer = new byte[1];
                writeBuffer[0] = 81;
                sensor.Write(writeBuffer, timeout);

                int range = 0;
                DateTime end = System.DateTime.Now.AddMilliseconds(500);
                while (range == 0 && System.DateTime.Now < end)
                {
                    // output is given as a 16bit integer,
                    // the most significant byte comming first
                    byte[] readBuffer = new byte[2];
                    sensor.Read(readBuffer, timeout);
                    range = readBuffer[0] * 256 + readBuffer[1];
                }

                Debug.Print("Value is " + range.ToString());
            }
        }
    }
}