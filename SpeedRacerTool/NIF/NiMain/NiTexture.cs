using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal abstract class NiTexture : NiObjectNET
{
	protected NiTexture(EndianBinaryReader r, int offset)
		: base(r, offset)
	{
		//
	}
}