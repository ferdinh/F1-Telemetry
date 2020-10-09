namespace F1Telemetry.Core.Packet
{
    public interface IPacket
    {
        PacketHeader Header { get; set; }
    }
}