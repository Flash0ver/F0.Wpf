﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows.Data;
using F0.Tests.Shared;
using F0.Windows.Data;
using Xunit;

namespace F0.Tests.Windows.Data
{
	public class NumericNegationConverterTests
	{
		[Theory]
		[MemberData(nameof(GetSignedIntegralNumericTestData))]
		[MemberData(nameof(GetFloatingPointNumericTestData))]
		[MemberData(nameof(GetNumericOneTestData))]
		[MemberData(nameof(GetNumericDefaultZeroTestData))]
		[MemberData(nameof(GetNumericEpsilonTestData))]
		[MemberData(nameof(GetNumericInfinityTestData))]
		[MemberData(nameof(GetNotANumberTestData))]
		public void Convert_NegatesTheValue(object value, object converted, Type expectedType)
		{
			IValueConverter converter = new NumericNegationConverter();

			object convert = converter.Convert(value, default, default, default);
			Assert.IsType(expectedType, convert);
			Assert.Equal(converted, convert);

			// Roundtrip
			object roundTripConvert = converter.Convert(convert, default, default, default);
			Assert.IsType(expectedType, roundTripConvert);
			Assert.Equal(value, roundTripConvert);
		}

		[Theory]
		[MemberData(nameof(GetSignedIntegralNumericTestData))]
		[MemberData(nameof(GetFloatingPointNumericTestData))]
		[MemberData(nameof(GetNumericOneTestData))]
		[MemberData(nameof(GetNumericDefaultZeroTestData))]
		[MemberData(nameof(GetNumericEpsilonTestData))]
		[MemberData(nameof(GetNumericInfinityTestData))]
		[MemberData(nameof(GetNotANumberTestData))]
		public void ConvertBack_NegatesTheValue(object value, object converted, Type expectedType)
		{
			IValueConverter converter = new NumericNegationConverter();

			object convertBack = converter.ConvertBack(value, default, default, default);
			Assert.IsType(expectedType, convertBack);
			Assert.Equal(converted, convertBack);

			// Roundtrip
			object roundTripConvertBack = converter.ConvertBack(convertBack, default, default, default);
			Assert.IsType(expectedType, roundTripConvertBack);
			Assert.Equal(value, roundTripConvertBack);
		}

		[Fact]
		public void ConvertNull_ReturnsNull()
		{
			IValueConverter converter = new NumericNegationConverter();
			int? value = null;

			object convert = converter.Convert(value, default, default, default);
			object convertBack = converter.ConvertBack(value, default, default, default);

			Assert.Null(convert);
			Assert.Null(convertBack);
		}

		[Fact]
		public void ConvertNotSupported_ThrowsException()
		{
			IValueConverter converter = new NumericNegationConverter();
			string text = String.Empty;

			Exception fromSourceToTarget = Assert.Throws<NotSupportedException>(() => converter.Convert(text, default, default, default));
			Exception fromTargetToSource = Assert.Throws<NotSupportedException>(() => converter.ConvertBack(text, default, default, default));

			string message = TestTypeConverter.CreateConvertFromException(typeof(NumericNegationConverter), text);
			Assert.Equal(message, fromSourceToTarget.Message);
			Assert.Equal(message, fromTargetToSource.Message);
		}

		[Theory]
		[MemberData(nameof(GetUnsignedIntegralNumericTestData))]
		[MemberData(nameof(GetOverflowTestData))]
		public void ConvertOverflow_ThrowsException(object value)
		{
			IValueConverter converter = new NumericNegationConverter();

			Assert.Throws<OverflowException>(() => converter.Convert(value, default, default, default));
			Assert.Throws<OverflowException>(() => converter.ConvertBack(value, default, default, default));
		}

		public static IEnumerable<object[]> GetSignedIntegralNumericTestData()
		{
			return new object[][]
			{
				new object[] { SByte.MaxValue, (sbyte)-127, typeof(sbyte) },
				new object[] { Int16.MaxValue, (short)-32_767, typeof(short) },
				new object[] { Int32.MaxValue, -Int32.MaxValue, typeof(int) },
				new object[] { Int64.MaxValue, -Int64.MaxValue, typeof(long) },
				new object[] { new BigInteger(240), new BigInteger(-240), typeof(BigInteger) },
			};
		}

		public static IEnumerable<object[]> GetUnsignedIntegralNumericTestData()
		{
			return new object[][]
			{
				new object[] { Byte.MaxValue },
				new object[] { UInt16.MaxValue },
				new object[] { UInt32.MaxValue },
				new object[] { UInt64.MaxValue },
			};
		}

		public static IEnumerable<object[]> GetFloatingPointNumericTestData()
		{
			return new object[][]
			{
				new object[] { Single.MaxValue, Single.MinValue, typeof(float) },
				new object[] { Double.MaxValue, Double.MinValue, typeof(double) },
				new object[] { Decimal.MaxValue, Decimal.MinValue, typeof(decimal) },
			};
		}

		public static IEnumerable<object[]> GetOverflowTestData()
		{
			return new object[][]
			{
				new object[] { SByte.MinValue },
				new object[] { (byte)1 },
				new object[] { Int16.MinValue },
				new object[] { (ushort)1 },
				new object[] { Int32.MinValue },
				new object[] { 1u },
				new object[] { Int64.MinValue },
				new object[] { 1ul },
			};
		}

		public static IEnumerable<object[]> GetNumericOneTestData()
		{
			return new object[][]
			{
				new object[] { (sbyte)1, (sbyte)-1, typeof(sbyte) },
				new object[] { (short)1, (short)-1, typeof(short) },
				new object[] { 1, -1, typeof(int) },
				new object[] { 1L, -1L, typeof(long) },
				new object[] { 1.0f, -1.0f, typeof(float) },
				new object[] { 1.0, -1.0, typeof(double) },
				new object[] { Decimal.One, Decimal.MinusOne, typeof(decimal) },
				new object[] { BigInteger.One, BigInteger.MinusOne, typeof(BigInteger) },
			};
		}

		public static IEnumerable<object[]> GetNumericDefaultZeroTestData()
		{
			return new object[][]
			{
				new object[] { default(sbyte), (sbyte)0, typeof(sbyte) },
				new object[] { default(byte), Byte.MinValue, typeof(byte) },
				new object[] { default(short), (short)0, typeof(short) },
				new object[] { default(ushort), UInt16.MinValue, typeof(ushort) },
				new object[] { default(int), 0, typeof(int) },
				new object[] { default(uint), UInt32.MinValue, typeof(uint) },
				new object[] { default(long), 0L, typeof(long) },
				new object[] { default(ulong), UInt64.MinValue, typeof(ulong) },
				new object[] { default(float), 0.0f, typeof(float) },
				new object[] { default(double), 0.0, typeof(double) },
				new object[] { default(decimal), Decimal.Zero, typeof(decimal) },
				new object[] { default(BigInteger), BigInteger.Zero, typeof(BigInteger) },
			};
		}

		public static IEnumerable<object[]> GetNumericEpsilonTestData()
		{
			return new object[][]
			{
				new object[] { Single.Epsilon, -Single.Epsilon, typeof(float) },
				new object[] { Double.Epsilon, -Double.Epsilon, typeof(double) },
			};
		}

		public static IEnumerable<object[]> GetNumericInfinityTestData()
		{
			return new object[][]
			{
				new object[] { Single.PositiveInfinity, Single.NegativeInfinity, typeof(float) },
				new object[] { Double.PositiveInfinity, Double.NegativeInfinity, typeof(double) },
			};
		}

		public static IEnumerable<object[]> GetNotANumberTestData()
		{
			return new object[][]
			{
				new object[] { Single.NaN, -Single.NaN, typeof(float) },
				new object[] { Double.NaN, -Double.NaN, typeof(double) },
			};
		}
	}
}
