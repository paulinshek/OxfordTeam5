using System;
using Microsoft.SPOT;

using GT = Gadgeteer;
using GTI = Gadgeteer.Interfaces;

using BlindPeople.DomainModel;

namespace BlindPeople.Sound
{
    public class TunesModule
    {
        private GT.Socket socket;
        private GTI.PWMOutput pwm;

        private bool isBeeping;
        private int beepFreq;

        // Initialises the tunes module to be on the given socket.
        public TunesModule(int socketNumber, DomainModel.Model model)
        {
            socket = GT.Socket.GetSocket(socketNumber, true, null, null);

            pwm = new GTI.PWMOutput(socket, GT.Socket.Pin.Nine, false, null);

            isBeeping = false;
            beepFreq = 800;
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

        //start beeping makes a beeping sound
        public void startBeeping(int freq, double volume)
        {
            isBeeping = true;
            while (isBeeping)
            {
                play(freq, volume, 250);
                System.Threading.Thread.Sleep(beepFreq);
            }
        }

        //stops the beeping sound
        public void stopBeeping()
        {
            isBeeping = false;
        }

        //sets the speed of the beeping sound
        //can be used when isBeeping to increase the number of 
        //beeps per second
        public void setBeepFreq(int bpm)
        {
            beepFreq = (60 / bpm) * 1000;
        }

        public bool getIsBeeping()
        {
            return isBeeping;
        }


        class TunesModelListener : ModelListener
        {
            TunesModule tunes;

            public TunesModelListener(TunesModule tunes)
            {
                this.tunes = tunes;
            }

            //grade is from 0 to 5
            public void distanceLessThanThreshold(int grade)
            {
                tunes.setBeepFreq(100 + (8 * grade));

                if (tunes.getIsBeeping() == false)
                {
                    tunes.startBeeping(440, 0.05);
                }
            }

            public void distanceGreaterThanThreshold()
            {
                tunes.stopBeeping();
            }

            public void calibrationStarted()
            {
                tunes.play(440, 0.05);
            }

            public void calibrationFinished()
            {
                tunes.stop();
            }
        }

    }
}