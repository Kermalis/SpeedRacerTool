using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>Describes a mesh, built from triangles.</summary>
internal abstract class NiTriBasedGeomData : NiGeometryData
{
	public readonly ushort NumTris;

	protected NiTriBasedGeomData(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		NumTris = r.ReadUInt16();
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.AppendLine(nameof(NumTris), NumTris, hex: false);
	}
}