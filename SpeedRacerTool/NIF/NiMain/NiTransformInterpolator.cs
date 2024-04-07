using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain.Data;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiTransformInterpolator : NiKeyBasedInterpolator
{
	public readonly QTransform Transform;
	public readonly ChunkRef<NIFUnknownChunk> Data; // TODO: NiTransformData

	public NiTransformInterpolator(EndianBinaryReader r, int index, int offset)
		: base(index, offset)
	{
		Transform = new QTransform(r);
		Data = new ChunkRef<NIFUnknownChunk>(r);
	}

	public override void SetParentAndChildren(NIFFile nif, NiObject? parent)
	{
		base.SetParentAndChildren(nif, parent);

		Data.Resolve(nif).SetParentAndChildren(nif, this);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		Transform.DebugStr(sb, nameof(Transform));

		sb.WriteChunk(nameof(Data), nif, Data.Resolve(nif));
	}
}