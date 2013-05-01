using System;
using Microsoft.SPOT;
using BlindPeople.DomainModel;
using System.Threading;

namespace BlindPeople.Sound
{
    class TunesListener : ModelListener
    {
        private TunesModule tunesL, tunesR;

        private BeepingTimer timerL, timerR, timerF;

        public TunesListener(TunesModule tunesL, TunesModule tunesR)
        {
            this.tunesL = tunesL;
            this.tunesR = tunesR;

            timerL = new BeepingTimer(beepingFunctionLeft);
            timerR = new BeepingTimer(beepingFunctionRight);
            timerF = new BeepingTimer(beepingFunctionFront);
        }

        private void beepingFunctionLeft()
        {
            tunesL.play(440, 0.05);
            Thread.Sleep(50);
            tunesL.stop();
        }

        private void beepingFunctionRight()
        {
            tunesR.play(440, 0.05);
            Thread.Sleep(50);
            tunesR.stop();
        }
        
        private void beepingFunctionFront()
        {
            tunesL.play(440, 0.05);
            tunesR.play(440, 0.05);
            Thread.Sleep(50);
            tunesL.stop();
            tunesR.stop();
        }

        private int calculatePeriod(int distance)
        {
            return 250 + (distance - 20) * (1500 - 250) / (400 - 20);
        }

        public void distanceLessThanThreshold(Direction d, int distance)
        {
            if (d == Direction.Left) timerL.change(calculatePeriod(distance));
            if (d == Direction.Right) timerR.change(calculatePeriod(distance));
            if (d == Direction.Front) timerF.change(calculatePeriod(distance));
        }

        public void distanceGreaterThanThreshold(Direction d)
        {
            if (d == Direction.Left) timerL.change(0);
            if (d == Direction.Right) timerR.change(0);
            if (d == Direction.Front) timerF.change(0);
        }

        public void calibrationStarted()
        {
            tunesL.play(440, 0.05);
            tunesR.play(440, 0.05);
        }

        public void calibrationFinished()
        {
            tunesL.stop();
            tunesR.stop();
        }
    }
}
