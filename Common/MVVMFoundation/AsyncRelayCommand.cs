using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Common.MVVMFoundation
{
    public class AsyncRelayCommand : ICommand
    {
        #region Fields

        readonly Func<Task> _execute;
        bool _isExecuting;
        readonly Action<Exception> _onException;

        #endregion

        #region Properites

        public bool IsExecuting
        {
            get
            {
                return _isExecuting;
            }
            set
            {
                _isExecuting = value;
                CanExecuteChanged?.Invoke(this, new EventArgs()); // Will trigger "CanExecute" method
            }
        }

        #endregion

        #region Constructors

        public AsyncRelayCommand(Func<Task> execute, Action<Exception> onException)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _onException = onException;
        }

        #endregion

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return !IsExecuting;
        }

        public event EventHandler CanExecuteChanged;

        public async void Execute(object parameter)
        {
            // 防止异步重入
            IsExecuting = true;

            try
            {
                await _execute();

            }
            catch (Exception ex)
            {
                _onException?.Invoke(ex);
            }

            IsExecuting = false;
        }

        #endregion
    }

    public class AsyncRelayCommand<T> : ICommand
    {
        #region Fields

        readonly Func<T, Task> _execute;
        bool _isExecuting;
        readonly Action<Exception> _onException;

        #endregion

        #region Properites

        public bool IsExecuting
        {
            get
            {
                return _isExecuting;
            }
            set
            {
                _isExecuting = value;
                CanExecuteChanged?.Invoke(this, new EventArgs()); // Will trigger "CanExecute" method
            }
        }

        #endregion

        #region Constructors

        public AsyncRelayCommand(Func<T, Task> execute, Action<Exception> onException)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _onException = onException;
        }

        #endregion

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return !IsExecuting;
        }

        public event EventHandler CanExecuteChanged;

        public async void Execute(object parameter)
        {
            // 防止异步重入
            IsExecuting = true;

            try
            {
                await _execute((T)parameter);

            }
            catch (Exception ex)
            {
                _onException?.Invoke(ex);
            }

            IsExecuting = false;
        }

        #endregion
    }
}
