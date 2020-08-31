using F1Telemetry.Core.Data;
using F1Telemetry.Core.Packet;
using System;
using System.Collections.Generic;

namespace F1Telemetry.Core
{
    /// <summary>
    /// Manages incoming telemetry data.
    /// </summary>
    public class TelemetryManager
    {
        private List<Driver> drivers = new List<Driver>();
        public IReadOnlyList<Driver> Drivers { get => drivers.AsReadOnly(); }

        public PacketSessionData Session { get; private set; }

        private int PlayerCarIndex;

        public event EventHandler NewSession;

        public TelemetryManager()
        {
            drivers = NewDriverList();
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

                            drivers = NewDriverList();

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

        /// <summary>
        /// Creates new driver list.
        /// </summary>
        /// <returns>Clean Driver List.</returns>
        private List<Driver> NewDriverList()
        {
            var newDrivers = new List<Driver>();

            for (int i = 0; i < Decode.MaxNumberOfCarsOnTrack; i++)
            {
                newDrivers.Add(new Driver(this));
            }
            return newDrivers;
        }
    }
}