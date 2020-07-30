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

        }
    }
}
