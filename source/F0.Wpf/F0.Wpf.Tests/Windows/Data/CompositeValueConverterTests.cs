using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Markup;
using F0.Windows.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace F0.Tests.Windows.Data
{
	[TestClass]
	public class CompositeValueConverterTests
	{
		[TestMethod]
		public void CompositeConverter_HasCollectionOfValueConverters_AsXamlContentProperty()
		{
			Type type = typeof(CompositeValueConverter);

			ContentPropertyAttribute attribute = type.GetCustomAttribute<ContentPropertyAttribute>();
			Assert.IsNotNull(attribute);
			Assert.IsNotNull(attribute.Name);

			PropertyInfo contentProperty = type.GetProperty(attribute.Name);
			Assert.IsNotNull(contentProperty);

			TypeInfo typeInfo = contentProperty.PropertyType.GetTypeInfo();
			Assert.IsFalse(typeInfo.IsInterface);
			CollectionAssert.Contains(typeInfo.ImplementedInterfaces.ToArray(), typeof(System.Collections.IList));
			Assert.IsTrue(typeInfo.IsGenericType);
			CollectionAssert.AreEqual(new Type[] { typeof(IValueConverter) }, typeInfo.GenericTypeArguments);
		}

		[TestMethod]
		public void CompositeConverter_Create_EmptyContent()
		{
			var converter = new CompositeValueConverter();

			ContentPropertyAttribute attribute = converter.GetType().GetCustomAttribute<ContentPropertyAttribute>();
			Assert.AreEqual(attribute.Name, nameof(converter.Converters));

			Assert.IsNotNull(converter.Converters);
			Assert.AreEqual(0, converter.Converters.Count);
		}

		[TestMethod]
		public void CompositeConverter_Convert_InnerConvertersAreCombinedInOrder_AsPipelineChain()
		{
			IValueConverter converter = CreateComposite();
			object text = converter.Convert("initial", GetType(), 0, CultureInfo.InvariantCulture);

			string target = nameof(CompositeValueConverterTests);
			Assert.AreEqual($"initial-FIRST.{target}.0.{0x7F}-LAST.{target}.0.{0x7F}", text);
		}

		[TestMethod]
		public void CompositeConverter_ConvertBack_InnerConvertersAreCombinedInInverseOrder_AsPipelineChain()
		{
			IValueConverter converter = CreateComposite();
			object text = converter.ConvertBack("initial", GetType(), 1, CultureInfo.InvariantCulture);

			string target = nameof(CompositeValueConverterTests);
			Assert.AreEqual($"initial+LAST.{target}.1.{0x7F}+FIRST.{target}.1.{0x7F}", text);
		}

		private static IValueConverter CreateComposite()
		{
			var composite = new CompositeValueConverter();
			composite.Converters.Add(new ValueConverter("FIRST"));
			composite.Converters.Add(new ValueConverter("LAST"));

			return composite;
		}
	}

	internal class ValueConverter : IValueConverter
	{
		internal string Text { get; }

		internal ValueConverter(string text)
		{
			Text = text;
		}

		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = value as string;
			return text + $"-{Text}.{targetType.Name}.{parameter}.{culture.LCID}";
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = value as string;
			return text + $"+{Text}.{targetType.Name}.{parameter}.{culture.LCID}";
		}
	}
}
