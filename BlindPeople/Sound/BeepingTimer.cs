using System;
using System.Threading;
using Microsoft.SPOT;

namespace BlindPeople.Sound
{
    class BeepingTimer
    {
        public delegate void Callback();
        private Callback callback;

        //the amount of time between each tick
        private int interval;

        //the time of the last tick
        private DateTime lastCall;

        private Timer timer;

        public BeepingTimer(Callback callback)
        {
            this.callback = callback;

            interval = 0;
            lastCall = DateTime.Now;

            //timer doesn't start ticking
            timer = new Timer(tick, null, Timeout.Infinite, 0);
        }

        //change the interval. if the timer is currently ticking then calcuate from the 
        //time of the last tick, when the next tick is due (with the new interval).
        //if is it past the new time, then then start start the first tick now
        //otherwise, wait until the next tick is due and wait until then
        //interval = 0 means stop beeping 
        public void change(int interval)
        {
            this.interval = interval;

            if (interval == 0)
            {
                timer.Change(Timeout.Infinite, 0);
            }
            else
            {
                TimeSpan delta = DateTime.Now.Subtract(lastCall);
                int duetime = System.Math.Max(0, interval - delta.Seconds * 1000 - delta.Milliseconds);
                timer.Change(duetime, interval);
            }
        }

        //at each tick, call callback() and record the current time
        public void tick(Object obj)
        {
            callback();

            //DateTime.Now is accurate up to 16 milliseconds
            lastCall = DateTime.Now;

        }
    }
}
