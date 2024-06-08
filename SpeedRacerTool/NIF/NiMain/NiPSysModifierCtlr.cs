using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain.Data;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal abstract class NiPSysModifierCtlr : NiSingleInterpController
{
	public readonly StringIndex ModName;

	protected NiPSysModifierCtlr(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		ModName = new StringIndex(r);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		sb.AppendLine(nameof(ModName), ModName.Resolve(nif));
	}
}