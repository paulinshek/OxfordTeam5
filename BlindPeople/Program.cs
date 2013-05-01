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

using BlindPeople.Sensors;
using BlindPeople.DomainModel;
using BlindPeople.Sound;

namespace BlindPeople
{
    public partial class Program
    {
        void ProgramStarted()
        {
            Debug.Print("Program Started");

            // create the model
            Model model = new Model(4);

            // setup the sensors
            int[] sockets = { 3, 3, 4, 4 };
            byte[] addresses = { 1, 2, 3, 4 };
            Ranger ranger = new Ranger(sockets, addresses);
            Controller controller = new Controller(model, ranger);

            // setup the tunes modules
            TunesModule leftTunes = new TunesModule(11, model);
            TunesModule rightTunes = new TunesModule(8, model);
            TunesListener tunesListener = new TunesListener(leftTunes, rightTunes);
            model.addModelListener(tunesListener);

            // calibrate everything
            controller.calibrate();

            // start ranging
            ranger.startRanging();

            Debug.Print("Initialisation Ended");
        }
    }
}