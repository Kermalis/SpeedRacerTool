using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal abstract class NiInterpController : NiTimeController
{
	protected NiInterpController(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		//
	}
}