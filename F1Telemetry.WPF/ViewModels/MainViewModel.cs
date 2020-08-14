using F12020Telemetry;
using F12020Telemetry.Data;
using F12020Telemetry.Util.Extensions;
using F12020Telemetry.Util.Network;
using F1Telemetry.WPF.Command;
using F1Telemetry.WPF.Model;
using ScottPlot;
using System.ComponentModel;
using System.Drawing.Text;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace F1Telemetry.WPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public PlottableSignalXY[] BrakeGraph = new PlottableSignalXY[3];
        public double[] CurrentRenderPosition = new double[1] { 0 };
        public double[] CurrentRenderValue = new double[1] { 1000 };

        public PlottableSignalXY[] GearGraph = new PlottableSignalXY[3];
        public DispatcherTimer GraphRenderTimer = new DispatcherTimer();
        public PlottableSignalXY[] SpeedGraph = new PlottableSignalXY[3];
        public PlottableSignalXY[] ThrottleGraph = new PlottableSignalXY[3];
        private CancellationTokenSource ListeningCancellationTokenSource;

        public MainViewModel()
        {
            SetTopmostCommand = new RelayCommand<bool>(SetTopmost);

            for (int i = 0; i < LapData.Length; i++)
            {
                LapData[i] = new CurrentLapDataModel();
            }

            StartListeningCommand = new RelayCommand(async (s) => { await StartListeningAsync(s); });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int CurrentLapCursor { get; set; }
        public CurrentTelemetryDataModel CurrentTelemetry { get; set; } = new CurrentTelemetryDataModel();
        public int IndexCursor { get; internal set; }
        public bool IsListening { get; set; }
        public bool IsTopmost { get; internal set; }
        public CurrentLapDataModel[] LapData { get; } = new CurrentLapDataModel[3];
        public TelemetryManager Manager { get; } = new TelemetryManager();
        public SessionViewModel SessionInfo { get; set; } = new SessionViewModel();
        public RelayCommand<bool> SetTopmostCommand { get; }
        public RelayCommand StartListeningCommand { get; }
        public UDPListener UDPListener { get; private set; }

        public void ResetRenderCursor()
        {
            CurrentRenderPosition[0] = 0.0;
        }

        private void SetTopmost(bool topmost)
        {
            IsTopmost = topmost;
        }

        private async Task StartListeningAsync(object sender)
        {
            if (UDPListener == null)
            {
                UDPListener = new UDPListener(20777);
            }

            var senderButton = sender as Button;

            if (IsListening)
            {
                senderButton.Content = "Start Listening";
                StopListening();
            }
            else
            {
                senderButton.Content = "Stop Listening";
                GraphRenderTimer.Start();
                IsListening = true;
                IndexCursor = 0;

                UDPListener.BytesReceived += (s, e) =>
                {
                    var eventArgs = e as UDPPacketReceivedEventArgs;

                    Manager.Feed(eventArgs.Bytes);

                    SessionInfo.SessionType = Manager.Session != null ? Manager.Session.SessionType.GetDisplayName() : "";

                    var currentTelemetry = Manager.GetPlayerInfo()?.CurrentTelemetry;
                    var currentLapData = Manager.GetPlayerInfo()?.LapData.LastOrDefault();

                    if (currentTelemetry != null)
                    {
                        if (currentLapData.DriverStatus == DriverStatus.InGarage)
                        {
                            IndexCursor = 0;
                        }
                        else
                        {
                            CurrentRenderPosition[0] = currentLapData.LapDistance;

                            var currentLapDataModel = LapData[CurrentLapCursor];

                            var lapNumberLabel = $"Lap {currentLapData.CurrentLapNum}";

                            currentLapDataModel.Speed[IndexCursor] = currentTelemetry.Speed;
                            currentLapDataModel.Distance[IndexCursor] = currentLapData.LapDistance;
                            currentLapDataModel.Gear[IndexCursor] = currentTelemetry.Gear;

                            currentLapDataModel.Throttle[IndexCursor] = currentTelemetry.Throttle;
                            currentLapDataModel.Brake[IndexCursor] = currentTelemetry.Brake;

                            SpeedGraph[CurrentLapCursor].maxRenderIndex = IndexCursor;
                            SpeedGraph[CurrentLapCursor].label = lapNumberLabel;

                            GearGraph[CurrentLapCursor].maxRenderIndex = IndexCursor;
                            GearGraph[CurrentLapCursor].label = lapNumberLabel;

                            BrakeGraph[CurrentLapCursor].maxRenderIndex = IndexCursor;
                            BrakeGraph[CurrentLapCursor].label = lapNumberLabel;

                            ThrottleGraph[CurrentLapCursor].maxRenderIndex = IndexCursor;
                            ThrottleGraph[CurrentLapCursor].label = lapNumberLabel;

                            CurrentTelemetry.LapNumber = currentLapData.CurrentLapNum;
                            CurrentTelemetry.Brake = currentTelemetry.Brake;
                            CurrentTelemetry.Throttle = currentTelemetry.Throttle;
                            CurrentTelemetry.EngineRPM = currentTelemetry.EngineRPM;
                            CurrentTelemetry.Speed = currentTelemetry.Speed;
                            CurrentTelemetry.LapTime = currentLapData.CurrentLapTime;

                            IndexCursor++;
                        }
                    }
                };

                try
                {
                    ListeningCancellationTokenSource = new CancellationTokenSource();

                    await Task.Run(() =>
                    {
                        while (IsListening)
                        {
                            UDPListener.Listen();
                        }
                    }, ListeningCancellationTokenSource.Token);
                }
                catch (SocketException sE)
                {
                }
                finally
                {
                    UDPListener.Close();
                    UDPListener = null;
                }
            }

            void StopListening()
            {
                UDPListener.Close();
                ListeningCancellationTokenSource.Cancel();
                GraphRenderTimer.Stop();
                IsListening = false;
            }
        }
    }
}