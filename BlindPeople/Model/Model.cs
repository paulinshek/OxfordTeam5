using System;
using System.Collections;
using Microsoft.SPOT;
using GT = Gadgeteer;
using BlindPeople.Sensors;

namespace BlindPeople.DomainModel
{
    public class Model
    {
        int numSensors;

        //maps between sensors and their identities.
        const int leftSide = 3;
        const int leftForward = 2;
        const int rightForward = 1;
        const int rightSide = 0;
        
        //the number of readings to keep for each sensor
        const int maxReadings = 10;
        
        //stores the most recent maxReadings readings for each ultrasonic sensor
        //(array of LimitedLists)
        LimitedList[] sensorArray;

        ArrayList modelListeners;

        //for reference: sensor readings are accurate from 20cm and to 4m

        //there are two modes for the user: "HighThreshold" and "LowThreshold"
        //it enables to user to ignore readings that are above 50cm, which might
        //be useful if they are in a cramped environment such as a tunnel, or 
        //somewhere 
        int currentThreshold;
        const int HighThreshold = 400;
        const int LowThreshold = 50;
        
        public Model(int numSensors)
        {
            this.numSensors = numSensors;

            //initialise the array
            sensorArray = new LimitedList[numSensors];
            for (int i = 0; i < numSensors; i++)
            {
                sensorArray[i] = new LimitedList(maxReadings);
            }

            modelListeners = new ArrayList();

            currentThreshold = HighThreshold;
        }

        // take a range, store the results and inform any listeners
        // treat range from front sensors as a special case - only warn the user if distance is 
        //less than frontWarning
        public void updateRange(int i, int range)
        {
            sensorArray[i].add(range);
            Direction d = (i == leftSide) ? Direction.Left : (i == rightSide) ? Direction.Right : Direction.Front;

            if (range < currentThreshold)
            {
                fireDistanceLessThanThreshold(d, range);
            } else {
                fireDistanceGreaterThanThreshold(d);
            }
        }

        //calibration in the sensors has started
        public void calibrationStarted()
        {
            fireCalibrationStarted();
        }

        //calibration in the sensors has finished
        public void calibrationFinished()
        {
            fireCalibrationFinished();
        }

        //switch form HighThreshold to LowThreshold and vice versa
        public void changeThreshold()
        {
            if (currentThreshold == HighThreshold) currentThreshold = LowThreshold;
            else currentThreshold = HighThreshold;

        }

        public void addModelListener(ModelListener l)
        {
            modelListeners.Add(l);
        }

        private void fireDistanceLessThanThreshold(Direction d, int distance)
        {
            foreach (ModelListener l in modelListeners)
            {
                l.distanceLessThanThreshold(d, distance);
            }
        }

        private void fireDistanceGreaterThanThreshold(Direction d)
        {
            foreach (ModelListener l in modelListeners)
            {
                l.distanceGreaterThanThreshold(d);
            }
        }

        private void fireCalibrationStarted()
        {
            foreach (ModelListener l in modelListeners)
            {
                l.calibrationStarted();
            }
        }

        private void fireCalibrationFinished()
        {
            foreach (ModelListener l in modelListeners)
            {
                l.calibrationFinished();
            }
        }
    }
}
