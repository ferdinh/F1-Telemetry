namespace F12020Telemetry.Data
{
    public class LapData
    {
        /// <summary>
        /// Last lap time in seconds
        /// </summary>
        public float LastLapTime;

        /// <summary>
        /// Current time around the lap in seconds
        /// </summary>
        public float CurrentLapTime;

        /// <summary>
        /// Sector 1 time in milliseconds
        /// </summary>
        public ushort Sector1TimeInMS;

        /// <summary>
        /// Sector 2 time in milliseconds
        /// </summary>
        public ushort Sector2TimeInMS;

        /// <summary>
        /// Best lap time of the session in seconds
        /// </summary>
        public float BestLapTime;

        /// <summary>
        /// Lap number best time achieved on
        /// </summary>
        public byte BestLapNum;

        /// <summary>
        /// Sector 1 time of best lap in the session in milliseconds
        /// </summary>
        public ushort BestLapSector1TimeInMS;

        /// <summary>
        /// Sector 2 time of best lap in the session in milliseconds
        /// </summary>
        public ushort BestLapSector2TimeInMS;

        /// <summary>
        /// Sector 3 time of best lap in the session in milliseconds
        /// </summary>
        public ushort BestLapSector3TimeInMS;

        /// <summary>
        /// Best overall sector 1 time of the session in milliseconds
        /// </summary>
        public ushort BestOverallSector1TimeInMS;

        /// <summary>
        /// Lap number best overall sector 1 time achieved on
        /// </summary>
        public byte BestOverallSector1LapNum;

        /// <summary>
        /// Best overall sector 2 time of the session in milliseconds
        /// </summary>
        public ushort BestOverallSector2TimeInMS;

        /// <summary>
        /// Lap number best overall sector 2 time achieved on
        /// </summary>
        public byte BestOverallSector2LapNum;

        /// <summary>
        /// Best overall sector 3 time of the session in milliseconds
        /// </summary>
        public ushort BestOverallSector3TimeInMS;

        /// <summary>
        /// Lap number best overall sector 3 time achieved on
        /// </summary>
        public byte BestOverallSector3LapNum;

        /// <summary>
        /// Distance vehicle is around current lap in metres – could
        /// be negative if line hasn’t been crossed yet
        /// </summary>
        public float LapDistance;

        /// <summary>
        /// Total distance travelled in session in metres – could
        /// be negative if line hasn’t been crossed yet
        /// </summary>
        public float TotalDistance;

        /// <summary>
        /// Delta in seconds for safety car
        /// </summary>
        public float SafetyCarDelta;

        /// <summary>
        /// Car race position
        /// </summary>
        public byte CarPosition;

        /// <summary>
        /// Current lap number
        /// </summary>
        public byte CurrentLapNum;

        /// <summary>
        /// The pit status; 0 = none, 1 = pitting, 2 = in pit area
        /// </summary>
        public byte PitStatus;

        /// <summary>
        /// The sector; 0 = sector1, 1 = sector2, 2 = sector3
        /// </summary>
        public byte Sector;

        /// <summary>
        /// Current lap invalid - 0 = valid, 1 = invalid
        /// </summary>
        public byte CurrentLapInvalid;

        /// <summary>
        /// Accumulated time penalties in seconds to be added
        /// </summary>
        public byte Penalties;

        /// <summary>
        /// Grid position the vehicle started the race in
        /// </summary>
        public byte GridPosition;

        /// <summary>
        /// Status of driver - 0 = in garage, 1 = flying lap
        /// 2 = in lap, 3 = out lap, 4 = on track
        /// </summary>
        public byte DriverStatus;

        /// <summary>
        /// Result status - 0 = invalid, 1 = inactive, 2 = active
        /// 3 = finished, 4 = disqualified, 5 = not classified
        /// 6 = retired
        /// </summary>
        public byte ResultStatus;
    }
}