using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain;
using System;
using System.Runtime.InteropServices;

namespace Kermalis.SpeedRacerTool;

internal static class SRUtils
{
	// For doubles but also works for float
	public const string TOSTRING_NO_SCIENTIFIC = "0.###################################################################################################################################################################################################################################################################################################################################################";

	public static void ReadArray<T>(this EndianBinaryReader r, ChunkRef<T>[] arr)
		where T : NiObject
	{
		Span<int> arrInt = MemoryMarshal.Cast<ChunkRef<T>, int>(arr);
		r.ReadInt32s(arrInt);
	}
	public static void ReadArray<T>(this EndianBinaryReader r, ChunkPtr<T>[] arr)
		where T : NiObject
	{
		Span<int> arrInt = MemoryMarshal.Cast<ChunkPtr<T>, int>(arr);
		r.ReadInt32s(arrInt);
	}
	public static void ReadArray<T>(this EndianBinaryReader r, NullableChunkRef<T>[] arr)
		where T : NiObject
	{
		Span<int> arrInt = MemoryMarshal.Cast<NullableChunkRef<T>, int>(arr);
		r.ReadInt32s(arrInt);
	}
	public static void ReadArray(this EndianBinaryReader r, StringIndex[] arr)
	{
		Span<int> arrInt = MemoryMarshal.Cast<StringIndex, int>(arr);
		r.ReadInt32s(arrInt);
	}
}