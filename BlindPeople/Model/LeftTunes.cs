using System;
using Microsoft.SPOT;
using BlindPeople.Sound;

namespace BlindPeople.DomainModel
{
    //this class is for the tunes modules which will be on the left side of the user's head
    class LeftTunes : ModelListener
    {
        TunesModule tunes;

        public LeftTunes(TunesModule tunes)
        {
            //TODO: LeftTunes
            this.tunes = tunes;
        }


        public void distanceLessThanThreshold()
        {
            throw new NotImplementedException();
        }

        public void distanceGreaterThanThreshold()
        {
            throw new NotImplementedException();
        }

        public void calibrationStarted()
        {
            throw new NotImplementedException();
        }

        public void calibrationFinished()
        {
            throw new NotImplementedException();
        }
    }
}
