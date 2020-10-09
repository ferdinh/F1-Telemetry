using F1Telemetry.Core;
using F1Telemetry.Core.Data;
using System.ComponentModel;

namespace F1Telemetry.WPF.Model
{
    /// <summary>
    /// Contains the data for the current condition/telemetry of the car.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class CurrentTelemetryDataModel : INotifyPropertyChanged
    {
        public int LapNumber { get; set; }
        public float Brake { get; set; }
        public float Throttle { get; set; }
        public ushort EngineRPM { get; set; }
        public ushort Speed { get; set; }
        public float LapTime { get; set; }
        public float BestLapTime { get; set; }
        public CarTyreTemperatureModel TyreCarcassTemperature { get; set; } = new CarTyreTemperatureModel();
        public CarTyreTemperatureModel TyreSurfaceTemperature { get; set; } = new CarTyreTemperatureModel();

        public TyreCompound TyreCompound { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}