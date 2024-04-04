using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>"tUpLink" type from NifSkope. It refers to things prior in the hierarchy, and will not be <see langword="null"/>, unlike <see cref="ChunkRef{T}"/> which points to objects later and can be <see langword="null"/></summary>
internal readonly struct ChunkPtr<T> where T : NIFChunk
{
	public readonly int ChunkIndex;

	internal ChunkPtr(EndianBinaryReader r)
	{
		ChunkIndex = r.ReadInt32();
	}

	public T Resolve(NIFFile nif)
	{
		NIFChunk c = nif.BlockDatas[ChunkIndex]; // Don't 1-line. I'm debugging chunks I haven't added yet
		return (T)c;
	}
}