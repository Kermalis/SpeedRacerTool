using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

internal abstract class NiDynamicEffect : NiAVObject
{
	public readonly bool IsEnabled;
	public readonly ChunkRef<NiAVObject>[] AffectedNodes;

	protected NiDynamicEffect(EndianBinaryReader r, int offset)
		: base(r, offset)
	{
		IsEnabled = r.ReadBoolean();

		AffectedNodes = new ChunkRef<NiAVObject>[r.ReadUInt32()];
		ChunkRef<NiAVObject>.ReadArray(r, AffectedNodes);
	}
}