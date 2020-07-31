using System;
using System.Windows.Input;

namespace F1Telemetry.WPF.Command
{
    public abstract class BaseCommand : ICommand
    {
        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);
    }

    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action<object> executeAction) : base(executeAction)
        {
        }

        public RelayCommand(Action<object> executeAction, Predicate<object> canExecute) : base(executeAction, canExecute)
        {
        }
    }

    public class RelayCommand<T> : BaseCommand
    {
        private readonly Predicate<T> CanExecutePredicate;
        private readonly Action<T> ExecuteAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class. The CanExecute will always return <see langword="true" />.
        /// </summary>
        /// <param name="executeAction">The execute action.</param>
        public RelayCommand(Action<T> executeAction) : this(executeAction, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="executeAction">The action to perform when Execute is called.</param>
        /// <param name="canExecute">The logic that determines if the command can execute.</param>
        public RelayCommand(Action<T> executeAction, Predicate<T> canExecute)
        {
            if (executeAction == null)
            {
                throw new ArgumentNullException(nameof(executeAction));
            }

            ExecuteAction = executeAction;
            CanExecutePredicate = canExecute;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
        /// <returns>
        ///   <see langword="true" /> if this command can be executed; otherwise, <see langword="false" />.
        /// </returns>
        public override bool CanExecute(object parameter)
        {
            return CanExecutePredicate == null ? true : CanExecutePredicate((T)parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
        public override void Execute(object parameter)
        {
            ExecuteAction?.Invoke((T)parameter);
        }
    }
}