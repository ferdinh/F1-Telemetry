using System.ComponentModel;

namespace F1Telemetry.WPF.ViewModels
{
    public class SessionViewModel : INotifyPropertyChanged
    {
        public string SessionType { get; set; }
        public ushort TrackLength { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}