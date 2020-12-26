using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace F0.Windows.Data
{
	[ContentProperty(nameof(Converter))]
	public sealed class InverseValueConverter : IValueConverter
	{
		public IValueConverter Converter { get; set; }

		public InverseValueConverter()
		{
		}

		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			ThrowIfContentIsNull();

			return Converter.ConvertBack(value, targetType, parameter, culture);
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			ThrowIfContentIsNull();

			return Converter.Convert(value, targetType, parameter, culture);
		}

		private void ThrowIfContentIsNull()
		{
			if (Converter is null)
			{
				string message = $"The content property '{nameof(Converter)}' is not set to an instance of {typeof(IValueConverter)}.";
				throw new InvalidOperationException(message);
			}
		}
	}
}
