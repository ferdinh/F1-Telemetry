using F1Telemetry.Core;
using F1Telemetry.Core.Data;
using F1Telemetry.Core.Util.Extensions;
using F1Telemetry.Core.Util.Network;
using F1Telemetry.WPF.Command;
using F1Telemetry.WPF.Model;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace F1Telemetry.WPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged, IDisposable
    {
        private int CurrentTelemetryIndexCursor;
        private CancellationTokenSource ListeningCancellationTokenSource;

        public MainViewModel()
        {
            SetTopmostCommand = new RelayCommand<bool>(SetTopmost);
            EnableLiveTelemetryCommand = new RelayCommand<bool>(EnableLiveTelemetry);
            ToggleToGraphCommand = new RelayCommand<(bool, int)>(ToggleToGraph);
            ClearAllGraphCommand = new RelayCommand(ClearAllGraph, CanClearAllGraph);
            ClearLiveTelemetryGraphCommand = new RelayCommand(ClearLiveTelemetryGraph, _ => LapData != null);

            StartListeningCommand = new RelayCommand(async (s) => { await StartListeningAsync(s).ConfigureAwait(false); });

            GraphRenderTimer.Interval = TimeSpan.FromMilliseconds(33);

            Manager.NewSession += Manager_NewSession;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This is quite an unfortunate side effect of ScottPlot library and cannot control the chart rendering via
        // MVVM. For now hold the WpfPlot reference to the model to handle the adding and removal of plot.
        public WpfPlot SpeedGraphPlot { get; set; }

        public WpfPlot BrakeGraphPlot { get; set; }
        public WpfPlot ThrottleGraphPlot { get; set; }
        public WpfPlot GearGraphPlot { get; set; }

        /// <summary>
        /// Holds the plotted graph reference.
        /// </summary>
        /// <value>
        /// The plotted lap data.
        /// </value>
        private Dictionary<int, Plottable[]> PlottedLapData { get; } = new Dictionary<int, Plottable[]>();

        private readonly double DefaultLineWidth = 1.75;

        public PlottableSignalXY[] BrakeGraph { get; } = new PlottableSignalXY[3];
        public int CurrentLapCursor { get; set; }
        public double[] CurrentRenderPosition { get; } = new double[] { 0 };
        public double[] CurrentRenderValue { get; } = new double[] { 1000 };

        public bool IsLiveTelemetryEnabled { get; set; }

        public CurrentTelemetryDataModel CurrentTelemetry { get; set; } = new CurrentTelemetryDataModel();
        public PlottableSignalXY[] GearGraph { get; } = new PlottableSignalXY[3];
        public DispatcherTimer GraphRenderTimer { get; } = new DispatcherTimer();
        public bool IsListening { get; set; }
        public bool IsTopmost { get; internal set; }
        public CurrentLapDataModel[] LapData { get; set; }
        public TelemetryManager Manager { get; } = new TelemetryManager();
        public SessionViewModel SessionInfo { get; set; } = new SessionViewModel();
        public RelayCommand<bool> EnableLiveTelemetryCommand { get; }
        public RelayCommand<bool> SetTopmostCommand { get; }
        public RelayCommand<(bool, int)> ToggleToGraphCommand { get; }
        public RelayCommand ClearLiveTelemetryGraphCommand { get; }
        public RelayCommand ClearAllGraphCommand { get; }
        public PlottableSignalXY[] SpeedGraph { get; } = new PlottableSignalXY[3];
        public RelayCommand StartListeningCommand { get; }
        public PlottableSignalXY[] ThrottleGraph { get; } = new PlottableSignalXY[3];
        public UDPListener UDPListener { get; private set; }

        public ObservableCollection<LapSummaryModel> LapSummaries { get; } = new ObservableCollection<LapSummaryModel>();

        private void ResetCurrentTelemetryIndexCursor()
        {
            CurrentTelemetryIndexCursor = 0;
        }

        private void ResetRenderCursor()
        {
            CurrentRenderPosition[0] = 0.0;
        }

        private void ResetCurrentLapCursor()
        {
            CurrentLapCursor = 0;
        }

        private void Manager_NewSession(object sender, EventArgs e)
        {
            var manager = sender as TelemetryManager;

            if (manager != null)
            {
                if (LapData != null)
                {
                    foreach (var lapModel in LapData)
                    {
                        lapModel.Clear();
                    }
                }

                SessionInfo.SessionType = Manager.Session != null ? Manager.Session.SessionType.GetDisplayName() : "";
                SessionInfo.TrackLength = (ushort)(Manager.Session != null ? Manager.Session.TrackLength : 0);

                UpdateGraphXAxisToTrackLength();
                LimitGraphAxisBound();

                ResetRenderCursor();

                manager.GetPlayerInfo().NewLap += (s, e) =>
                {
                    if (IsLiveTelemetryEnabled)
                    {
                        ResetRenderCursor();
                        CurrentLapCursor = (CurrentLapCursor + 1) % LapData.Length;
                        LapData[CurrentLapCursor].Clear();
                        ResetCurrentTelemetryIndexCursor();
                    }

                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        var lapSummary = e.LapSummary;

                        var deltaToFastestTime = 0.0f;

                        foreach (var lapSum in LapSummaries)
                        {
                            var updatedDeltaTime = lapSum.LapTime - lapSummary.BestLapTime;
                            lapSum.DeltaToBestTime = updatedDeltaTime;
                        }

                        deltaToFastestTime = lapSummary.LapTime - lapSummary.BestLapTime;

                        LapSummaries.Add(new LapSummaryModel
                        {
                            LapNumber = lapSummary.LapNumber,
                            LapTime = lapSummary.LapTime,
                            DeltaToBestTime = deltaToFastestTime,
                            TyreCompoundUsed = lapSummary.TyreCompoundUsed,
                            ERSDeployed = lapSummary.ERSDeployed,
                            ERSDeployedPercentage = lapSummary.ERSDeployedPercentage,
                            FuelUsed = lapSummary.FuelUsed
                        });
                    });
                };

                manager.GetPlayerInfo().Pitting += (s, e) =>
                {
                    manager.GetPlayerInfo().RemoveLap(CurrentTelemetry.LapNumber);
                };
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                LapSummaries.Clear();
            });

            ClearPlottedLapData();
        }

        private void LimitGraphAxisBound()
        {
            SpeedGraphPlot.plt.AxisBounds(minX: 0, maxX: Manager.Session.TrackLength, minY: 0, maxY: 360);
            ThrottleGraphPlot.plt.AxisBounds(minX: 0, maxX: Manager.Session.TrackLength, minY: 0, maxY: 1.02);
            BrakeGraphPlot.plt.AxisBounds(minX: 0, maxX: Manager.Session.TrackLength, minY: 0, maxY: 1.02);
            GearGraphPlot.plt.AxisBounds(minX: 0, maxX: Manager.Session.TrackLength, minY: 0, maxY: 9);
        }

        private void SetTopmost(bool topmost)
        {
            IsTopmost = topmost;
        }

        private void EnableLiveTelemetry(bool isLiveTelemetryEnabled)
        {
            if (isLiveTelemetryEnabled)
            {
                UnbindLiveTelemetryGraph();
                BindLiveTelemetryGraph();
            }

            IsLiveTelemetryEnabled = isLiveTelemetryEnabled;
        }

        private void ToggleToGraph((bool shouldPlot, int lapNumber) toggleLapInfo)
        {
            if (toggleLapInfo.shouldPlot && !PlottedLapData.ContainsKey(toggleLapInfo.lapNumber))
            {
                var player = Manager.GetPlayerInfo();

                var lapNumberLabel = $"Lap {toggleLapInfo.lapNumber}";

                if (player.LapSummaries.TryGetValue(toggleLapInfo.lapNumber, out var lapSummary))
                {
                    var carData = lapSummary.CarTelemetryData;
                    var distance = lapSummary.LapData.Select(l => (double)l.LapDistance).ToArray();

                    if (carData != null)
                    {
                        var speed = carData.Select(c => (double)c.Speed).ToArray();
                        var throttle = carData.Select(c => (double)c.Throttle).ToArray();
                        var brake = carData.Select(c => (double)c.Brake).ToArray();
                        var gear = carData.Select(c => (double)c.Gear).ToArray();

                        var graphPlots = new Plottable[4];

                        var speedGraphPlot = SpeedGraphPlot.plt.PlotScatter(distance, speed, lineWidth: 1.75, markerShape: MarkerShape.none);
                        speedGraphPlot.label = lapNumberLabel;

                        var throttleGraphPlot = ThrottleGraphPlot.plt.PlotScatter(distance, throttle, lineWidth: 1.75, markerShape: MarkerShape.none);
                        throttleGraphPlot.label = lapNumberLabel;

                        var brakeGraphPlot = BrakeGraphPlot.plt.PlotScatter(distance, brake, lineWidth: 1.75, markerShape: MarkerShape.none);
                        brakeGraphPlot.label = lapNumberLabel;

                        var gearGraphPlot = GearGraphPlot.plt.PlotScatter(distance, gear, lineWidth: 1.75, markerShape: MarkerShape.none);
                        gearGraphPlot.label = lapNumberLabel;

                        graphPlots[0] = speedGraphPlot;
                        graphPlots[1] = throttleGraphPlot;
                        graphPlots[2] = brakeGraphPlot;
                        graphPlots[3] = gearGraphPlot;

                        SpeedGraphPlot.plt.Legend();
                        ThrottleGraphPlot.plt.Legend();
                        BrakeGraphPlot.plt.Legend();
                        GearGraphPlot.plt.Legend();

                        PlottedLapData.Add(toggleLapInfo.lapNumber, graphPlots);
                    }
                }
            }

            if (!toggleLapInfo.shouldPlot)
            {
                if (PlottedLapData.TryGetValue(toggleLapInfo.lapNumber, out Plottable[] plots))
                {
                    SpeedGraphPlot.plt.Remove(plots[0]);
                    ThrottleGraphPlot.plt.Remove(plots[1]);
                    BrakeGraphPlot.plt.Remove(plots[2]);
                    GearGraphPlot.plt.Remove(plots[3]);

                    PlottedLapData.Remove(toggleLapInfo.lapNumber);
                }
            }
        }

        private void ClearLiveTelemetryGraph(object parameter)
        {
            foreach (var lapModel in LapData)
            {
                lapModel.Clear();
            }

            UnbindLiveTelemetryGraph();
            IsLiveTelemetryEnabled = false;
            ResetCurrentLapCursor();
            ResetRenderCursor();
        }

        /// <summary>
        /// Determines whether this instance [can clear all graph].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        ///   <c>true</c> if this instance [can clear all graph]; otherwise, <c>false</c>.
        /// </returns>
        private bool CanClearAllGraph(object parameter)
        {
            return (LapSummaries.FirstOrDefault(s => s.IsChecked) != default) || ClearLiveTelemetryGraphCommand.CanExecute(null);
        }

        private void ClearAllGraph(object parameter)
        {
            if (ClearLiveTelemetryGraphCommand.CanExecute(null))
            {
                ClearLiveTelemetryGraphCommand.Execute(null);
            }

            ClearPlottedLapData();

            foreach (var lapSummary in LapSummaries)
            {
                lapSummary.IsChecked = false;
            }
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

            var currentTelemetry = Manager.GetPlayerInfo()?.CurrentTelemetry;
            var currentLapData = Manager.GetPlayerInfo()?.CurrentLapData;
            var currentCarStatus = Manager.GetPlayerInfo()?.CurrentCarStatus;

            if (currentTelemetry != null)
            {
                if (currentLapData?.DriverStatus == DriverStatus.InGarage)
                {
                    ResetCurrentTelemetryIndexCursor();
                }
                else if (currentLapData.DriverStatus == DriverStatus.InLap || currentLapData.DriverStatus == DriverStatus.OutLap)
                {
                }
                else
                {
                    if (IsLiveTelemetryEnabled)
                    {
                        UpdateLiveTelemetry(currentTelemetry, currentLapData);
                    }
                }

                UpdateCurrentTelemetry(currentTelemetry);
            }

            if (currentCarStatus != null)
            {
                CurrentTelemetry.TyreCompound = (TyreCompound)currentCarStatus.ActualTyreCompound;
            }

            void UpdateCurrentTelemetry(CarTelemetryData currentTelemetry)
            {
                CurrentTelemetry.LapNumber = currentLapData.CurrentLapNum;
                CurrentTelemetry.Brake = currentTelemetry.Brake;
                CurrentTelemetry.Throttle = currentTelemetry.Throttle;
                CurrentTelemetry.EngineRPM = currentTelemetry.EngineRPM;
                CurrentTelemetry.Speed = currentTelemetry.Speed;
                CurrentTelemetry.LapTime = currentLapData.CurrentLapTime;
                CurrentTelemetry.BestLapTime = currentLapData.BestLapTime;

                if (currentCarStatus != null)
                {
                    CurrentTelemetry.FuelRemainingLap = currentCarStatus.FuelRemainingLaps;
                }

                CurrentTelemetry.TyreSurfaceTemperature.FrontLeft.Update(currentTelemetry.TyresSurfaceTemperature[(int)WheelPositions.FrontLeft]);
                CurrentTelemetry.TyreSurfaceTemperature.FrontRight.Update(currentTelemetry.TyresSurfaceTemperature[(int)WheelPositions.FrontRight]);
                CurrentTelemetry.TyreSurfaceTemperature.RearLeft.Update(currentTelemetry.TyresSurfaceTemperature[(int)WheelPositions.RearLeft]);
                CurrentTelemetry.TyreSurfaceTemperature.RearRight.Update(currentTelemetry.TyresSurfaceTemperature[(int)WheelPositions.RearRight]);

                CurrentTelemetry.TyreCarcassTemperature.FrontLeft.Update(currentTelemetry.TyresInnerTemperature[(int)WheelPositions.FrontLeft]);
                CurrentTelemetry.TyreCarcassTemperature.FrontRight.Update(currentTelemetry.TyresInnerTemperature[(int)WheelPositions.FrontRight]);
                CurrentTelemetry.TyreCarcassTemperature.RearLeft.Update(currentTelemetry.TyresInnerTemperature[(int)WheelPositions.RearLeft]);
                CurrentTelemetry.TyreCarcassTemperature.RearRight.Update(currentTelemetry.TyresInnerTemperature[(int)WheelPositions.RearRight]);
            }
        }

        private void UpdateLiveTelemetry(CarTelemetryData currentTelemetry, LapData currentLapData)
        {
            CurrentRenderPosition[0] = currentLapData.LapDistance;

            if (LapData != null)
            {
                var currentLapDataModel = LapData[CurrentLapCursor];

                var lapNumberLabel = $"Lap {currentLapData.CurrentLapNum}";

                currentLapDataModel.Speed[CurrentTelemetryIndexCursor] = currentTelemetry.Speed;
                currentLapDataModel.Distance[CurrentTelemetryIndexCursor] = currentLapData.LapDistance;
                currentLapDataModel.Gear[CurrentTelemetryIndexCursor] = currentTelemetry.Gear;

                currentLapDataModel.Throttle[CurrentTelemetryIndexCursor] = currentTelemetry.Throttle;
                currentLapDataModel.Brake[CurrentTelemetryIndexCursor] = currentTelemetry.Brake;

                var currentSpeedGraph = SpeedGraph[CurrentLapCursor];
                var currentGearGraph = GearGraph[CurrentLapCursor];
                var currentThrottleGraph = ThrottleGraph[CurrentLapCursor];
                var currentBrakeGraph = BrakeGraph[CurrentLapCursor];

                if (currentSpeedGraph != null && currentGearGraph != null && currentThrottleGraph != null && currentBrakeGraph != null)
                {
                    currentSpeedGraph.maxRenderIndex = CurrentTelemetryIndexCursor;
                    currentSpeedGraph.label = lapNumberLabel;

                    currentGearGraph.maxRenderIndex = CurrentTelemetryIndexCursor;
                    currentGearGraph.label = lapNumberLabel;

                    currentThrottleGraph.maxRenderIndex = CurrentTelemetryIndexCursor;
                    currentThrottleGraph.label = lapNumberLabel;

                    currentBrakeGraph.maxRenderIndex = CurrentTelemetryIndexCursor;
                    currentBrakeGraph.label = lapNumberLabel;
                }
            }

            CurrentTelemetryIndexCursor++;
        }

        /// <summary>
        /// Binds the graphs to view model.
        /// </summary>
        private void BindLiveTelemetryGraph()
        {
            BindCursorBar();

            LapData = new CurrentLapDataModel[3];

            for (int i = 0; i < LapData.Length; i++)
            {
                LapData[i] = new CurrentLapDataModel();
            }

            for (int i = 0; i < LapData.Length; i++)
            {
                SpeedGraph[i] = SpeedGraphPlot.plt.PlotSignalXY(LapData[i].Distance, LapData[i].Speed, lineWidth: DefaultLineWidth);
                SpeedGraphPlot.plt.YLabel("Speed");
                SpeedGraphPlot.plt.Legend();

                GearGraph[i] = GearGraphPlot.plt.PlotSignalXY(LapData[i].Distance, LapData[i].Gear, lineWidth: DefaultLineWidth);

                GearGraphPlot.plt.YLabel("Gear");
                GearGraphPlot.plt.Legend();

                BrakeGraph[i] = BrakeGraphPlot.plt.PlotSignalXY(LapData[i].Distance, LapData[i].Brake, lineWidth: DefaultLineWidth);
                ThrottleGraph[i] = ThrottleGraphPlot.plt.PlotSignalXY(LapData[i].Distance, LapData[i].Throttle, lineWidth: DefaultLineWidth);

                ThrottleGraphPlot.plt.YLabel("Throttle");
                ThrottleGraphPlot.plt.Legend();

                BrakeGraphPlot.plt.YLabel("Brake");
                BrakeGraphPlot.plt.Legend();
            }

            UpdateGraphXAxisToTrackLength();
        }

        private void BindCursorBar()
        {
            SpeedGraphPlot.plt.PlotBar(CurrentRenderPosition, CurrentRenderValue);
            GearGraphPlot.plt.PlotBar(CurrentRenderPosition, CurrentRenderValue);
            BrakeGraphPlot.plt.PlotBar(CurrentRenderPosition, CurrentRenderValue);
            ThrottleGraphPlot.plt.PlotBar(CurrentRenderPosition, CurrentRenderValue);
        }

        private void UnbindLiveTelemetryGraph()
        {
            for (int i = 0; i < SpeedGraph.Length; i++)
            {
                SpeedGraphPlot.plt.Remove(SpeedGraph[i]);
                ThrottleGraphPlot.plt.Remove(ThrottleGraph[i]);
                BrakeGraphPlot.plt.Remove(BrakeGraph[i]);
                GearGraphPlot.plt.Remove(GearGraph[i]);
            }

            Array.Clear(SpeedGraph, 0, SpeedGraph.Length);
            Array.Clear(ThrottleGraph, 0, ThrottleGraph.Length);
            Array.Clear(BrakeGraph, 0, BrakeGraph.Length);
            Array.Clear(GearGraph, 0, GearGraph.Length);

            ResetCurrentTelemetryIndexCursor();
            LapData = null;
        }

        private void ClearPlottedLapData()
        {
            foreach (var plots in PlottedLapData)
            {
                SpeedGraphPlot.plt.Remove(plots.Value[0]);
                ThrottleGraphPlot.plt.Remove(plots.Value[1]);
                BrakeGraphPlot.plt.Remove(plots.Value[2]);
                GearGraphPlot.plt.Remove(plots.Value[3]);
            }

            PlottedLapData.Clear();
        }

        private void UpdateGraphXAxisToTrackLength()
        {
            SpeedGraphPlot.plt.Axis(0, SessionInfo.TrackLength, 0, 360);
            GearGraphPlot.plt.Axis(0, SessionInfo.TrackLength, 0, 9);
            ThrottleGraphPlot.plt.Axis(0, SessionInfo.TrackLength, 0, 1.05);
            BrakeGraphPlot.plt.Axis(0, SessionInfo.TrackLength, 0, 1.05);
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