using F1Telemetry.WPF.ViewModels;
using FluentAssertions;
using Xunit;

namespace F1Telemetry.Test.ViewModelTest
{
    public class MainViewModelTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TopmostCommand_Should_Set_To_Given_Input(bool input)
        {
            var viewModel = new MainViewModel();

            viewModel.SetTopmostCommand.Execute(input);

            viewModel.IsTopmost.Should().Be(input, because: $"it is instructed to be set to {input}");
        }
    }
}