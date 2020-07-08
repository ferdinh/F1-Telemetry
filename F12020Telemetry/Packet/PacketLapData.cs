using F12020Telemetry.Data;

namespace F12020Telemetry.Packet
{
    internal struct PacketLapData
    {
        public PacketHeader Header;

        /// <summary>
        ///  Lap data for all cars on track
        /// </summary>
        public LapData[] LapData;
    };
}