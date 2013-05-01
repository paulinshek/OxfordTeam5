using System;
using System.Collections;
using Microsoft.SPOT;
using GT = Gadgeteer;
using BlindPeople.Sensors;
using Gadgeteer.Modules.Seeed;

namespace BlindPeople.DomainModel
{
    public class Model
    {
        int numSensors;

        //maps between sensors and their identities.
        const int leftSide = 0;
        const int leftForward = 1;
        const int rightForward = 2;
        const int rightSide = 3;
        
        //the number of readings to keep for each sensor
        const int maxReadings = 10;
        
        //stores the most recent maxReadings readings for each ultrasonic sensor
        //(array of LimitedLists)
        LimitedList<int>[] sensorArray;

        ArrayList modelListeners;

        //for reference: sensor readings are accurate from 20cm and to 2m
        int currentThreshold;
        const int HighThreshold = 100;
        const int LowThreshold = 30;
        
        public Model(int numSensors)
        {
            this.numSensors = numSensors;

            //initialise the array
            sensorArray = new LimitedList<int>[numSensors];
            for (int i = 0; i < numSensors; i++)
            {
                sensorArray[i] = new LimitedList<int>(maxReadings);
            }

            modelListeners = new ArrayList();

            currentThreshold = HighThreshold;
        }

        // take a range, store the results and inform any listeners
        public void updateRange(int i, int range)
        {
            sensorArray[i].add(range);
            Direction d = (i == leftSide) ? Direction.Left : (i == rightSide) ? Direction.Right : Direction.Front;

            if (range < currentThreshold)
            {
                fireDistanceLessThanThreshold(d, range);
            }
            else
            {
                fireDistanceGreaterThanThreshold(d);
            }
        }

        public void calibrationStarted()
        {
            fireCalibrationStarted();
        }

        public void calibrationFinished()
        {
            fireCalibrationFinished();
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
