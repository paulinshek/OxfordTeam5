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

        private BeepingTimer timerL, timerR, timerF;

        const double volume = 0.008;
        const int duration = 30;

        public TunesListener(TunesModule tunesL, TunesModule tunesR)
        {
            this.tunesL = tunesL;
            this.tunesR = tunesR;

            timerL = new BeepingTimer(beepingFunctionLeft);
            timerR = new BeepingTimer(beepingFunctionRight);
            timerF = new BeepingTimer(beepingFunctionFront);
        }

        //one beep for the the left tunes module
        private void beepingFunctionLeft()
        {
            tunesL.play(262, volume);
            Thread.Sleep(duration);
            tunesL.stop();
        }

        //one beep for the right tunes module
        private void beepingFunctionRight()
        {
            tunesR.play(415, volume);
            Thread.Sleep(duration);
            tunesR.stop();
        }
        

        //two beeps for both tunes modules at the same time
        //this is used to represent obstacles in front of the user
        //440 is A4
        private void beepingFunctionFront()
        {

            tunesL.play(660, volume);
            tunesR.play(660, volume);
            Thread.Sleep(duration);
            tunesL.stop();
            tunesR.stop();
        }

        //calculates the period of beeping needed for if the nearest obstacle is
        //distance cm away.
        private int calculatePeriod(int distance)
        {
            int minPeriod = 200;
            int maxPeriod = 2000;
            int minDistance = 40;
            int maxDistance = 400;

            // clamp it between minDistance and maxDistance
            int d = System.Math.Max(minDistance, System.Math.Min(maxDistance, distance));

            return minPeriod + (d - minDistance) * (maxPeriod - minPeriod) / (maxDistance - minDistance);
        }

        //start beeping. speed of beeps depend on distance
        public void distanceLessThanThreshold(Direction d, int distance)
        {
            if (d == Direction.Left) timerL.change(calculatePeriod(distance));
            else if (d == Direction.Right) timerR.change(calculatePeriod(distance));
            else if (d == Direction.Front)
            {
                Debug.Print(distance.ToString());
                timerF.change(calculatePeriod(distance));
            }
        }

        //stop beeping
        public void distanceGreaterThanThreshold(Direction d)
        {
            if (d == Direction.Left) timerL.change(0);
            else if (d == Direction.Right) timerR.change(0);
            else if (d == Direction.Front) timerF.change(0);
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
