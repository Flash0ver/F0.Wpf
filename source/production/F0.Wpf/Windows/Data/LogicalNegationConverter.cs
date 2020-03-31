using System;
using System.Globalization;
using System.Windows.Data;

namespace F0.Windows.Data
{
	public sealed class LogicalNegationConverter : IValueConverter
	{
		public LogicalNegationConverter()
		{
		}

		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Negate(value);
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Negate(value);
		}

		private static object Negate(object value)
		{
			if (value is null)
			{
				return value;
			}
			else if (value is bool boolean)
			{
				return !boolean;
			}
			else
			{
				string message = $"{nameof(LogicalNegationConverter)} cannot convert from {value.GetType().FullName}.";
				throw new NotSupportedException(message);
			}
		}
	}
}
