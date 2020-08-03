using System;

namespace F1Telemetry.WPF.Model
{
    /// <summary>
    /// Contains the lap data series for one lap.
    /// </summary>
    public class CurrentLapDataModel
    {
        // 5000 for one minute of data.
        public double[] Speed = new double[25_000];

        public double[] Distance = new double[25_000];
        public double[] Gear = new double[25_000];

        public double[] Throttle = new double[25_000];
        public double[] Brake = new double[25_000];

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            Array.Clear(Speed, 0, Speed.Length);
            Array.Clear(Distance, 0, Speed.Length);
            Array.Clear(Gear, 0, Speed.Length);
            Array.Clear(Throttle, 0, Speed.Length);
            Array.Clear(Brake, 0, Speed.Length);
        }
    }
}