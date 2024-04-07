using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain.Data;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiShadeProperty : NiProperty
{
	public readonly ShadeFlags Flags;

	public NiShadeProperty(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		Flags = r.ReadEnum<ShadeFlags>();
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.AppendLine(nameof(Flags), Flags.ToString());
	}
}