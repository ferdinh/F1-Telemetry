using F1Telemetry.Core;
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

            RegisterGraphRenderDispatcher();

            MainViewModel.SpeedGraphPlot = SpeedGraphPlot;
            MainViewModel.ThrottleGraphPlot = ThrottleGraphPlot;
            MainViewModel.BrakeGraphPlot = BrakeGraphPlot;
            MainViewModel.GearGraphPlot = GearGraphPlot;

            lapLeaderboard.ItemsSource = MainViewModel.LapSummaries;

            MainViewModel.Manager.NewSession += ManagerNewSession;
        }

        private MainViewModel MainViewModel { get; set; }

        private void RegisterGraphRenderDispatcher()
        {
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
            }
        }
    }
}