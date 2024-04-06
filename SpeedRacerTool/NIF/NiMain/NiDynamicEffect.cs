using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal abstract class NiDynamicEffect : NiAVObject
{
	public readonly bool IsEnabled;
	public readonly ChunkRef<NiAVObject>[] AffectedNodes;

	protected NiDynamicEffect(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		IsEnabled = r.ReadSafeBoolean();

		AffectedNodes = new ChunkRef<NiAVObject>[r.ReadUInt32()];
		r.ReadArray(AffectedNodes);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.WriteTODO(nameof(NiDynamicEffect));
	}
}