using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.Chunks;

namespace Kermalis.SpeedRacerTool.NIF.Chunks.NiMain;

internal abstract class NiExtraData : Chunk
{
    public readonly StringIndex Name;

    protected NiExtraData(EndianBinaryReader r, int offset)
        : base(offset)
    {
        Name = new StringIndex(r);
    }
}