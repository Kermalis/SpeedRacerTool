using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

// Not sealed
internal class NiTriShape : NiTriBasedGeom
{
	internal NiTriShape(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		//
	}
}