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

            GT.Timer timer = new GT.Timer(250);
            timer.Tick += new GT.Timer.TickEventHandler(printRanges);
            timer.Start();

            Debug.Print("Initialisation Ended");
        }

        // take all ranges and store the results in the ranges array
        private void printRanges(GT.Timer timer)
        {
            Debug.Print("Sensor 1 returns " + ranger.getRange(0));
            Debug.Print("Sensor 2 returns " + ranger.getRange(1));
        }
    }
}