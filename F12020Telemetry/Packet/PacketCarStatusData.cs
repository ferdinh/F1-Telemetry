using F12020Telemetry.Data;

namespace F12020Telemetry.Packet
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