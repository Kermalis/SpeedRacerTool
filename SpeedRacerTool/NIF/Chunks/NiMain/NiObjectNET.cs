﻿using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.Chunks;

namespace Kermalis.SpeedRacerTool.NIF.Chunks.NiMain;

/// <summary>Abstract base class for NiObjects that support names, extra data, and time controllers.</summary>
internal abstract class NiObjectNET : NiObject
{
    public readonly StringIndex Name;
    public readonly ChunkRef<NiExtraData>[] ExtraDataList;
    public readonly ChunkRef<UnknownChunk> Controller; // TODO: Ref<NiTimeController>

    protected NiObjectNET(EndianBinaryReader r, int offset)
        : base(offset)
    {
        Name = new StringIndex(r);

        ExtraDataList = new ChunkRef<NiExtraData>[r.ReadInt32()];
        ChunkRef<NiExtraData>.ReadArray(r, ExtraDataList);

        Controller = new ChunkRef<UnknownChunk>(r);
    }
}