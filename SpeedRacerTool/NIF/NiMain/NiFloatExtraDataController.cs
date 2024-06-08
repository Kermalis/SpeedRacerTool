using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain.Data;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiFloatExtraDataController : NiExtraDataController
{
	public readonly StringIndex ControllerData;

	public NiFloatExtraDataController(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		ControllerData = new StringIndex(r);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.AppendLine(nameof(ControllerData), ControllerData.Resolve(nif));
	}
}