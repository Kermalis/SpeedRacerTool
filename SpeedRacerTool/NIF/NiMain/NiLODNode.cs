using Kermalis.EndianBinaryIO;
using Kermalis.SpeedRacerTool.NIF.NiMain.Data;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiLODNode : NiSwitchNode
{
	public readonly NullableChunkRef<NIFUnknownChunk> LODData; // TODO: Ref<NiLODData>

	internal NiLODNode(EndianBinaryReader r, int index, int offset)
		: base(r, index, offset)
	{
		LODData = new NullableChunkRef<NIFUnknownChunk>(r);
	}

	protected override void DebugStr(NIFFile nif, NIFStringBuilder sb)
	{
		base.DebugStr(nif, sb);

		sb.WriteChunk(nameof(LODData), nif, LODData.Resolve(nif));
	}
}