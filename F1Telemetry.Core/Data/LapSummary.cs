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
        public IReadOnlyList<LapData> LapData { get; }
        public IReadOnlyList<CarStatusData> CarStatusData { get; }
        public IReadOnlyList<CarTelemetryData> CarTelemetryData { get; }
        public TyreCompound TyreCompoundUsed { get; }

        public LapSummary(int lapNumber, float lapTime, List<LapData> lapDatas, List<CarStatusData> carStatusDatas, List<CarTelemetryData> carTelemetryDatas)
        {
            LapData = lapDatas.AsReadOnly();
            CarStatusData = carStatusDatas.AsReadOnly();
            CarTelemetryData = carTelemetryDatas.AsReadOnly();
            LapNumber = lapNumber;
            LapTime = lapTime;

            var carStatus = carStatusDatas.LastOrDefault();

            TyreCompoundUsed = carStatus != null ? (TyreCompound)carStatus.ActualTyreCompound : TyreCompound.Unknown;
        }
    }
}