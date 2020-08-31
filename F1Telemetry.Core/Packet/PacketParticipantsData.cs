using F1Telemetry.Core.Data;

namespace F1Telemetry.Core.Packet
{
    public class PacketParticipantsData : IPacket
    {
        public PacketHeader Header { get; set; }

        public byte NumActiveCars { get; set; }
        public ParticipantData[] Participants { get; set; }

        private PacketParticipantsData()
        {
        }

        public PacketParticipantsData(PacketHeader packetHeader)
        {
            Header = packetHeader;

            Participants = new ParticipantData[Decode.MaxNumberOfCarsOnTrack];

            for (int i = 0; i < Decode.MaxNumberOfCarsOnTrack; i++)
            {
                Participants[i] = new ParticipantData();
            }
        }
    }
}