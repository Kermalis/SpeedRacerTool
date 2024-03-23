using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>Generic node object for grouping.</summary>
internal sealed class NiNode : NiAVObject
{
    public const string NAME = "NiNode";

    public readonly ChunkRef<NiAVObject>[] Children;
    public readonly ChunkRef<UnknownChunk>[] Effects; // TODO: Ref<NiDynamicEffect>[]

    internal NiNode(EndianBinaryReader r, int offset)
        : base(r, offset)
    {
        Children = new ChunkRef<NiAVObject>[r.ReadUInt32()];
        ChunkRef<NiAVObject>.ReadArray(r, Children);

        Effects = new ChunkRef<UnknownChunk>[r.ReadUInt32()];
        ChunkRef<UnknownChunk>.ReadArray(r, Effects);
    }

    internal override string DebugStr(NIFFile nif)
    {
        return DebugStr(NAME, string.Format("Name=\"{0}\"",
            Name.Resolve(nif)));
    }
}