using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiBoolInterpolator : NiKeyBasedInterpolator
{
	public readonly bool Value;
	public readonly ChunkRef<NIFUnknownChunk> Data; // TODO: Ref<NiBoolData>

	public NiBoolInterpolator(EndianBinaryReader r, int index, int offset)
		: base(index, offset)
	{
		Value = r.ReadSafeBoolean();
		Data = new ChunkRef<NIFUnknownChunk>(r);
	}

	public override void SetParentAndChildren(NIFFile nif, NiObject? parent)
	{
		base.SetParentAndChildren(nif, parent);

		Data.Resolve(nif).SetParentAndChildren(nif, this);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		sb.AppendLine_Boolean(nameof(Value), Value);

		sb.WriteChunk(nameof(Data), nif, Data.Resolve(nif));
	}
}