using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace F0.Windows.Data
{
	[ContentProperty(nameof(Converters))]
	public class CompositeValueConverter : IValueConverter
	{
		public Collection<IValueConverter> Converters { get; }

		public CompositeValueConverter()
		{
			Converters = new Collection<IValueConverter>();
		}

		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Converters.Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture));
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Converters.Reverse().Aggregate(value, (current, converter) => converter.ConvertBack(current, targetType, parameter, culture));
		}
	}
}
