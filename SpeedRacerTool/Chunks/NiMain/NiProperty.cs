using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.Chunks.NiMain;

internal abstract class NiProperty : NiObjectNET
{
	protected NiProperty(EndianBinaryReader r, int offset)
		: base(r, offset)
	{
		//
	}
}