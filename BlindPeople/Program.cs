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

using BlindPeople.Sensors;
using BlindPeople.DomainModel;
using BlindPeople.Sound;
using Gadgeteer.Modules.Seeed;

namespace BlindPeople
{
    public partial class Program
    {
        void ProgramStarted()
        {
            Debug.Print("Program Started");

            Model model = new Model(2);

            int[] sockets = { 3, 3 };
            byte[] addresses = { 1, 2 };
            Ranger ranger = new Ranger(sockets, addresses);
            Controller controller = new Controller(model, accelerometer, ranger);

            TunesModule leftTunes = new TunesModule(11);
            TunesModule rightTunes = new TunesModule(8);

            Debug.Print("Initialisation Ended");
        }
    }
}