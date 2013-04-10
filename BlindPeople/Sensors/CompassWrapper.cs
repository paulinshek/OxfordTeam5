using System;
using Microsoft.SPOT;
using Gadgeteer.Modules.Seeed;

namespace BlindPeople.Sensors
{
    class CompassWrapper
    {
        Compass compass;
        //most recent reading;
        double reading;

        public CompassWrapper(Compass compass)
        {
            this.compass = compass;

            compass.MeasurementComplete += new Compass.MeasurementCompleteEventHandler(compass_MeasurementComplete);
            compass.StartContinuousMeasurements();
        }

        void compass_MeasurementComplete(Compass sender, Compass.SensorData sensorData)
        {
            reading = sensorData.Angle;
        }


        public double getReading()
        {
            return reading;
        }
    }
}
