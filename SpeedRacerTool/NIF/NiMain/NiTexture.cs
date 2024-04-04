using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal abstract class NiTexture : NiObjectNET
{
	protected NiTexture(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		//
	}
}