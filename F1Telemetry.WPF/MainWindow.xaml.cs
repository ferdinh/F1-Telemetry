using F12020Telemetry;
using F12020Telemetry.Data;
using F12020Telemetry.Util.Extensions;
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

        public MainWindow()
        {
            InitializeComponent();

            DataContext = MainViewModel;

            SpeedGraph = MainGraphPlot.plt.PlotSignalXY(MainViewModel.time, MainViewModel.speed);
            MainGraphPlot.plt.PlotBar(currentRenderPosition, currentRenderValue);
            MainGraphPlot.plt.YLabel("Speed");

            GearGraph = GearGraphPlot.plt.PlotSignalXY(MainViewModel.time, MainViewModel.gear);
            GearGraphPlot.plt.PlotBar(currentRenderPosition, currentRenderValue);
            GearGraphPlot.plt.YLabel("Gear");

            BrakeGraph = BrakeGraphPlot.plt.PlotSignalXY(MainViewModel.time, MainViewModel.brake, color: Color.Red);
            ThrottleGraph = ThrottleGraphPlot.plt.PlotSignalXY(MainViewModel.time, MainViewModel.throttle, color: Color.Green);

            BrakeGraphPlot.plt.PlotBar(currentRenderPosition, currentRenderValue);
            ThrottleGraphPlot.plt.PlotBar(currentRenderPosition, currentRenderValue);

            MainGraphPlot.plt.Axis(0, 6000, 0, 360);
            GearGraphPlot.plt.Axis(0, 6000, 0, 9);

            ThrottleGraphPlot.plt.Axis(0, 6000, 0, 1.05);
            ThrottleGraphPlot.plt.YLabel("Throttle");
            BrakeGraphPlot.plt.Axis(0, 6000, 0, 1.05);
            BrakeGraphPlot.plt.YLabel("Brake");

            GraphRenderTimer.Interval = TimeSpan.FromMilliseconds(20);
            GraphRenderTimer.Tick += (s, e) =>
            {
                MainGraphPlot.Render(recalculateLayout: true);

                GearGraphPlot.Render(recalculateLayout: true);

                BrakeGraphPlot.Render(recalculateLayout: true);
                ThrottleGraphPlot.Render(recalculateLayout: true);
            };
        }
        }
    }
}
