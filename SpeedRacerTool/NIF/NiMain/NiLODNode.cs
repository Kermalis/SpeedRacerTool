using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal sealed class NiLODNode : NiSwitchNode
{
	public readonly ChunkRef<NIFUnknownChunk> LODData; // TODO: Ref<NiLODData>

	internal NiLODNode(EndianBinaryReader r, int offset)
		: base(r, offset)
	{
		LODData = new ChunkRef<NIFUnknownChunk>(r);
	}
}