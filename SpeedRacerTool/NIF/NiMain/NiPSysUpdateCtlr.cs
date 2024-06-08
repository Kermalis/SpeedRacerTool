using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiPSysUpdateCtlr : NiTimeController
{
	public NiPSysUpdateCtlr(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		//
	}
}