using System;
using Microsoft.SPOT;
using BlindPeople.DomainModel;
using System.Threading;

namespace BlindPeople.Sound
{
    //manages the left and right tunes module
    class TunesListener : ModelListener
    {
        private TunesModule tunesL, tunesR;

        private BeepingTimer timerL, timerR;

        public TunesListener(TunesModule tunesL, TunesModule tunesR)
        {
            this.tunesL = tunesL;
            this.tunesR = tunesR;

            timerL = new BeepingTimer(beepingFunctionLeft);
            timerR = new BeepingTimer(beepingFunctionRight);
        }

        //one beep for the the left tunes module
        private void beepingFunctionLeft()
        {
            tunesL.play(440, 0.05);
            Thread.Sleep(50);
            tunesL.stop();
        }

        //one beep for the right tunes module
        private void beepingFunctionRight()
        {
            tunesR.play(440, 0.05);
            Thread.Sleep(50);
            tunesR.stop();
        }
        

        //two beeps for both tunes modules at the same time
        //this is used to represent obstacles in front of the user
        //440 is A4
        private void twoBeepsFront()
        {

            tunesL.play(440, 0.05);
            tunesR.play(440, 0.05);
            Thread.Sleep(50);
            tunesL.stop();
            tunesR.stop();

            Thread.Sleep(50);
            
            tunesL.play(440, 0.05);
            tunesR.play(440, 0.05);
            Thread.Sleep(50);
            tunesL.stop();
            tunesR.stop();
        }

        //calculates the period of beeping needed for if the nearest obstacle is
        //distance cm away.
        private int calculatePeriod(int distance)
        {
            return 250 + (distance - 20) * (1500 - 250) / (400 - 20);
        }

        //start beeping. speed of beeps depend on distance
        public void distanceLessThanThreshold(Direction d, int distance)
        {
            if (d == Direction.Left) timerL.change(calculatePeriod(distance));
            else if (d == Direction.Right) timerR.change(calculatePeriod(distance));
            else if (d == Direction.Front) twoBeepsFront();
        }

        //stop beeping
        public void distanceGreaterThanThreshold(Direction d)
        {
            if (d == Direction.Left) timerL.change(0);
            else if (d == Direction.Right) timerR.change(0);
        }

        //660 is E5
        //ie. 2 Es above middle C
        public void calibrationStarted()
        {
            tunesL.play(660, 0.05);
            tunesR.play(660, 0.05);
        }

        public void calibrationFinished()
        {
            tunesL.stop();
            tunesR.stop();
        }
    }
}
