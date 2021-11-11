using System;

namespace F0.Diagnostics
{
	internal sealed class DataBindingException : Exception
	{
		public DataBindingException(string message)
			: base(message)
		{
		}
	}
}
