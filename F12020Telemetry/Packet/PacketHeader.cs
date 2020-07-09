using System;

namespace F12020Telemetry.Packet
{
    public struct PacketHeader
    {
        /// <summary>
        /// 2020
        /// </summary>
        public UInt16 PacketFormat;

        /// <summary>
        /// Game major version - "X.00"
        /// </summary>
        public byte GameMajorVersion;

        /// <summary>
        /// Game minor version - "1.XX"
        /// </summary>
        public byte GameMinorVersion;

        /// <summary>
        /// Version of this packet type, all start from 1
        /// </summary>
        public byte PacketVersion;

        /// <summary>
        /// Identifier for the packet type
        /// </summary>
        public PacketTypes PacketId;

        /// <summary>
        /// Unique identifier for the session
        /// </summary>
        public UInt64 SessionUID;

        /// <summary>
        /// Session timestamp
        /// </summary>
        public float SessionTime;

        /// <summary>
        /// Identifier for the frame the data was retrieved on
        /// </summary>
        public UInt32 FrameIdentifier;

        /// <summary>
        /// Index of player's car in the array
        /// </summary>
        public byte PlayerCarIndex;

        /// <summary>
        /// Index of secondary player's car in the array (splitscreen); 255 if no second player
        /// </summary>
        public byte SecondaryPlayerCarIndex;
    }
}