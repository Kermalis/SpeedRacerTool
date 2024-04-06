using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal abstract class NiSingleInterpController : NiInterpController
{
	public readonly ChunkRef<NiInterpolator> Interpolator;

	protected NiSingleInterpController(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		Interpolator = new ChunkRef<NiInterpolator>(r);
	}

	public override void SetParentAndChildren(NIFFile nif, NiObject? parent)
	{
		base.SetParentAndChildren(nif, parent);

		Interpolator.Resolve(nif).SetParentAndChildren(nif, this);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.WriteChunk(nameof(Interpolator), nif, Interpolator.Resolve(nif));
	}
}