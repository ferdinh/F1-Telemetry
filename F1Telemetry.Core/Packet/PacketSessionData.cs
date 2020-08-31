using F1Telemetry.Core.Data;
using System;
using System.Collections.Generic;

namespace F1Telemetry.Core.Packet
{
    public class PacketSessionData : IPacket
    {
        public PacketHeader Header { get; set; }

        /// <summary>
        /// Weather - 0 = clear, 1 = light cloud, 2 = overcast
        /// 3 = light rain, 4 = heavy rain, 5 = storm
        /// </summary>
        public WeatherType Weather { get; set; }

        /// <summary>
        /// Track temp. in degrees celsius
        /// </summary>
        public sbyte TrackTemperature { get; set; }

        /// <summary>
        /// Air temp. in degrees celsius
        /// </summary>
        public sbyte AirTemperature { get; set; }

        /// <summary>
        /// Total number of laps in this race
        /// </summary>
        public byte TotalLaps { get; set; }

        /// <summary>
        /// Track length in metres
        /// </summary>
        public ushort TrackLength { get; set; }

        /// <summary>
        /// The session type
        /// </summary>
        public SessionType SessionType { get; set; }

        /// <summary>
        /// The track identifier; -1 for unknown, 0-21 for tracks, see appendix
        /// </summary>
        public sbyte TrackId { get; set; }

        /// <summary>
        /// Formula, 0 = F1 Modern, 1 = F1 Classic, 2 = F2, 3 = F1 Generic
        /// </summary>
        public byte Formula { get; set; }

        /// <summary>
        /// Time left in session in seconds
        /// </summary>
        public ushort SessionTimeLeft { get; set; }

        /// <summary>
        /// Session duration in seconds
        /// </summary>
        public ushort SessionDuration { get; set; }

        /// <summary>
        /// Pit speed limit in kilometres per hour
        /// </summary>
        public byte PitSpeedLimit { get; set; }

        /// <summary>
        /// Whether the game is paused
        /// </summary>
        public byte GamePaused { get; set; }

        /// <summary>
        /// Whether the player is spectating
        /// </summary
        public byte IsSpectating { get; set; }

        /// <summary>
        /// Index of the car being spectated
        /// </summary>
        public byte SpectatorCarIndex { get; set; }

        /// <summary>
        /// SLI Pro support, 0 = inactive, 1 = active
        /// </summary>
        public byte SliProNativeSupport { get; set; }

        /// <summary>
        /// Number of marshal zones to follow
        /// </summary>
        public byte NumMarshalZones { get; set; }

        /// <summary>
        /// List of marshal zones – max 21
        /// </summary>
        public MarshalZone[] MarshalZones { get; set; } = new MarshalZone[Decode.MaxNumberOfMarshalZones];

        /// <summary>
        /// 0 = no safety car, 1 = full safety car, 2 = virtual safety car
        /// </summary>
        public byte SafetyCarStatus { get; set; }

        /// <summary>
        /// 0 = offline, 1 = online
        /// </summary>
        public byte NetworkGame { get; set; }

        /// <summary>
        /// Number of weather samples to follow
        /// </summary>
        public byte NumWeatherForecastSamples { get; set; }

        /// <summary>
        /// Array of weather forecast samples
        /// </summary>
        public WeatherForecastSample[] WeatherForecastSamples { get; set; } = new WeatherForecastSample[Decode.MaxNumberOfWeatherSamples];

        public PacketSessionData(PacketHeader packetHeader)
        {
            Header = packetHeader;
        }

        public override bool Equals(object obj)
        {
            return obj is PacketSessionData data &&
                   EqualityComparer<ulong>.Default.Equals(Header.SessionUID, data.Header.SessionUID);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header.SessionUID);
        }
    }
}