using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>A shape node that refers to data organized into strips of triangles.</summary>
internal sealed class NiTriStrips : NiTriBasedGeom
{
	internal NiTriStrips(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		//
	}
}