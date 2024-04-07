using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain.Data;
using System.Numerics;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>Mesh data: vertices, vertex normals, etc.</summary>
internal abstract class NiGeometryData : NiObject
{
	public readonly ushort NumVerts;
	public readonly byte KeepFlags;
	public readonly byte CompressFlags;
	public readonly Vector3[]? Vertices;
	public readonly NiGeometryDataFlags DataFlags;
	public readonly Vector3[]? Normals;
	public readonly Vector3[]? Tangents;
	public readonly Vector3[]? Bitangents;
	public readonly NiBound BoundingSphere;
	public readonly Vector4[]? VertexColors;
	public readonly TexCoord[][] UVSets;
	public readonly ConsistencyType ConsistencyFlags;
	public readonly NullableChunkRef<NIFUnknownChunk> AdditionalData; // TODO: Ref<AbstractAdditionalGeometryData>

	protected NiGeometryData(EndianBinaryReader r, int index, int offset)
		: base(index, offset)
	{
		SRAssert.Equal(r.ReadInt32(), 0);

		NumVerts = r.ReadUInt16();
		SRAssert.GreaterEqual(NumVerts, 1);

		KeepFlags = r.ReadByte();
		CompressFlags = r.ReadByte();

		if (r.ReadSafeBoolean())
		{
			Vertices = new Vector3[NumVerts];
			r.ReadVector3s(Vertices);
		}

		DataFlags = new NiGeometryDataFlags(r);

		bool hasNorms = r.ReadSafeBoolean();
		SRAssert.False(hasNorms);

		if (hasNorms)
		{
			Normals = new Vector3[NumVerts];
			r.ReadVector3s(Normals);

			if (DataFlags.NBTMethod == NiNBTMethod.NBT_METHOD_NDL)
			{
				SRAssert.True(false); // Force error so I can debug
				Tangents = new Vector3[NumVerts];
				r.ReadVector3s(Tangents);
				Bitangents = new Vector3[NumVerts];
				r.ReadVector3s(Bitangents);
			}
		}

		BoundingSphere = new NiBound(r);

		if (r.ReadSafeBoolean())
		{
			VertexColors = new Vector4[NumVerts];
			r.ReadVector4s(VertexColors);
		}

		// TODO: Might have i/j inverted, but I don't think so actually
		UVSets = new TexCoord[DataFlags.NumUVSets][];
		SRAssert.Equal(UVSets.Length, 0);
		for (int i = 0; i < UVSets.Length; i++)
		{
			UVSets[i] = new TexCoord[NumVerts];
			for (int j = 0; j < NumVerts; j++)
			{
				UVSets[i][j] = new TexCoord(r);
			}
		}

		ConsistencyFlags = r.ReadEnum<ConsistencyType>();

		AdditionalData = new NullableChunkRef<NIFUnknownChunk>(r);
		SRAssert.Equal(AdditionalData.ChunkIndex, -1);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.AppendLine(nameof(NumVerts), NumVerts, hex: false);
		sb.AppendLine(nameof(KeepFlags), KeepFlags);
		sb.AppendLine(nameof(CompressFlags), CompressFlags);
		sb.AppendLine(nameof(ConsistencyFlags), ConsistencyFlags.ToString());

		DataFlags.DebugStr(sb, nameof(DataFlags));

		sb.WriteChunk(nameof(AdditionalData), nif, AdditionalData.Resolve(nif));

		sb.WriteTODO(nameof(NiGeometryData));
	}
}