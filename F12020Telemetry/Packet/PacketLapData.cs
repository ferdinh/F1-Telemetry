using F12020Telemetry.Data;

namespace F12020Telemetry.Packet
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