using F1Telemetry.WPF.Converter;
using FluentAssertions;
using System.Globalization;
using Xunit;

namespace F1Telemetry.Test.ConverterTests
{
    public class ToPercentageConverterTest
    {
        [Theory]
        [InlineData(1, "100.00%")]
        [InlineData(0.236, "23.60%")]
        [InlineData(0.5, "50.00%")]
        [InlineData(0.5368, "53.68%")]
        [InlineData(0.53684, "53.68%")]
        [InlineData(0.53685, "53.69%")]
        public void Convert_Should_Return_StringInPercentage_From_FloatingNumber(double value, string expected)
        {
            // Arrange
            var converter = new ToPercentageConverter();

            // Act
            var actual = converter.Convert(value, typeof(ToPercentageConverterTest), "", CultureInfo.InvariantCulture);

            // Assert
            actual.Should().Be(expected);
        }
    }
}