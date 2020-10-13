using F1Telemetry.WPF.Converter;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace F1Telemetry.Test.ConverterTests
{
    public class ToLapTimeStringTest
    {
        [Theory]
        [InlineData(10, "10.000")]
        [InlineData(60, "01:00.000")]
        [InlineData(70, "01:10.000")]
        [InlineData(155.364, "02:35.364")]
        [InlineData(112.12, "01:52.120")]
        public void Convert_Should_Return_To_String_LapTime_Format_From_Seconds(float seconds, string expected)
        {
            // Arrange
            var converter = new ToLapTimeString();

            // Act
            var actual = converter.Convert(seconds, typeof(ToLapTimeStringTest), "", CultureInfo.InvariantCulture);

            // Assert
            actual.Should().Be(expected);
        }
    }
}
