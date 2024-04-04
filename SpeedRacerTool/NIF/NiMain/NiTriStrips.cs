using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>A shape node that refers to data organized into strips of triangles.</summary>
internal sealed class NiTriStrips : NiTriBasedGeom
{
	internal NiTriStrips(EndianBinaryReader r, int offset)
		: base(r, offset)
	{
		//
	}

	internal override string DebugStr(NIFFile nif)
	{
		return DebugStr(nameof(NiTriStrips), string.Format("Name=\"{0}\"", Name.Resolve(nif)));
	}
}