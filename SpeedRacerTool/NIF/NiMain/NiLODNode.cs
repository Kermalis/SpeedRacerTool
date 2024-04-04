using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiLODNode : NiSwitchNode
{
	public readonly ChunkRef<NIFUnknownChunk> LODData; // TODO: Ref<NiLODData>

	internal NiLODNode(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		LODData = new ChunkRef<NIFUnknownChunk>(r);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.WriteTODO(nameof(NiLODNode));
	}
}