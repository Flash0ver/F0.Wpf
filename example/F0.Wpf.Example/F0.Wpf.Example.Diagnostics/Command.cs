using System;
using System.Windows.Input;

namespace F0.Wpf.Example.Diagnostics
{
	internal class Command : ICommand
	{
		public event EventHandler CanExecuteChanged;

		private readonly Action execute;

		internal Command(Action execute)
		{
			this.execute = execute;
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			execute();
		}
	}
}
