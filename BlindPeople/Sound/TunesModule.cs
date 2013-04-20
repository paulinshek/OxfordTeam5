using System;
using Microsoft.SPOT;

using GT = Gadgeteer;
using GTI = Gadgeteer.Interfaces;

namespace BlindPeople.Sound
{
    public class TunesModule
    {
        private GT.Socket socket;
        private GTI.PWMOutput pwm;

        // Initialises the tunes module to be on the given socket.
        public TunesModule(int socketNumber)
        {
            socket = GT.Socket.GetSocket(socketNumber, true, null, null);

            pwm = new GTI.PWMOutput(socket, GT.Socket.Pin.Nine, false, null);
        }

        // Plays a continuous tone from the tunes module at the given frequency (pitch) and volume.
        // For volume, it must be between 0.0 and 1.0 with 0.5 being the loudest,
        // it is symetric around 0.5, so 0.1 sounds the same as 0.9,
        // for reference:
        //     0.5 is very loud, too loud for our use
        //     0.005 is good if the module is attached to the head
        //     0.002 is good if the module is right in the ear
        public void play(int freq, double volume)
        {
            pwm.Set(freq, volume);
        }

        // Stops the tunes module from playing
        public void stop()
        {
            pwm.Active = false;
        }

        // Play a tone at the given frequency and volume for the given duration
        public void play(int freq, double volume, int duration)
        {
            play(freq, volume);
            System.Threading.Thread.Sleep(duration);
            stop();
        }
    }
}