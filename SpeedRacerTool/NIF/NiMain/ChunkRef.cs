using Kermalis.EndianBinaryIO;
using System;
using System.Runtime.InteropServices;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>"tLink" type from NifSkope. It usually refers to things later in the hierarchy</summary>
internal readonly struct ChunkRef<T> where T : Chunk
{
	public readonly int ChunkIndex;

	internal ChunkRef(EndianBinaryReader r)
	{
		ChunkIndex = r.ReadInt32();
	}

	public T? Resolve(NIFFile nif)
	{
		return ChunkIndex == -1 ? null : (T)nif.BlockDatas[ChunkIndex];
	}

	public static void ReadArray(EndianBinaryReader r, ChunkRef<T>[] arr)
	{
		Span<int> arrInt = MemoryMarshal.Cast<ChunkRef<T>, int>(arr);
		r.ReadInt32s(arrInt);
	}
}