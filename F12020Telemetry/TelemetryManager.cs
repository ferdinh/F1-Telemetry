using F12020Telemetry.Data;
using F12020Telemetry.Packet;
using System;
using System.Collections.Generic;

namespace F12020Telemetry
{
    /// <summary>
    /// Manages incoming telemetry data.
    /// </summary>
    public class TelemetryManager
    {
        public IReadOnlyList<Driver> Drivers { get; }

        public PacketSessionData Session { get; private set; }

        private int PlayerCarIndex;

        public event EventHandler NewSession;

        public TelemetryManager()
        {
            var newDrivers = new List<Driver>();

            for (int i = 0; i < Decode.MaxNumberOfCarsOnTrack; i++)
            {
                newDrivers.Add(new Driver());
            }

            Drivers = newDrivers;
        }

        /// <summary>
        /// Feeds packet data into the manager.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>Type of packet received.</returns>
        public PacketTypes Feed(byte[] bytes)
        {
            IPacket packet = Decode.Packet(bytes);

            var packetTypeReceived = PacketTypes.Invalid;

            switch (packet)
            {
                //case PacketSessionData pSessionData:
                //    packetSessionData = pSessionData;
                //    packetQueue.Enqueue(packetSessionData);
                //    break;

                //case PacketCarTelemetryData pCarTelemetryData:
                //    packetCarTelemetryData = pCarTelemetryData;
                //    packetQueue.Enqueue(packetCarTelemetryData);
                //    break;

                //case PacketLapData pLapData:
                //    LapData = pLapData;
                //    packetQueue.Enqueue(LapData);
                //    break;

                //case PacketMotionData pMotionData:
                //    packetCarMotionData = pMotionData;
                //    packetQueue.Enqueue(packetCarMotionData);
                //    break;

                //default:
                //    break;

                case PacketSessionData packetSessionData:

                    if (Session == null)
                    {
                        PlayerCarIndex = packetSessionData.Header.PlayerCarIndex;
                        Session = packetSessionData;
                        packetTypeReceived = PacketTypes.Session;
                        NewSession?.Invoke(this, EventArgs.Empty);
                    }
                    else
                    {
                        bool isNewSession = !packetSessionData.Equals(Session);

                        if (isNewSession)
                        {
                            PlayerCarIndex = packetSessionData.Header.PlayerCarIndex;
                            Session = packetSessionData;
                            packetTypeReceived = PacketTypes.Session;
                            NewSession?.Invoke(this, EventArgs.Empty);
                        }
                    }

                    break;

                case PacketCarTelemetryData packetCarTelemetryData:

                    for (int i = 0; i < packetCarTelemetryData.CarTelemetryData.Length; i++)
                    {
                        var carTelemData = packetCarTelemetryData.CarTelemetryData[i];
                        Drivers[i].CarTelemetryData.Add(carTelemData);
                    }

                    break;

                case PacketLapData pLapData:

                    for (int i = 0; i < pLapData.LapData.Length; i++)
                    {
                        var lapData = pLapData.LapData[i];
                        Drivers[i].AddLapData(lapData);
                    }

                    break;

                    //case PacketMotionData pMotionData:
                    //    packetCarMotionData = pMotionData;
                    //    packetQueue.Enqueue(packetCarMotionData);
                    //    break;
            }

            return packetTypeReceived;
        }

        public Driver GetPlayerInfo()
        {
            return Drivers[PlayerCarIndex];
        }
    }
}