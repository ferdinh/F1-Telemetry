using F1Telemetry.Core.Data;
using System.ComponentModel;

namespace F1Telemetry.WPF.Model
{
    public class LapSummaryModel : INotifyPropertyChanged
    {
        public bool IsChecked { get; set; }
        public int LapNumber { get; set; }
        public float LapTime { get; set; }
        public SectorTime SectorTime { get; set; }
        public float DeltaToBestTime { get; set; }
        public TyreCompound TyreCompoundUsed { get; set; }
        public float ERSDeployed { get; set; }
        public float TotalERSHarvestedPercentage { get; set; }
        public float ERSDeployedPercentage { get; set; }
        public float FuelUsed { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}