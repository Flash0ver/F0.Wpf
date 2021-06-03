using System;
using System.ComponentModel;

namespace F0.Tests.Shared
{
	internal sealed class TestTypeConverter : TypeConverter
	{
		private static readonly TestTypeConverter typeConverter = new();

		internal static string CreateConvertFromException(Type converterType, object sourceValue)
		{
			string message = typeConverter.CaptureGetConvertFromException(sourceValue).Message;
			return message.Replace(typeConverter.GetType().Name, converterType.Name);
		}

		private TestTypeConverter()
		{
		}

		internal Exception CaptureGetConvertFromException(object value)
		{
			try
			{
				_ = GetConvertFromException(value);
			}
			catch (Exception ex)
			{
				return ex;
			}

			throw new InvalidOperationException("'ConvertFromException' was not thrown.");
		}
	}
}
