using System.ComponentModel;

namespace F1Telemetry.WPF.ViewModels
{
    public class CurrentTelemetryDataViewModel : INotifyPropertyChanged
    {
        public float Brake { get; set; }
        public float Throttle { get; set; }
        public ushort EngineRPM { get; set; }
        public ushort Speed { get; set; }
        public float LapTime {get;set;}

        public event PropertyChangedEventHandler PropertyChanged;
    }
}