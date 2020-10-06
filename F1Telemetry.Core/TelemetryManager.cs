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

        private float PreviousSessionTime;
        public float CurrentSessionTime { get; private set; }

        /// <summary>
        /// Invoked when there is a new F1 game session.
        /// </summary>
        public event EventHandler NewSession;

        public event EventHandler OnRestart;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetryManager"/> class.
        /// </summary>
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

            if (packet != null)
            {
                if (packet.Header.SessionTime < PreviousSessionTime)
                {
                    OnRestarting();
                    PreviousSessionTime = CurrentSessionTime = packet.Header.SessionTime;
                } 
                else
                {
                    PreviousSessionTime = CurrentSessionTime;
                    CurrentSessionTime = packet == null ? 0.0f : packet.Header.SessionTime;
                }

            }

            return packetTypeReceived;
        }

        private void OnRestarting()
        {
            OnRestart?.Invoke(this, EventArgs.Empty);
            Console.WriteLine("Restarting");
        }

        /// <summary>
        /// Gets the player information.
        /// </summary>
        /// <returns></returns>
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
                OnNewSession();
            }
            else
            {
                bool isNewSession = !packetSessionData.Equals(Session);

                if (isNewSession)
                {
                    PlayerCarIndex = packetSessionData.Header.PlayerCarIndex;
                    Session = packetSessionData;

                    drivers = NewDriverList();

                    OnNewSession();
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

        private void OnNewSession()
        {
            NewSession?.Invoke(this, EventArgs.Empty);
        }

        private void ResetSessionTime()
        {
            PreviousSessionTime = CurrentSessionTime = 0.0f;
        }
    }
}