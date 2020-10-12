﻿using System;
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
        public float BestLapTime { get; }
        public IReadOnlyList<LapData> LapData { get; }
        public IReadOnlyList<CarStatusData> CarStatusData { get; }
        public IReadOnlyList<CarTelemetryData> CarTelemetryData { get; }
        public TyreCompound TyreCompoundUsed { get; }
        public float ERSEnergyStore { get; }
        public float ERSDeployed { get; set; }
        public float ERSDeployedPercentage => ERSDeployed / CarInfo.F1.MaxDeployableERS;
        public float FuelUsed => CalculateFuelUsage();

        public LapSummary(int lapNumber, float lapTime, float bestLapTime, List<LapData> lapDatas, List<CarStatusData> carStatusDatas, List<CarTelemetryData> carTelemetryDatas)
        {
            LapData = lapDatas.AsReadOnly();
            CarStatusData = carStatusDatas.AsReadOnly();
            CarTelemetryData = carTelemetryDatas.AsReadOnly();
            LapNumber = lapNumber;
            LapTime = lapTime;
            BestLapTime = bestLapTime;

            var carStatus = carStatusDatas.LastOrDefault();

            TyreCompoundUsed = carStatus != null ? (TyreCompound)carStatus.ActualTyreCompound : TyreCompound.Unknown;
            ERSEnergyStore = carStatus != null ? carStatus.ErsStoreEnergy : 0.0f;
            ERSDeployed = carStatus != null ? carStatus.ErsDeployedThisLap : 0.0f;
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