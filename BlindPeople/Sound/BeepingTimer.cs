using System;
using System.Threading;
using Microsoft.SPOT;

namespace BlindPeople.Sound
{
    class BeepingTimer
    {
        public delegate void Callback();
        private Callback callback;

        private int interval;
        private DateTime lastCall;

        private Timer timer;

        public BeepingTimer(Callback callback)
        {
            this.callback = callback;

            interval = 0;
            lastCall = DateTime.Now;

            timer = new Timer(tick, null, Timeout.Infinite, 0);
        }

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

        public void tick(Object obj)
        {
            callback();

            lastCall = DateTime.Now;
        }
    }
}
