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

            lapLeaderboardDataGrid.ItemsSource = MainViewModel.LapSummaries;

            SpeedGraphPlot.plt.YLabel("Speed");
            ThrottleGraphPlot.plt.YLabel("Throttle");
            BrakeGraphPlot.plt.YLabel("Brake");
            GearGraphPlot.plt.YLabel("Gear");
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
    }
}