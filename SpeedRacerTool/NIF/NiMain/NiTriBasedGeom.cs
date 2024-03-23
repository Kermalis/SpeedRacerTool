using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>Describes a mesh, built from triangles.</summary>
internal abstract class NiTriBasedGeom : NiGeometry
{
	protected NiTriBasedGeom(EndianBinaryReader r, int offset)
		: base(r, offset)
	{
		//
	}
}