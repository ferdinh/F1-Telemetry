using F1Telemetry.WPF.Model;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace F1Telemetry.Test.ModelTests
{
    public class TyreTemperatureTest
    {
        [Theory]
        [InlineData(new byte[] { 10, 20, 30, 40, 50 })]
        [InlineData(new byte[] { 10, 5, 3, 86, 12 })]
        [InlineData(new byte[] { 50, 40, 40, 40, 20 })]
        public void Update_Should_SetMaxTemperatureAutomatically(byte[] temperatures)
        {
            var tyreTemperature = new TyreTemperature();
            var maxExpectedTemperature = temperatures.Max();

            // Update the temperatures
            foreach (var temp in temperatures)
            {
                tyreTemperature.Update(temp);
            }

            tyreTemperature.Max.Should().Be(maxExpectedTemperature);
        }

        [Theory]
        [InlineData(new byte[] { 10, 20, 30, 40, 50 })]
        [InlineData(new byte[] { 10, 5, 3, 86, 12 })]
        [InlineData(new byte[] { 50, 40, 40, 40, 20 })]
        public void Update_Should_SetMinTemperatureAutomatically(byte[] temperatures)
        {
            var tyreTemperature = new TyreTemperature();
            var minExpectedTemperature = temperatures.Min();

            // Update the temperatures
            foreach (var temp in temperatures)
            {
                tyreTemperature.Update(temp);
            }

            tyreTemperature.Min.Should().Be(minExpectedTemperature);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(30)]
        [InlineData(40)]
        [InlineData(50)]
        public void Update_Should_Update_The_Current_Temperature(byte temperature)
        {
            var tyreTemperature = new TyreTemperature();

            tyreTemperature.Update(temperature);
            tyreTemperature.Current.Should().Be(temperature);
        }
    }
}