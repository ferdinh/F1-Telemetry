using F1Telemetry.Core;
using F1Telemetry.Core.Data;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace F1Telemetry.Test.Core
{
    public class LapSummaryTest
    {
        [Fact]
        public void Contructor_Should_Throw_NullReference_If_No_Latest_LapData_Given()
        {
            Action action = () => new LapSummary(null, null, null, null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Contructor_Should_Throw_NullReference_If_No_LapDatas_Given()
        {
            Action action = () => new LapSummary(new LapData(0, 0), null, null, null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Contructor_Should_Throw_NullReference_If_No_CarStatusDatas_Given()
        {
            Action action = () => new LapSummary(new LapData(0, 0), new List<LapData>(), null, null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Contructor_Should_Throw_NullReference_If_No_CarTelemetryDatas_Given()
        {
            Action action = () => new LapSummary(new LapData(0, 0), new List<LapData>(), new List<CarStatusData>(), null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(100.12f)]
        [InlineData(2000.0f)]
        [InlineData(30000.0f)]
        [InlineData(1234.45f)]
        public void Given_ERSDeployed_Percentage_Should_Return_Based_On_TheMax_DeployableERS(float ersDeployed)
        {
            var expected = (float)Math.Round(ersDeployed / CarInfo.F1.MaxDeployableERS, 2);

            var carStatusData = new List<CarStatusData>
            {
                new CarStatusData(0, 0)
                {
                    ErsDeployedThisLap = ersDeployed
                }
            };

            var lapSummary = new LapSummary(new LapData(0, 0), new List<LapData>(), carStatusData, new List<CarTelemetryData>());

            var actual = lapSummary.ERSDeployedPercentage;

            actual.Should().BeApproximately(expected, 0.01f);
        }

        [Theory]
        [InlineData(100.12f)]
        [InlineData(2000.0f)]
        [InlineData(30000.0f)]
        [InlineData(1234.45f)]
        public void Given_TotalERSHarvested_Percentage_Should_Return_Based_On_TheMax_DeployableERS(float ersHarvested)
        {
            var expected = (float)Math.Round(ersHarvested / CarInfo.F1.MaxDeployableERS, 2);

            var carStatusData = new List<CarStatusData>
            {
                new CarStatusData(0, 0)
                {
                    ErsHarvestedThisLapMGUH = ersHarvested
                }
            };

            var lapSummary = new LapSummary(new LapData(0, 0), new List<LapData>(), carStatusData, new List<CarTelemetryData>());

            var actual = lapSummary.TotalERSHarvestedPercentage;

            actual.Should().BeApproximately(expected, 0.01f);
        }

        [Theory]
        [InlineData(100.0f, 150.0f)]
        [InlineData(2000.0f, 4000.0f)]
        [InlineData(30000.0f, 30000.0f)]
        [InlineData(1234.45f, 1234.45f)]
        public void Given_TotalERSHarvested_Should_Return_Combined_ERSHarvested(float ersHarvestedMGUK, float ersHarvestedMGUH)
        {
            var expected = ersHarvestedMGUH + ersHarvestedMGUK;

            var carStatusData = new List<CarStatusData>
            {
                new CarStatusData(0, 0)
                {
                    ErsHarvestedThisLapMGUH = ersHarvestedMGUH,
                    ErsHarvestedThisLapMGUK = ersHarvestedMGUK
                }
            };

            var lapSummary = new LapSummary(new LapData(0, 0), new List<LapData>(), carStatusData, new List<CarTelemetryData>());

            var actual = lapSummary.TotalERSHarvested;

            actual.Should().BeApproximately(expected, 0.05f);
        }

        [Fact]
        public void FuelUsed_Should_Return_The_Difference_Between_Fuel_At_The_Start_Of_The_Lap_and_End_Of_Lap()
        {
            var carStatusData = new List<CarStatusData>
            {
                new CarStatusData(0, 0)
                {
                    FuelInTank = 100
                },
                new CarStatusData(0, 0)
                {
                    FuelInTank = 98
                },
                new CarStatusData(0, 0)
                {
                    FuelInTank = 96
                },
                new CarStatusData(0, 0)
                {
                    FuelInTank = 94
                }
            };

            var lapSummary = new LapSummary(new LapData(0, 0), new List<LapData>(), carStatusData, new List<CarTelemetryData>());

            lapSummary.FuelUsed.Should().BeApproximately(6.0f, 0.01f);
        }

        [Fact]
        public void FuelUsed_Should_Not_Return_Negative_If_There_Is_Incorrect_Data()
        {
            var carStatusData = new List<CarStatusData>
            {
                new CarStatusData(0, 0)
                {
                    FuelInTank = 94
                },
                new CarStatusData(0, 0)
                {
                    FuelInTank = 100
                }
            };

            var lapSummary = new LapSummary(new LapData(0, 0), new List<LapData>(), carStatusData, new List<CarTelemetryData>());

            lapSummary.FuelUsed.Should().BeGreaterOrEqualTo(0.0f, because: "There is no refueling and a lap should always consume fuel. You can't make fuel from thin air.");
        }

        [Fact]
        public void FuelUsed_Should_Return_Zero_When_There_Is_No_Data()
        {
            var lapSummary = new LapSummary(new LapData(0, 0), new List<LapData>(), new List<CarStatusData>(), new List<CarTelemetryData>());

            lapSummary.FuelUsed.Should().Be(0.0f);
        }

        [Fact]
        public void LapSummary_Should_Return_SectorTime_in_Seconds()
        {
            var lapSummary = new LapSummary(new LapData(0, 0) { LastLapTime = 3 }, new List<LapData> { new LapData(0, 0) { Sector1TimeInMS = 1000, Sector2TimeInMS = 1000 } }, new List<CarStatusData>(), new List<CarTelemetryData>());

            var expected = new SectorTime(1, 1, 1);

            lapSummary.SectorTime.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void TyreWearPercentage_Should_Measure_The_Difference_Of_The_Wear_Of_The_Same_Tyre()
        {
            var carStatusData = new List<CarStatusData>
            {
                new CarStatusData(0, 0)
                {
                    TyresWear = new byte[]
                    {
                        2,
                        1,
                        4, // <- Should compare this
                        5
                    }
                },
                new CarStatusData(0, 0)
                {
                    TyresWear = new byte[]
                    {
                        3,
                        2,
                        20, // <- Should compare this
                        13
                    }
                }
            };

            var lapSummary = new LapSummary(new LapData(0, 0), new List<LapData>(), carStatusData, new List<CarTelemetryData>());
            var expected = 0.16f;

            lapSummary.TyreWearPercentage.Should().BeApproximately(expected, 0.01f);
        }

        [Fact]
        public void TyreWearPercentage_Should_Return_Minus_One_If_Data_Is_Corrupted()
        {
            var carStatusData = new List<CarStatusData>
            {
                new CarStatusData(0, 0)
                {
                    TyresWear = new byte[]
                    {
                        2,
                        1,
                        25, 
                        5
                    }
                },
                new CarStatusData(0, 0)
                {
                    TyresWear = new byte[]
                    {
                        3,
                        2,
                        20,
                        13
                    }
                }
            };

            var lapSummary = new LapSummary(new LapData(0, 0), new List<LapData>(), carStatusData, new List<CarTelemetryData>());
            var expected = -1.0f;

            lapSummary.TyreWearPercentage.Should().BeApproximately(expected, 0.01f);
        }

        [Fact]
        public void ErsDeployMode_Should_Return_The_Highest_Mode_Used_During_The_Lap()
        {
            var expectedErsDeployMode = ErsDeployMode.Hotlap;

            var carStatusData = new List<CarStatusData>
            {
                new CarStatusData(0, 0)
                {
                    ErsDeployMode = 0
                },
                new CarStatusData(0, 0)
                {
                    ErsDeployMode = 0
                },
                new CarStatusData(0, 0)
                {
                    ErsDeployMode = 3
                },
                new CarStatusData(0, 0)
                {
                    ErsDeployMode = 0
                }
            };

            var lapSummary = new LapSummary(new LapData(0, 0), new List<LapData>(), carStatusData, new List<CarTelemetryData>());

            lapSummary.ErsDeployMode.Should().Be(expectedErsDeployMode);
        }
    }
}