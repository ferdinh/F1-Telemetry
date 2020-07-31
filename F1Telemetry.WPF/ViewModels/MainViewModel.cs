using F1Telemetry.WPF.Command;
using F1Telemetry.WPF.Model;
using System.ComponentModel;

namespace F1Telemetry.WPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public SessionViewModel SessionInfo { get; set; } = new SessionViewModel();

        // 5000 for one minute of data.
        public double[] speed = new double[25_000];

        public double[] time = new double[25_000];
        public double[] gear = new double[25_000];

        public double[] throttle = new double[25_000];
        public double[] brake = new double[25_000];

        public CurrentTelemetryDataModel CurrentTelemetry { get; set; } = new CurrentTelemetryDataModel();

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand<bool> SetTopmostCommand { get; private set; }

        public bool IsTopmost { get; internal set; }

        private void SetTopmost(bool topmost)
        {
            IsTopmost = topmost;
        }

        public MainViewModel()
        {
            SetTopmostCommand = new RelayCommand<bool>(SetTopmost);
        }
    }
}