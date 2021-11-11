using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Windows.Data;

namespace F0.Windows.Data
{
	public sealed class NumericNegationConverter : IValueConverter
	{
		private static readonly IReadOnlyDictionary<Type, Converter<object, object>> converters = new Dictionary<Type, Converter<object, object>>
		{
			{ typeof(sbyte), new Converter<object, object>(NegateSByte)},
			{ typeof(byte), new Converter<object, object>(NegateByte)},
			{ typeof(short), new Converter<object, object>(NegateInt16)},
			{ typeof(ushort), new Converter<object, object>(NegateUInt16)},
			{ typeof(int), new Converter<object, object>(NegateInt32)},
			{ typeof(uint), new Converter<object, object>(NegateUInt32)},
			{ typeof(long), new Converter<object, object>(NegateInt64)},
			{ typeof(ulong), new Converter<object, object>(NegateUInt64)},
			{ typeof(nint), new Converter<object, object>(NegateIntPtr)},
			{ typeof(nuint), new Converter<object, object>(NegateUIntPtr)},
			{ typeof(BigInteger), new Converter<object, object>(NegateBigInteger) },
			{ typeof(float), new Converter<object, object>(NegateSingle)},
			{ typeof(double), new Converter<object, object>(NegateDouble)},
			{ typeof(decimal), new Converter<object, object>(NegateDecimal)},
		};

		object? IValueConverter.Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			return Negate(value);
		}

		object? IValueConverter.ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			return Negate(value);
		}

		private static object? Negate(object? value)
		{
			if (value is null)
			{
				return value;
			}

			Type type = value.GetType();

			if (converters.TryGetValue(type, out Converter<object, object>? converter))
			{
				return converter.Invoke(value);
			}
			else
			{
				string message = $"{nameof(NumericNegationConverter)} cannot convert from {type.FullName}.";
				throw new NotSupportedException(message);
			}
		}

		private static object NegateSByte(object value)
		{
			sbyte integral = (sbyte)value;
			sbyte negation = checked((sbyte)-integral);
			return negation;
		}

		private static object NegateByte(object value)
		{
			byte integral = (byte)value;
			byte negation = checked((byte)-integral);
			return negation;
		}

		private static object NegateInt16(object value)
		{
			short integral = (short)value;
			short negation = checked((short)-integral);
			return negation;
		}

		private static object NegateUInt16(object value)
		{
			ushort integral = (ushort)value;
			ushort negation = checked((ushort)-integral);
			return negation;
		}

		private static object NegateInt32(object value)
		{
			int integral = (int)value;
			int negation = checked(-integral);
			return negation;
		}

		private static object NegateUInt32(object value)
		{
			uint integral = (uint)value;
			uint negation = checked((uint)-integral);
			return negation;
		}

		private static object NegateInt64(object value)
		{
			long integral = (long)value;
			long negation = checked(-integral);
			return negation;
		}

		private static object NegateUInt64(object value)
		{
			ulong integral = (ulong)value;
			ulong negation = checked(0ul - integral);
			return negation;
		}

		private static object NegateIntPtr(object value)
		{
			nint native = (nint)value;
			nint negation = checked(-native);
			return negation;
		}

		private static object NegateUIntPtr(object value)
		{
			nuint native = (nuint)value;
			nuint negation = checked(0 - native);
			return negation;
		}

		private static object NegateBigInteger(object value)
		{
			BigInteger integral = (BigInteger)value;
			BigInteger negation = -integral;
			return negation;
		}

		private static object NegateSingle(object value)
		{
			float real = (float)value;
			float negation = -real;
			return negation;
		}

		private static object NegateDouble(object value)
		{
			double real = (double)value;
			double negation = -real;
			return negation;
		}

		private static object NegateDecimal(object value)
		{
			decimal real = (decimal)value;
			decimal negation = Decimal.Negate(real);
			return negation;
		}
	}
}
