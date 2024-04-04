using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiBillboardNode : NiNode
{
	public readonly BillboardMode Mode;

	internal NiBillboardNode(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		Mode = r.ReadEnum<BillboardMode>();
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.WriteTODO(nameof(NiBillboardNode));
	}
}