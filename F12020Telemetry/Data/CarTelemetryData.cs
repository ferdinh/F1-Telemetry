using System;

namespace F12020Telemetry.Data
{
    public class CarTelemetryData
    {
        /// <summary>
        /// Speed of car in kilometres per hour
        /// </summary>
        public UInt16 Speed;

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
        public UInt16 EngineRPM;

        /// <summary>
        /// The DRS (0 = off, 1 = on)
        /// </summary>
        public byte Drs;

        /// <summary>
        /// Rev lights indicator (percentage)
        /// </summary>
        public byte RevLightsPercent;

        /// <summary>
        /// Brakes temperature (celsius)
        /// </summary>
        public UInt16[] BrakesTemperature;

        /// <summary>
        /// Tyres surface temperature (celsius)
        /// </summary>
        public byte[] TyresSurfaceTemperature;

        /// <summary>
        /// Tyres inner temperature (celsius)
        /// </summary>
        public byte[] TyresInnerTemperature;

        /// <summary>
        /// Engine temperature (celsius)
        /// </summary>
        public UInt16 EngineTemperature;

        /// <summary>
        /// Tyres pressure (PSI)
        /// </summary>
        public float[] TyresPressure;

        /// <summary>
        /// Driving surface, see appendices
        /// </summary>
        public SurfaceTypes[] SurfaceType;
    }
}