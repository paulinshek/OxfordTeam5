using System;
using Microsoft.SPOT;
using BlindPeople.Sound;

namespace BlindPeople.DomainModel
{
    public interface ModelListener
    {
        void distanceLessThanThreshold(Direction d);
        void distanceGreaterThanThreshold(Direction d);
        void distanceIncreasing(Direction d);
        void distanceDecreasing(Direction d);
        void calibrationStarted();
        void calibrationFinished();
    }
}
