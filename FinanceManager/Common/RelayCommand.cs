using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FinanceManager.Common
{
    public class RelayCommand : ICommand
    {
        private readonly Action _Execute;
        private readonly Func<bool> _CanExecute;

        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action inExecute) : this(inExecute, null)
        {
        }

        public RelayCommand(Action inExecute, Func<bool> inCanExecute)
        {
            _Execute = inExecute ?? throw new ArgumentNullException("execute");
            _CanExecute = inCanExecute;
        }

        public bool CanExecute(object inParameter)
        {
            return _CanExecute == null ? true : _CanExecute();
        }

        public void Execute(object inParameter)
        {
            _Execute();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
