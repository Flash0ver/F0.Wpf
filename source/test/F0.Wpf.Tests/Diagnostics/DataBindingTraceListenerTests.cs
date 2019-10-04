﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using F0.Diagnostics;
using Xunit;

namespace F0.Tests.Diagnostics
{
	public class DataBindingTraceListenerTests
	{
		[Fact]
		public void HandlerMustNotBeNull()
		{
			Assert.Throws<ArgumentNullException>(() => new DataBindingTraceListener(null));
		}

		[Fact]
		public void CreateWithName()
		{
			var traceListener = new DataBindingTraceListener(_ => { });
			Assert.Equal("DataBindingTraceListener", traceListener.Name);
		}

		[Fact]
		public void MessageIsBuiltOnFlushAndIsBasedOnDataPassedViaTraceEvent()
		{
			string actualMessage = null;
			TraceListener traceListener = new DataBindingTraceListener(m => actualMessage = m);

			string header = "Source Verbose: 1 : ";
			string expectedMessage = header + "Format-Argument0-Argument1" + Environment.NewLine;

			traceListener.TraceEvent(null, "Source", TraceEventType.Verbose, 1, "Format-{0}-{1}", "Argument0", "Argument1");

			Assert.Null(actualMessage);
			traceListener.Flush();
			Assert.Equal(expectedMessage, actualMessage);
		}

		[Fact]
		public void TraceEventOnSourceCallsTraceEventOfListener_WhichCallsWriteViaWriteHeaderAndWriteLineOfListener()
		{
			string actualMessage = null;
			using (DataBindingTraceListener.BeginScope(m => actualMessage = m))
			{
				string header = "System.Windows.Data Error: 2 : ";
				string expectedMessage = header + "Format" + Environment.NewLine;

				PresentationTraceSources.DataBindingSource.TraceEvent(TraceEventType.Error, 2, "Format", new object[0]);

				Assert.Null(actualMessage);
				PresentationTraceSources.DataBindingSource.Flush();
				Assert.Equal(expectedMessage, actualMessage);
			}
		}

		[Fact]
		public void BeginningTheScopeAddsTheListenerToTheSource_EndingTheScopeRemovesTheListenerFromTheSource()
		{
			CheckThatSourceDoesNotContainTheListener();
			IDisposable defaultScope = DataBindingTraceListener.BeginScope();
			CheckThatSourceContainsTheListener();
			IDisposable customScope = DataBindingTraceListener.BeginScope(Handle);
			CheckThatSourceContainsTheListener();
			customScope.Dispose();
			CheckThatSourceContainsTheListener();
			defaultScope.Dispose();
			CheckThatSourceDoesNotContainTheListener();

			void CheckThatSourceDoesNotContainTheListener()
			{
				Assert.Null(PresentationTraceSources.DataBindingSource.Listeners[nameof(DataBindingTraceListener)]);
			}

			void CheckThatSourceContainsTheListener()
			{
				Assert.IsType<DataBindingTraceListener>(PresentationTraceSources.DataBindingSource.Listeners[nameof(DataBindingTraceListener)]);
			}

			void Handle(string message)
			{
				//no-op
			}
		}

		[Fact]
		public void AdjustSourceLevelsOfSourceSwitchWithinScope()
		{
			CheckThatLevelIsUnmodified();
			IDisposable customScope = DataBindingTraceListener.BeginScope(Handle);
			CheckThatLevelIsAdjusted();
			IDisposable defaultScope = DataBindingTraceListener.BeginScope();
			CheckThatLevelIsAdjusted();
			defaultScope.Dispose();
			CheckThatLevelIsAdjusted();
			customScope.Dispose();
			CheckThatLevelIsUnmodified();

			void CheckThatLevelIsUnmodified()
			{
				Assert.NotEqual(SourceLevels.Error, PresentationTraceSources.DataBindingSource.Switch.Level);
			}

			void CheckThatLevelIsAdjusted()
			{
				Assert.Equal(SourceLevels.Error, PresentationTraceSources.DataBindingSource.Switch.Level);
			}

			void Handle(string message)
			{
				//no-op
			}
		}

		[WpfFact]
		public void ThrowIfBindingPathOnDataContextOfElementNotFound()
		{
			using (DataBindingTraceListener.BeginScope())
			{
				EnsureThatFlushIsCalledOnTheListenersAfterEveryWrite();

				object dataContext = new DataContext();

				var textBlock = new TextBlock
				{
					Name = "MyTextBlock",
					Text = "240",
					DataContext = dataContext
				};

				var validBinding = new Binding(nameof(DataContext.DataContextProperty));
				var invalidBinding = new Binding("PropertyNotFound");

				Assert.Equal("240", textBlock.Text);
				textBlock.SetBinding(TextBlock.TextProperty, validBinding);
				Assert.Equal("DataContext.DataContextProperty", textBlock.Text);

				string expectedMessage = GetMessage("PropertyNotFound", dataContext, textBlock, TextBlock.TextProperty);

				Exception exception = Assert.Throws<DataBindingException>(() => textBlock.SetBinding(TextBlock.TextProperty, invalidBinding));
				Assert.Equal(expectedMessage, exception.Message);
				Assert.NotEqual("DataContext.DataContextProperty", textBlock.Text);
			}
		}

		[WpfFact]
		public void ThrowIfBindingPathOnSourceOfBindingNotFound()
		{
			using (DataBindingTraceListener.BeginScope())
			{
				EnsureThatFlushIsCalledOnTheListenersAfterEveryWrite();

				object bindingSource = new BindingSource();

				var textBlock = new TextBlock
				{
					Name = "MyTextBlock",
					Text = "240"
				};

				var validBinding = new Binding(nameof(BindingSource.BindingSourceProperty))
				{
					Source = bindingSource
				};
				var invalidBinding = new Binding("PropertyNotFound")
				{
					Source = bindingSource
				};

				Assert.Equal("240", textBlock.Text);
				textBlock.SetBinding(TextBlock.TextProperty, validBinding);
				Assert.Equal("BindingSource.BindingSourceProperty", textBlock.Text);

				string expectedMessage = GetMessage("PropertyNotFound", bindingSource, textBlock, TextBlock.TextProperty);

				Exception exception = Assert.Throws<DataBindingException>(() => textBlock.SetBinding(TextBlock.TextProperty, invalidBinding));
				Assert.Equal(expectedMessage, exception.Message);
				Assert.NotEqual("BindingSource.BindingSourceProperty", textBlock.Text);
			}
		}

		private static void EnsureThatFlushIsCalledOnTheListenersAfterEveryWrite()
		{
			if (!Trace.AutoFlush)
			{
				Trace.AutoFlush = true;
			}
		}

		private static string GetMessage(string path, object bindingSource, FrameworkElement targetElement, DependencyProperty targetProperty)
		{
			string source = "System.Windows.Data";
			TraceEventType eventType = TraceEventType.Error;
			int id = 40;
			string format = GetFormat(path, bindingSource, targetElement, targetProperty);

			string header = $"{source} {eventType}: {id} : ";
			string message = header + format + Environment.NewLine;

			return message;
		}

		private static string GetFormat(string path, object bindingSource, FrameworkElement targetElement, DependencyProperty targetProperty)
		{
			string sourceName = bindingSource.GetType().Name;
			string hashCode = bindingSource.GetHashCode().ToString();
			string targetName = targetElement.GetType().Name;

			return $"BindingExpression path error: '{path}' property not found on 'object' ''{sourceName}' (HashCode={hashCode})'." +
				$" BindingExpression:Path={path}; DataItem='{sourceName}' (HashCode={hashCode}); target element is '{targetName}' (Name='{targetElement.Name}'); target property is '{targetProperty.Name}' (type '{targetProperty.PropertyType.Name}')";
		}
	}
}