using System;
using System.Windows.Data;
using F0.Tests.Shared;
using F0.Windows.Data;
using Xunit;

namespace F0.Tests.Windows.Data
{
	public class LogicalNegationConverterTests
	{
		[Fact]
		public void ConvertFromFalseToTrue()
		{
			IValueConverter converter = new LogicalNegationConverter();
			bool boolean = false;

			object fromSourceToTarget = converter.Convert(boolean, default, default, default);
			object fromTargetToSource = converter.ConvertBack(boolean, default, default, default);

			bool convert = Assert.IsType<bool>(fromSourceToTarget);
			Assert.True(convert);
			bool convertBack = Assert.IsType<bool>(fromTargetToSource);
			Assert.True(convertBack);
		}

		[Fact]
		public void ConvertFromTrueToFalse()
		{
			IValueConverter converter = new LogicalNegationConverter();
			bool boolean = true;

			object fromSourceToTarget = converter.Convert(boolean, default, default, default);
			object fromTargetToSource = converter.ConvertBack(boolean, default, default, default);

			bool convert = Assert.IsType<bool>(fromSourceToTarget);
			Assert.False(convert);
			bool convertBack = Assert.IsType<bool>(fromTargetToSource);
			Assert.False(convertBack);
		}

		[Fact]
		public void ConvertFromFalseToTrue_NullableBoolean()
		{
			IValueConverter converter = new LogicalNegationConverter();
			bool? nullable = false;

			object fromSourceToTarget = converter.Convert(nullable, default, default, default);
			object fromTargetToSource = converter.ConvertBack(nullable, default, default, default);

			bool convert = Assert.IsType<bool>(fromSourceToTarget);
			Assert.True(convert);
			bool convertBack = Assert.IsType<bool>(fromTargetToSource);
			Assert.True(convertBack);
		}

		[Fact]
		public void ConvertFromTrueToFalse_NullableBoolean()
		{
			IValueConverter converter = new LogicalNegationConverter();
			bool? nullable = true;

			object fromSourceToTarget = converter.Convert(nullable, default, default, default);
			object fromTargetToSource = converter.ConvertBack(nullable, default, default, default);

			bool convert = Assert.IsType<bool>(fromSourceToTarget);
			Assert.False(convert);
			bool convertBack = Assert.IsType<bool>(fromTargetToSource);
			Assert.False(convertBack);
		}

		[Fact]
		public void ConvertNull_ReturnsNull()
		{
			IValueConverter converter = new LogicalNegationConverter();
			object? value = null;

			object fromSourceToTarget = converter.Convert(value, default, default, default);
			object fromTargetToSource = converter.ConvertBack(value, default, default, default);

			Assert.Null(fromSourceToTarget);
			Assert.Null(fromTargetToSource);
		}

		[Fact]
		public void ConvertNull_ReturnsNull_NullableBoolean()
		{
			IValueConverter converter = new LogicalNegationConverter();
			bool? nullable = null;

			object fromSourceToTarget = converter.Convert(nullable, default, default, default);
			object fromTargetToSource = converter.ConvertBack(nullable, default, default, default);

			Assert.Null(fromSourceToTarget);
			Assert.Null(fromTargetToSource);
		}

		[Fact]
		public void ConvertNotSupported_ThrowsException()
		{
			IValueConverter converter = new LogicalNegationConverter();
			string text = String.Empty;

			Exception fromSourceToTarget = Assert.Throws<NotSupportedException>(() => converter.Convert(text, default, default, default));
			Exception fromTargetToSource = Assert.Throws<NotSupportedException>(() => converter.ConvertBack(text, default, default, default));

			string message = TestTypeConverter.CreateConvertFromException(typeof(LogicalNegationConverter), text);
			Assert.Equal(message, fromSourceToTarget.Message);
			Assert.Equal(message, fromTargetToSource.Message);
		}
	}
}
