using System;
using System.Collections.Generic;
using System.Text;

namespace F12020Telemetry.Packet
{
    public interface IPacket
    {
        PacketHeader Header { get; set; }
    }
}
