using F12020Telemetry.Data;
using F12020Telemetry.Packet;
using F12020Telemetry.Util.Extensions;
using System;
using System.Net;
using System.Net.Sockets;
using System.Resources;

namespace F12020Telemetry
{
    internal class Program
    {
        private static int listenPort = 20777;

        private static void Main(string[] args)
        {
            StartListener();
        }

        private static string[] surfaceTypes =
        {
            "Tarmac",
            "Rumble Strip",
            "Concrete",
            "Rock",
            "Gravel",
            "Mud",
            "Sand",
            "Grass",
            "Water",
            "Cobblestone",
            "Metal",
            "Ridged",
        };

        private static void StartListener()
        {
            UdpClient listener = new UdpClient(listenPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

            try
            {
                var packetHeader = new PacketHeader();
                PacketLapData LapData = new PacketLapData(packetHeader);
                PacketMotionData packetCarMotionData = new PacketMotionData(packetHeader);
                PacketSessionData packetSessionData = new PacketSessionData();
                PacketCarTelemetryData packetCarTelemetryData = new PacketCarTelemetryData(packetHeader);

                while (true)
                {
                    byte[] bytes = listener.Receive(ref groupEP);

                    IPacket packet = Decode.Packet(bytes);

                    switch (packet)
                    {
                        case PacketSessionData pSessionData:
                            packetSessionData = pSessionData;
                            break;

                        case PacketCarTelemetryData pCarTelemetryData:
                            packetCarTelemetryData = pCarTelemetryData;
                            break;

                        case PacketLapData pLapData:
                            LapData = pLapData;
                            break;
                        case PacketMotionData pMotionData:
                            packetCarMotionData = pMotionData;
                            break;
                        default:
                            break;
                    }

                    var playerIndex = LapData.Header.PlayerCarIndex;

                    var lapDistance = LapData.LapData[playerIndex].LapDistance / 1000.0f;
                    var totalDistance = LapData.LapData[playerIndex].TotalDistance / 1000.0f;
                    var sector = LapData.LapData[playerIndex].Sector + 1;

                    var lat = 1.0f + packetCarMotionData.CarMotionData[playerIndex].GForceLateral;
                    var glong = 1.0f + packetCarMotionData.CarMotionData[playerIndex].GForceLongitudinal;
                    var vert = 1.0f + packetCarMotionData.CarMotionData[playerIndex].GForceVertical;

                    var trackLength = packetSessionData.TrackLength / 1000.0f;

                    var speed = packetCarTelemetryData.CarTelemetryData[playerIndex].Speed;
                    var throttle = packetCarTelemetryData.CarTelemetryData[playerIndex].Throttle;
                    var brake = packetCarTelemetryData.CarTelemetryData[playerIndex].Brake;
                    var gear = packetCarTelemetryData.CarTelemetryData[playerIndex].Gear;

                    Console.SetCursorPosition(0, 0);

                    Console.WriteLine($"Lat: {lat:F3} Long: {glong:F3} Vert: {vert:F3}                                        ");
                    Console.WriteLine($"Current: {lapDistance:F3}/{trackLength:F3}km; Total Distance: {totalDistance:F3}km; Current Lap: {LapData.LapData[0].CurrentLapNum} Sector: {sector}                                    ");
                    Console.WriteLine($"Speed: {speed}km/h; Throttle: {throttle:P2}; Brake: {brake:P2}; Gear: {gear}                                                       ");

                    var telemetry = packetCarTelemetryData.CarTelemetryData[playerIndex];

                    if (telemetry.SurfaceType != null)
                    {
                        Console.WriteLine("Wheel stats (Surface Type):");
                        Console.WriteLine($"RL: {telemetry.SurfaceType[0].GetDisplayName()} | Spd: {packetCarMotionData.WheelSpeed[0]:F2} | Slp: {packetCarMotionData.WheelSlip[0]:F4}             ");
                        Console.WriteLine($"RR: {telemetry.SurfaceType[1].GetDisplayName()} | Spd: {packetCarMotionData.WheelSpeed[1]:F2} | Slp: {packetCarMotionData.WheelSlip[1]:F4}             ");
                        Console.WriteLine($"FL: {telemetry.SurfaceType[2].GetDisplayName()} | Spd: {packetCarMotionData.WheelSpeed[2]:F2} | Slp: {packetCarMotionData.WheelSlip[2]:F4}             ");
                        Console.WriteLine($"FR: {telemetry.SurfaceType[3].GetDisplayName()} | Spd: {packetCarMotionData.WheelSpeed[3]:F2} | Slp: {packetCarMotionData.WheelSlip[3]:F4}             ");
                    }

                    Console.WriteLine(telemetry.EngineRPM + "          ");

                }
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                listener.Close();
            }
        }
    }
}