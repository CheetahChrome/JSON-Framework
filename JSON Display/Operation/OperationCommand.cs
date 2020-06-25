using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace JSON_Display.Operation
{
    public class OperationCommand : ICommand
    {

        #region Variables

        private Func<object, bool> CanExecuteHandler { get; set; }
        private Action<object> ExecuteActionHandler { get; set; }

        public bool InSeparateThread { get; set; }
        #endregion

        #region Properties

        #endregion

        #region Construction/Initialization

        public OperationCommand(Action<object> executeAction, bool inSeparateThread = false)
        {
            InSeparateThread = inSeparateThread;
            ExecuteActionHandler = executeAction ?? throw new ArgumentNullException(nameof(executeAction));
        }

        public OperationCommand(Action<object> executeAction, Func<object, bool> canExecute) : this(executeAction)
        {
            CanExecuteHandler = canExecute;
        }

        // Here to adhere to ICommand, change to below if needed
        //public event EventHandler CanExecuteChanged;
        event EventHandler ICommand.CanExecuteChanged
        {
            add { }
            remove { }
        }

        #endregion

        #region Methods

        public bool CanExecute(object parameter) => CanExecuteHandler?.Invoke(parameter) ?? true;

        //        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, new EventArgs());

        public void Execute(object parameter)
        {
            ExecuteActionHandler?.Invoke(parameter);
        }

        #endregion
    }
}

