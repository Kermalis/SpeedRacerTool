using Kermalis.EndianBinaryIO;
using System;

namespace Kermalis.SpeedRacerTool.XDS;

internal struct Magic_OneAyyArray
{
	public uint ArrayLen;
	public MagicValue Magic;

	internal Magic_OneAyyArray(EndianBinaryReader r, XDSFile xds)
	{
		ArrayLen = xds.ReadFileUInt32(r);
		Magic = new MagicValue(r);
	}

	internal static void ReadEmpty(EndianBinaryReader r)
	{
		SRAssert.Equal(r.ReadUInt64(), 0);
	}
	internal readonly void AssertIs0()
	{
		if (ArrayLen != 0 || Magic.Value != 0)
		{
			throw new Exception();
		}
	}
	internal readonly void AssertNot0()
	{
		if (ArrayLen == 0 || Magic.Value == 0)
		{
			throw new Exception();
		}
	}
	internal readonly void AssertEqual(uint value)
	{
		if (ArrayLen != value)
		{
			throw new Exception();
		}
	}

	public override readonly string ToString()
	{
		return Magic.ToString();
	}
}