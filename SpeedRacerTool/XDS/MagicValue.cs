using Kermalis.EndianBinaryIO;
using System;
using System.Runtime.InteropServices;

namespace Kermalis.SpeedRacerTool.XDS;

/// <summary>Some uint_LE that are similar between files. PS2 values around 0x34XXXX corresponde with WII values around 0x3AXXXX.
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
		SRAssert.Equal(r.ReadUInt32(), 0);
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