using System;
using System.Collections.Generic;
using System.Text;

namespace F1Telemetry.Core.Packet
{
    public interface IPacket
    {
        PacketHeader Header { get; set; }
    }
}
