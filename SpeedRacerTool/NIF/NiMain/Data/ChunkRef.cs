using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain.Data;

/// <summary>"tLink" type from NifSkope. It refers to things later in the hierarchy</summary>
internal readonly struct ChunkRef<T> where T : NiObject
{
    public readonly int ChunkIndex;

    internal ChunkRef(EndianBinaryReader r)
    {
        ChunkIndex = r.ReadInt32();
        SRAssert.GreaterEqual(ChunkIndex, 0);
    }

    public T Resolve(NIFFile nif)
    {
        NiObject o = nif.BlockDatas[ChunkIndex]; // Don't 1-line. I'm debugging chunks I haven't added yet
        return (T)o;
    }

    public override string ToString()
    {
        return string.Format("{0} ({1})", typeof(T).Name, ChunkIndex);
    }
}