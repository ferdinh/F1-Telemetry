using F12020Telemetry.Data;

namespace F12020Telemetry.Packet
{
    public class PacketParticipantsData : IPacket
    {
        public PacketHeader Header { get; set; }

        public byte NumActiveCars;
        public ParticipantData[] Participants;

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