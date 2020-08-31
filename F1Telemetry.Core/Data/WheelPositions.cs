using System.ComponentModel;

namespace F1Telemetry.Core.Data
{
    public enum WheelPositions
    {
        [Description("Rear Left")]
        RearLeft,

        [Description("Rear Right")]
        RearRight,

        [Description("Front Left")]
        FrontLeft,

        [Description("Front Right")]
        FrontRight
    }
}