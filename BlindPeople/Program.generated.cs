﻿
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Gadgeteer Designer.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Gadgeteer;
using GTM = Gadgeteer.Modules;

namespace BlindPeople
{
    public partial class Program : Gadgeteer.Program
    {
        // GTM.Module definitions
        Gadgeteer.Modules.GHIElectronics.Breakout breakout;
        Gadgeteer.Modules.GHIElectronics.UsbClientDP usbClientDP;
        Gadgeteer.Modules.Seeed.Accelerometer accelerometer;

        public static void Main()
        {
            //Important to initialize the Mainboard first
            Mainboard = new GHIElectronics.Gadgeteer.FEZSpider();			

            Program program = new Program();
            program.InitializeModules();
            program.ProgramStarted();
            program.Run(); // Starts Dispatcher
        }

        private void InitializeModules()
        {   
            // Initialize GTM.Modules and event handlers here.		
            usbClientDP = new GTM.GHIElectronics.UsbClientDP(1);
		
            breakout = new GTM.GHIElectronics.Breakout(3);
		
            accelerometer = new GTM.Seeed.Accelerometer(4);

        }
    }
}
