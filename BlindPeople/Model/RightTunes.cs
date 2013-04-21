using System;
using Microsoft.SPOT;
using BlindPeople.Sound;

namespace BlindPeople.DomainModel
{
    //this class is for the tunes modules which will be on the left side of the user's head
    class RightTunes : ModelListener
    {
        TunesModule tunes;

        public RightTunes(TunesModule tunes)
        {
            this.tunes = tunes;
        }

        public void distanceLessThanThreshold()
        {
            //TODO: RightTunes
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
