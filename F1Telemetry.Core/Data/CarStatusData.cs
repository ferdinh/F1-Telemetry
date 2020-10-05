namespace F1Telemetry.Core.Data
{
    public class CarStatusData : BasePacketData
    {
        /// <summary>
        /// The traction control [0 (off) - 2 (high)]
        /// </summary>
        public byte TractionControl { get; set; }

        /// <summary>
        /// The anti lock brakes 0 [(off) - 1 (on)]
        /// </summary>
        public byte AntiLockBrakes { get; set; }

        /// <summary>
        /// Fuel mix - 0 = lean, 1 = standard, 2 = rich, 3 = max
        /// </summary>
        public byte FuelMix { get; set; }

        /// <summary>
        /// Front brake bias (percentage)
        /// </summary>
        public byte FrontBrakeBias { get; set; }

        /// <summary>
        /// Pit limiter status - 0 = off, 1 = on
        /// </summary>
        public byte pitLimiterStatus { get; set; }

        /// <summary>
        /// Current fuel mass
        /// </summary>
        public float FuelInTank { get; set; }

        /// <summary>
        /// Fuel capacity
        /// </summary>
        public float FuelCapacity { get; set; }

        /// <summary>
        /// Fuel remaining in terms of laps (value on MFD)
        /// </summary>
        public float FuelRemainingLaps { get; set; }

        /// <summary>
        /// Cars max RPM, point of rev limiter
        /// </summary>
        public ushort MaxRPM { get; set; }

        /// <summary>
        /// Cars idle RPM
        /// </summary>
        public ushort IdleRPM { get; set; }

        /// <summary>
        /// Maximum number of gears
        /// </summary>
        public byte MaxGears { get; set; }

        /// <summary>
        /// 0 = not allowed, 1 = allowed, -1 = unknown
        /// </summary>
        public byte DrsAllowed { get; set; }

        /// <summary>
        /// The DRS activation distance
        /// 0 = DRS not available, non-zero - DRS will be available
        /// in [X] metres
        /// </summary>
        public ushort DrsActivationDistance { get; set; }

        /// <summary>
        /// Tyre wear percentage
        /// </summary>
        public byte[] TyresWear { get; set; } = new byte[Decode.NumberOfTyres];

        /// <summary>
        /// The actual tyre compound
        /// F1 Modern - 16 = C5, 17 = C4, 18 = C3, 19 = C2, 20 = C1, 7 = inter, 8 = wet
        /// F1 Classic - 9 = dry, 10 = wet
        /// F2 – 11 = super soft, 12 = soft, 13 = medium, 14 = hard, 15 = wet
        /// </summary>
        public byte ActualTyreCompound { get; set; }

        /// <summary>
        /// F1 visual (can be different from actual compound)
        /// 16 = soft, 17 = medium, 18 = hard, 7 = inter, 8 = wet
        /// F1 Classic – same as above
        /// F2 – same as above
        /// </summary>
        public byte VisualTyreCompound { get; set; }

        /// <summary>
        /// Age in laps of the current set of tyres
        /// </summary>
        public byte TyresAgeLaps { get; set; }

        /// <summary>
        /// Tyre damage (percentage)
        /// </summary>
        public byte[] TyresDamage { get; set; } = new byte[Decode.NumberOfTyres];

        /// <summary>
        /// Front left wing damage (percentage)
        /// </summary>
        public byte FrontLeftWingDamage { get; set; }

        /// <summary>
        ///  Front right wing damage (percentage)
        /// </summary>
        public byte FrontRightWingDamage { get; set; }

        /// <summary>
        /// Rear wing damage (percentage)
        /// </summary>
        public byte RearWingDamage { get; set; }

        /// <summary>
        /// Indicator for DRS fault, 0 = OK, 1 = fault
        /// </summary>
        public byte DrsFault { get; set; }

        /// <summary>
        /// Engine damage (percentage)
        /// </summary>
        public byte EngineDamage { get; set; }

        /// <summary>
        /// Gear box damage (percentage)
        /// </summary>
        public byte GearBoxDamage { get; set; }

        /// <summary>
        /// The vehicle fia flags
        /// -1 = invalid/unknown, 0 = none, 1 = green
        /// 2 = blue, 3 = yellow, 4 = red
        /// </summary>
        public sbyte VehicleFiaFlags { get; set; }

        /// <summary>
        /// T ERS energy store in Joules
        /// </summary>
        public float ErsStoreEnergy { get; set; }

        /// <summary>
        /// ERS deployment mode, 0 = none, 1 = medium, 2 = overtake, 3 = hotlap
        /// </summary>
        public byte ErsDeployMode { get; set; }

        /// <summary>
        /// ERS energy harvested this lap by MGU-K
        /// </summary>
        public float ErsHarvestedThisLapMGUK { get; set; }

        /// <summary>
        /// ERS energy harvested this lap by MGU-H
        /// </summary>
        public float ErsHarvestedThisLapMGUH { get; set; }

        /// <summary>
        /// ERS energy deployed this lap
        /// </summary>
        public float ErsDeployedThisLap { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CarStatusData"/> class.
        /// </summary>
        /// <param name="sessionUID">The session uid.</param>
        /// <param name="sessionTime">The session time.</param>
        public CarStatusData(ulong sessionUID, float sessionTime) : base(sessionUID, sessionTime)
        {
        }
    }
}