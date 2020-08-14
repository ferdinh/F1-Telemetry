namespace F12020Telemetry.Data
{
    public class WeatherForecastSample
    {
        /// <summary>
        /// The session type
        /// </summary>
        public SessionType SessionType { get; set; }

        /// <summary>
        /// Time in minutes the forecast is for
        /// </summary>
        public byte TimeOffset { get; set; }

        /// <summary>
        /// Weather - 0 = clear, 1 = light cloud, 2 = overcast, 3 = light rain, 4 = heavy rain, 5 = storm
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
    }
}