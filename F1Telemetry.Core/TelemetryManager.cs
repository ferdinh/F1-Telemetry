﻿using F1Telemetry.Core.Data;
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

            switch (packet)
            {
                case PacketSessionData packetSessionData:
                    ProcessPacketSessionData(packetSessionData);
                    break;

                case PacketCarTelemetryData packetCarTelemetryData:
                    ProcessPacketCarTelemetryData(packetCarTelemetryData);
                    break;

                case PacketLapData packetLapData:
                    ProcessPacketLapData(packetLapData);
                    break;

                case PacketCarStatusData packetCarStatusData:
                    ProcessPacketCarStatusData(packetCarStatusData);
                    break;
            }

            var packetTypeReceived = packet == null ? PacketTypes.Invalid : packet.Header.PacketTypes;

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

        private void ProcessPacketSessionData(PacketSessionData packetSessionData)
        {
            if (Session == null)
            {
                PlayerCarIndex = packetSessionData.Header.PlayerCarIndex;
                Session = packetSessionData;
                NewSession?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                bool isNewSession = !packetSessionData.Equals(Session);

                if (isNewSession)
                {
                    PlayerCarIndex = packetSessionData.Header.PlayerCarIndex;
                    Session = packetSessionData;

                    drivers = NewDriverList();

                    NewSession?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void ProcessPacketCarTelemetryData(PacketCarTelemetryData packetCarTelemetryData)
        {
            for (int i = 0; i < packetCarTelemetryData.CarTelemetryData.Length; i++)
            {
                var carTelemData = packetCarTelemetryData.CarTelemetryData[i];
                Drivers[i].CarTelemetryData.Add(carTelemData);
            }
        }

        private void ProcessPacketCarStatusData(PacketCarStatusData packetCarStatusData)
        {
            for (int i = 0; i < packetCarStatusData.CarStatusData.Length; i++)
            {
                var carStatusData = packetCarStatusData.CarStatusData[i];
                Drivers[i].AddCarStatusData(carStatusData);
            }
        }

        private void ProcessPacketLapData(PacketLapData packetLapData)
        {
            for (int i = 0; i < packetLapData.LapData.Length; i++)
            {
                var lapData = packetLapData.LapData[i];
                Drivers[i].AddLapData(lapData);
            }
        }
    }
}