using System.ComponentModel;

namespace F1Telemetry.WPF.Model
{
    public class LapSummaryModel : INotifyPropertyChanged
    {
        public bool IsChecked { get; set; }
        public int LapNumber { get; set; }
        public float LapTime { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
