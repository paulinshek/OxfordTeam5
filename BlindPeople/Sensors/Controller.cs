using System;
using Microsoft.SPOT;
using BlindPeople.DomainModel;

using System.Threading;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using GTI = Gadgeteer.Interfaces;


namespace BlindPeople.Sensors
{
    //Class managing the ranger
    public class Controller
    {
        Model model;

        Ranger ranger;

        GTM.GHIElectronics.Button thresholdButton;

        public Controller(Model model, Ranger ranger, GTM.GHIElectronics.Button thresholdButton)
        {
            this.model = model;

            this.ranger = ranger;
            ranger.MeasurementComplete += new Microsoft.SPOT.EventHandler(ranger_MeasurementComplete);

            this.thresholdButton = thresholdButton;
            thresholdButton.ButtonPressed += new GTM.GHIElectronics.Button.ButtonEventHandler(thresholdButton_ButtonPressed);

        }

        void thresholdButton_ButtonPressed(GTM.GHIElectronics.Button sender, GTM.GHIElectronics.Button.ButtonState state)
        {
            model.changeThreshold();
        }

        public void calibrate()
        {
            //Calibrate the accelerometer, need to ensure that sensor is not moving.
            //Give the user some warning time, before actually calibrating
            model.calibrationStarted();
            Thread.Sleep(250);
            model.calibrationFinished();
        }

        void ranger_MeasurementComplete(object sender, Microsoft.SPOT.EventArgs e)
        {
            Ranger.SensorData sensorData = (Ranger.SensorData) e;
            model.updateRange(sensorData.id, sensorData.dist);
        }
    }
}
