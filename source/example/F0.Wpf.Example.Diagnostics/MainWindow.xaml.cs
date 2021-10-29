using System;
using System.Diagnostics;
using System.Windows;

namespace F0.Wpf.Example.Diagnostics
{
	internal partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		protected override void OnClosed(EventArgs e)
		{
			Debug.Assert(DataContext is MainViewModel);
			(DataContext as IDisposable)!.Dispose();

			base.OnClosed(e);
		}
	}
}
