using System;
using System.Numerics;

namespace Kermalis.SpeedRacerTool;

internal static class SRAssert
{
	internal static void GreaterEqual(long value, long expected)
	{
		if (value < expected)
		{
			throw new Exception();
		}
	}
	internal static void LessEqual(long value, long expected)
	{
		if (value > expected)
		{
			throw new Exception();
		}
	}
	internal static void Equal(long value, long expected)
	{
		if (value != expected)
		{
			throw new Exception();
		}
	}
	internal static void Equal(ulong value, ulong expected)
	{
		if (value != expected)
		{
			throw new Exception();
		}
	}
	internal static void Equal(float value, float expected)
	{
		if (value != expected)
		{
			throw new Exception();
		}
	}
	internal static void Equal(Vector3 value, Vector3 expected)
	{
		if (value != expected)
		{
			throw new Exception();
		}
	}
	internal static void NotEqual(float value, float expected)
	{
		if (value == expected)
		{
			throw new Exception();
		}
	}
	internal static void NotEqual(Vector3 value, Vector3 expected)
	{
		if (value == expected)
		{
			throw new Exception();
		}
	}
	internal static void False(bool value)
	{
		if (value)
		{
			throw new Exception();
		}
	}
	internal static void True(bool value)
	{
		if (!value)
		{
			throw new Exception();
		}
	}

	internal static void SequenceEqual(ReadOnlySpan<char> value, ReadOnlySpan<char> expected,
		string? error = null)
	{
		if (!value.SequenceEqual(expected))
		{
			throw new Exception(error);
		}
	}
}