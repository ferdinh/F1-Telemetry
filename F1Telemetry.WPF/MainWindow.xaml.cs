using F12020Telemetry;
using F1Telemetry.WPF.ViewModels;
using System;
using System.Windows;

namespace F1Telemetry.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainViewModel = new MainViewModel();

            DataContext = MainViewModel;

            BindGraphToViewModel();
            RegisterGraphRenderDispatcher();

            MainViewModel.Manager.NewSession += ManagerNewSession;
        }

        private MainViewModel MainViewModel { get; set; }

        /// <summary>
        /// Binds the graphs to view model.
        /// </summary>
        private void BindGraphToViewModel()
        {
            for (int i = 0; i < MainViewModel.LapData.Length; i++)
            {
                MainViewModel.SpeedGraph[i] = SpeedGraphPlot.plt.PlotSignalXY(MainViewModel.LapData[i].Distance, MainViewModel.LapData[i].Speed);
                SpeedGraphPlot.plt.PlotBar(MainViewModel.currentRenderPosition, MainViewModel.currentRenderValue);
                SpeedGraphPlot.plt.YLabel("Speed");
                SpeedGraphPlot.plt.Legend();

                MainViewModel.GearGraph[i] = GearGraphPlot.plt.PlotSignalXY(MainViewModel.LapData[i].Distance, MainViewModel.LapData[i].Gear);
                GearGraphPlot.plt.PlotBar(MainViewModel.currentRenderPosition, MainViewModel.currentRenderValue);
                GearGraphPlot.plt.YLabel("Gear");
                GearGraphPlot.plt.Legend();

                MainViewModel.BrakeGraph[i] = BrakeGraphPlot.plt.PlotSignalXY(MainViewModel.LapData[i].Distance, MainViewModel.LapData[i].Brake);
                MainViewModel.ThrottleGraph[i] = ThrottleGraphPlot.plt.PlotSignalXY(MainViewModel.LapData[i].Distance, MainViewModel.LapData[i].Throttle);

                BrakeGraphPlot.plt.PlotBar(MainViewModel.currentRenderPosition, MainViewModel.currentRenderValue);
                ThrottleGraphPlot.plt.PlotBar(MainViewModel.currentRenderPosition, MainViewModel.currentRenderValue);

                SpeedGraphPlot.plt.Axis(0, 6000, 0, 360);
                GearGraphPlot.plt.Axis(0, 6000, 0, 9);

                ThrottleGraphPlot.plt.Axis(0, 6000, 0, 1.05);
                ThrottleGraphPlot.plt.YLabel("Throttle");
                ThrottleGraphPlot.plt.Legend();

                BrakeGraphPlot.plt.Axis(0, 6000, 0, 1.05);
                BrakeGraphPlot.plt.YLabel("Brake");
                BrakeGraphPlot.plt.Legend();
            }
        }

        private void RegisterGraphRenderDispatcher()
        {
            MainViewModel.GraphRenderTimer.Interval = TimeSpan.FromMilliseconds(20);
            MainViewModel.GraphRenderTimer.Tick += (s, e) =>
            {
                SpeedGraphPlot.Render(recalculateLayout: true);

                GearGraphPlot.Render(recalculateLayout: true);

                BrakeGraphPlot.Render(recalculateLayout: true);
                ThrottleGraphPlot.Render(recalculateLayout: true);
            };
        }

        private void ManagerNewSession(object sender, EventArgs e)
        {
            var manager = sender as TelemetryManager;

            if (manager != null)
            {
                SpeedGraphPlot.plt.Axis(0, manager.Session.TrackLength, 0, 360);
                GearGraphPlot.plt.Axis(0, manager.Session.TrackLength, 0, 9);

                ThrottleGraphPlot.plt.Axis(0, manager.Session.TrackLength, 0, 1.05);
                BrakeGraphPlot.plt.Axis(0, manager.Session.TrackLength, 0, 1.05);

                foreach (var lapModel in MainViewModel.LapData)
                {
                    lapModel.Clear();
                }

                MainViewModel.ResetRenderCursor();

                manager.GetPlayerInfo().NewLap += (s, e) =>
                {
                    MainViewModel.ResetRenderCursor();
                    MainViewModel.CurrentLapCursor = (MainViewModel.CurrentLapCursor + 1) % MainViewModel.LapData.Length;
                    MainViewModel.IndexCursor = 0;
                };
            }
        }
    }
}