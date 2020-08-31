using System.ComponentModel.DataAnnotations;

namespace F1Telemetry.Core.Data
{
    public class LapData
    {
        public ulong SessionUID { get; set; }
        public float SessionTime { get; set; }

        /// <summary>
        /// Last lap time in seconds
        /// </summary>
        public float LastLapTime { get; set; }

        /// <summary>
        /// Current time around the lap in seconds
        /// </summary>
        public float CurrentLapTime { get; set; }

        /// <summary>
        /// Sector 1 time in milliseconds
        /// </summary>
        public ushort Sector1TimeInMS { get; set; }

        /// <summary>
        /// Sector 2 time in milliseconds
        /// </summary>
        public ushort Sector2TimeInMS { get; set; }

        /// <summary>
        /// Best lap time of the session in seconds
        /// </summary>
        public float BestLapTime { get; set; }

        /// <summary>
        /// Lap number best time achieved on
        /// </summary>
        public byte BestLapNum { get; set; }

        /// <summary>
        /// Sector 1 time of best lap in the session in milliseconds
        /// </summary>
        public ushort BestLapSector1TimeInMS { get; set; }

        /// <summary>
        /// Sector 2 time of best lap in the session in milliseconds
        /// </summary>
        public ushort BestLapSector2TimeInMS { get; set; }

        /// <summary>
        /// Sector 3 time of best lap in the session in milliseconds
        /// </summary>
        public ushort BestLapSector3TimeInMS { get; set; }

        /// <summary>
        /// Best overall sector 1 time of the session in milliseconds
        /// </summary>
        public ushort BestOverallSector1TimeInMS { get; set; }

        /// <summary>
        /// Lap number best overall sector 1 time achieved on
        /// </summary>
        public byte BestOverallSector1LapNum { get; set; }

        /// <summary>
        /// Best overall sector 2 time of the session in milliseconds
        /// </summary>
        public ushort BestOverallSector2TimeInMS { get; set; }

        /// <summary>
        /// Lap number best overall sector 2 time achieved on
        /// </summary>
        public byte BestOverallSector2LapNum { get; set; }

        /// <summary>
        /// Best overall sector 3 time of the session in milliseconds
        /// </summary>
        public ushort BestOverallSector3TimeInMS { get; set; }

        /// <summary>
        /// Lap number best overall sector 3 time achieved on
        /// </summary>
        public byte BestOverallSector3LapNum { get; set; }

        /// <summary>
        /// Distance vehicle is around current lap in metres – could
        /// be negative if line hasn’t been crossed yet
        /// </summary>
        public float LapDistance { get; set; }

        /// <summary>
        /// Total distance travelled in session in metres – could
        /// be negative if line hasn’t been crossed yet
        /// </summary>
        public float TotalDistance { get; set; }

        /// <summary>
        /// Delta in seconds for safety car
        /// </summary>
        public float SafetyCarDelta { get; set; }

        /// <summary>
        /// Car race position
        /// </summary>
        public byte CarPosition { get; set; }

        /// <summary>
        /// Current lap number
        /// </summary>
        public byte CurrentLapNum { get; set; }

        /// <summary>
        /// The pit status; 0 = none, 1 = pitting, 2 = in pit area
        /// </summary>
        public PitStatus PitStatus { get; set; }

        /// <summary>
        /// The sector; 0 = sector1, 1 = sector2, 2 = sector3
        /// </summary>
        public byte Sector { get; set; }

        /// <summary>
        /// Current lap invalid - 0 = valid, 1 = invalid
        /// </summary>
        public byte CurrentLapInvalid { get; set; }

        /// <summary>
        /// Accumulated time penalties in seconds to be added
        /// </summary>
        public byte Penalties { get; set; }

        /// <summary>
        /// Grid position the vehicle started the race in
        /// </summary>
        public byte GridPosition { get; set; }

        /// <summary>
        /// Status of driver - 0 = in garage, 1 = flying lap
        /// 2 = in lap, 3 = out lap, 4 = on track
        /// </summary>
        public DriverStatus DriverStatus { get; set; }

        /// <summary>
        /// Result status - 0 = invalid, 1 = inactive, 2 = active
        /// 3 = finished, 4 = disqualified, 5 = not classified
        /// 6 = retired
        /// </summary>
        public ResultStatus ResultStatus { get; set; }
    }

    public enum ResultStatus : byte
    {
        Invalid,
        Inactive,
        Active,
        Finished,
        Disqualified,

        [Display(Name = "Not Classified")]
        NotClassified,

        Retired
    }

    public enum DriverStatus : byte
    {
        [Display(Name = "In Garage")]
        InGarage,

        [Display(Name = "Flying Lap")]
        FlyingLap,

        [Display(Name = "In Lap")]
        InLap,

        [Display(Name = "Out Lap")]
        OutLap,

        [Display(Name = "On Track")]
        OnTrack
    }

    public enum PitStatus : byte
    {
        None,
        Pitting,

        [Display(Name = "In Pit Area")]
        InPitArea
    }
}