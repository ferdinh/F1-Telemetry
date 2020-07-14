using F12020Telemetry.Data;
using F12020Telemetry.Packet;
using System;
using System.IO;

namespace F12020Telemetry
{
    public static class Decode
    {
        public static readonly int MaxNumberOfCarsOnTrack = 22;
        public static readonly int NumberOfTyres = 4;
        public static readonly int MaxNumberOfWeatherSamples = 20;
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
            packetHeader.PacketTypes = (PacketTypes)reader.ReadByte();
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
                var carMotionData = new CarMotionData();

                carMotionData.WorldPositionX = reader.ReadSingle();
                carMotionData.WorldPositionY = reader.ReadSingle();
                carMotionData.WorldPositionZ = reader.ReadSingle();

                carMotionData.WorldVelocityX = reader.ReadSingle();
                carMotionData.WorldVelocityY = reader.ReadSingle();
                carMotionData.WorldVelocityZ = reader.ReadSingle();

                carMotionData.WorldForwardDirX = reader.ReadInt16();
                carMotionData.WorldForwardDirY = reader.ReadInt16();
                carMotionData.WorldForwardDirZ = reader.ReadInt16();

                carMotionData.WorldRightDirX = reader.ReadInt16();
                carMotionData.WorldRightDirY = reader.ReadInt16();
                carMotionData.WorldRightDirZ = reader.ReadInt16();

                carMotionData.GForceLateral = reader.ReadSingle();
                carMotionData.GForceLongitudinal = reader.ReadSingle();
                carMotionData.GForceVertical = reader.ReadSingle();

                carMotionData.Yaw = reader.ReadSingle();
                carMotionData.Pitch = reader.ReadSingle();
                carMotionData.Roll = reader.ReadSingle();

                packetCarMotionData.CarMotionData[i] = carMotionData;
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

            packetSessionData.Weather = (WeatherType)reader.ReadByte();

            packetSessionData.TrackTemperature = reader.ReadSByte();
            packetSessionData.AirTemperature = reader.ReadSByte();

            packetSessionData.TotalLaps = reader.ReadByte();
            packetSessionData.TrackLength = reader.ReadUInt16();
            packetSessionData.SessionType = (SessionType)reader.ReadByte();

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
                var marshalZone = new MarshalZone();

                marshalZone.ZoneStart = reader.ReadSingle();
                marshalZone.ZoneFlag = (ZoneFlag)reader.ReadSByte();

                packetSessionData.MarshalZones[j] = marshalZone;
            }

            packetSessionData.SafetyCarStatus = reader.ReadByte();

            packetSessionData.NetworkGame = reader.ReadByte();
            packetSessionData.NumWeatherForecastSamples = reader.ReadByte();

            for (int j = 0; j < MaxNumberOfWeatherSamples; j++)
            {
                var newWeatherForecastSample = new WeatherForecastSample();

                newWeatherForecastSample.SessionType = (SessionType)reader.ReadByte();

                newWeatherForecastSample.TimeOffset = reader.ReadByte();
                newWeatherForecastSample.Weather = (WeatherType)reader.ReadByte();

                newWeatherForecastSample.TrackTemperature = reader.ReadSByte();
                newWeatherForecastSample.AirTemperature = reader.ReadSByte();

                packetSessionData.WeatherForecastSamples[j] = newWeatherForecastSample;
            }

            return packetSessionData;
        }

        private static IPacket Lap(PacketHeader packetHeader, BinaryReader reader)
        {
            var packetLapData = new PacketLapData(packetHeader);

            for (int i = 0; i < MaxNumberOfCarsOnTrack; i++)
            {
                var lapData = new LapData();

                lapData.LastLapTime = reader.ReadSingle();
                lapData.CurrentLapTime = reader.ReadSingle();

                lapData.Sector1TimeInMS = reader.ReadUInt16();
                lapData.Sector2TimeInMS = reader.ReadUInt16();

                lapData.BestLapTime = reader.ReadSingle();
                lapData.BestLapNum = reader.ReadByte();

                lapData.BestLapSector1TimeInMS = reader.ReadUInt16();
                lapData.BestLapSector2TimeInMS = reader.ReadUInt16();
                lapData.BestLapSector3TimeInMS = reader.ReadUInt16();

                lapData.BestOverallSector1TimeInMS = reader.ReadUInt16();
                lapData.BestOverallSector1LapNum = reader.ReadByte();

                lapData.BestOverallSector2TimeInMS = reader.ReadUInt16();
                lapData.BestOverallSector2LapNum = reader.ReadByte();

                lapData.BestOverallSector3TimeInMS = reader.ReadUInt16();
                lapData.BestOverallSector3LapNum = reader.ReadByte();

                lapData.LapDistance = reader.ReadSingle();
                lapData.TotalDistance = reader.ReadSingle();

                lapData.SafetyCarDelta = reader.ReadSingle();
                lapData.CarPosition = reader.ReadByte();

                lapData.CurrentLapNum = reader.ReadByte();
                lapData.PitStatus = (PitStatus)reader.ReadByte();
                lapData.Sector = reader.ReadByte();
                lapData.CurrentLapInvalid = reader.ReadByte();
                lapData.Penalties = reader.ReadByte();
                lapData.GridPosition = reader.ReadByte();
                lapData.DriverStatus = (DriverStatus)reader.ReadByte();
                lapData.ResultStatus = (ResultStatus)reader.ReadByte();

                packetLapData.LapData[i] = lapData;
            }

            return packetLapData;
        }

        private static IPacket CarTelemetry(PacketHeader packetHeader, BinaryReader reader)
        {
            var packetCarTelemetryData = new PacketCarTelemetryData(packetHeader);

            for (int i = 0; i < MaxNumberOfCarsOnTrack; i++)
            {
                var carTelemData = new CarTelemetryData();

                carTelemData.Speed = reader.ReadUInt16();
                carTelemData.Throttle = reader.ReadSingle();
                carTelemData.Steer = reader.ReadSingle();
                carTelemData.Brake = reader.ReadSingle();
                carTelemData.Clutch = reader.ReadByte();
                carTelemData.Gear = reader.ReadSByte();
                carTelemData.EngineRPM = reader.ReadUInt16();
                carTelemData.Drs = (DRS)reader.ReadByte();
                carTelemData.RevLightsPercent = reader.ReadByte();

                //packetCarTelemetryData.CarTelemetryData[i].BrakesTemperature = new UInt16[NumberOfTyres];
                //packetCarTelemetryData.CarTelemetryData[i].TyresSurfaceTemperature = new byte[NumberOfTyres];
                //packetCarTelemetryData.CarTelemetryData[i].TyresInnerTemperature = new byte[NumberOfTyres];
                //packetCarTelemetryData.CarTelemetryData[i].TyresPressure = new float[NumberOfTyres];
                //packetCarTelemetryData.CarTelemetryData[i].SurfaceType = new SurfaceTypes[NumberOfTyres];

                for (int j = 0; j < NumberOfTyres; j++)
                {
                    carTelemData.BrakesTemperature[j] = reader.ReadUInt16();
                }

                for (int j = 0; j < NumberOfTyres; j++)
                {
                    carTelemData.TyresSurfaceTemperature[j] = reader.ReadByte();
                }

                for (int j = 0; j < NumberOfTyres; j++)
                {
                    carTelemData.TyresInnerTemperature[j] = reader.ReadByte();
                }

                carTelemData.EngineTemperature = reader.ReadUInt16();

                for (int j = 0; j < NumberOfTyres; j++)
                {
                    carTelemData.TyresPressure[j] = reader.ReadSingle();
                }

                for (int j = 0; j < NumberOfTyres; j++)
                {
                    carTelemData.SurfaceType[j] = (SurfaceTypes)reader.ReadByte();
                }

                packetCarTelemetryData.CarTelemetryData[i] = carTelemData;
            }

            packetCarTelemetryData.ButtonStatus = reader.ReadUInt32();

            packetCarTelemetryData.MfdPanelIndex = reader.ReadByte();
            packetCarTelemetryData.MfdPanelIndexSecondaryPlayer = reader.ReadByte();

            packetCarTelemetryData.SuggestedGear = reader.ReadSByte();

            return packetCarTelemetryData;
        }

        private static IPacket CarStatus(PacketHeader packetHeader, BinaryReader reader)
        {
            var packetCarStatus = new PacketCarStatusData(packetHeader);

            for (int i = 0; i < MaxNumberOfCarsOnTrack; i++)
            {
                var carStatusData = new CarStatusData();
                packetCarStatus.CarStatusData[i] = carStatusData;

                carStatusData.TractionControl = reader.ReadByte();
                carStatusData.AntiLockBrakes = reader.ReadByte();
                carStatusData.FuelMix = reader.ReadByte();
                carStatusData.FrontBrakeBias = reader.ReadByte();
                carStatusData.pitLimiterStatus = reader.ReadByte();
                carStatusData.FuelInTank = reader.ReadSingle();
                carStatusData.FuelCapacity = reader.ReadSingle();
                carStatusData.FuelRemainingLaps = reader.ReadSingle();
                carStatusData.MaxRPM = reader.ReadUInt16();
                carStatusData.IdleRPM = reader.ReadUInt16();
                carStatusData.MaxGears = reader.ReadByte();
                carStatusData.DrsAllowed = reader.ReadByte();

                carStatusData.DrsActivationDistance = reader.ReadUInt16();

                carStatusData.TyresWear = new byte[NumberOfTyres];

                for (int j = 0; j < NumberOfTyres; j++)
                {
                    carStatusData.TyresWear[j] = reader.ReadByte();
                }

                carStatusData.ActualTyreCompound = reader.ReadByte();
                carStatusData.VisualTyreCompound = reader.ReadByte();

                carStatusData.TyresAgeLaps = reader.ReadByte();

                carStatusData.TyresDamage = new byte[NumberOfTyres];

                for (int j = 0; j < NumberOfTyres; j++)
                {
                    carStatusData.TyresDamage[j] = reader.ReadByte();
                }

                carStatusData.FrontLeftWingDamage = reader.ReadByte();
                carStatusData.FrontRightWingDamage = reader.ReadByte();
                carStatusData.RearWingDamage = reader.ReadByte();

                carStatusData.DrsFault = reader.ReadByte();

                carStatusData.EngineDamage = reader.ReadByte();
                carStatusData.GearBoxDamage = reader.ReadByte();
                carStatusData.VehicleFiaFlags = reader.ReadSByte();

                carStatusData.ErsStoreEnergy = reader.ReadSingle();
                carStatusData.ErsDeployMode = reader.ReadByte();

                carStatusData.ErsHarvestedThisLapMGUK = reader.ReadSingle();
                carStatusData.ErsHarvestedThisLapMGUH = reader.ReadSingle();
                carStatusData.ErsDeployedThisLap = reader.ReadSingle();
            }

            return packetCarStatus;
        }

        private static IPacket Participants(PacketHeader packetHeader, BinaryReader reader)
        {
            var packetParticipantsData = new PacketParticipantsData(packetHeader);

            packetParticipantsData.NumActiveCars = reader.ReadByte();

            for (int i = 0; i < MaxNumberOfCarsOnTrack; i++)
            {
                var participantData = new ParticipantData();

                packetParticipantsData.Participants[i] = participantData;

                participantData.AiControlled = reader.ReadBoolean();
                participantData.DriverId = reader.ReadByte();
                participantData.TeamId = reader.ReadByte();
                participantData.RaceNumber = reader.ReadByte();
                participantData.Nationality = reader.ReadByte();
                participantData.Name = reader.ReadChars(48);

                participantData.YourTelemetry = (TelemetrySetting)reader.ReadByte();

            }

            return packetParticipantsData;
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

                    switch (packetHeader.PacketTypes)
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

                        case PacketTypes.Participants:
                            decoder = Decode.Participants;
                            break;

                        //case PacketTypes.CarSetups:
                        //    break;

                        case PacketTypes.CarTelemetry:
                            decoder = Decode.CarTelemetry;
                            break;

                        case PacketTypes.CarStatus:
                            decoder = Decode.CarStatus;
                            break;

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