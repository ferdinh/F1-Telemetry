using F1Telemetry.WPF.Command;
using F1Telemetry.WPF.Model;
using System.ComponentModel;

namespace F1Telemetry.WPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public SessionViewModel SessionInfo { get; set; } = new SessionViewModel();

        public CurrentLapDataModel[] LapData { get; } = new CurrentLapDataModel[3];
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
            for (int i = 0; i < LapData.Length; i++)
            {
                LapData[i] = new CurrentLapDataModel();
            }
        }
    }
}