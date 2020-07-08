﻿using F12020Telemetry.Data;
using System;

namespace F12020Telemetry.Packet
{
    public struct PacketCarTelemetryData
    {
        public PacketHeader Header;

        public CarTelemetryData[] CarTelemetryData;

        /// <summary>
        /// Bit flags specifying which buttons are being pressed currently
        /// </summary>
        public UInt32 ButtonStatus;

        /// <summary>
        /// Index of MFD panel open - 255 = MFD closed
        /// </summary>
        public byte MfdPanelIndex;

        // Single player, race – 0 = Car setup, 1 = Pits
        // 2 = Damage, 3 =  Engine, 4 = Temperatures
        // May vary depending on game mode
        public byte MfdPanelIndexSecondaryPlayer;

        /// <summary>
        /// Suggested gear for the player (1-8)
        /// 0 if no gear suggested
        /// </summary>
        public sbyte SuggestedGear;
    }
}