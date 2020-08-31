using F1Telemetry.Core.Util.Extensions;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace F1Telemetry.Test.ExtensionTests
{
    public class EnumExtensionsTests
    {
        private enum TestEnum
        {
            [Display(Name = "With Display Name")]
            WithDisplayAttrName,

            WithoutDisplayAttrName
        }

        [Fact]
        public void GetDisplayName_Given_Enum_With_DisplayAttribute_Should_Return_DisplayName()
        {
            // Arrange
            var expected = "With Display Name";

            // Act
            var actual = TestEnum.WithDisplayAttrName.GetDisplayName();

            // Arrange
            actual.Should().Be(expected);
        }

        [Fact]
        public void GetDisplayName_Given_Enum_Without_DisplayAttribute_Should_Return_EnumToString()
        {
            // Arrange
            var expected = "WithoutDisplayAttrName";

            // Act
            var actual = TestEnum.WithoutDisplayAttrName.GetDisplayName();

            // Arrange
            actual.Should().Be(expected);
        }
    }
}