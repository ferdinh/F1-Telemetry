using F12020Telemetry.Data;
using System;

namespace F12020Telemetry.Packet
{
    internal struct PacketSessionData : IPacket
    {
        public PacketHeader Header { get; set; }

        /// <summary>
        /// Weather - 0 = clear, 1 = light cloud, 2 = overcast
        /// 3 = light rain, 4 = heavy rain, 5 = storm
        /// </summary>
        public byte Weather;

        /// <summary>
        /// Track temp. in degrees celsius
        /// </summary>
        public sbyte TrackTemperature;

        /// <summary>
        /// Air temp. in degrees celsius
        /// </summary>
        public sbyte AirTemperature;

        /// <summary>
        /// Total number of laps in this race
        /// </summary>
        public byte TotalLaps;

        /// <summary>
        /// Track length in metres
        /// </summary>
        public UInt16 TrackLength;

        /// <summary>
        /// The session type; 0 = unknown, 1 = P1, 2 = P2, 3 = P3, 4 = Short P, 5 = Q1, 6 = Q2, 7 = Q3, 8 = Short Q, 9 = OSQ, 10 = R, 11 = R2, 12 = Time Trial
        /// </summary>
        public byte SessionType;

        /// <summary>
        /// The track identifier; -1 for unknown, 0-21 for tracks, see appendix
        /// </summary>
        public sbyte TrackId;

        /// <summary>
        /// Formula, 0 = F1 Modern, 1 = F1 Classic, 2 = F2, 3 = F1 Generic
        /// </summary>
        public byte Formula;

        /// <summary>
        /// Time left in session in seconds
        /// </summary>
        public UInt16 SessionTimeLeft;

        /// <summary>
        /// Session duration in seconds
        /// </summary>
        public UInt16 SessionDuration;

        /// <summary>
        /// Pit speed limit in kilometres per hour
        /// </summary>
        public byte PitSpeedLimit;

        /// <summary>
        /// Whether the game is paused
        /// </summary>
        public byte GamePaused;

        /// <summary>
        /// Whether the player is spectating
        /// </summary
        public byte IsSpectating;

        /// <summary>
        /// Index of the car being spectated
        /// </summary>
        public byte SpectatorCarIndex;

        /// <summary>
        /// SLI Pro support, 0 = inactive, 1 = active
        /// </summary>
        public byte SliProNativeSupport;

        /// <summary>
        /// Number of marshal zones to follow
        /// </summary>
        public byte NumMarshalZones;

        /// <summary>
        /// List of marshal zones – max 21
        /// </summary>
        public MarshalZone[] MarshalZones;

        /// <summary>
        /// 0 = no safety car, 1 = full safety car, 2 = virtual safety car
        /// </summary>
        public byte SafetyCarStatus;

        /// <summary>
        /// 0 = offline, 1 = online
        /// </summary>
        public byte NetworkGame;

        /// <summary>
        /// Number of weather samples to follow
        /// </summary>
        public byte NumWeatherForecastSamples;

        /// <summary>
        /// Array of weather forecast samples
        /// </summary>
        public WeatherForecastSample[] WeatherForecastSamples;
    }
}