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

using BlindPeople.Sensors;
using BlindPeople.DomainModel;

namespace BlindPeople
{
    public partial class Program
    {
        Ranger ranger;

        void ProgramStarted()
        {
            Debug.Print("Program Started");

            int[] sockets = { 3, 3 };
            byte[] addresses = { 1, 2 };
            ranger = new Ranger(sockets, addresses);
            GyroWrapper gyroWrapper = new GyroWrapper(gyro);
            CompassWrapper compassWrapper = new CompassWrapper(compass);

            //Calibrate the gyro, need to ensure that sensor is not moving.
            //TODO: need some way of notifying the user.
            gyro.Calibrate();

            Model model = new Model(ranger,gyroWrapper,compassWrapper);

            Debug.Print("Initialisation Ended");
        }
    }
}