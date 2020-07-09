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
            packetSessionData.GamePaused = reader.ReadByte();
            packetSessionData.SpectatorCarIndex = reader.ReadByte();
            packetSessionData.SliProNativeSupport = reader.ReadByte();
            packetSessionData.NumMarshalZones = reader.ReadByte();

            for (int j = 0; j < 21; j++)
            {
                packetSessionData.MarshalZones[j].ZoneStart = reader.ReadSingle();
                packetSessionData.MarshalZones[j].ZoneFlag = reader.ReadSByte();
            }

            packetSessionData.SafetyCarStatus = reader.ReadByte();

            packetSessionData.NetworkGame = reader.ReadByte();
            packetSessionData.NumWeatherForecastSamples = reader.ReadByte();

            for (int j = 0; j < NumberOfWeatherSamples; j++)
            {
                packetSessionData.WeatherForecastSamples[j].SessionType = reader.ReadByte();

                packetSessionData.WeatherForecastSamples[j].TimeOffset = reader.ReadByte();
                packetSessionData.WeatherForecastSamples[j].Weather = reader.ReadByte();

                packetSessionData.WeatherForecastSamples[j].TrackTemperature = reader.ReadSByte();
                packetSessionData.WeatherForecastSamples[j].AirTemperature = reader.ReadSByte();
            }

            return packetSessionData;
        }
    }
}