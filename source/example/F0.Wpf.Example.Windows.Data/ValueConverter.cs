using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace F0.Wpf.Example.Windows.Data
{
	internal class ValueConverter : IValueConverter
	{
		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Binding.DoNothing;
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Binding.DoNothing;
		}
	}

	internal class FirstValueConverter : IValueConverter
	{
		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is string text)
			{
				return Int32.TryParse(text, out int integer) ? integer : Binding.DoNothing;
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int integer = (int)value;
			return integer % 2 == 0
				? $"{integer} is even!"
				: $"{integer} is odd!";
		}
	}

	internal class LastValueConverter : IValueConverter
	{
		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value is int integer
				? integer % 2 == 0
				: null;
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is double number)
			{
				return Convert.ToInt32(number);
			}
			else
			{
				throw new NotSupportedException();
			}
		}
	}

	internal class NameToInitialConverter : IValueConverter
	{
		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string name = value as string;
			string[] nameParts = name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			IEnumerable<char> initials = nameParts.Select(static n => n.First());
			string initial = String.Join("", initials);
			return initial;
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string initial = value as string;
			char[] initials = initial.ToCharArray();
			string name = String.Join(". ", initials) + ".";
			return name;
		}
	}
}
