using System;

namespace F0.Primitives
{
	internal sealed class Scope : IDisposable
	{
		private readonly Action onDispose;

		public Scope(Action onDispose)
		{
			this.onDispose = onDispose ?? throw new ArgumentNullException(nameof(onDispose));
		}

		void IDisposable.Dispose()
		{
			onDispose();
		}
	}

	internal sealed class Scope<TState> : IDisposable
	{
		private readonly TState state;
		private readonly Action<TState> onDispose;

		public Scope(TState state, Action<TState> onDispose)
		{
			if (state == null)
			{
				throw new ArgumentNullException(nameof(state));
			}

			this.state = state;
			this.onDispose = onDispose ?? throw new ArgumentNullException(nameof(onDispose));
		}

		void IDisposable.Dispose()
		{
			onDispose(state);
		}
	}
}
