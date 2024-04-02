using Kermalis.EndianBinaryIO;
using System;

namespace Kermalis.SpeedRacerTool.XDS;

internal struct OneBeeString
{
	public string Str;

	internal OneBeeString(EndianBinaryReader r)
	{
		SRAssert.Equal(r.ReadUInt16(), 0x001B);
		SRAssert.Equal(r.ReadUInt16(), 0x0002);

		Str = r.ReadString_Count(r.ReadUInt16());
	}

	internal static void ReadEmpty(EndianBinaryReader r)
	{
		SRAssert.Equal(r.ReadUInt16(), 0x001B);
		SRAssert.Equal(r.ReadUInt16(), 0x0002);
		SRAssert.Equal(r.ReadUInt16(), 0x0000);
	}

	internal readonly void Write(EndianBinaryWriter w)
	{
		if (Str.Length > ushort.MaxValue)
		{
			throw new Exception();
		}

		w.WriteUInt16(0x001B);
		w.WriteUInt16(0x0002);
		w.WriteUInt16((ushort)Str.Length);
		w.WriteChars(Str);
	}

	public override readonly string ToString()
	{
		return $"\"{Str}\"";
	}
}