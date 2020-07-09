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
        private static readonly int MaxNumberOfWeatherSamples = 20;
        public static readonly int MaxNumberOfMarshalZones = 21;

        /// <summary>
        /// Decode header packet.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns>Packet Header</returns>
        private static PacketHeader Header(BinaryReader reader)
        {
            var packetHeader = new PacketHeader();

            packetHeader.PacketFormat = reader.ReadUInt16();
            packetHeader.GameMajorVersion = reader.ReadByte();
            packetHeader.GameMinorVersion = reader.ReadByte();
            packetHeader.PacketVersion = reader.ReadByte();
            packetHeader.PacketId = (PacketTypes)reader.ReadByte();
            packetHeader.SessionUID = reader.ReadUInt64();
            packetHeader.SessionTime = reader.ReadSingle();
            packetHeader.FrameIdentifier = reader.ReadUInt32();
            packetHeader.PlayerCarIndex = reader.ReadByte();
            packetHeader.SecondaryPlayerCarIndex = reader.ReadByte();

            return packetHeader;
        }

        private static IPacket Motion(PacketHeader packetHeader, BinaryReader reader)
        {
            var packetCarMotionData = new PacketMotionData(packetHeader);

            for (int i = 0; i < MaxNumberOfCarsOnTrack; i++)
            {
                packetCarMotionData.CarMotionData[i].WorldPositionX = reader.ReadSingle();
                packetCarMotionData.CarMotionData[i].WorldPositionY = reader.ReadSingle();
                packetCarMotionData.CarMotionData[i].WorldPositionZ = reader.ReadSingle();

                packetCarMotionData.CarMotionData[i].WorldVelocityX = reader.ReadSingle();
                packetCarMotionData.CarMotionData[i].WorldVelocityY = reader.ReadSingle();
                packetCarMotionData.CarMotionData[i].WorldVelocityZ = reader.ReadSingle();

                packetCarMotionData.CarMotionData[i].WorldForwardDirX = reader.ReadInt16();
                packetCarMotionData.CarMotionData[i].WorldForwardDirY = reader.ReadInt16();
                packetCarMotionData.CarMotionData[i].WorldForwardDirZ = reader.ReadInt16();

                packetCarMotionData.CarMotionData[i].WorldRightDirX = reader.ReadInt16();
                packetCarMotionData.CarMotionData[i].WorldRightDirY = reader.ReadInt16();
                packetCarMotionData.CarMotionData[i].WorldRightDirZ = reader.ReadInt16();

                packetCarMotionData.CarMotionData[i].GForceLateral = reader.ReadSingle();
                packetCarMotionData.CarMotionData[i].GForceLongitudinal = reader.ReadSingle();
                packetCarMotionData.CarMotionData[i].GForceVertical = reader.ReadSingle();

                packetCarMotionData.CarMotionData[i].Yaw = reader.ReadSingle();
                packetCarMotionData.CarMotionData[i].Pitch = reader.ReadSingle();
                packetCarMotionData.CarMotionData[i].Roll = reader.ReadSingle();
            }

            // Extra player data
            for (int j = 0; j < NumberOfTyres; j++)
            {
                packetCarMotionData.SuspensionPosition[j] = reader.ReadSingle();
            }

            for (int j = 0; j < NumberOfTyres; j++)
            {
                packetCarMotionData.SuspensionVelocity[j] = reader.ReadSingle();
            }

            for (int j = 0; j < NumberOfTyres; j++)
            {
                packetCarMotionData.SuspensionAcceleration[j] = reader.ReadSingle();
            }

            for (int j = 0; j < NumberOfTyres; j++)
            {
                packetCarMotionData.WheelSpeed[j] = reader.ReadSingle();
            }

            for (int j = 0; j < NumberOfTyres; j++)
            {
                packetCarMotionData.WheelSlip[j] = reader.ReadSingle();
            }

            packetCarMotionData.LocalVelocityX = reader.ReadSingle();
            packetCarMotionData.LocalVelocityY = reader.ReadSingle();
            packetCarMotionData.LocalVelocityZ = reader.ReadSingle();

            packetCarMotionData.AngularVelocityX = reader.ReadSingle();
            packetCarMotionData.AngularVelocityY = reader.ReadSingle();
            packetCarMotionData.AngularVelocityZ = reader.ReadSingle();

            packetCarMotionData.AngularAccelerationX = reader.ReadSingle();
            packetCarMotionData.AngularAccelerationY = reader.ReadSingle();
            packetCarMotionData.AngularAccelerationZ = reader.ReadSingle();

            packetCarMotionData.FrontWheelsAngle = reader.ReadSingle();

            return packetCarMotionData;
        }

        private static IPacket Session(PacketHeader packetHeader, BinaryReader reader)
        {
            var packetSessionData = new PacketSessionData(packetHeader);

            packetSessionData.Weather = reader.ReadByte();

            packetSessionData.TrackTemperature = reader.ReadSByte();
            packetSessionData.AirTemperature = reader.ReadSByte();

            packetSessionData.TotalLaps = reader.ReadByte();
            packetSessionData.TrackLength = reader.ReadUInt16();
            packetSessionData.SessionType = reader.ReadByte();

            packetSessionData.TrackId = reader.ReadSByte();
            packetSessionData.Formula = reader.ReadByte();

            packetSessionData.SessionTimeLeft = reader.ReadUInt16();
            packetSessionData.SessionDuration = reader.ReadUInt16();
            packetSessionData.PitSpeedLimit = reader.ReadByte();
            packetSessionData.GamePaused = reader.ReadByte();
            packetSessionData.IsSpectating = reader.ReadByte();
            packetSessionData.SpectatorCarIndex = reader.ReadByte();
            packetSessionData.SliProNativeSupport = reader.ReadByte();
            packetSessionData.NumMarshalZones = reader.ReadByte();

            for (int j = 0; j < MaxNumberOfMarshalZones; j++)
            {
                packetSessionData.MarshalZones[j].ZoneStart = reader.ReadSingle();
                packetSessionData.MarshalZones[j].ZoneFlag = reader.ReadSByte();
            }

            packetSessionData.SafetyCarStatus = reader.ReadByte();

            packetSessionData.NetworkGame = reader.ReadByte();
            packetSessionData.NumWeatherForecastSamples = reader.ReadByte();

            for (int j = 0; j < MaxNumberOfWeatherSamples; j++)
            {
                packetSessionData.WeatherForecastSamples[j].SessionType = reader.ReadByte();

                packetSessionData.WeatherForecastSamples[j].TimeOffset = reader.ReadByte();
                packetSessionData.WeatherForecastSamples[j].Weather = reader.ReadByte();

                packetSessionData.WeatherForecastSamples[j].TrackTemperature = reader.ReadSByte();
                packetSessionData.WeatherForecastSamples[j].AirTemperature = reader.ReadSByte();
            }

            return packetSessionData;
        }

        private static IPacket Lap(PacketHeader packetHeader, BinaryReader reader)
        {
            var packetLapData = new PacketLapData(packetHeader);

            for (int i = 0; i < MaxNumberOfCarsOnTrack; i++)
            {
                packetLapData.LapData[i].LastLapTime = reader.ReadSingle();
                packetLapData.LapData[i].CurrentLapTime = reader.ReadSingle();

                packetLapData.LapData[i].Sector1TimeInMS = reader.ReadUInt16();
                packetLapData.LapData[i].Sector2TimeInMS = reader.ReadUInt16();

                packetLapData.LapData[i].BestLapTime = reader.ReadSingle();
                packetLapData.LapData[i].BestLapNum = reader.ReadByte();

                packetLapData.LapData[i].BestLapSector1TimeInMS = reader.ReadUInt16();
                packetLapData.LapData[i].BestLapSector2TimeInMS = reader.ReadUInt16();
                packetLapData.LapData[i].BestLapSector3TimeInMS = reader.ReadUInt16();

                packetLapData.LapData[i].BestOverallSector1TimeInMS = reader.ReadUInt16();
                packetLapData.LapData[i].BestOverallSector1LapNum = reader.ReadByte();

                packetLapData.LapData[i].BestOverallSector2TimeInMS = reader.ReadUInt16();
                packetLapData.LapData[i].BestOverallSector2LapNum = reader.ReadByte();

                packetLapData.LapData[i].BestOverallSector3TimeInMS = reader.ReadUInt16();
                packetLapData.LapData[i].BestOverallSector3LapNum = reader.ReadByte();

                packetLapData.LapData[i].LapDistance = reader.ReadSingle();
                packetLapData.LapData[i].TotalDistance = reader.ReadSingle();

                packetLapData.LapData[i].SafetyCarDelta = reader.ReadSingle();
                packetLapData.LapData[i].CarPosition = reader.ReadByte();

                packetLapData.LapData[i].CurrentLapNum = reader.ReadByte();
                packetLapData.LapData[i].PitStatus = reader.ReadByte();
                packetLapData.LapData[i].Sector = reader.ReadByte();
                packetLapData.LapData[i].CurrentLapInvalid = reader.ReadByte();
                packetLapData.LapData[i].Penalties = reader.ReadByte();
                packetLapData.LapData[i].GridPosition = reader.ReadByte();
                packetLapData.LapData[i].DriverStatus = reader.ReadByte();
                packetLapData.LapData[i].ResultStatus = reader.ReadByte();
            }

            return packetLapData;
        }

        private static IPacket CarTelemetry(PacketHeader packetHeader, BinaryReader reader)
        {
            var packetCarTelemetryData = new PacketCarTelemetryData(packetHeader);

            for (int i = 0; i < MaxNumberOfCarsOnTrack; i++)
            {
                packetCarTelemetryData.CarTelemetryData[i].Speed = reader.ReadUInt16();
                packetCarTelemetryData.CarTelemetryData[i].Throttle = reader.ReadSingle();
                packetCarTelemetryData.CarTelemetryData[i].Steer = reader.ReadSingle();
                packetCarTelemetryData.CarTelemetryData[i].Brake = reader.ReadSingle();
                packetCarTelemetryData.CarTelemetryData[i].Clutch = reader.ReadByte();
                packetCarTelemetryData.CarTelemetryData[i].Gear = reader.ReadSByte();
                packetCarTelemetryData.CarTelemetryData[i].EngineRPM = reader.ReadUInt16();
                packetCarTelemetryData.CarTelemetryData[i].Drs = reader.ReadByte();
                packetCarTelemetryData.CarTelemetryData[i].RevLightsPercent = reader.ReadByte();

                packetCarTelemetryData.CarTelemetryData[i].BrakesTemperature = new UInt16[NumberOfTyres];
                packetCarTelemetryData.CarTelemetryData[i].TyresSurfaceTemperature = new byte[NumberOfTyres];
                packetCarTelemetryData.CarTelemetryData[i].TyresInnerTemperature = new byte[NumberOfTyres];
                packetCarTelemetryData.CarTelemetryData[i].TyresPressure = new float[NumberOfTyres];
                packetCarTelemetryData.CarTelemetryData[i].SurfaceType = new byte[NumberOfTyres];

                for (int j = 0; j < NumberOfTyres; j++)
                {
                    packetCarTelemetryData.CarTelemetryData[i].BrakesTemperature[j] = reader.ReadUInt16();
                }

                for (int j = 0; j < NumberOfTyres; j++)
                {
                    packetCarTelemetryData.CarTelemetryData[i].TyresSurfaceTemperature[j] = reader.ReadByte();
                }

                for (int j = 0; j < NumberOfTyres; j++)
                {
                    packetCarTelemetryData.CarTelemetryData[i].TyresInnerTemperature[j] = reader.ReadByte();
                }

                packetCarTelemetryData.CarTelemetryData[i].EngineTemperature = reader.ReadUInt16();

                for (int j = 0; j < NumberOfTyres; j++)
                {
                    packetCarTelemetryData.CarTelemetryData[i].TyresPressure[j] = reader.ReadSingle();
                }

                for (int j = 0; j < NumberOfTyres; j++)
                {
                    packetCarTelemetryData.CarTelemetryData[i].SurfaceType[j] = reader.ReadByte();
                }
            }

            packetCarTelemetryData.ButtonStatus = reader.ReadUInt32();

            packetCarTelemetryData.MfdPanelIndex = reader.ReadByte();
            packetCarTelemetryData.MfdPanelIndexSecondaryPlayer = reader.ReadByte();

            packetCarTelemetryData.SuggestedGear = reader.ReadSByte();

            return packetCarTelemetryData;
        }

        public static IPacket Packet(byte[] packetBytes)
        {
            IPacket packet = null;

            using (var stream = new MemoryStream(packetBytes))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    var packetHeader = Decode.Header(reader);

                    Func<PacketHeader, BinaryReader, IPacket> decoder = null;

                    switch (packetHeader.PacketId)
                    {
                        case PacketTypes.Motion:
                            decoder = Decode.Motion;
                            break;

                        case PacketTypes.Session:
                            decoder = Decode.Session;
                            break;

                        case PacketTypes.LapData:
                            decoder = Decode.Lap;
                            break;

                        //case PacketTypes.Event:
                        //    break;

                        //case PacketTypes.Participants:
                        //    break;

                        //case PacketTypes.CarSetups:
                        //    break;

                        case PacketTypes.CarTelemetry:
                            decoder = Decode.CarTelemetry;
                            break;

                        //case PacketTypes.CarStatus:
                        //    break;

                        //case PacketTypes.FinalClassification:
                        //    break;

                        //case PacketTypes.LobbyInfo:
                        //    break;

                        default:
                            break;
                    }

                    if (decoder != null)
                    {
                        packet = decoder(packetHeader, reader);
                    }
                }

                return packet;
            }
        }
    }
}