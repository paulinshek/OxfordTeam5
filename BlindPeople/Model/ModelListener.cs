using System;
using Microsoft.SPOT;
using BlindPeople.Sound;

namespace BlindPeople.DomainModel
{
    interface ModelListener
    {
        public void distanceLessThanThreshold();
        public void distanceGreaterThanThreshold();
        public void calibrationStarted();
        public void calibrationFinished();
    }
}
