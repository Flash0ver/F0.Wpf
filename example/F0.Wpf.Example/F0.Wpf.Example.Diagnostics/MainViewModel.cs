﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using F0.ComponentModel;
using F0.Diagnostics;

namespace F0.Wpf.Example.Diagnostics
{
	internal class MainViewModel : ViewModel, IDisposable
	{
		public IReadOnlyList<BindingSource> Commands { get; }

		private object content;
		public object Content
		{
			get => content;
			private set => SetField(ref content, value);
		}

		private string log;
		public string Log
		{
			get => log;
			private set => SetField(ref log, value);
		}

		private readonly IDisposable traceScope;

		public MainViewModel()
		{
			traceScope = DataBindingTraceListener.BeginScope(OnTraceEvent);

			Commands = new ObservableCollection<BindingSource>()
			{
				new BindingSource("Path and Property name match", new Command(() => Content = new View(new { Property = "Value" }))),
				new BindingSource("DataContext not set", new Command(() => Content = new View())),
				new BindingSource("DataContext set to null", new Command(() => Content = new View(null))),
				new BindingSource("Path and Property name mismatch", new Command(() => Content = new View(new { Message = "Hello World!" })))
			};
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
