using System;
using F0.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace F0.Tests.Primitives
{
	[TestClass]
	public class ScopeTests
	{
		[TestMethod]
		public void HandlerOfStatelessScopeMustNotBeNull()
		{
			Assert.ThrowsException<ArgumentNullException>(() => new Scope(null));
		}

		[TestMethod]
		public void HandlerOfStatefulScopeMustNotBeNull()
		{
			Assert.ThrowsException<ArgumentNullException>(() => new Scope<string>(String.Empty, null));
		}

		[TestMethod]
		public void StateOfStatefulScopeMustNotBeNull()
		{
			Assert.ThrowsException<ArgumentNullException>(() => new Scope<string>(null, _ => { }));
		}

		[TestMethod]
		public void StatelessScopeIsAnIDisposableThatEndsTheLogicalOperationScopeOnDispose()
		{
			bool isDisposed = false;
			IDisposable scope = new Scope(() => isDisposed = true);

			Assert.IsFalse(isDisposed);
			scope.Dispose();
			Assert.IsTrue(isDisposed);
		}

		[TestMethod]
		public void StatefulScopeIsAnIDisposableThatEndsTheLogicalOperationScopeOnDispose()
		{
			string state = "notDisposed";
			IDisposable scope = new Scope<string>("disposed", s => state = s);

			Assert.AreEqual("notDisposed", state);
			scope.Dispose();
			Assert.AreEqual("disposed", state);
		}
	}
}
