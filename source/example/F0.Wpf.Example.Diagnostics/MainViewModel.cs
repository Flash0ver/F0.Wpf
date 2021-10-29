using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using F0.ComponentModel;
using F0.Diagnostics;
using F0.Windows.Input;

namespace F0.Wpf.Example.Diagnostics
{
	internal class MainViewModel : ViewModel, IDisposable
	{
		public IReadOnlyList<BindingSource> Commands { get; }

		private object content;
		public object Content
		{
			get => content;
			private set => SetProperty(ref content, value);
		}

		private string log;
		public string Log
		{
			get => log;
			private set => SetProperty(ref log, value);
		}

		private readonly IDisposable traceScope;

		public MainViewModel()
		{
			traceScope = DataBindingTraceListener.BeginScope(OnTraceEvent);

			Commands = new ObservableCollection<BindingSource>()
			{
				new BindingSource("Path and Property name match", Command.Create(() => Content = new View(new { Property = "Value" }))),
				new BindingSource("DataContext not set", Command.Create(() => Content = new View())),
				new BindingSource("DataContext set to null", Command.Create(() => Content = new View(null))),
				new BindingSource("Path and Property name mismatch", Command.Create(() => Content = new View(new { Message = "Hello World!" })))
			};

			content = null!;
			log = null!;
		}

		private void OnTraceEvent(string message)
		{
			Log += message;
		}

		void IDisposable.Dispose()
		{
			traceScope.Dispose();
		}
	}
}
