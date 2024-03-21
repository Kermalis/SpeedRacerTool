using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.Chunks.NiMain;

/// <summary>Describes a mesh, built from triangles.</summary>
internal abstract class NiTriBasedGeomData : NiGeometryData
{
	public readonly ushort NumTris;

	protected NiTriBasedGeomData(EndianBinaryReader r, int offset)
		: base(r, offset)
	{
		NumTris = r.ReadUInt16();
	}
}