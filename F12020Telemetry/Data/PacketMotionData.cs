using F12020Telemetry.Packet;

namespace F12020Telemetry.Data
{
    internal struct PacketMotionData : IPacket
    {
        public PacketHeader Header { get; set; }

        /// <summary>
        /// Data for all cars on track
        /// </summary>
        public CarMotionData[] CarMotionData;

        // Note: All wheel arrays have the following order:
        // RL, RR, FL, FR

        // Extra player car ONLY data

        /// <summary>
        /// The suspension position
        /// </summary>
        public float[] SuspensionPosition;

        /// <summary>
        /// The suspension velocity
        /// </summary>
        public float[] SuspensionVelocity;

        /// <summary>
        /// The suspension acceleration
        /// </summary>
        public float[] SuspensionAcceleration;

        /// <summary>
        /// Speed of each wheel
        /// </summary>
        public float[] WheelSpeed;

        /// <summary>
        /// Slip ratio for each wheel
        /// </summary>
        public float[] WheelSlip;

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
        private PacketHeader packetHeader;

        public PacketMotionData(PacketHeader packetHeader) : this()
        {
            this.packetHeader = packetHeader;
            this.CarMotionData = new CarMotionData[22];
        }
    }
}