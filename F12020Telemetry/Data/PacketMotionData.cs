using F12020Telemetry.Packet;

namespace F12020Telemetry.Data
{
    public class PacketMotionData : IPacket
    {
        public PacketHeader Header { get; set; }

        /// <summary>
        /// Data for all cars on track
        /// </summary>
        public CarMotionData[] CarMotionData = new CarMotionData[Decode.MaxNumberOfCarsOnTrack];

        // Note: All wheel arrays have the following order:
        // RL, RR, FL, FR

        // Extra player car ONLY data

        /// <summary>
        /// The suspension position
        /// </summary>
        public float[] SuspensionPosition = new float[Decode.NumberOfTyres];

        /// <summary>
        /// The suspension velocity
        /// </summary>
        public float[] SuspensionVelocity = new float[Decode.NumberOfTyres];

        /// <summary>
        /// The suspension acceleration
        /// </summary>
        public float[] SuspensionAcceleration = new float[Decode.NumberOfTyres];

        /// <summary>
        /// Speed of each wheel
        /// </summary>
        public float[] WheelSpeed = new float[Decode.NumberOfTyres];

        /// <summary>
        /// Slip ratio for each wheel
        /// </summary>
        public float[] WheelSlip = new float[Decode.NumberOfTyres];

        /// <summary>
        /// Velocity in local space
        /// </summary>
        public float LocalVelocityX;

        /// <summary>
        /// Velocity in local space
        /// </summary>
        public float LocalVelocityY;

        /// <summary>
        /// Velocity in local space
        /// </summary>
        public float LocalVelocityZ;

        /// <summary>
        /// Angular velocity x-component
        /// </summary>
        public float AngularVelocityX;

        /// <summary>
        ///  Angular velocity y-component
        /// </summary>
        public float AngularVelocityY;

        /// <summary>
        /// Angular velocity z-component
        /// </summary>
        public float AngularVelocityZ;

        /// <summary>
        /// Angular velocity x-component
        /// </summary>
        public float AngularAccelerationX;

        /// <summary>
        /// Angular velocity y-component
        /// </summary>
        public float AngularAccelerationY;

        /// <summary>
        /// Angular velocity z-component
        /// </summary>
        public float AngularAccelerationZ;

        /// <summary>
        /// Current front wheels angle in radians
        /// </summary>
        public float FrontWheelsAngle;

        public PacketMotionData(PacketHeader packetHeader)
        {
            Header = packetHeader;
        }
    }
}