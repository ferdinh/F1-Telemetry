using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using F12020Telemetry;
using F12020Telemetry.Data;
using F12020Telemetry.Util.Extensions;
using F12020Telemetry.Util.Network;
using F1Telemetry.WPF.Command;
using F1Telemetry.WPF.Model;
using ScottPlot;

namespace F1Telemetry.WPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged, IDisposable
    {
        private int CurrentTelemetryIndexCursor;
        private CancellationTokenSource ListeningCancellationTokenSource;

        public MainViewModel()
        {
            SetTopmostCommand = new RelayCommand<bool>(SetTopmost);

            for (int i = 0; i < LapData.Length; i++)
            {
                LapData[i] = new CurrentLapDataModel();
            }

            StartListeningCommand = new RelayCommand(async (s) => { await StartListeningAsync(s).ConfigureAwait(false); });

            GraphRenderTimer.Interval = TimeSpan.FromMilliseconds(20);

            Manager.NewSession += Manager_NewSession;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public PlottableSignalXY[] BrakeGraph { get; } = new PlottableSignalXY[3];
        public int CurrentLapCursor { get; set; }
        public double[] CurrentRenderPosition { get; } = new double[] { 0 };
        public double[] CurrentRenderValue { get; } = new double[] { 1000 };

        public CurrentTelemetryDataModel CurrentTelemetry { get; set; } = new CurrentTelemetryDataModel();
        public PlottableSignalXY[] GearGraph { get; } = new PlottableSignalXY[3];
        public DispatcherTimer GraphRenderTimer { get; } = new DispatcherTimer();
        public bool IsListening { get; set; }
        public bool IsTopmost { get; internal set; }
        public CurrentLapDataModel[] LapData { get; } = new CurrentLapDataModel[3];
        public TelemetryManager Manager { get; } = new TelemetryManager();
        public SessionViewModel SessionInfo { get; set; } = new SessionViewModel();
        public RelayCommand<bool> SetTopmostCommand { get; }
        public PlottableSignalXY[] SpeedGraph { get; } = new PlottableSignalXY[3];
        public RelayCommand StartListeningCommand { get; }
        public PlottableSignalXY[] ThrottleGraph { get; } = new PlottableSignalXY[3];
        public UDPListener UDPListener { get; private set; }

        private void ResetCurrentTelemetryIndexCursor()
        {
            CurrentTelemetryIndexCursor = 0;
        }

        private void ResetRenderCursor()
        {
            CurrentRenderPosition[0] = 0.0;
        }

        private void Manager_NewSession(object sender, EventArgs e)
        {
            var manager = sender as TelemetryManager;

            if (manager != null)
            {
                foreach (var lapModel in LapData)
                {
                    lapModel.Clear();
                }

                ResetRenderCursor();

                manager.GetPlayerInfo().NewLap += (s, e) =>
                {
                    ResetRenderCursor();
                    CurrentLapCursor = (CurrentLapCursor + 1) % LapData.Length;
                    ResetCurrentTelemetryIndexCursor();
                };
            }
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
                UDPListener.BytesReceived += UDPListener_BytesReceived;
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
                await StartListening().ConfigureAwait(false);
            }

            async Task StartListening()
            {
                GraphRenderTimer.Start();
                IsListening = true;

                ResetCurrentTelemetryIndexCursor();

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

        private void UDPListener_BytesReceived(object sender, EventArgs e)
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
                    ResetCurrentTelemetryIndexCursor();
                }
                else
                {
                    CurrentRenderPosition[0] = currentLapData.LapDistance;

                    var currentLapDataModel = LapData[CurrentLapCursor];

                    var lapNumberLabel = $"Lap {currentLapData.CurrentLapNum}";

                    currentLapDataModel.Speed[CurrentTelemetryIndexCursor] = currentTelemetry.Speed;
                    currentLapDataModel.Distance[CurrentTelemetryIndexCursor] = currentLapData.LapDistance;
                    currentLapDataModel.Gear[CurrentTelemetryIndexCursor] = currentTelemetry.Gear;

                    currentLapDataModel.Throttle[CurrentTelemetryIndexCursor] = currentTelemetry.Throttle;
                    currentLapDataModel.Brake[CurrentTelemetryIndexCursor] = currentTelemetry.Brake;

                    SpeedGraph[CurrentLapCursor].maxRenderIndex = CurrentTelemetryIndexCursor;
                    SpeedGraph[CurrentLapCursor].label = lapNumberLabel;

                    GearGraph[CurrentLapCursor].maxRenderIndex = CurrentTelemetryIndexCursor;
                    GearGraph[CurrentLapCursor].label = lapNumberLabel;

                    BrakeGraph[CurrentLapCursor].maxRenderIndex = CurrentTelemetryIndexCursor;
                    BrakeGraph[CurrentLapCursor].label = lapNumberLabel;

                    ThrottleGraph[CurrentLapCursor].maxRenderIndex = CurrentTelemetryIndexCursor;
                    ThrottleGraph[CurrentLapCursor].label = lapNumberLabel;

                    CurrentTelemetry.LapNumber = currentLapData.CurrentLapNum;
                    CurrentTelemetry.Brake = currentTelemetry.Brake;
                    CurrentTelemetry.Throttle = currentTelemetry.Throttle;
                    CurrentTelemetry.EngineRPM = currentTelemetry.EngineRPM;
                    CurrentTelemetry.Speed = currentTelemetry.Speed;
                    CurrentTelemetry.LapTime = currentLapData.CurrentLapTime;

                    CurrentTelemetryIndexCursor++;
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            ListeningCancellationTokenSource.Dispose();
        }
    }
}
