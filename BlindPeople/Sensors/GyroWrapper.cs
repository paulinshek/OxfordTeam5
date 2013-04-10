using System;
using Microsoft.SPOT;
using Gadgeteer.Modules.Seeed;

namespace BlindPeople.Sensors
{
    class GyroWrapper
    {
        Gyro gyro;
        //most recent reading;
        Coordinate reading;

        public GyroWrapper(Gyro gyro)
        {
            this.gyro = gyro;
            
            gyro.MeasurementComplete += new Gyro.MeasurementCompleteEventHandler(gyro_MeasurementComplete);
            gyro.StartContinuousMeasurements();
        }

        void gyro_MeasurementComplete(Gyro sender, Gyro.SensorData sensorData)
        {
            reading = new Coordinate(sensorData.X, sensorData.Y, sensorData.Z);
        }

        public Coordinate getReading()
        {
            return new Coordinate(reading.x,reading.y,reading.z);
        }

    }
}
