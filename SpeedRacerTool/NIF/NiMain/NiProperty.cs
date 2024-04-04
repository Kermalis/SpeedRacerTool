using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal abstract class NiProperty : NiObjectNET
{
	protected NiProperty(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		//
	}
}