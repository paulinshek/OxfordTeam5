using System;
using Microsoft.SPOT;
using BlindPeople.DomainModel;

using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using GTI = Gadgeteer.Interfaces;
using Gadgeteer.Modules.GHIElectronics;
using Gadgeteer.Modules.Seeed;

namespace BlindPeople.Sensors
{
    //Class managing gyro, compass, and ranger
    class Controller
    {
        Model model;

        //Gyro gyro;
        //Compass compass;
        Ranger ranger;
        Accelerometer accelerometer;

        public Controller(Model model, Accelerometer accelerometer, Ranger ranger)
        {
            this.model = model;

            this.accelerometer = accelerometer;
            accelerometer.MeasurementComplete += new Accelerometer.MeasurementCompleteEventHandler(accelerometer_MeasurementComplete);

            this.ranger = ranger;
            ranger.MeasurementComplete += new Microsoft.SPOT.EventHandler(ranger_MeasurementComplete);

            //Calibrate the accelerometer, need to ensure that sensor is not moving.
            //Give the user some warning time, before actually calibrating
            model.calibrationStarted();
            Thread.Sleep(250);
            accelerometer.Calibrate();
            model.calibrationFinished();
            
        }

        void ranger_MeasurementComplete(object sender, Microsoft.SPOT.EventArgs e)
        {
            Ranger.SensorData sensorData = (Ranger.SensorData) e;
            model.updateRange(sensorData.id, sensorData.dist);

        }

        void accelerometer_MeasurementComplete(Accelerometer sender, Accelerometer.Acceleration acceleration)
        {
            //TODO: accelerometer measurement
            throw new System.NotImplementedException();
        }
    }
}
