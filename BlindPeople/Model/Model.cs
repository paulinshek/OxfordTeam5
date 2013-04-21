using System;
using System.Collections;
using Microsoft.SPOT;
using GT = Gadgeteer;
using BlindPeople.Sensors;
using Gadgeteer.Modules.Seeed;

namespace BlindPeople.DomainModel
{
    class Model
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

        //stores gryo readings
        LimitedList<Coordinate> accelReadings;

        ArrayList modelListeners;
        

        
        public Model(int numSensors)
        {
            this.numSensors = numSensors;

            //initialise the array
            sensorArray = new LimitedList<int>[numSensors];
            for (int i = 0; i < numSensors; i++)
            {
                sensorArray[i] = new LimitedList<int>(maxReadings);
            }

            accelReadings = new LimitedList<Coordinate>(maxReadings);

        }

        // take all ranges and store the results in the ranges array
        public void updateRange(int i, int range)
        {
            sensorArray[i].add(range);
            //TODO: fire computations
        }

        public void updateAccelerometer(Coordinate coord){
            accelReadings.add(coord);
            //TODO: fire computations
        }

        public void calibrationStarted()
        {
            fireCalibrationStarted();
        }

        public void calibrationFinished()
        {
            fireCalibrationFinished();
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
