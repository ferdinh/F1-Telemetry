using F1Telemetry.Core.Packet;

namespace F1Telemetry.Core.Data
{
    public class PacketMotionData : IPacket
    {
        public PacketHeader Header { get; set; }

        /// <summary>
        /// Data for all cars on track
        /// </summary>
        public CarMotionData[] CarMotionData { get; set; } = new CarMotionData[Decode.MaxNumberOfCarsOnTrack];

        // Note: All wheel arrays have the following order:
        // RL, RR, FL, FR

        // Extra player car ONLY data

        /// <summary>
        /// The suspension position
        /// </summary>
        public float[] SuspensionPosition { get; set; } = new float[Decode.NumberOfTyres];

        /// <summary>
        /// The suspension velocity
        /// </summary>
        public float[] SuspensionVelocity { get; set; } = new float[Decode.NumberOfTyres];

        /// <summary>
        /// The suspension acceleration
        /// </summary>
        public float[] SuspensionAcceleration { get; set; } = new float[Decode.NumberOfTyres];

        /// <summary>
        /// Speed of each wheel
        /// </summary>
        public float[] WheelSpeed { get; set; } = new float[Decode.NumberOfTyres];

        /// <summary>
        /// Slip ratio for each wheel
        /// </summary>
        public float[] WheelSlip { get; set; } = new float[Decode.NumberOfTyres];

        /// <summary>
        /// Velocity in local space
        /// </summary>
        public float LocalVelocityX { get; set; }

        /// <summary>
        /// Velocity in local space
        /// </summary>
        public float LocalVelocityY { get; set; }

        /// <summary>
        /// Velocity in local space
        /// </summary>
        public float LocalVelocityZ { get; set; }

        /// <summary>
        /// Angular velocity x-component
        /// </summary>
        public float AngularVelocityX { get; set; }

        /// <summary>
        ///  Angular velocity y-component
        /// </summary>
        public float AngularVelocityY { get; set; }

        /// <summary>
        /// Angular velocity z-component
        /// </summary>
        public float AngularVelocityZ { get; set; }

        /// <summary>
        /// Angular velocity x-component
        /// </summary>
        public float AngularAccelerationX { get; set; }

        /// <summary>
        /// Angular velocity y-component
        /// </summary>
        public float AngularAccelerationY { get; set; }

        /// <summary>
        /// Angular velocity z-component
        /// </summary>
        public float AngularAccelerationZ { get; set; }

        /// <summary>
        /// Current front wheels angle in radians
        /// </summary>
        public float FrontWheelsAngle { get; set; }

        public PacketMotionData(PacketHeader packetHeader)
        {
            Header = packetHeader;
        }
    }
}