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
	public class CompositeValueConverterTests
	{
		[Fact]
		public void CompositeConverter_HasCollectionOfValueConverters_AsXamlContentProperty()
		{
			Type type = typeof(CompositeValueConverter);

			ContentPropertyAttribute attribute = type.GetCustomAttribute<ContentPropertyAttribute>();
			Assert.NotNull(attribute);
			Assert.NotNull(attribute.Name);

			PropertyInfo contentProperty = type.GetProperty(attribute.Name);
			Assert.NotNull(contentProperty);
			Assert.NotNull(contentProperty.GetGetMethod());
			Assert.Null(contentProperty.GetSetMethod());

			TypeInfo typeInfo = contentProperty.PropertyType.GetTypeInfo();
			Assert.False(typeInfo.IsInterface);
			Assert.Contains(typeof(System.Collections.IList), typeInfo.ImplementedInterfaces);
			Assert.True(typeInfo.IsGenericType);
			Assert.Equal(new Type[] { typeof(IValueConverter) }, typeInfo.GenericTypeArguments);
		}

		[Fact]
		public void CompositeConverter_CreateDefault_ContentIsEmpty()
		{
			var converter = new CompositeValueConverter();

			ContentPropertyAttribute attribute = converter.GetType().GetCustomAttribute<ContentPropertyAttribute>();
			Assert.Equal(nameof(converter.Converters), attribute.Name);

			Assert.NotNull(converter.Converters);
			Assert.Empty(converter.Converters);
		}

		[Fact]
		public void CompositeConverter_CreateDefault_PassThrough()
		{
			IValueConverter converter = new CompositeValueConverter();

			object convertValue = converter.Convert(nameof(IValueConverter.Convert), GetType(), default, CultureInfo.InvariantCulture);
			Assert.Equal("Convert", convertValue);

			object convertBackValue = converter.ConvertBack(nameof(IValueConverter.ConvertBack), GetType(), default, CultureInfo.InvariantCulture);
			Assert.Equal("ConvertBack", convertBackValue);
		}

		[Fact]
		public void CompositeConverter_Convert_InnerConvertersAreCombinedInOrder_AsPipelineChain()
		{
			IValueConverter converter = CreateCompositeConverter();
			object text = converter.Convert("initial", GetType(), 0, CultureInfo.InvariantCulture);

			string target = nameof(CompositeValueConverterTests);
			Assert.Equal($"initial-FIRST.{target}.0.{0x7F}-LAST.{target}.0.{0x7F}", text);
		}

		[Fact]
		public void CompositeConverter_ConvertBack_InnerConvertersAreCombinedInInverseOrder_AsPipelineChain()
		{
			IValueConverter converter = CreateCompositeConverter();
			object text = converter.ConvertBack("initial", GetType(), 1, CultureInfo.InvariantCulture);

			string target = nameof(CompositeValueConverterTests);
			Assert.Equal($"initial+LAST.{target}.1.{0x7F}+FIRST.{target}.1.{0x7F}", text);
		}

		private static IValueConverter CreateCompositeConverter()
		{
			var composite = new CompositeValueConverter();
			composite.Converters.Add(new TestValueConverter("FIRST"));
			composite.Converters.Add(new TestValueConverter("LAST"));

			return composite;
		}
	}
}
