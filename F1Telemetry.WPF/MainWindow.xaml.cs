using F12020Telemetry;
using F12020Telemetry.Data;
using F12020Telemetry.Util.Extensions;
using F1Telemetry.WPF.Model;
using F1Telemetry.WPF.ViewModels;
using ScottPlot;
using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace F1Telemetry.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PlottableSignalXY BrakeGraph;
        private double[] currentRenderPosition = new double[1] { 0 };
        private double[] currentRenderValue = new double[1] { 1000 };
        private PlottableSignalXY GearGraph;
        private DispatcherTimer GraphRenderTimer = new DispatcherTimer();
        private bool IsListening;
        private CancellationTokenSource ListeningCancellationTokenSource;
        private PlottableSignalXY SpeedGraph;
        private PlottableSignalXY ThrottleGraph;
        private int currentLapCursor;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = MainViewModel;

            foreach (var lap in MainViewModel.LapData)
            { 
                SpeedGraph = SpeedGraphPlot.plt.PlotSignalXY(lap.Distance, lap.Speed);
                SpeedGraphPlot.plt.PlotBar(currentRenderPosition, currentRenderValue);
                SpeedGraphPlot.plt.YLabel("Speed");

                GearGraph = GearGraphPlot.plt.PlotSignalXY(lap.Distance, lap.Gear);
                GearGraphPlot.plt.PlotBar(currentRenderPosition, currentRenderValue);
                GearGraphPlot.plt.YLabel("Gear");

                BrakeGraph = BrakeGraphPlot.plt.PlotSignalXY(lap.Distance, lap.Brake, color: Color.Red);
                ThrottleGraph = ThrottleGraphPlot.plt.PlotSignalXY(lap.Distance, lap.Throttle, color: Color.Green);

                BrakeGraphPlot.plt.PlotBar(currentRenderPosition, currentRenderValue);
                ThrottleGraphPlot.plt.PlotBar(currentRenderPosition, currentRenderValue);

                SpeedGraphPlot.plt.Axis(0, 6000, 0, 360);
                GearGraphPlot.plt.Axis(0, 6000, 0, 9);

                ThrottleGraphPlot.plt.Axis(0, 6000, 0, 1.05);
                ThrottleGraphPlot.plt.YLabel("Throttle");
                BrakeGraphPlot.plt.Axis(0, 6000, 0, 1.05);
                BrakeGraphPlot.plt.YLabel("Brake");
            }

            GraphRenderTimer.Interval = TimeSpan.FromMilliseconds(20);
            GraphRenderTimer.Tick += (s, e) =>
            {
                SpeedGraphPlot.Render(recalculateLayout: true);

                GearGraphPlot.Render(recalculateLayout: true);

                BrakeGraphPlot.Render(recalculateLayout: true);
                ThrottleGraphPlot.Render(recalculateLayout: true);
            };
        }

        private MainViewModel MainViewModel { get; set; } = new MainViewModel();

        private async Task StartListenerAsync()
        {
            Application.Current.Properties["Manager"] = new TelemetryManager();
            var telemetryManager = (TelemetryManager)Application.Current.Properties["Manager"];

            UdpClient listener = new UdpClient(20777);
            IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, 20777);

            try
            {
                int cursor = 0;

                telemetryManager.NewSession += (s, e) =>
                {
                    var manager = s as TelemetryManager;

                    if (manager != null)
                    {
                        SpeedGraphPlot.plt.Axis(0, manager.Session.TrackLength, 0, 360);
                        GearGraphPlot.plt.Axis(0, manager.Session.TrackLength, 0, 9);

                        ThrottleGraphPlot.plt.Axis(0, manager.Session.TrackLength, 0, 1.05);
                        BrakeGraphPlot.plt.Axis(0, manager.Session.TrackLength, 0, 1.05);

                        manager.GetPlayerInfo().NewLap += (s, e) =>
                        {
                            cursor = 0;
                            currentLapCursor = (currentLapCursor + 1) % MainViewModel.LapData.Length;
                        };
                    }
                };

                while (true)
                {
                    byte[] bytes = listener.Receive(ref groupEP);
                    telemetryManager.Feed(bytes);

                    await Dispatcher.InvokeAsync(() =>
                    {
                        MainViewModel.SessionInfo.SessionType = telemetryManager.Session != null ? telemetryManager.Session.SessionType.GetDisplayName() : "";

                        var currentTelemetry = telemetryManager.GetPlayerInfo()?.CurrentTelemetry;
                        var currentLapTime = telemetryManager.GetPlayerInfo()?.LapData.LastOrDefault();

                        if (currentTelemetry != null)
                        {
                            if (currentLapTime.DriverStatus == DriverStatus.InGarage)
                            {
                                cursor = 0;
                            }
                            else
                            {
                                currentRenderPosition[0] = currentLapTime.LapDistance;

                                var currentLap = MainViewModel.LapData[currentLapCursor];

                                currentLap.Speed[cursor] = currentTelemetry.Speed;
                                currentLap.Distance[cursor] = currentLapTime.LapDistance;
                                currentLap.Gear[cursor] = currentTelemetry.Gear;

                                currentLap.Throttle[cursor] = currentTelemetry.Throttle;
                                currentLap.Brake[cursor] = currentTelemetry.Brake;

                                SpeedGraph.maxRenderIndex = cursor;
                                GearGraph.maxRenderIndex = cursor;

                                BrakeGraph.maxRenderIndex = cursor;
                                ThrottleGraph.maxRenderIndex = cursor;

                                MainViewModel.CurrentTelemetry.Brake = currentTelemetry.Brake;
                                MainViewModel.CurrentTelemetry.Throttle = currentTelemetry.Throttle;
                                MainViewModel.CurrentTelemetry.EngineRPM = currentTelemetry.EngineRPM;
                                MainViewModel.CurrentTelemetry.Speed = currentTelemetry.Speed;
                                MainViewModel.CurrentTelemetry.LapTime = currentLapTime.CurrentLapTime;

                                cursor++;
                            }
                        }
                    });
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

        private void StartListening(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            if (!IsListening)
            {
                ListeningCancellationTokenSource = new CancellationTokenSource();
                var cancelToken = ListeningCancellationTokenSource.Token;

                Task.Run(async () => await StartListenerAsync(), cancelToken);

                GraphRenderTimer.Start();

                IsListening = !IsListening;

                ListenButton.Content = "Listening";
            }
            else
            {
                ListeningCancellationTokenSource.Cancel();
                IsListening = !IsListening;

                ListenButton.Content = "Start Listening";
            }
        }
    }
}