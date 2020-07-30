using F12020Telemetry.Data;
using F12020Telemetry.Util.Extensions;
using ScottPlot;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace F12020Telemetry
{
    internal static class Program
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

                telemetryManager.NewSession += (s, e) =>
                {
                    var manager = s as TelemetryManager;

                    if (manager != null)
                    {
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine($"Session ID      : {manager.Session.Header.SessionUID}                       ");
                        Console.WriteLine($"Session Type    : {manager.Session.SessionType.GetDisplayName()}                  ");
                        Console.WriteLine($"Formula Series  : {manager.Session.Formula}                  ");
                        Console.WriteLine($"Track Name      : {TrackInfo.TrackNames[telemetryManager.Session.TrackId]}                             ");
                        Console.WriteLine($"Player car index: {manager.Session.Header.PlayerCarIndex}                              ");

                        manager.GetPlayerInfo().LapInterval += OnPlayerLapInterval;
                    }
                };

                while (true)
                {
                    byte[] bytes = listener.Receive(ref groupEP);

                    telemetryManager.Feed(bytes);

                    Console.SetCursorPosition(0, 6);

                    var playerData = telemetryManager.GetPlayerInfo();

                    if (playerData != null && playerData.CurrentTelemetry != null)
                    {
                        Console.WriteLine($"Player Speed: {playerData.CurrentTelemetry.Speed}km/h             ");
                        Console.WriteLine($"Player Gear : {playerData.CurrentTelemetry.Gear}             ");
                        Console.WriteLine($"Steer       : {playerData.CurrentTelemetry.Steer}                                 ");
                        Console.WriteLine($"DRS         : {playerData.CurrentTelemetry.Drs.GetDisplayName()}            ");
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

        private static void OnPlayerLapInterval(object sender, EventArgs e)
        {
            var player = sender as Driver;

            if (player != null)
            {
                var plt = new Plot(1200, 900);

                for (int i = 2; i >= 0; i--)
                {
                    int lapNumber = player.CurrentLapNumber - i;

                    var lapData = player.LapData.GetLap(lapNumber);
                    var carTelemetryData = player.CarTelemetryData.GetForLap(lapData);

                    var time = lapData?.Select(l => l.CurrentLapTime);
                    var speed = carTelemetryData?.Select(t => t.Speed);

                    if (time != null && speed != null)
                    {
                        plt.PlotScatter(Array.ConvertAll(time.ToArray(), x => (double)x), Array.ConvertAll(speed.ToArray(), x => (double)x), label: $"Lap {lapNumber}", lineWidth: 3, markerSize: 0);
                    }
                }

                plt.Legend();
                plt.Title($"{TrackInfo.TrackNames[player.Manager.Session.TrackId]}");
                plt.YLabel("Speed");
                plt.XLabel("Time");

                plt.SaveFig($"Lap{player.CurrentLapNumber}-{TrackInfo.TrackNames[player.Manager.Session.TrackId]}.png");
            }
        }
    }
}