using System;
using System.Collections.Generic;
using System.Linq;

namespace F1Telemetry.Core.Data
{
    /// <summary>
    /// Contains the car and telemetry summary of a lap.
    /// </summary>
    public class LapSummary
    {
        public int LapNumber { get; }
        public float LapTime { get; }
        public SectorTime SectorTime { get; }
        public float BestLapTime { get; }
        public IReadOnlyList<LapData> LapData { get; }
        public IReadOnlyList<CarStatusData> CarStatusData { get; }
        public IReadOnlyList<CarTelemetryData> CarTelemetryData { get; }
        public TyreCompound TyreCompoundUsed { get; } = TyreCompound.Unknown;
        public float ERSEnergyStore { get; }
        public float ERSHarvestedThisLapMGUK { get; }
        public float ERSHarvestedThisLapMGUH { get; }
        public float TotalERSHarvested => ERSHarvestedThisLapMGUH + ERSHarvestedThisLapMGUK;
        public float TotalERSHarvestedPercentage => TotalERSHarvested / CarInfo.F1.MaxDeployableERS;
        public float ERSDeployed { get; set; }
        public float ERSDeployedPercentage => ERSDeployed / CarInfo.F1.MaxDeployableERS;
        public float FuelUsed => CalculateFuelUsage();

        /// <summary>
        /// Initializes a new instance of the <see cref="LapSummary"/> class.
        /// </summary>
        /// <param name="latestLapData">The latest lap data given from the game. This is usually the next packet after a new lap.</param>
        /// <param name="lapDatas">The lap datas.</param>
        /// <param name="carStatusDatas">The car status datas.</param>
        /// <param name="carTelemetryDatas">The car telemetry datas.</param>
        public LapSummary(LapData latestLapData, List<LapData> lapDatas, List<CarStatusData> carStatusDatas, List<CarTelemetryData> carTelemetryDatas)
        {
            _ = latestLapData ?? throw new ArgumentNullException(nameof(latestLapData));

            LapData = lapDatas ?? throw new ArgumentNullException(nameof(lapDatas));
            CarStatusData = carStatusDatas ?? throw new ArgumentNullException(nameof(carStatusDatas));
            CarTelemetryData = carTelemetryDatas ?? throw new ArgumentNullException(nameof(carTelemetryDatas));

            LapNumber = latestLapData.CurrentLapNum - 1;
            LapTime = latestLapData.LastLapTime;
            BestLapTime = latestLapData.BestLapTime;

            var lapData = lapDatas.LastOrDefault();

            if (lapData != null)
            {
                var sector1 = lapData.Sector1TimeInMS / 1000.0f;
                var sector2 = lapData.Sector2TimeInMS / 1000.0f;
                var sector3 = LapTime - sector1 - sector2;

                SectorTime = new SectorTime(sector1, sector2, sector3);
            }

            var carStatus = carStatusDatas.LastOrDefault();

            if (carStatus != null)
            {
                TyreCompoundUsed = (TyreCompound)carStatus.ActualTyreCompound;
                ERSEnergyStore = carStatus.ErsStoreEnergy;
                ERSDeployed = carStatus.ErsDeployedThisLap;
                ERSHarvestedThisLapMGUK = carStatus.ErsHarvestedThisLapMGUK;
                ERSHarvestedThisLapMGUH = carStatus.ErsHarvestedThisLapMGUH;
            }
        }

        private float CalculateFuelUsage()
        {
            var fuelUsed = 0.0f;

            if (CarStatusData.Any())
            {
                var initialFuel = CarStatusData.First().FuelInTank;
                var endFuel = CarStatusData.Last().FuelInTank;

                fuelUsed = Math.Max(0, initialFuel - endFuel);
            }

            return fuelUsed;
        }
    }
}