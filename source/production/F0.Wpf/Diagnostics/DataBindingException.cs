using System;

namespace F0.Diagnostics
{
	internal class DataBindingException : Exception
	{
		public DataBindingException(string message)
			: base(message)
		{
		}
	}
}
