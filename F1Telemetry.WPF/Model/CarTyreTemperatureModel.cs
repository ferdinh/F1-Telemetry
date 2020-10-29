using F1Telemetry.Core.Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace F1Telemetry.WPF.Model
{
    public class CarTyreTemperatureModel
    {
        public TyreTemperature FrontLeft { get; set; } = new TyreTemperature();
        public TyreTemperature FrontRight { get; set; } = new TyreTemperature();
        public TyreTemperature RearLeft { get; set; } = new TyreTemperature();
        public TyreTemperature RearRight { get; set; } = new TyreTemperature();

        /// <summary>
        /// Updates all car tyre temperature.
        /// </summary>
        /// <param name="tyreTemperatures">The tyre temperatures.</param>
        public void UpdateAllTyres(byte[] tyreTemperatures)
        {
            FrontLeft.Update(tyreTemperatures[(int)WheelPositions.FrontLeft]);
            FrontRight.Update(tyreTemperatures[(int)WheelPositions.FrontRight]);
            RearLeft.Update(tyreTemperatures[(int)WheelPositions.RearLeft]);
            RearRight.Update(tyreTemperatures[(int)WheelPositions.RearRight]);
        }
    }

    public class TyreTemperature : INotifyPropertyChanged
    {
        public byte Min { get; private set; }
        public byte Max { get; private set; }
        public byte Current { get; private set; }

#pragma warning disable 169, 67

        public event PropertyChangedEventHandler PropertyChanged;

#pragma warning restore 169, 67

        public void Update(byte temperature)
        {
            Current = temperature;
            OnPropertyChanged(nameof(Current));

            if (IsMax(temperature))
            {
                Max = temperature;
                OnPropertyChanged(nameof(Max));
            }

            if (IsMin(temperature) || Min == 0)
            {
                Min = temperature;
                OnPropertyChanged(nameof(Min));
            }

            bool IsMax(byte temperature)
            {
                return temperature > Max;
            }

            bool IsMin(byte temperature)
            {
                return temperature < Min;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}