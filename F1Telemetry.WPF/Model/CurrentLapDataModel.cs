using System;
using System.Collections.Generic;
using System.Text;

namespace F1Telemetry.WPF.Model
{
    public class CurrentLapDataModel
    {
        // 5000 for one minute of data.
        public double[] Speed = new double[25_000];

        public double[] Distance = new double[25_000];
        public double[] Gear = new double[25_000];

        public double[] Throttle = new double[25_000];
        public double[] Brake = new double[25_000];
    }
}
