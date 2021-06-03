using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Markup;
using F0.Tests.Shared;
using F0.Windows.Data;
using Xunit;

namespace F0.Tests.Windows.Data
{
	public class InverseValueConverterTests
	{
		[Fact]
		public void InverseConverter_HasValueConverter_AsXamlContentProperty()
		{
			Type type = typeof(InverseValueConverter);

			ContentPropertyAttribute attribute = type.GetCustomAttribute<ContentPropertyAttribute>();
			Assert.NotNull(attribute);
			Assert.NotNull(attribute.Name);

			PropertyInfo contentProperty = type.GetProperty(attribute.Name);
			Assert.NotNull(contentProperty);
			Assert.NotNull(contentProperty.GetGetMethod());
			Assert.NotNull(contentProperty.GetSetMethod());

			TypeInfo typeInfo = contentProperty.PropertyType.GetTypeInfo();
			Assert.Equal(typeof(IValueConverter), typeInfo);
		}

		[Fact]
		public void InverseConverter_CreateDefault_ContentIsNull()
		{
			InverseValueConverter converter = new();

			ContentPropertyAttribute attribute = converter.GetType().GetCustomAttribute<ContentPropertyAttribute>();
			Assert.Equal(nameof(converter.Converter), attribute.Name);

			Assert.Null(converter.Converter);
		}

		[Fact]
		public void InverseConverter_CreateDefault_Throws()
		{
			IValueConverter converter = new InverseValueConverter();

			Exception convertException = Assert.Throws<InvalidOperationException>(() => converter.Convert("value", GetType(), default, CultureInfo.InvariantCulture));
			Assert.Equal($"The content property 'Converter' is not set to an instance of System.Windows.Data.IValueConverter.", convertException.Message);

			Exception convertBackException = Assert.Throws<InvalidOperationException>(() => converter.ConvertBack("value", GetType(), default, CultureInfo.InvariantCulture));
			Assert.Equal($"The content property 'Converter' is not set to an instance of System.Windows.Data.IValueConverter.", convertBackException.Message);
		}

		[Fact]
		public void InverseConverter_Convert_CallsToConvertBackFromInnerConverter()
		{
			IValueConverter converter = CreateInverseConverter();
			object text = converter.Convert("initial", GetType(), 0, CultureInfo.InvariantCulture);

			string target = nameof(InverseValueConverterTests);
			Assert.Equal($"ConvertBack(initial, {target}, 0, {0x7F})", text);
		}

		[Fact]
		public void InverseConverter_ConvertBack_CallsToConvertFromInnerConverter()
		{
			IValueConverter converter = CreateInverseConverter();
			object text = converter.ConvertBack("initial", GetType(), 1, CultureInfo.InvariantCulture);

			string target = nameof(InverseValueConverterTests);
			Assert.Equal($"Convert(initial, {target}, 1, {0x7F})", text);
		}

		private static IValueConverter CreateInverseConverter()
		{
			InverseValueConverter inverse = new()
			{
				Converter = new TestValueConverter()
			};

			return inverse;
		}
	}
}
