using F12020Telemetry.Packet;
using F12020Telemetry.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace F12020Telemetry
{
    public class Lap
    {
        public List<PacketLapData> LapData = new List<PacketLapData>();
        public List<PacketCarTelemetryData> CarTelemetryData = new List<PacketCarTelemetryData>();
    }

    internal class Program
    {
        private static int listenPort = 20777;

        private static void Main(string[] args)
        {
            StartListener();
        }

        private static void StartListener()
        {
            UdpClient listener = new UdpClient(listenPort);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

            try
            {
                var telemetryManager = new TelemetryManager();

                while (true)
                {
                    byte[] bytes = listener.Receive(ref groupEP);

                    telemetryManager.Feed(bytes);

                    Console.SetCursorPosition(0, 0);

                    Console.WriteLine($"Session Type: {telemetryManager.Session.SessionType.GetDisplayName()}                  ");
                    Console.WriteLine($"Formula Series: {telemetryManager.Session.Formula}                  ");
                    Console.WriteLine($"Track Name: {TrackInfo.TrackNames[telemetryManager.Session.TrackId]}                             ");

                    var playerData = telemetryManager.GetPlayerInfo();

                    if (playerData != null && playerData.PlayerCurrentTelemetry != null)
                    {
                        Console.WriteLine($"Player Speed: {playerData.PlayerCurrentTelemetry.Speed}             ");
                        Console.WriteLine($"Player Gear : {playerData.PlayerCurrentTelemetry.Gear}             ");
                    }
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