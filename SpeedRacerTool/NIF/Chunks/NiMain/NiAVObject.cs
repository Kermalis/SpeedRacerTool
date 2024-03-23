using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.Chunks;
using System.Numerics;

namespace Kermalis.SpeedRacerTool.NIF.Chunks.NiMain;

/// <summary>Abstract audio-visual base class from which all of Gamebryo's scene graph objects inherit.</summary>
internal abstract class NiAVObject : NiObjectNET
{
    public readonly ushort Flags;
    public readonly Vector3 Translation;
    public readonly Matrix3x3 Rotation;
    public readonly float Scale;
    public readonly ChunkRef<NiProperty>[] Properties;
    public readonly ChunkRef<UnknownChunk> CollisionObject; // TODO: Ref<NiCollisionObject>

    protected NiAVObject(EndianBinaryReader r, int offset)
        : base(r, offset)
    {
        Flags = r.ReadUInt16();
        Translation = r.ReadVector3();
        Rotation = new Matrix3x3(r);
        Scale = r.ReadSingle();

        Properties = new ChunkRef<NiProperty>[r.ReadInt32()];
        ChunkRef<NiProperty>.ReadArray(r, Properties);

        CollisionObject = new ChunkRef<UnknownChunk>(r);
    }
}