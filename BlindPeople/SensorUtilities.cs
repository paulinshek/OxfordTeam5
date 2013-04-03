using System;
using Microsoft.SPOT;

using GT = Gadgeteer;
using GTI = Gadgeteer.Interfaces;

namespace BlindPeople
{
    class SensorUtilities
    {
        // when communicating with the device, how long to wait before giving up if
        // we don't get a response. 100ms seems to be a good default value.
        const int timeout = 100;

        // changes the given sensor's address to the given value
        // valid addresses are 1-127
        static void changeAddress(int socketNumber, byte oldAddress, byte newAddress)
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
                GTI.I2CBus sensor = new GTI.I2CBus(GT.Socket.GetSocket(socket, true, null, null), n, 10, null);

                byte[] writeBuffer = new byte[1];
                writeBuffer[0] = 81;
                sensor.Write(writeBuffer, timeout);

                int range = 0;
                DateTime end = System.DateTime.Now.AddMilliseconds(200);
                while (range == 0 && System.DateTime.Now < end)
                {
                    // output is given as a 16bit integer,
                    // the most significant byte comming first
                    byte[] readBuffer = new byte[2];
                    sensor.Read(readBuffer, timeout);
                    range = readBuffer[0] * 256 + readBuffer[1];
                }

                if (range != 0)
                {
                    Debug.Print("Found sensor at address " + n.ToString());
                }
            }
        }
    }
}
