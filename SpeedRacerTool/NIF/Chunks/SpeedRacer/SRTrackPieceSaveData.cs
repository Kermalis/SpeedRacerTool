using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF;
using Kermalis.SpeedRacerTool.NIF.Chunks;
using System;

namespace Kermalis.SpeedRacerTool.NIF.Chunks.SpeedRacer;

// fwd_short.trk
internal sealed class SRTrackPieceSaveData : Chunk
{
    public const string NAME = "SRTrackPieceSaveData";

    public readonly float[] Data;

    internal SRTrackPieceSaveData(EndianBinaryReader r, int offset, uint size)
        : base(offset)
    {
        if (size != 8)
        {
            throw new Exception();
        }

        Data = new float[2];
        r.ReadSingles(Data);
    }

    internal override string DebugStr(NIFFile nif)
    {
        return DebugStr(NAME, string.Format("{0} | {1}", Data[0], Data[1]));
    }
}