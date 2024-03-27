using Kermalis.EndianBinaryIO;
using System;
using System.Runtime.InteropServices;

namespace Kermalis.SpeedRacerTool.XDS;

internal struct MagicValue
{
	public uint Value;

	public MagicValue(EndianBinaryReader r)
	{
		Value = r.ReadUInt32();
	}

	public static void ReadArray(EndianBinaryReader r, MagicValue[] arr)
	{
		Span<uint> arrInt = MemoryMarshal.Cast<MagicValue, uint>(arr);
		r.ReadUInt32s(arrInt);
	}
	public static void WriteArray(EndianBinaryWriter w, MagicValue[] arr)
	{
		Span<uint> arrInt = MemoryMarshal.Cast<MagicValue, uint>(arr);
		w.WriteUInt32s(arrInt);
	}

	internal readonly void Write(EndianBinaryWriter w)
	{
		w.WriteUInt32(Value);
	}

	public override readonly string ToString()
	{
		return "0x" + Value.ToString("X8");
	}
}