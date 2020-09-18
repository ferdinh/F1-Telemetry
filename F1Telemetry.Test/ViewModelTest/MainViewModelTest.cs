using F1Telemetry.WPF.ViewModels;
using FluentAssertions;
using ScottPlot;
using System;
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

        [Fact]
        public void ClearAllGraphCommand_Cannot_Execute_When_There_Is_No_Data()
        {
            var viewModel = new MainViewModel();

            var actual = viewModel.ClearAllGraphCommand.CanExecute(null);

            actual.Should().BeFalse();
        }

        [Fact]
        public void ClearAllGraphCommand_Cannot_Execute_When_There_Is_No_Checked_Lap_Data()
        {
            var viewModel = new MainViewModel();
            viewModel.LapSummaries.Add(new WPF.Model.LapSummaryModel
            {
                IsChecked = false
            });

            var actual = viewModel.ClearAllGraphCommand.CanExecute(null);

            actual.Should().BeFalse();
        }


        [Fact]
        public void ClearAllGraphCommand_Can_Execute_When_There_Is_Checked_Lap_Data()
        {
            var viewModel = new MainViewModel();
            viewModel.LapSummaries.Add(new WPF.Model.LapSummaryModel
            {
                IsChecked = true
            });

            var actual = viewModel.ClearAllGraphCommand.CanExecute(null);

            actual.Should().BeTrue();
        }

        [UIFact]
        public void ClearAllGraphCommand_Can_Execute_When_LiveTelemetryIsToggled()
        {
            var viewModel = new MainViewModel();

            viewModel.SpeedGraphPlot = new WpfPlot();
            viewModel.ThrottleGraphPlot = new WpfPlot();
            viewModel.BrakeGraphPlot = new WpfPlot();
            viewModel.GearGraphPlot = new WpfPlot();

            viewModel.EnableLiveTelemetryCommand.Execute(true);

            var actual = viewModel.ClearAllGraphCommand.CanExecute(null);

            actual.Should().BeTrue();
        }

        [UIFact]
        public void ClearAllGraphCommand_Can_Execute_When_LiveTelemetryIsToggledOn_Then_Off()
        {
            var viewModel = new MainViewModel();

            viewModel.SpeedGraphPlot = new WpfPlot();
            viewModel.ThrottleGraphPlot = new WpfPlot();
            viewModel.BrakeGraphPlot = new WpfPlot();
            viewModel.GearGraphPlot = new WpfPlot();

            viewModel.EnableLiveTelemetryCommand.Execute(true);

            viewModel.EnableLiveTelemetryCommand.Execute(false);

            var actual = viewModel.ClearAllGraphCommand.CanExecute(null);

            actual.Should().BeTrue();
        }

    }
}