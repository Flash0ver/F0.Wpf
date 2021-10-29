using System;
using System.Diagnostics;

namespace F0.Tests.Shared
{
	internal sealed class TestTraceListener : TraceListener
	{
		internal TestTraceListener()
			: base(nameof(TestTraceListener))
		{
		}

		internal int DisposeCount { get; private set; }

		public override void Write(string? message)
		{
			throw new NotImplementedException();
		}

		public override void WriteLine(string? message)
		{
			throw new NotImplementedException();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			DisposeCount++;
		}
	}
}
