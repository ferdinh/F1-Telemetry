using F1Telemetry.Core.Data;

namespace F1Telemetry.Core.Packet
{
    public class PacketCarStatusData : IPacket
    {
        public PacketHeader Header { get; set; }
        public CarStatusData[] CarStatusData { get; set; }

        private PacketCarStatusData()
        {
        }

        public PacketCarStatusData(PacketHeader packetHeader)
        {
            Header = packetHeader;
            CarStatusData = new CarStatusData[Decode.MaxNumberOfCarsOnTrack];
        }
    }
}