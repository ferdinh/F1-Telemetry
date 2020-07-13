using System;

namespace F12020Telemetry.Data
{
    public class CarTelemetryData
    {
        /// <summary>
        /// Speed of car in kilometres per hour
        /// </summary>
        public ushort Speed;

        /// <summary>
        /// Amount of throttle applied (0.0 to 1.0)
        /// </summary>
        public float Throttle;

        /// <summary>
        /// Steering (-1.0 (full lock left) to 1.0 (full lock right))
        /// </summary>
        public float Steer;

        /// <summary>
        /// Amount of brake applied (0.0 to 1.0)
        /// </summary>
        public float Brake;

        /// <summary>
        /// Amount of clutch applied (0 to 100)
        /// </summary>
        public byte Clutch;

        /// <summary>
        /// Gear selected (1-8, N=0, R=-1)
        /// </summary>
        public sbyte Gear;

        /// <summary>
        /// Engine RPM
        /// </summary>
        public ushort EngineRPM;

        /// <summary>
        /// The DRS (0 = off, 1 = on)
        /// </summary>
        public DRS Drs;

        /// <summary>
        /// Rev lights indicator (percentage)
        /// </summary>
        public byte RevLightsPercent;

        /// <summary>
        /// Brakes temperature (celsius)
        /// </summary>
        public ushort[] BrakesTemperature = new ushort[Decode.NumberOfTyres];

        /// <summary>
        /// Tyres surface temperature (celsius)
        /// </summary>
        public byte[] TyresSurfaceTemperature = new byte[Decode.NumberOfTyres];

        /// <summary>
        /// Tyres inner temperature (celsius)
        /// </summary>
        public byte[] TyresInnerTemperature = new byte[Decode.NumberOfTyres];

        /// <summary>
        /// Engine temperature (celsius)
        /// </summary>
        public ushort EngineTemperature;

        /// <summary>
        /// Tyres pressure (PSI)
        /// </summary>
        public float[] TyresPressure = new float[Decode.NumberOfTyres];

        /// <summary>
        /// Driving surface, see appendices
        /// </summary>
        public SurfaceTypes[] SurfaceType = new SurfaceTypes[Decode.NumberOfTyres];
    }

    public enum DRS
    {
        Off,
        On
    }
}