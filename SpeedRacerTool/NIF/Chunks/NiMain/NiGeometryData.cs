using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.Chunks;
using System.Numerics;

namespace Kermalis.SpeedRacerTool.NIF.Chunks.NiMain;

/// <summary>Mesh data: vertices, vertex normals, etc.</summary>
internal abstract class NiGeometryData : NiObject
{
    /// <summary>Used by NiGeometryData to control the volatility of the mesh.
    /// Consistency Type is masked to only the upper 4 bits(0xF000). Dirty mask is the lower 12 (0x0FFF) but only used at runtime.</summary>
    public enum ConsistencyType : ushort
    {
        CT_MUTABLE = 0x0000,
        CT_STATIC = 0x4000,
        CT_VOLATILE = 0x8000,
    }

    public readonly int GroupID;
    public readonly ushort NumVerts;
    public readonly byte KeepFlags;
    public readonly byte CompressFlags;
    public readonly Vector3[] Vertices;
    public readonly ushort DataFlags; // NiGeometryDataFlags
    public readonly Vector3[] Normals;
    public readonly Vector3[] Tangents;
    public readonly Vector3[] Bitangents;
    public readonly NiBound BoundingSphere;
    public readonly Vector4[] VertexColors;
    public readonly TexCoord[][] UVSets;
    public readonly ConsistencyType ConsistencyFlags;
    public readonly ChunkRef<UnknownChunk> AdditionalData; // TODO: Ref<AbstractAdditionalGeometryData>

    protected NiGeometryData(EndianBinaryReader r, int offset)
        : base(offset)
    {
        GroupID = r.ReadInt32();
        NumVerts = r.ReadUInt16();
        KeepFlags = r.ReadByte();
        CompressFlags = r.ReadByte();

        bool hasVerts = r.ReadBoolean();
        if (hasVerts)
        {
            Vertices = new Vector3[NumVerts];
            r.ReadVector3s(Vertices);
        }
        else
        {
            Vertices = [];
        }

        DataFlags = r.ReadUInt16();

        bool hasNorms = r.ReadBoolean();
        if (hasNorms)
        {
            Normals = new Vector3[NumVerts];
            r.ReadVector3s(Normals);

            if ((DataFlags & 0b0001_0000_0000_0000) != 0)
            {
                Tangents = new Vector3[NumVerts];
                r.ReadVector3s(Tangents);
                Bitangents = new Vector3[NumVerts];
                r.ReadVector3s(Bitangents);
            }
            else
            {
                Tangents = [];
                Bitangents = [];
            }
        }
        else
        {
            Normals = [];
            Tangents = [];
            Bitangents = [];
        }

        BoundingSphere = new NiBound(r);

        bool hasVertColors = r.ReadBoolean();
        if (hasVertColors)
        {
            VertexColors = new Vector4[NumVerts];
            r.ReadVector4s(VertexColors);
        }
        else
        {
            VertexColors = [];
        }

        // TODO: Might have i/j inverted, but I don't think so actually
        UVSets = new TexCoord[DataFlags & 0b0011_1111][];
        for (int i = 0; i < UVSets.Length; i++)
        {
            UVSets[i] = new TexCoord[NumVerts];
            for (int j = 0; j < NumVerts; j++)
            {
                UVSets[i][j] = new TexCoord(r);
            }
        }

        ConsistencyFlags = r.ReadEnum<ConsistencyType>();

        AdditionalData = new ChunkRef<UnknownChunk>(r);
    }
}