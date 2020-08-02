using F12020Telemetry;
using F12020Telemetry.Data;
using F12020Telemetry.Util.Extensions;
using F1Telemetry.WPF.ViewModels;
using ScottPlot;
using System;
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
        private double[] currentRenderPosition = new double[1] { 0 };
        private double[] currentRenderValue = new double[1] { 1000 };
        private DispatcherTimer GraphRenderTimer = new DispatcherTimer();
        private bool IsListening;
        private CancellationTokenSource ListeningCancellationTokenSource;

        private PlottableSignalXY[] SpeedGraph = new PlottableSignalXY[3];
        private PlottableSignalXY[] ThrottleGraph = new PlottableSignalXY[3];
        private PlottableSignalXY[] BrakeGraph = new PlottableSignalXY[3];
        private PlottableSignalXY[] GearGraph = new PlottableSignalXY[3];

        /// <summary>
        /// The current lap cursor for the array.
        /// </summary>
        private int CurrentLapCursor;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = MainViewModel;

            for (int i = 0; i < MainViewModel.LapData.Length; i++)
            {
                SpeedGraph[i] = SpeedGraphPlot.plt.PlotSignalXY(MainViewModel.LapData[i].Distance, MainViewModel.LapData[i].Speed);
                SpeedGraphPlot.plt.PlotBar(currentRenderPosition, currentRenderValue);
                SpeedGraphPlot.plt.YLabel("Speed");
                SpeedGraphPlot.plt.Legend();

                GearGraph[i] = GearGraphPlot.plt.PlotSignalXY(MainViewModel.LapData[i].Distance, MainViewModel.LapData[i].Gear);
                GearGraphPlot.plt.PlotBar(currentRenderPosition, currentRenderValue);
                GearGraphPlot.plt.YLabel("Gear");
                GearGraphPlot.plt.Legend();

                BrakeGraph[i] = BrakeGraphPlot.plt.PlotSignalXY(MainViewModel.LapData[i].Distance, MainViewModel.LapData[i].Brake);
                ThrottleGraph[i] = ThrottleGraphPlot.plt.PlotSignalXY(MainViewModel.LapData[i].Distance, MainViewModel.LapData[i].Throttle);

                BrakeGraphPlot.plt.PlotBar(currentRenderPosition, currentRenderValue);
                ThrottleGraphPlot.plt.PlotBar(currentRenderPosition, currentRenderValue);

                SpeedGraphPlot.plt.Axis(0, 6000, 0, 360);
                GearGraphPlot.plt.Axis(0, 6000, 0, 9);

                ThrottleGraphPlot.plt.Axis(0, 6000, 0, 1.05);
                ThrottleGraphPlot.plt.YLabel("Throttle");
                ThrottleGraphPlot.plt.Legend();

                BrakeGraphPlot.plt.Axis(0, 6000, 0, 1.05);
                BrakeGraphPlot.plt.YLabel("Brake");
                BrakeGraphPlot.plt.Legend();
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
                            CurrentLapCursor = (CurrentLapCursor + 1) % MainViewModel.LapData.Length;
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
                        var currentLapData = telemetryManager.GetPlayerInfo()?.LapData.LastOrDefault();

                        if (currentTelemetry != null)
                        {
                            if (currentLapData.DriverStatus == DriverStatus.InGarage)
                            {
                                cursor = 0;
                            }
                            else
                            {
                                currentRenderPosition[0] = currentLapData.LapDistance;

                                var currentLapDataModel = MainViewModel.LapData[CurrentLapCursor];

                                var lapNumberLabel = $"Lap {currentLapData.CurrentLapNum}";

                                currentLapDataModel.Speed[cursor] = currentTelemetry.Speed;
                                currentLapDataModel.Distance[cursor] = currentLapData.LapDistance;
                                currentLapDataModel.Gear[cursor] = currentTelemetry.Gear;

                                currentLapDataModel.Throttle[cursor] = currentTelemetry.Throttle;
                                currentLapDataModel.Brake[cursor] = currentTelemetry.Brake;

                                SpeedGraph[CurrentLapCursor].maxRenderIndex = cursor;
                                SpeedGraph[CurrentLapCursor].label = lapNumberLabel;

                                GearGraph[CurrentLapCursor].maxRenderIndex = cursor;
                                GearGraph[CurrentLapCursor].label = lapNumberLabel;

                                BrakeGraph[CurrentLapCursor].maxRenderIndex = cursor;
                                BrakeGraph[CurrentLapCursor].label = lapNumberLabel;

                                ThrottleGraph[CurrentLapCursor].maxRenderIndex = cursor;
                                ThrottleGraph[CurrentLapCursor].label = lapNumberLabel;

                                MainViewModel.CurrentTelemetry.Brake = currentTelemetry.Brake;
                                MainViewModel.CurrentTelemetry.Throttle = currentTelemetry.Throttle;
                                MainViewModel.CurrentTelemetry.EngineRPM = currentTelemetry.EngineRPM;
                                MainViewModel.CurrentTelemetry.Speed = currentTelemetry.Speed;
                                MainViewModel.CurrentTelemetry.LapTime = currentLapData.CurrentLapTime;

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