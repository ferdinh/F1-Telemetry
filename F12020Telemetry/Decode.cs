using F12020Telemetry.Data;
using F12020Telemetry.Packet;
using System;
using System.IO;

namespace F12020Telemetry
{
    public static class Decode
    {
        private static readonly int MaxNumberOfCarsOnTrack = 22;
        private static readonly int NumberOfTyres = 4;
        private static readonly int NumberOfWeatherSamples = 20;

        /// <summary>
        /// Decode header packet.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Packet Header</returns>
        private static PacketHeader Header(BinaryReader reader)
        {
            var packetHeader = new PacketHeader();

            packetHeader.PacketFormat = reader.ReadUInt16();
            packetHeader.GameMajorVersion = reader.ReadByte();
            packetHeader.GameMinorVersion = reader.ReadByte();
            packetHeader.PacketVersion = reader.ReadByte();
            packetHeader.PacketId = (PacketTypes)reader.ReadByte();
            packetHeader.SessionUID = reader.ReadUInt64();
            packetHeader.SessionTime = reader.ReadSingle();
            packetHeader.FrameIdentifier = reader.ReadUInt32();
            packetHeader.PlayerCarIndex = reader.ReadByte();
            packetHeader.SecondaryPlayerCarIndex = reader.ReadByte();

            return packetHeader;
        }
    }
}