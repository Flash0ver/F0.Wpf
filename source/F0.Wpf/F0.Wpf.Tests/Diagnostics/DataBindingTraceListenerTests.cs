using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using F0.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace F0.Tests.Diagnostics
{
	[TestClass]
	public class DataBindingTraceListenerTests
	{
		[TestMethod]
		public void HandlerMustNotBeNull()
		{
			Assert.ThrowsException<ArgumentNullException>(() => new DataBindingTraceListener(null));
		}

		[TestMethod]
		public void CreateWithName()
		{
			var traceListener = new DataBindingTraceListener(_ => { });
			Assert.AreEqual("DataBindingTraceListener", traceListener.Name);
		}

		[TestMethod]
		public void MessageIsBuiltOnFlushAndIsBasedOnDataPassedViaTraceEvent()
		{
			string actualMessage = null;
			TraceListener traceListener = new DataBindingTraceListener(m => actualMessage = m);

			string header = "Source Verbose: 1 : ";
			string expectedMessage = header + "Format-Argument0-Argument1" + Environment.NewLine;

			traceListener.TraceEvent(null, "Source", TraceEventType.Verbose, 1, "Format-{0}-{1}", "Argument0", "Argument1");

			Assert.IsNull(actualMessage);
			traceListener.Flush();
			Assert.AreEqual(expectedMessage, actualMessage);
		}

		[TestMethod]
		public void TraceEventOnSourceCallsTraceEventOfListener_WhichCallsWriteViaWriteHeaderAndWriteLineOfListener()
		{
			string actualMessage = null;
			using (DataBindingTraceListener.BeginScope(m => actualMessage = m))
			{
				string header = "System.Windows.Data Error: 2 : ";
				string expectedMessage = header + "Format" + Environment.NewLine;

				PresentationTraceSources.DataBindingSource.TraceEvent(TraceEventType.Error, 2, "Format", new object[0]);

				Assert.IsNull(actualMessage);
				PresentationTraceSources.DataBindingSource.Flush();
				Assert.AreEqual(expectedMessage, actualMessage);
			}
		}

		[TestMethod]
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
				Assert.IsNull(PresentationTraceSources.DataBindingSource.Listeners[nameof(DataBindingTraceListener)]);
			}

			void CheckThatSourceContainsTheListener()
			{
				Assert.IsInstanceOfType(PresentationTraceSources.DataBindingSource.Listeners[nameof(DataBindingTraceListener)], typeof(DataBindingTraceListener));
			}

			void Handle(string message)
			{
				//no-op
			}
		}

		[TestMethod]
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
				Assert.AreNotEqual(SourceLevels.Error, PresentationTraceSources.DataBindingSource.Switch.Level);
			}

			void CheckThatLevelIsAdjusted()
			{
				Assert.AreEqual(SourceLevels.Error, PresentationTraceSources.DataBindingSource.Switch.Level);
			}

			void Handle(string message)
			{
				//no-op
			}
		}

		[TestMethod]
		[DoNotParallelize]
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

				Assert.AreEqual("240", textBlock.Text);
				textBlock.SetBinding(TextBlock.TextProperty, validBinding);
				Assert.AreEqual("DataContext.DataContextProperty", textBlock.Text);

				string expectedMessage = GetMessage("PropertyNotFound", dataContext, textBlock, TextBlock.TextProperty);

				Exception exception = Assert.ThrowsException<DataBindingException>(() => textBlock.SetBinding(TextBlock.TextProperty, invalidBinding));
				Assert.AreEqual(expectedMessage, exception.Message);
				Assert.AreNotEqual("DataContext.DataContextProperty", textBlock.Text);
			}
		}

		[TestMethod]
		[DoNotParallelize]
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

				Assert.AreEqual("240", textBlock.Text);
				textBlock.SetBinding(TextBlock.TextProperty, validBinding);
				Assert.AreEqual("BindingSource.BindingSourceProperty", textBlock.Text);

				string expectedMessage = GetMessage("PropertyNotFound", bindingSource, textBlock, TextBlock.TextProperty);

				Exception exception = Assert.ThrowsException<DataBindingException>(() => textBlock.SetBinding(TextBlock.TextProperty, invalidBinding));
				Assert.AreEqual(expectedMessage, exception.Message);
				Assert.AreNotEqual("BindingSource.BindingSourceProperty", textBlock.Text);
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
