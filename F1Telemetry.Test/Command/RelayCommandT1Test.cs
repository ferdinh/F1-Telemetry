using F1Telemetry.WPF.Command;
using FluentAssertions;
using System;
using System.Windows.Input;
using Xunit;

namespace F1Telemetry.Test.Command
{
    public class RelayCommandT1Test
    {
        [Fact]
        public void Given_RelayCommand_T1_With_Action_Should_Run_TheAction_WhenCalled()
        {
            var message = "";
            var expected = "Hello";

            ICommand command = new RelayCommand<string>((m) => { message = m; });
            command.Execute(expected);

            message.Should().Be(expected);
        }

        [Fact]
        public void Given_RelayCommand_T1_With_Action_Null_Should_Throw_ArgumentExeption()
        {
            Action createCommand = () =>
            {
                ICommand command = new RelayCommand<string>(null);
            };

            createCommand.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Given_RelayCommand_T1_With_CanExecute_Should_Return_BasedOnPredicate()
        {
            ICommand command = new RelayCommand<string>((m) => { }, (m) => false);

            var actual = command.CanExecute(null);
            actual.Should().BeFalse();
        }

        [Fact]
        public void Given_RelayCommand_T1_With_CanExecute_Null_Should_Return_True()
        {
            ICommand command = new RelayCommand<string>((m) => { });

            var actual = command.CanExecute(null);

            actual.Should().BeTrue();
        }
    }
}