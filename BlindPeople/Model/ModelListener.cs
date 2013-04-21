using System;
using Microsoft.SPOT;
using BlindPeople.Sound;

namespace BlindPeople.DomainModel
{
    interface ModelListener
    {
        public void distanceLessThanThreshold(Direction d);
        public void distanceGreaterThanThreshold(Direction d);
        public void distanceIncreasing(Direction d);
        public void distanceDecreasing(Direction d);
        public void calibrationStarted();
        public void calibrationFinished();
    }
}
