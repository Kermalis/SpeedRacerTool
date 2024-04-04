using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiTriShapeData : NiTriBasedGeomData
{
	public readonly struct Tri
	{
		public readonly ushort VertID0;
		public readonly ushort VertID1;
		public readonly ushort VertID2;

		internal Tri(EndianBinaryReader r)
		{
			VertID0 = r.ReadUInt16();
			VertID1 = r.ReadUInt16();
			VertID2 = r.ReadUInt16();
		}
	}
	public readonly struct MatchGroup
	{
		public readonly ushort[] VertexIndices;

		internal MatchGroup(EndianBinaryReader r)
		{
			VertexIndices = new ushort[r.ReadUInt16()];
			r.ReadUInt16s(VertexIndices);
		}
	}

	/// <summary>Num Triangles times 3</summary>
	public readonly uint NumTrianglePoints;
	public readonly Tri[]? Triangles;
	/// <summary>The shared normals</summary>
	public readonly MatchGroup[] MatchGroups;

	internal NiTriShapeData(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		NumTrianglePoints = r.ReadUInt32();

		bool hasTris = r.ReadBoolean();
		if (hasTris)
		{
			Triangles = new Tri[NumTris];
			for (int i = 0; i < Triangles.Length; i++)
			{
				Triangles[i] = new Tri(r);
			}
		}
		else
		{
			Triangles = [];
		}

		MatchGroups = new MatchGroup[r.ReadUInt16()];
		for (int i = 0; i < MatchGroups.Length; i++)
		{
			MatchGroups[i] = new MatchGroup(r);
		}
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.WriteTODO(nameof(NiTriShapeData));
	}
}