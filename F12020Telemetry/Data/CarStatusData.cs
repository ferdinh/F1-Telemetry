namespace F12020Telemetry.Data
{
    public class CarStatusData
    {
        /// <summary>
        /// The traction control [0 (off) - 2 (high)]
        /// </summary>
        public byte TractionControl;

        /// <summary>
        /// The anti lock brakes 0 [(off) - 1 (on)]
        /// </summary>
        public byte AntiLockBrakes;

        /// <summary>
        /// Fuel mix - 0 = lean, 1 = standard, 2 = rich, 3 = max
        /// </summary>
        public byte FuelMix;

        /// <summary>
        /// Front brake bias (percentage)
        /// </summary>
        public byte FrontBrakeBias;

        /// <summary>
        /// Pit limiter status - 0 = off, 1 = on
        /// </summary>
        public byte pitLimiterStatus;

        /// <summary>
        /// Current fuel mass
        /// </summary>
        public float FuelInTank;

        /// <summary>
        /// Fuel capacity
        /// </summary>
        public float FuelCapacity;

        /// <summary>
        /// Fuel remaining in terms of laps (value on MFD)
        /// </summary>
        public float FuelRemainingLaps;

        /// <summary>
        /// Cars max RPM, point of rev limiter
        /// </summary>
        public ushort MaxRPM;

        /// <summary>
        /// Cars idle RPM
        /// </summary>
        public ushort IdleRPM;

        /// <summary>
        /// Maximum number of gears
        /// </summary>
        public byte MaxGears;

        /// <summary>
        /// 0 = not allowed, 1 = allowed, -1 = unknown
        /// </summary>
        public byte DrsAllowed;

        /// <summary>
        /// The DRS activation distance
        /// 0 = DRS not available, non-zero - DRS will be available
        /// in [X] metres
        /// </summary>
        public ushort DrsActivationDistance;

        /// <summary>
        /// Tyre wear percentage
        /// </summary>
        public byte[] TyresWear = new byte[Decode.NumberOfTyres];

        /// <summary>
        /// The actual tyre compound
        /// F1 Modern - 16 = C5, 17 = C4, 18 = C3, 19 = C2, 20 = C1, 7 = inter, 8 = wet
        /// F1 Classic - 9 = dry, 10 = wet
        /// F2 – 11 = super soft, 12 = soft, 13 = medium, 14 = hard, 15 = wet
        /// </summary>
        public byte ActualTyreCompound;

        /// <summary>
        /// F1 visual (can be different from actual compound)
        /// 16 = soft, 17 = medium, 18 = hard, 7 = inter, 8 = wet
        /// F1 Classic – same as above
        /// F2 – same as above
        /// </summary>
        public byte VisualTyreCompound;

        /// <summary>
        /// Age in laps of the current set of tyres
        /// </summary>
        public byte TyresAgeLaps;

        /// <summary>
        /// Tyre damage (percentage)
        /// </summary>
        public byte[] TyresDamage = new byte[Decode.NumberOfTyres];

        /// <summary>
        /// Front left wing damage (percentage)
        /// </summary>
        public byte FrontLeftWingDamage;

        /// <summary>
        ///  Front right wing damage (percentage)
        /// </summary>
        public byte FrontRightWingDamage;

        /// <summary>
        /// Rear wing damage (percentage)
        /// </summary>
        public byte RearWingDamage;

        /// <summary>
        /// Indicator for DRS fault, 0 = OK, 1 = fault
        /// </summary>
        public byte DrsFault;

        /// <summary>
        /// Engine damage (percentage)
        /// </summary>
        public byte EngineDamage;

        /// <summary>
        /// Gear box damage (percentage)
        /// </summary>
        public byte GearBoxDamage;

        /// <summary>
        /// The vehicle fia flags
        /// -1 = invalid/unknown, 0 = none, 1 = green
        /// 2 = blue, 3 = yellow, 4 = red
        /// </summary>
        public sbyte VehicleFiaFlags;

        /// <summary>
        /// T ERS energy store in Joules
        /// </summary>
        public float ErsStoreEnergy;

        /// <summary>
        /// ERS deployment mode, 0 = none, 1 = medium, 2 = overtake, 3 = hotlap
        /// </summary>
        public byte ErsDeployMode;

        /// <summary>
        /// ERS energy harvested this lap by MGU-K
        /// </summary>
        public float ErsHarvestedThisLapMGUK;

        /// <summary>
        /// ERS energy harvested this lap by MGU-H
        /// </summary>
        public float ErsHarvestedThisLapMGUH;

        /// <summary>
        /// ERS energy deployed this lap
        /// </summary>
        public float ErsDeployedThisLap;
    }
}