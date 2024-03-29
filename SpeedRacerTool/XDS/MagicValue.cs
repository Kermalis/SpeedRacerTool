using Kermalis.EndianBinaryIO;
using System;
using System.Runtime.InteropServices;

namespace Kermalis.SpeedRacerTool.XDS;

/// <summary>Some uint_LE that are similar between files. PS2 values are 0x34XXXX, 0x42XXXX, 0x46XXXX, or 0x4EXXXX. WII values are 0x3AXXXX or 0xB0XXXX.
/// Probably an allocator for <see cref="OneAyyArray{T}"/> and <see cref="OneBeeString"/>.
/// If this value is 0, it corresponds with an empty array/string.</summary>
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

	internal static void ReadEmpty(EndianBinaryReader r)
	{
		XDSFile.AssertValue(r.ReadUInt32(), 0);
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