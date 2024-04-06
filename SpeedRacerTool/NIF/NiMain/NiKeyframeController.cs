using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal abstract class NiKeyframeController : NiSingleInterpController
{
	protected NiKeyframeController(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		//
	}
}