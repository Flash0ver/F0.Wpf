using System;
using System.Globalization;
using System.Windows.Data;

namespace F0.Tests.Shared
{
	internal sealed class TestValueConverter : IValueConverter
	{
		internal string Text { get; }

		public TestValueConverter()
		{
		}

		public TestValueConverter(string text)
		{
			Text = text;
		}

		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = Text is null
				? $"Convert({value}, {targetType.Name}, {parameter}, {culture.LCID})"
				: $"{value}-{Text}.{targetType.Name}.{parameter}.{culture.LCID}";

			return result;
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = Text is null
				? $"ConvertBack({value}, {targetType.Name}, {parameter}, {culture.LCID})"
				: $"{value}+{Text}.{targetType.Name}.{parameter}.{culture.LCID}";

			return result;
		}
	}
}
