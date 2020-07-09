using F12020Telemetry.Data;
using F12020Telemetry.Packet;
using System;
using System.IO;

namespace F12020Telemetry
{
    public static class Decode
    {
        private static readonly int MaxNumberOfCarsOnTrack = 22;
        private static readonly int NumberOfTyres = 4;
        private static readonly int NumberOfWeatherSamples = 20;
    }
}