namespace F1Telemetry.Core.Data
{
    /// <summary>
    /// Contains the Driver Status.
    /// </summary>
    public class DriverStatusInfo
    {
        public PitStatus PitStatus { get; set; } = PitStatus.Invalid;
        public DriverStatus Status { get; set; }
    }
}