using System;
using System.Collections;
using Microsoft.SPOT;
using GT = Gadgeteer;
using BlindPeople.Sensors;

namespace BlindPeople.DomainModel
{
    class Model
    {
        Ranger ranger;
        int numSensors;

        GyroWrapper gyro;
        CompassWrapper compass;
        
        //the number of readings to keep for each sensor
        int maxReadings;
        
        //stores the most recent maxReadings readings for each ultrasonic sensor
        //(array of LimitedLists)
        LimitedList<int>[] sensorArray;

        //stores gryo readings
        LimitedList<Coordinate> gyroReadings;

        //stores the *change* in compass angle
        LimitedList<double> angleChanges;
        double oldCompassReading;
        
        public Model(Ranger ranger, GyroWrapper gyro, CompassWrapper compass)
        {
            this.ranger = ranger;
            numSensors = ranger.getNumSensors();

            this.gyro = gyro;
            this.compass = compass;

            //initialise the array
            sensorArray = new LimitedList<int>[numSensors];
            for (int i = 0; i < numSensors; i++)
            {
                sensorArray[i] = new LimitedList<int>(maxReadings);
            }

            gyroReadings = new LimitedList<Coordinate>(maxReadings);
            angleChanges = new LimitedList<double>(maxReadings);

            GT.Timer timer = new GT.Timer(250);

            timer.Tick += new GT.Timer.TickEventHandler(timer_Tick);
            timer.Start();
            
        }

        void timer_Tick(GT.Timer timer)
        {   
            updateRanges();
            takeGyroMeasurement();

        }

        // take all ranges and store the results in the ranges array
        private void updateRanges()
        {
            for (int i = 0; i < numSensors; i++)
            {
                sensorArray[i].add(ranger.getRange(i));
            }
        }

        //takes a gyro reading and adds to the LimitedList
        private void takeGyroMeasurement()
        {
            gyroReadings.add(gyro.getReading());
        }

        //takes a compass reading and subtracts the previous one from it
        private void getAngleChange()
        {
            double newReading = compass.getReading();
            double theta = newReading - oldCompassReading;
            angleChanges.add(theta);
            oldCompassReading = newReading;
        }
    }

    
}
