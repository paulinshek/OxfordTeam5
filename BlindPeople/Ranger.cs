using System;
using System.Threading;
using Microsoft.SPOT;

using GT = Gadgeteer;
using GTI = Gadgeteer.Interfaces;

namespace BlindPeople
{
    class Ranger
    {
        // how many sensors are we using
        int numSensors;

        // the actual sensors
        GTI.I2CBus[] sensors;

        // the most recent ranges taken
        int[] ranges;

        // the frequency in MHz that hubs will run at
        const int freq = 10;

        // when communicating with the device, how long to wait before giving up if
        // we don't get a response. 100ms seems to be a good default value.
        const int timeout = 100;

        // the thread that constantly takes ranges in the background
        Thread rangingThread;

        public Ranger(int[] socketNumbers, byte[] addresses)
        {
            numSensors = addresses.Length;
            sensors = new GTI.I2CBus[numSensors];
            ranges = new int[numSensors];

            for (int i = 0; i < numSensors; i++)
            {
                GT.Socket socket = GT.Socket.GetSocket(socketNumbers[i], true, null, null);
                sensors[i] = new GTI.I2CBus(socket, addresses[i], freq, null);
            }

            rangingThread = new Thread(new ThreadStart(takeRanges));
            rangingThread.Start();
        }

        // starts the ranging thead
        public void startRanging()
        {
            Monitor.Exit(this);
        }

        // stops the ranging thread
        public void stopRanging()
        {
            Monitor.Enter(this);
        }

        // take all ranges and store the results in the ranges array
        private void takeRanges()
        {
            while (true)
            {
                Monitor.Enter(this);
                
                for (int i = 0; i < numSensors; i++)
                {
                    ranges[i] = takeRange(sensors[i]);
                    Thread.Sleep(100);
                }

                Monitor.Exit(this);
            }
        }

        // instructs the sensor to take a single reading and returns the result
        // this function blocks while the reading is being taken so may take up to 30-40ms to return
        private int takeRange(GTI.I2CBus sensor)
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

        // return the most recent range from the specified sensor
        public int getRange(int i)
        {
            return ranges[i];
        }
    }
}
