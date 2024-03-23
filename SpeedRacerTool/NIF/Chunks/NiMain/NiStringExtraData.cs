using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF;
using System;

namespace Kermalis.SpeedRacerTool.NIF.Chunks.NiMain;

internal sealed class NiStringExtraData : NiExtraData
{
    public const string NAME = "NiStringExtraData";

    public readonly StringIndex StringData;

    internal NiStringExtraData(EndianBinaryReader r, int offset, uint size)
        : base(r, offset)
    {
        if (size != 8)
        {
            throw new Exception();
        }

        StringData = new StringIndex(r);
    }

    internal override string DebugStr(NIFFile nif)
    {
        return DebugStr(NAME, string.Format("Name=\"{0}\" | Str=\"{1}\"",
            Name.Resolve(nif),
            StringData.Resolve(nif)));
    }
}