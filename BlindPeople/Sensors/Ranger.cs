using System;
using System.Threading;
using Microsoft.SPOT;

using GT = Gadgeteer;
using GTI = Gadgeteer.Interfaces;

namespace BlindPeople.Sensors
{
    // The purpose of this class is to automatically perform range finds of
    // multiple sensors so that the calling code can have the latest ranges
    // at any point.
    // You may have multiple instances of this class at one time however they
    // should not try to use the same sensors simultaneously.
    class Ranger
    {
        //class containing distance reading and the identity of the ranger frem which it was taken from
        public class SensorData : Microsoft.SPOT.EventArgs
        {
            public int id;
            public int dist;
            public SensorData(int id, int dist)
            {
                this.id = id;
                this.dist = dist;
            }
        }

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

        // is the rangingThread currently running or not
        bool isRangingThreadRunning;
        
        public event EventHandler MeasurementComplete;

        // Initialises the ranger and starts ranging automatically.
        // The arguemnts should be two arrays of equal length, the first being
        // the socket on the mainboard used by that sensor and the second being
        // the addrses used by that sensor.
        // Once started, the sensors will be labelled 0,1,2,... in the order
        // that they appear in these arrays
        public Ranger(int[] socketNumbers, byte[] addresses)
        {
            numSensors = addresses.Length;
            sensors = new GTI.I2CBus[numSensors];
            ranges = new int[numSensors];

            // create the busses for communicating with the sensors
            for (int i = 0; i < numSensors; i++)
            {
                GT.Socket socket = GT.Socket.GetSocket(socketNumbers[i], true, null, null);
                sensors[i] = new GTI.I2CBus(socket, addresses[i], freq, null);
            }

            // create the ranging thread and start it going
            rangingThread = new Thread(new ThreadStart(takeRanges));
            rangingThread.Start();
        }

        // starts the ranging thead,
        // safe to call multiple times
        public void startRanging()
        {
            Monitor.Exit(this);
            isRangingThreadRunning = true;
        }

        // stops the ranging thread,
        // safe to call multiple times
        public void stopRanging()
        {
            if (isRangingThreadRunning)
            {
                Monitor.Enter(this);
                isRangingThreadRunning = false;
            }
        }

        // return the most recent range from the specified sensor
        // safe to call with an invalid index, but please don't
        public int requestRange(int i)
        {
            if (0 <= i && i < numSensors)
            {
                return ranges[i];
            }
            else
            {
                return 0;
            }
        }

        // performs a range find on all sensors and store the results in the ranges array,
        // should be run in a separate thread as it never terminates
        private void takeRanges()
        {
            while (true)
            {
                // pausing and resuming the thread is controlled by this monitor,
                // we can only enter if the controlling thread allows us
                Monitor.Enter(this);
                
                // perform a range find of all sensors in sequence
                for (int i = 0; i < numSensors; i++)
                {
                    // store the results in the ranges array, don't worry about
                    // locking the array through a monitor, if someone does access
                    // it midway and gets half of the previous ranges then it's no problem
                    ranges[i] = takeRange(sensors[i]);

                    //raise an event
                    var handler = MeasurementComplete;
                    if (handler != null) handler(this, new SensorData(i,ranges[i]));
                    
                    // sleep briefly between range finds, this is to allow ultrasonic waves
                    // to dissipate, if not performed then we get erroneous ranges
                    Thread.Sleep(100);
                }

                // signal that we've finished for this pass
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

        public int getNumSensors()
        {
            return numSensors;
        }
    }
}
