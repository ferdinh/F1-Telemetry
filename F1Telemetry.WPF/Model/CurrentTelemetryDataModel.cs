using System.ComponentModel;

namespace F1Telemetry.WPF.Model
{
    public class CurrentTelemetryDataModel : INotifyPropertyChanged
    {
        public float Brake { get; set; }
        public float Throttle { get; set; }
        public ushort EngineRPM { get; set; }
        public ushort Speed { get; set; }
        public float LapTime {get;set;}

        public event PropertyChangedEventHandler PropertyChanged;
    }
}