using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal abstract class NiBoolInterpController : NiSingleInterpController
{
	protected NiBoolInterpController(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		//
	}
}