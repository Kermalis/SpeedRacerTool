using Kermalis.EndianBinaryIO;
using System;
using System.Runtime.InteropServices;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>"tUpLink" type from NifSkope. It refers to things prior in the hierarchy, and will not be <see langword="null"/>, unlike <see cref="ChunkRef{T}"/> which points to objects later. <see cref="NullableChunkRef{T}"/> can be <see langword="null"/></summary>
internal readonly struct ChunkPtr<T> where T : NiObject
{
	public readonly int ChunkIndex;

	internal ChunkPtr(EndianBinaryReader r)
	{
		ChunkIndex = r.ReadInt32();
		SRAssert.GreaterEqual(ChunkIndex, 0);
	}

	public T Resolve(NIFFile nif)
	{
		NiObject o = nif.BlockDatas[ChunkIndex]; // Don't 1-line. I'm debugging chunks I haven't added yet
		return (T)o;
	}

	public static void ReadArray(EndianBinaryReader r, ChunkPtr<T>[] arr)
	{
		Span<int> arrInt = MemoryMarshal.Cast<ChunkPtr<T>, int>(arr);
		r.ReadInt32s(arrInt);
	}
}