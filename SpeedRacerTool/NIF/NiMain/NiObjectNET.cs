using Kermalis.EndianBinaryIO;

namespace Kermalis.SpeedRacerTool.NIF.NiMain;

/// <summary>Abstract base class for NiObjects that support names, extra data, and time controllers.</summary>
internal abstract class NiObjectNET : NiObject
{
	public readonly StringIndex Name;
	public readonly ChunkRef<NiExtraData>[] ExtraDataList;
	public readonly ChunkRef<NIFUnknownChunk> Controller; // TODO: Ref<NiTimeController>

	protected NiObjectNET(EndianBinaryReader r, int offset)
		: base(offset)
	{
		Name = new StringIndex(r);

		ExtraDataList = new ChunkRef<NiExtraData>[r.ReadInt32()];
		ChunkRef<NiExtraData>.ReadArray(r, ExtraDataList);

		Controller = new ChunkRef<NIFUnknownChunk>(r);
	}
}