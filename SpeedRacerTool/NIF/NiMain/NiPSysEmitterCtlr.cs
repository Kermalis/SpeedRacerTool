using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain.Data;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiPSysEmitterCtlr : NiPSysModifierCtlr
{
	public readonly ChunkRef<NiInterpolator> VisibilityOperator;

	public NiPSysEmitterCtlr(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		VisibilityOperator = new ChunkRef<NiInterpolator>(r);
	}

	public override void SetParentAndChildren(NIFFile nif, NiObject? parent)
	{
		base.SetParentAndChildren(nif, parent);

		VisibilityOperator.Resolve(nif).SetParentAndChildren(nif, this);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.WriteChunk(nameof(VisibilityOperator), nif, VisibilityOperator.Resolve(nif));
	}
}