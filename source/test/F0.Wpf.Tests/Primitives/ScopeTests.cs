using System;
using F0.Primitives;
using Xunit;

namespace F0.Tests.Primitives
{
	public class ScopeTests
	{
		[Fact]
		public void HandlerOfStatelessScopeMustNotBeNull()
		{
			Assert.Throws<ArgumentNullException>("onDispose", () => new Scope(null));
		}

		[Fact]
		public void HandlerOfStatefulScopeMustNotBeNull()
		{
			Assert.Throws<ArgumentNullException>("onDispose", () => new Scope<string>(String.Empty, null));
		}

		[Fact]
		public void StateOfStatefulScopeMustNotBeNull()
		{
			Assert.Throws<ArgumentNullException>("state", () => new Scope<string>(null, _ => { }));
		}

		[Fact]
		public void StatelessScopeIsAnIDisposableThatEndsTheLogicalOperationScopeOnDispose()
		{
			bool isDisposed = false;
			IDisposable scope = new Scope(() => isDisposed = true);

			Assert.False(isDisposed);
			scope.Dispose();
			Assert.True(isDisposed);
		}

		[Fact]
		public void StatefulScopeIsAnIDisposableThatEndsTheLogicalOperationScopeOnDispose()
		{
			string state = "notDisposed";
			IDisposable scope = new Scope<string>("disposed", s => state = s);

			Assert.Equal("notDisposed", state);
			scope.Dispose();
			Assert.Equal("disposed", state);
		}
	}
}
