using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>"tLink" type from NifSkope. It refers to things later in the hierarchy, but can refer to <see langword="null"/></summary>
internal readonly struct NullableChunkRef<T> where T : NiObject
{
	public readonly int ChunkIndex;

	internal NullableChunkRef(EndianBinaryReader r)
	{
		ChunkIndex = r.ReadInt32();
		SRAssert.GreaterEqual(ChunkIndex, -1);
	}

	public T? Resolve(NIFFile nif)
	{
		if (ChunkIndex == -1)
		{
			return null;
		}
		NiObject o = nif.BlockDatas[ChunkIndex]; // Don't 1-line. I'm debugging chunks I haven't added yet
		return (T)o;
	}

	public override string ToString()
	{
		return string.Format("{0} ({1})", typeof(T).Name, ChunkIndex);
	}
}