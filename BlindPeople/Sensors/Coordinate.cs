using System;
using Microsoft.SPOT;

namespace BlindPeople.Sensors
{
    class Coordinate
    {
        public double x, y, z;

        public Coordinate(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

    }
}
