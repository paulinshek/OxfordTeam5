using System;
using Microsoft.SPOT;
using BlindPeople.Sound;

namespace BlindPeople.DomainModel
{
    public interface ModelListener
    {
        //called when a range is taken that is less than the threshold
        void distanceLessThanThreshold(Direction d, int distance);

        //called when a range is taken that is greater than the threshold
        void distanceGreaterThanThreshold(Direction d);
        
        //called when the calibration of the sensors has started
        void calibrationStarted();

        //called when the calibration of the sensors has finished
        void calibrationFinished();
    }
}
