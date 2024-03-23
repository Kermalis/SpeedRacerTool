using Kermalis.SpeedRacerTool.NIF.Chunks;

namespace Kermalis.SpeedRacerTool.NIF.Chunks.NiMain;

/// <summary>Abstract object type.</summary>
internal abstract class NiObject : Chunk
{
    protected NiObject(int offset)
        : base(offset)
    {
        //
    }
}