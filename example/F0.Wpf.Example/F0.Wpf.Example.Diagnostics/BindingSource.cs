using System.Windows.Input;

namespace F0.Wpf.Example.Diagnostics
{
	internal class BindingSource
	{
		public string Caption { get; }
		public ICommand Command { get; }

		internal BindingSource(string caption, ICommand command)
		{
			Caption = caption;
			Command = command;
		}
	}
}
