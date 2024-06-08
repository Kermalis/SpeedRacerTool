using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal abstract class NiExtraDataController : NiSingleInterpController
{
	protected NiExtraDataController(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		//
	}
}