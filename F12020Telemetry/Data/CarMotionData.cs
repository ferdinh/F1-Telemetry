namespace F12020Telemetry.Data
{
    public class CarMotionData
    {
        /// <summary>
        /// World space X position
        /// </summary>
        public float WorldPositionX { get; set; }

        /// <summary>
        /// World space Y position
        /// </summary>
        public float WorldPositionY { get; set; }

        /// <summary>
        /// World space Z position
        /// </summary>
        public float WorldPositionZ { get; set; }

        /// <summary>
        /// Velocity in world space X
        /// </summary>
        public float WorldVelocityX { get; set; }

        /// <summary>
        /// Velocity in world space Y
        /// </summary>
        public float WorldVelocityY { get; set; }

        /// <summary>
        /// Velocity in world space Z
        /// </summary>
        public float WorldVelocityZ { get; set; }

        /// <summary>
        /// World space forward X direction (normalised)
        /// </summary>
        public short WorldForwardDirX { get; set; }

        /// <summary>
        /// World space forward Y direction (normalised)
        /// </summary>
        public short WorldForwardDirY { get; set; }

        /// <summary>
        /// World space forward Z direction (normalised)
        /// </summary>
        public short WorldForwardDirZ { get; set; }

        /// <summary>
        /// World space right X direction (normalised)
        /// </summary>
        public short WorldRightDirX { get; set; }

        /// <summary>
        /// World space right Y direction (normalised)
        /// </summary>
        public short WorldRightDirY { get; set; }

        /// <summary>
        /// World space right Z direction (normalised)
        /// </summary>
        public short WorldRightDirZ { get; set; }

        /// <summary>
        /// Lateral G-Force component
        /// </summary>
        public float GForceLateral { get; set; }

        /// <summary>
        /// Longitudinal G-Force component
        /// </summary>
        public float GForceLongitudinal { get; set; }

        /// <summary>
        /// Vertical G-Force component
        /// </summary>
        public float GForceVertical { get; set; }

        /// <summary>
        /// Yaw angle in radians
        /// </summary>
        public float Yaw { get; set; }

        /// <summary>
        /// Pitch angle in radians
        /// </summary>
        public float Pitch { get; set; }

        /// <summary>
        /// Roll angle in radians
        /// </summary>
        public float Roll { get; set; }
    }
}