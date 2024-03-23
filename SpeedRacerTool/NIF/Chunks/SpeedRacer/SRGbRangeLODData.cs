using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF;
using Kermalis.SpeedRacerTool.NIF.Chunks;
using System;

namespace Kermalis.SpeedRacerTool.NIF.Chunks.SpeedRacer;

// fwd_short.trk
internal sealed class SRGbRangeLODData : Chunk
{
    public const string NAME = "SRGbRangeLODData";

    public readonly float[] Data;

    internal SRGbRangeLODData(EndianBinaryReader r, int offset, uint size)
        : base(offset)
    {
        if (size != 28)
        {
            throw new Exception();
        }

        Data = new float[7];
        r.ReadSingles(Data);
    }

    internal override string DebugStr(NIFFile nif)
    {
        return DebugStr(NAME, string.Join(", ", Data));
    }
}