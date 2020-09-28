using F1Telemetry.Core.Data;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace F1Telemetry.Test.Core
{
    public class DriverTests
    {
        [Fact]
        public void Pitting_Should_Raise_When_Status_Changed_To_Pitting_From_Other_Status()
        {
            var driver = new Driver(new F1Telemetry.Core.TelemetryManager());

            using(var monitoredDriver = driver.Monitor())
            {
                driver.AddLapData(new LapData
                {
                    PitStatus = PitStatus.None
                });

                driver.AddLapData(new LapData
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
                driver.AddLapData(new LapData
                {
                    PitStatus = PitStatus.Pitting
                });

                monitoredDriver.Should().NotRaise(nameof(Driver.Pitting));
            }
        }
    }
}
