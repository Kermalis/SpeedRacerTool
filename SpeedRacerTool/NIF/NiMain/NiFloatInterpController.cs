using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal abstract class NiFloatInterpController : NiSingleInterpController
{
	protected NiFloatInterpController(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		//
	}
}