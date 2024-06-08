using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain.Data;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiFloatInterpolator : NiKeyBasedInterpolator
{
	public readonly float Value;
	public readonly NullableChunkRef<NiFloatData> Data;

	public NiFloatInterpolator(EndianBinaryReader r, int index, int offset)
		: base(index, offset)
	{
		Value = r.ReadSingle();
		Data = new NullableChunkRef<NiFloatData>(r);
	}

	public override void SetParentAndChildren(NIFFile nif, NiObject? parent)
	{
		base.SetParentAndChildren(nif, parent);

		Data.Resolve(nif)?.SetParentAndChildren(nif, this);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.AppendLine(nameof(Value), Value);

		sb.WriteChunk(nameof(Data), nif, Data.Resolve(nif));
	}
}