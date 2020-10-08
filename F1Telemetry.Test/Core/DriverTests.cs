using F1Telemetry.Core.Data;
using FluentAssertions;
using Xunit;

namespace F1Telemetry.Test.Core
{
    public class DriverTests
    {
        [Fact]
        public void Pitting_Should_Raise_When_Status_Changed_To_Pitting_From_Other_Status()
        {
            var driver = new Driver(new F1Telemetry.Core.TelemetryManager());

            using (var monitoredDriver = driver.Monitor())
            {
                driver.AddLapData(new LapData(0, 0)
                {
                    PitStatus = PitStatus.None
                });

                driver.AddLapData(new LapData(0, 0)
                {
                    PitStatus = PitStatus.Pitting
                });

                monitoredDriver.Should().Raise(nameof(Driver.Pitting));
            }
        }

        [Fact]
        public void Pitting_Should_Not_Raise_When_Initially_Set_And_Driver_Starts_From_Garage()
        {
            var driver = new Driver(new F1Telemetry.Core.TelemetryManager());

            using (var monitoredDriver = driver.Monitor())
            {
                driver.AddLapData(new LapData(0, 0)
                {
                    PitStatus = PitStatus.Pitting
                });

                monitoredDriver.Should().NotRaise(nameof(Driver.Pitting));
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void NewLap_Should_Not_Raise_When_First_Initialised(byte lapNumber)
        {
            var driver = new Driver(new F1Telemetry.Core.TelemetryManager());

            using (var monitoredDriver = driver.Monitor())
            {
                driver.AddLapData(new LapData(0, 0)
                {
                    CurrentLapNum = lapNumber
                });

                monitoredDriver.Should().NotRaise(nameof(Driver.NewLap), because: "It is not considered as a new lap on first assignment.");
            }
        }

        [Fact]
        public void NewLap_Should_Raise_When_Lap_Number_Is_Increases()
        {
            var driver = new Driver(new F1Telemetry.Core.TelemetryManager());

            using (var monitoredDriver = driver.Monitor())
            {
                driver.AddLapData(new LapData(0, 0)
                {
                    CurrentLapNum = 1
                });

                driver.AddLapData(new LapData(0, 0)
                {
                    CurrentLapNum = 2
                });

                monitoredDriver
                    .Should()
                    .Raise(nameof(Driver.NewLap), because: "It is a new lap.")
                    .WithSender(driver)
                    .WithArgs<NewLapEventArgs>(e => e.LapSummary.LapNumber.Equals(1));
            }
        }
    }
}