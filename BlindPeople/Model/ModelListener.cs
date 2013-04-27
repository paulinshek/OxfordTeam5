using System;
using Microsoft.SPOT;
using BlindPeople.Sound;

namespace BlindPeople.DomainModel
{
    public interface ModelListener
    {
        //called when the distance has changed to less then the threshold
        void distanceLessThanThreshold(Direction d);

        //called when the distance has changed from below to above the threshold
        void distanceGreaterThanThreshold(Direction d);
        
        //called when the calibration of the sensors has started
        void calibrationStarted();

        //called when the calibration of the sensors has finished
        void calibrationFinished();
    }
}
