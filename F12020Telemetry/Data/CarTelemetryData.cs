namespace F12020Telemetry.Data
{
    public class CarTelemetryData
    {
        public ulong SessionUID { get; set; }
        public float SessionTime { get; set; }

        /// <summary>
        /// Speed of car in kilometres per hour
        /// </summary>
        public ushort Speed { get; set; }

        /// <summary>
        /// Amount of throttle applied (0.0 to 1.0)
        /// </summary>
        public float Throttle { get; set; }

        /// <summary>
        /// Steering (-1.0 (full lock left) to 1.0 (full lock right))
        /// </summary>
        public float Steer { get; set; }

        /// <summary>
        /// Amount of brake applied (0.0 to 1.0)
        /// </summary>
        public float Brake { get; set; }

        /// <summary>
        /// Amount of clutch applied (0 to 100)
        /// </summary>
        public byte Clutch { get; set; }

        /// <summary>
        /// Gear selected (1-8, N=0, R=-1)
        /// </summary>
        public sbyte Gear { get; set; }

        /// <summary>
        /// Engine RPM
        /// </summary>
        public ushort EngineRPM { get; set; }

        /// <summary>
        /// The DRS (0 = off, 1 = on)
        /// </summary>
        public DRS Drs { get; set; }

        /// <summary>
        /// Rev lights indicator (percentage)
        /// </summary>
        public byte RevLightsPercent { get; set; }

        /// <summary>
        /// Brakes temperature (celsius)
        /// </summary>
        public ushort[] BrakesTemperature { get; set; } = new ushort[Decode.NumberOfTyres];

        /// <summary>
        /// Tyres surface temperature (celsius)
        /// </summary>
        public byte[] TyresSurfaceTemperature { get; set; } = new byte[Decode.NumberOfTyres];

        /// <summary>
        /// Tyres inner temperature (celsius)
        /// </summary>
        public byte[] TyresInnerTemperature { get; set; } = new byte[Decode.NumberOfTyres];

        /// <summary>
        /// Engine temperature (celsius)
        /// </summary>
        public ushort EngineTemperature { get; set; }

        /// <summary>
        /// Tyres pressure (PSI)
        /// </summary>
        public float[] TyresPressure { get; set; } = new float[Decode.NumberOfTyres];

        /// <summary>
        /// Driving surface, see appendices
        /// </summary>
        public SurfaceTypes[] SurfaceType { get; set; } = new SurfaceTypes[Decode.NumberOfTyres];
    }

    public enum DRS
    {
        Off,
        On
    }
}