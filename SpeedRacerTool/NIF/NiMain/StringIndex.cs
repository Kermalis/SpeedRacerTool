using Kermalis.EndianBinaryIO;
using System;
using System.Runtime.InteropServices;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal readonly struct StringIndex
{
	public readonly int Index;

	internal StringIndex(EndianBinaryReader r)
	{
		Index = r.ReadInt32();
	}

	public string? Resolve(NIFFile nif)
	{
		return Index == -1 ? null : nif.Strings[Index];
	}

	public static void ReadArray(EndianBinaryReader r, StringIndex[] arr)
	{
		Span<int> arrInt = MemoryMarshal.Cast<StringIndex, int>(arr);
		r.ReadInt32s(arrInt);
	}
}