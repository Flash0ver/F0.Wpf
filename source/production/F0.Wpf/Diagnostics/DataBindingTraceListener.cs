using System;
using System.Diagnostics;
using System.Text;
using F0.Primitives;

namespace F0.Diagnostics
{
	public sealed class DataBindingTraceListener : TraceListener
	{
		public static IDisposable BeginScope()
		{
			return BeginScope(new DataBindingTraceListener(m => throw new DataBindingException(m)));
		}

		public static IDisposable BeginScope(Action<string> onFlush)
		{
			return BeginScope(new DataBindingTraceListener(onFlush));
		}

		private static IDisposable BeginScope(TraceListener traceListener)
		{
			if (ContainsNone())
			{
				PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;
			}

			_ = PresentationTraceSources.DataBindingSource.Listeners.Add(traceListener);
			return new Scope<TraceListener>(traceListener, EndScope);
		}

		private static void EndScope(TraceListener traceListener)
		{
			PresentationTraceSources.DataBindingSource.Listeners.Remove(traceListener);
			traceListener.Dispose();

			if (ContainsNone())
			{
				PresentationTraceSources.DataBindingSource.Switch.Level = initialLevel;
			}
		}

		private static bool ContainsNone()
		{
			return PresentationTraceSources.DataBindingSource.Listeners[nameof(DataBindingTraceListener)] is null;
		}

		static DataBindingTraceListener()
		{
			PresentationTraceSources.Refresh();
			initialLevel = PresentationTraceSources.DataBindingSource.Switch.Level;
		}

		private static readonly SourceLevels initialLevel;

		private readonly StringBuilder messageBuffer = new StringBuilder();
		private readonly Action<string> onFlush;

		internal DataBindingTraceListener(Action<string> onFlush)
			: base(nameof(DataBindingTraceListener))
		{
			this.onFlush = onFlush ?? throw new ArgumentNullException(nameof(onFlush));
		}

		public override void Write(string message)
		{
			_ = messageBuffer.Append(message);
		}

		public override void WriteLine(string message)
		{
			_ = messageBuffer.AppendLine(message);
		}

		public override void Flush()
		{
			base.Flush();

			string message = messageBuffer.ToString();
			_ = messageBuffer.Clear();

			onFlush(message);
		}
	}
}
