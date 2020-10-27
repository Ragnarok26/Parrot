using System;
using System.Windows.Input;

namespace Parrot.ViewModel.Common
{
    /// <summary>
    /// Class for create and execute Commands (ICommand)
    /// </summary>
    public class RelayCommand<T> : ICommand
    {

        readonly Action _execute;

        readonly Func<bool> _canExecute;

        /// <summary>
        /// Action to execute
        /// </summary>
        private Action<T> execute;

        /// <summary>
        /// Predicate for know if Action can execute
        /// </summary>
        private Predicate<T> canExecute;

        /// <summary>
        /// Event handler Property for Can Execute Predicate
        /// </summary>
        private event EventHandler CanExecuteChangedInternal;

        /// <summary>
        /// Contructor of Command
        /// </summary>
        /// <param name="execute">Action to execute</param>
        public RelayCommand(Action<T> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        /// <summary>
        /// Constructor of Command
        /// </summary>
        /// <param name="execute">Action to execute</param>
        /// <param name="canExecute">Predicate for know if Action can execute</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException("execute");
            this.canExecute = canExecute ?? throw new ArgumentNullException("canExecute");
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }


        /// <summary>
        /// Event handler for Can Execute Predicate
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                CanExecuteChangedInternal += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                CanExecuteChangedInternal -= value;
            }
        }

        /// <summary>
        /// Method to know if Action can execute
        /// </summary>
        /// <param name="parameter">Arguments for send to Action</param>
        /// <returns>Return <strong>True</strong> if Action can execute, otherwise <strong>False</strong></returns>
        public bool CanExecute(object parameter)
        {
            return canExecute != null && canExecute((T)parameter);
        }

        /// <summary>
        /// Methot that executes the Action
        /// </summary>
        /// <param name="parameter">Arguments for send to Action</param>
        public void Execute(object parameter)
        {
            execute((T)parameter);
        }

        /// <summary>
        /// Method that fires CanExecuteChanged Event
        /// </summary>
        public void OnCanExecuteChanged()
        {
            EventHandler handler = CanExecuteChangedInternal;
            if (handler != null)
            {
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Destroy the instance
        /// </summary>
        public void Destroy()
        {
            canExecute = _ => false;
            execute = _ => { return; };
        }

        /// <summary>
        /// Default method to know if Action can execute
        /// </summary>
        /// <param name="parameter">Arguments for send to Action</param>
        /// <returns>Always return <strong>True</strong></returns>
        private static bool DefaultCanExecute(T parameter)
        {
            return true;
        }
    }
}
