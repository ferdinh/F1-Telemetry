using F1Telemetry.WPF.Model;
using FluentAssertions;
using Xunit;

namespace F1Telemetry.Test.ModelTests
{
    public class TyreTemperatureTest
    {
        [Fact]
        public void Set_Current_Should_SetMaxTemperatureAutomatically()
        {
            var tyreTemperature = new TyreTemperature();

            // Ascending temperature
            tyreTemperature.Current = 10;
            tyreTemperature.Max.Should().Be(10);

            tyreTemperature.Current = 15; 
            tyreTemperature.Max.Should().Be(15);

            tyreTemperature.Current = 20;
            tyreTemperature.Max.Should().Be(20);

            // Lower temperature than current max.
            tyreTemperature.Current = 10;
            tyreTemperature.Max.Should().Be(20);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(30)]
        [InlineData(40)]
        [InlineData(50)]
        public void Set_Current_Should_SetToCurrentAssignedTemperature(byte tempToSet)
        {
            var tyreTemperature = new TyreTemperature();

            tyreTemperature.Current = tempToSet;
            tyreTemperature.Max.Should().Be(tempToSet);
        }

        [Fact]
        public void Set_Current_Should_SetMinTemperatureAutomatically()
        {
            var tyreTemperature = new TyreTemperature();

            // Ascending temperature
            tyreTemperature.Current = 10;
            tyreTemperature.Min.Should().Be(10);

            tyreTemperature.Current = 15;
            tyreTemperature.Min.Should().Be(10);

            tyreTemperature.Current = 20;
            tyreTemperature.Min.Should().Be(10);

            // Lower temperature than current min.
            tyreTemperature.Current = 5;
            tyreTemperature.Min.Should().Be(5);

            // Higher temperature set
            tyreTemperature.Current = 20;
            tyreTemperature.Min.Should().Be(5);
        }
    }
}