using F1Telemetry.Core.Data;

namespace F1Telemetry.Core.Packet
{
    public class PacketLapData : IPacket
    {
        public PacketHeader Header { get; set; }

        /// <summary>
        ///  Lap data for all cars on track
        /// </summary>
        public LapData[] LapData { get; set; } = new LapData[Decode.MaxNumberOfCarsOnTrack];

        public PacketLapData(PacketHeader header)
        {
            Header = header;
        }
    };
}